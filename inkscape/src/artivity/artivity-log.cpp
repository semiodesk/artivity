/*
 * Authors:
 * Sebastian Faubel <sebastian@semiodesk.com>
 * Moritz Eberl <moritz@semiodesk.com>
 *
 * Copyright (c) 2015 Authors
 *
 * Released under GNU GPL, see the file 'COPYING' for more information
 */

#include "artivity/artivity-log.h"

namespace Inkscape
{
    using namespace std;
    using namespace artivity;
    using Inkscape::Util::List;
    using Inkscape::Util::rest;
    using Inkscape::XML::AttributeRecord;
    using Inkscape::XML::Node;
    using Inkscape::XML::NodeType;
    
    ArtivityLog::ArtivityLog(SPDocument* doc, SPDesktop* desktop) : UndoStackObserver(), _doc(doc)
    {
        _desktop = desktop;

        _log = ActivityLog();
        _instrument = new Resource("application://inkscape.desktop");

        g_message("ArtivityLog(SPDocument*, SPDesktop*) called; doc=%p", doc);
        
        logEvent(art::Open, NULL);
    }

    ArtivityLog::~ArtivityLog()
    {
        g_message("~ArtivityLog() called;");
        
        if(_target != NULL) delete _target;
        if(_instrument != NULL) delete _instrument;
    }
    
    void
    ArtivityLog::notifyUndoEvent(Event* e)
    {
        logEvent(as::Undo, e);
    }

    void
    ArtivityLog::notifyRedoEvent(Event* e)
    {        
        logEvent(as::Redo, e);
    }

    void
    ArtivityLog::notifyUndoCommitEvent(Event* e)
    {        
        logEvent(as::Update, e);
    }

    void
    ArtivityLog::notifyClearUndoEvent()
    {       
        logEvent(art::Close, NULL);
    }

    void
    ArtivityLog::notifyClearRedoEvent()
    {
        g_message("notifyClearRedoEvent() called");
    }

    void
    ArtivityLog::logEvent(const Resource& type, Event* e)
    {
        Activity* activity = new Activity();
        activity->setValue(rdf::_type, type);
        
        logEvent(activity, e);
    }
    
    void
    ArtivityLog::logEvent(Activity* activity, Event* e)
    {
        time_t now;
        
        time(&now);

        activity->setTime(now);
        activity->setInstrument(*_instrument);
        
        if(_target != NULL)
        {
            activity->setTarget(*_target);
        }
            
        setEventType(activity, e);
        setEventViewport(activity);

        _resourcePointers.push_back(activity);
        
        _log.push_back(*activity);
        
        processEventQueue();
    }

    void
    ArtivityLog::processEventQueue()
    {
        if(_doc == NULL)
        {
            g_error("processEventQueue: _doc is NULL");
            return;
        }
        
        //if(!zeitgeist_log_is_connected(_log))
        //{
        //    g_warning("processEventQueue: No connection to Zeitgeist log.");
        //}

        const gchar* uri = _doc->getURI();

        if(uri == NULL)
        {
            // The document is not yet persisted. Therefore we keep our events
            // in the queue until we have a valid file system path.
            return;
        }

        if(_target == NULL)
        {
            uri = g_strconcat("file://", uri, NULL);

            _target = new Resource(uri);
            _log.setTarget(*_target);
        }
        
        _log.transmit();
    
        clearPointers();
    }

    void
    ArtivityLog::clearPointers()
    {
        vector<Resource*>::iterator iter, end;
        
        for(iter = _resourcePointers.begin(), end = _resourcePointers.end() ; iter != end; ++iter) 
        {
            delete (*iter);
        }
        
        _resourcePointers.clear();
    }

/*  
    void
    ArtivityLog::logSubject(ZeitgeistSubject* s, const char* typeUri, Event* e, const gchar* subjectUri, const gchar* originUri, gint64 timestamp)
    {
        if(_doc == NULL)
        {
            g_error("logSubject: _doc is NULL");
            return;
        }
        
        if(subjectUri != NULL)
        {
            zeitgeist_subject_set_uri(s, subjectUri);
            zeitgeist_subject_set_current_uri(s, subjectUri);
        }

        if(originUri != NULL)
        {
            zeitgeist_subject_set_origin(s, originUri);
        }
        
        ZeitgeistEvent* event = zeitgeist_event_new_full(
            typeUri,
            ZEITGEIST_ZG_USER_ACTIVITY,
            "application://inkscape.desktop", s, NULL);

        zeitgeist_event_set_timestamp(event, timestamp);

        
        if(event != NULL)
        {
            if(e != NULL)
            {
                GByteArray* data = getEventPayload(e);

                if(data != NULL)
                {
                    zeitgeist_event_set_payload(event, data);
                }
            }
            
            zeitgeist_log_insert_events_no_reply(_log, event, NULL);

            // TODO: Free data, xml and event in completed event handler.
            // zeitgeist_log_insert_events(_log, NULL, logSubjectComplete, event, NULL);
        }
        else
        {
            g_error("logSubject: Failed to create Zeitgeist event.");
        }
    }
*/
   
