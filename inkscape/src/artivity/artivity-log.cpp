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

namespace art
{
    const char* BeginEditingEvent = "http://purl.org/ontologies/art/1.0/terms#BeginEditingEvent";
    
    const char* EndEditingEvent = "http://purl.org/ontologies/art/1.0/terms#EndEditingEvent";
    
    const char* EditEvent = "http://purl.org/ontologies/art/1.0/terms#EditEvent";
    
    const char* UndoEvent = "http://purl.org/ontologies/art/1.0/terms#UndoEvent";
    
    const char* RedoEvent = "http://purl.org/ontologies/art/1.0/terms#RedoEvent";
}

namespace Inkscape
{
    using namespace std;
    using Inkscape::Util::List;
    using Inkscape::Util::rest;
    using Inkscape::XML::AttributeRecord;
    using Inkscape::XML::Node;
    using Inkscape::XML::NodeType;
    
    ArtivityLog::ArtivityLog(SPDocument* doc, SPDesktop* desktop) : UndoStackObserver(), _doc(doc)
    {
        _desktop= desktop;

        _log = artivity::ActivityLog();
        _instrument = new artivity::Resource("application://inkscape.desktop");

        g_message("ArtivityLog(SPDocument*, SPDesktop*) called; doc=%p", doc);

        _queue = new std::vector<EventRecord>();

        //_queue->insert(_queue->begin(), { subject, art::BeginEditingEvent, NULL, timestamp});

        processEventQueue();
    }

    ArtivityLog::~ArtivityLog()
    {
        g_message("~ArtivityLog() called;");
        if( _resource != NULL )
            delete _resource;
        if(_instrument != NULL)
            delete _instrument;
    }
    
    std::string random_string( size_t length )
    {
       static const char alphanum[] =
        "0123456789"
        "abcdefghijklmnopqrstuvwxyz"; 
        std::string str = std::string();
	for( size_t i = 0; i < length; ++i)
        {
            str += alphanum[rand() % (sizeof(alphanum) -1)];
        } 
       return str;
    }
    
    void
    ArtivityLog::notifyUndoEvent(Event* e)
    {
	artivity::Undo undo = artivity::Undo(std::string("http://semiodesk.com/artivity/"+random_string(5)).c_str());
        logEvent(e, undo);
    }

    void
    ArtivityLog::notifyRedoEvent(Event* e)
    {
        logEvent(e, art::RedoEvent);
    }

    void
    ArtivityLog::notifyUndoCommitEvent(Event* e)
    {
        string uri = std::string  ("http://semiodesk.com/artivity/");
        uri.append(random_string(5));
        artivity::Update update = artivity::Update(uri.c_str());
        logEvent(e, update);

	//artivity::Update update = 
        //logEvent(e, art::EditEvent);
    }

    void
    ArtivityLog::notifyClearUndoEvent()
    {
        //ZeitgeistSubject* subject = newSubject();

        //gint64 timestamp = (gint64)(time(NULL) * 1000);

        //_queue->insert(_queue->begin(), { subject, art::EndEditingEvent, NULL, timestamp});

        processEventQueue();
    }

    void
    ArtivityLog::notifyClearRedoEvent()
    {
        g_message("notifyClearRedoEvent() called");
    }

    void
    ArtivityLog::logEvent(Event* e, artivity::Activity activity)
    {

        time_t now;
        time(&now);

        activity.setTime(now);
        activity.setInstrument(*_instrument);
        
        if( _resource != NULL)
            activity.setTarget(*_resource);

        _log.push_back(activity);
        processEventQueue();
    }

    void
    ArtivityLog::logEvent(Event* e, const char* typeUri)
    {
        // NOTE: The current document may have not been saved yet. Therefore,
        // we create an event without a URI and push it into the queue.
        //ZeitgeistSubject* subject = newSubject();


       // _queue->insert(_queue->begin(), { subject, typeUri, e, timestamp});

       // g_message("Queued subject=%p desc=%s", subject, e->description.data());
        
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
        {
        //    g_warning("processEventQueue: No connection to Zeitgeist log.");
        }

        const gchar* uri = _doc->getURI();

        if(uri == NULL)
        {
            // The document is not yet persisted. Therefore we keep our events
            // in the queue until we have a valid file system path.
            return;
        }

	if( _resource == NULL)
	{
            uri = g_strconcat("file://", uri, NULL);

            _resource = new artivity::Resource(uri);
            _log.setTarget(*_resource);
            g_message("Target set!");
	}
    	_log.transmit();    
    }
   
/* 
    ZeitgeistSubject*
    ArtivityLog::newSubject()
    {       
        return zeitgeist_subject_new_full(
          NULL,
          ZEITGEIST_NFO_VISUAL,
          ZEITGEIST_NFO_FILE_DATA_OBJECT,
          "image/svg+xml", NULL, "", "net");
    }
  */

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

        //g_message("getEventPayload: Data=%s", xml->str);

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