    void
    ArtivityLog::setEventViewport(Activity* activity)
    {        
        if(_desktop == NULL) return;
       
        Geom::Rect vbox = _desktop->get_display_area();

        Resource* viewbox = new Resource(UriGenerator::getUri());
        viewbox->addProperty(rdf::_type, art::Viewbox);
        viewbox->addProperty(art::left, vbox.left());
        viewbox->addProperty(art::right, vbox.right());
        viewbox->addProperty(art::top, vbox.top());
        viewbox->addProperty(art::bottom, vbox.bottom());
        viewbox->addProperty(art::zoomFactor, _desktop->current_zoom());

        activity->addProperty(art::viewbox, *viewbox);
            
        _log.addAnnotation(viewbox);
    }
   
    void
    ArtivityLog::setEventType(Activity* activity, Event* e)
    {
        if(e == NULL || e->event == NULL)
            return;
            
        if(activity->hasProperty(rdf::_type, as::Undo) || activity->hasProperty(rdf::_type, as::Redo))
            return;
        
        if(is<Inkscape::XML::EventAdd*>(e->event))
        {
            activity->setValue(rdf::_type, as::Add);
        }
        else if(is<Inkscape::XML::EventDel*>(e->event))
        {
            activity->setValue(rdf::_type, as::Delete);
        }
        else if(is<Inkscape::XML::EventChgAttr*>(e->event))
        {
            Inkscape::XML::EventChgAttr* x = as<Inkscape::XML::EventChgAttr*>(e->event);
            
            const gchar* attribute = g_quark_to_string(x->key);
            
            GString* fromValue = g_string_new("");
            g_string_append_printf(fromValue, "%s", x->oldval);
            
            GString* toValue = g_string_new("");
            g_string_append_printf(toValue, "%s", x->newval);
            
            const char* from = fromValue->str;
            const char* to = toValue->str;
            
            activity->setValue(rdf::_type, art::UpdateAttribute);
            activity->setValue(art::attribute, attribute);
            activity->setValue(art::fromValue, from);
            activity->setValue(art::toValue, to);
        }
        else if(is<Inkscape::XML::EventChgContent*>(e->event))
        {
            Inkscape::XML::EventChgContent* x = as<Inkscape::XML::EventChgContent*>(e->event);
                        
            GString* fromValue = g_string_new("");
            g_string_append_printf(fromValue, "%s", x->oldval);
            
            GString* toValue = g_string_new("");
            g_string_append_printf(toValue, "%s", x->newval);
            
            activity->setValue(rdf::_type, art::UpdateContent);
            activity->setValue(art::fromValue, fromValue->str);
            activity->setValue(art::toValue, toValue->str);
        }
        else if(is<Inkscape::XML::EventChgOrder*>(e->event))
        {
            Inkscape::XML::EventChgOrder* x = as<Inkscape::XML::EventChgOrder*>(e->event);
            
            activity->setValue(rdf::_type, as::Update);
        }
        
        setObject(activity, e);
    }
    
    void
    ArtivityLog::setObject(Activity* activity, Event* e)
    {
        if(e == NULL || e->event == NULL || e->event->repr == NULL)
            return;
        
        Node* n = e->event->repr;
        
        Resource* shape = new Resource(UriGenerator::getUri());
        shape->setValue(rdf::_type, svg::Shape);
        
        const gchar* id = n->attribute("id");
        
        if(id != NULL)
        {
            shape->setValue(svg::id, id);
        }
        
        activity->setValue(as::object, *shape);
        
        _log.addAnnotation(shape);
    }
   
    GByteArray*
    ArtivityLog::getEventPayload(Event* e)
    {
        if(e == NULL) return NULL;

        if(e->event == NULL)
        {
            g_warning("getEventPayload: e->event was NULL");
            
            return NULL;
        }
        
        GString* xml = g_string_new("");

        Geom::Rect viewbox = _desktop->get_display_area();

        // Bottom and top are switched
        g_string_append_printf(xml, "<artsvg:viewbox left=\"%f\" top=\"%f\"  right=\"%f\" bottom=\"%f\" />", viewbox.left(), viewbox.bottom(), viewbox.right(), viewbox.top());

        Inkscape::XML::Event* event = e->event;
        
        do
        {
            g_string_append_printf(xml, "%s", serializeEvent(event)->str);
            
            event = event->next;
        }
        while(event != NULL);

        GByteArray* result = g_byte_array_new_take((guint8*)xml->str, xml->len);
        
        return result;
    }
    
    GString* 
    ArtivityLog::serializeEvent(Inkscape::XML::Event* event)
    {
        GString* xml = g_string_new("");
        
        if(is<Inkscape::XML::EventAdd*>(event))
        {
            Inkscape::XML::EventAdd* e = as<Inkscape::XML::EventAdd*>(event);

            if(e->child != NULL)
            {
                const gchar* name = e->child->name();
                
                g_string_append_printf(xml, "<artsvg:addEvent>%s</artsvg:addEvent>", name);
            }
            else
            {
                g_warning("handleEvent: e->child of Inkscape::XML::EventAdd* was NULL.");
            }
        }
        else if(is<Inkscape::XML::EventDel*>(event))
        {
            Inkscape::XML::EventDel* e = as<Inkscape::XML::EventDel*>(event);

            if(e->child != NULL)
            {
                const gchar* name = e->child->name();
                
                g_string_append_printf(xml, "<artsvg:delEvent>%s</artsvg:delEvent>", name);
            }
            else
            {
                g_warning("handleEvent: e->child of Inkscape::XML::EventDel* was NULL.");
            }
        }
        else if(is<Inkscape::XML::EventChgAttr*>(event))
        {
            Inkscape::XML::EventChgAttr* e = as<Inkscape::XML::EventChgAttr*>(event);
            
            const gchar* attributeName = g_quark_to_string(e->key);

            g_string_append_printf(xml, "<artsvg:changeAttribute key=\"%s\" ", attributeName);
            
            if(e->oldval.pointer() != NULL)
            {
                g_string_append_printf(xml, "old=\"%s\" ", e->oldval);
            }
            
            if(e->newval.pointer() != NULL)
            {
                g_string_append_printf(xml, "new=\"%s\" ", e->newval);
            }
            
            g_string_append_printf(xml, "/>");
        }
        else if(is<Inkscape::XML::EventChgContent*>(event))
        {
            Inkscape::XML::EventChgContent* e = as<Inkscape::XML::EventChgContent*>(event);
            
            g_string_append_printf(xml, "<artsvg:changeContent");
            
            if(e->oldval.pointer() != NULL)
            {
                g_string_append_printf(xml, "old=\"%s\" ", e->oldval);
            }
            
            if(e->newval.pointer() != NULL)
            {
                g_string_append_printf(xml, "new=\"%s\" ", e->newval);
            }
            
            g_string_append_printf(xml, "/>");
        }
        else if(is<Inkscape::XML::EventChgOrder*>(event))
        {
            Inkscape::XML::EventChgOrder* e = as<Inkscape::XML::EventChgOrder*>(event);

            if(e->child != NULL)
            {
                const gchar* content = e->child->content();
                
                g_message("Change order. node %s ", content);
            }
            else
            {
                g_warning("handleEvent: e->child of Inkscape::XML::EventChgOrder* was NULL.");
            }
        }
        else
        {
            g_warning("handleEvent: Handled unknown event type.");
        }
        
        return xml;
    }

    template<class TEvent> bool
    ArtivityLog::is(Inkscape::XML::Event* event)
    {
        return dynamic_cast<TEvent>(event) != NULL;
    }

    template<class TEvent> TEvent
    ArtivityLog::as(Inkscape::XML::Event* event)
    {
        return dynamic_cast<TEvent>(event);
    }
    
    AttributeValueMap*
    ArtivityLog::getChangedAttributes(const char* a, const char* b)
    {       
        AttributeValueMap* attributes = new AttributeValueMap();
        
        string buffer = "";
        string key = "";
        unsigned int i = 0;
        
        while(i < strlen(a))
        {
            char c = a[i];
            
            if(c == ':')
            {
                key = buffer;
                buffer = "";
            }
            else if(c == ';')
            {
                ValueRecord r = ValueRecord();
                r.oldval = buffer;
                
                attributes->operator[](key) = r;
                buffer = "";
            }
            else
            {
                buffer += c;
            }
            
            i++;
        }
        
        buffer = "";
        key = "";
        i = 0;
        
        while(i < strlen(b))
        {
            char c = b[i];
            
            if(c == ':')
            {
                key = buffer;
                buffer = "";
            }
            else if(c == ';')
            {
                AttributeValueIterator it = attributes->find(key);
                
                if(it != attributes->end())
                {
                    ValueRecord r = attributes->operator[](key);
                    
                    if(r.oldval == buffer)
                    {
                        attributes->erase(it);
                    }
                    else
                    {
                        r.newval = buffer;
                    }
                }
                else
                {
                    ValueRecord r = ValueRecord();
                    r.oldval = "";
                    r.newval = buffer;
                    
                    attributes->operator[](key) = r;
                }

                buffer = "";
            }
            else
            {
                buffer += c;
            }
            
            i++;
        }
        
        return attributes;
    }
}

/*
  Local Variables:
  mode:c++
  c-file-style:"stroustrup"
  c-file-offsets:((innamespace . 0)(inline-open . 0)(case-label . +))
  indent-tabs-mode:nil
  fill-column:99
  End:
*/
// vim: filetype=cpp:expandtab:shiftwidth=4:tabstop=8:softtabstop=4:fileencoding=utf-8:textwidth=99 :
