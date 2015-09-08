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
        _agent = new SoftwareAgent("application://inkscape.desktop");
        _resourcePointers = std::vector<artivity::Resource*>();

        cout << "ArtivityLog(SPDocument*, SPDesktop*) called; doc=" << doc << endl << flush;
        
        logEvent(art::Open, NULL);
    }

    ArtivityLog::~ArtivityLog()
    {
        cout << "~ArtivityLog() called;" << endl << flush;
        
        logEvent(art::Close, NULL);
                
        if(_agent != NULL) delete _agent;
        if(_entity != NULL) delete _entity;
    }
    
    void
    ArtivityLog::notifyUndoEvent(Event* e)
    {
        logEvent(art::Undo, e);
    }

    void
    ArtivityLog::notifyRedoEvent(Event* e)
    {        
        logEvent(art::Redo, e);
    }

    void
    ArtivityLog::notifyUndoCommitEvent(Event* e)
    {        
        logEvent(art::Update, e);
    }

    void
    ArtivityLog::notifyClearUndoEvent()
    {       
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
        SoftwareAssociation* agent = getSoftwareAssociation();
        
        if(agent == NULL) return;
        
        time_t now;
        time(&now);

        activity->setTime(now);
        activity->addAssociation(agent);
        setType(activity, e);
        //setObject(activity, e);

        _log.push_back(*activity);
        _resourcePointers.push_back(activity);
        
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

        if(_doc->getURI() == NULL)
        {
            // The document is not yet persisted. Therefore we keep our events
            // in the queue until we have a valid file system path.
            return;
        }

        string uri = "file://" + string(_doc->getURI());
        
        if(_entity == NULL)
        {
            _entity = new FileDataObject(uri.c_str());
            _log.setGeneratedEntity(_entity);
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
   
    SoftwareAgent*
    ArtivityLog::getSoftwareAgent()
    {
        return _agent;
    }
    
    SoftwareAssociation*
    ArtivityLog::getSoftwareAssociation()
    {
        Viewbox* viewbox = getViewbox();
        
        if(viewbox == NULL) return NULL;
        
        _log.addAnnotation(viewbox);
        //_resourcePointers.push_back(viewbox);
                
        SoftwareAssociation* association = new SoftwareAssociation();
        association->setAgent(_agent);
        association->setViewbox(viewbox);
        
        _log.addAnnotation(association);
        //_resourcePointers.push_back(association);
        
        return association;
    }
   
    Viewbox*
    ArtivityLog::getViewbox()
    {        
        if(_desktop == NULL) return NULL;
       
        Geom::Rect vbox = _desktop->get_display_area();

        Viewbox* viewbox = new Viewbox();
        viewbox->setLeft(vbox.left());
        viewbox->setRight(vbox.right());
        viewbox->setTop(vbox.top());
        viewbox->setBottom(vbox.bottom());
        viewbox->setZoomFactor(_desktop->current_zoom());
            
        return viewbox;
    }
   
    void
    ArtivityLog::setType(Activity* activity, Event* e)
    {
        if(e == NULL || e->event == NULL)
            return;
            
        if(activity->hasProperty(rdf::_type, art::Undo) || activity->hasProperty(rdf::_type, art::Redo))
            return;
        
        if(is<Inkscape::XML::EventAdd*>(e->event))
        {
            activity->setValue(rdf::_type, art::Add);
        }
        else if(is<Inkscape::XML::EventDel*>(e->event))
        {
            activity->setValue(rdf::_type, art::Delete);
        }
        else if(is<Inkscape::XML::EventChgAttr*>(e->event))
        {
            /*
            Inkscape::XML::EventChgAttr* x = as<Inkscape::XML::EventChgAttr*>(e->event);
            
            string oldval = string(x->oldval);
            string newval = string(x->newval);
            
            AttributeValueMap* values = getChangedAttributes(oldval, newval);
            AttributeValueIterator it = values->begin();
            
            while(it != values->end())
            {                     
                const char* name = it->first.c_str();
                const char* oldval = it->second->oldval.c_str();
                const char* newval = it->second->newval.c_str();
                
                Resource* m = new Resource(UriGenerator::getUri());
                m->setValue(rdf::_type, art::AttributeModification);                
                m->setValue(art::attributeName, name);
                m->setValue(art::fromValue, oldval);
                m->setValue(art::toValue, newval);
                                                
                activity->setValue(art::modification, *m);
                
                _log.addAnnotation(m);
                
                delete it->second;
                
                it++;
            }
            
            delete values;
            */
        }
        else if(is<Inkscape::XML::EventChgContent*>(e->event))
        {
            /*
            Inkscape::XML::EventChgContent* x = as<Inkscape::XML::EventChgContent*>(e->event);
                        
            const char* oldval = string(x->oldval).c_str();
            const char* newval = string(x->newval).c_str();
            
            Resource* m = new Resource(UriGenerator::getUri());
            m->setValue(rdf::_type, art::ContentModification);                
            m->setValue(art::fromValue, oldval);
            m->setValue(art::toValue, newval);
                                            
            activity->setValue(art::modification, *m);
            
            _log.addAnnotation(m);
            */
        }
        else if(is<Inkscape::XML::EventChgOrder*>(e->event))
        {
            //Inkscape::XML::EventChgOrder* x = as<Inkscape::XML::EventChgOrder*>(e->event);
            
            activity->setValue(rdf::_type, art::Update);
        }
    }
    
    void
    ArtivityLog::setObject(Activity* activity, Event* e)
    {
        /*
        if(e == NULL || e->event == NULL || e->event->repr == NULL || !e->event->repr->attribute("id"))
            return;
        
        string id = string(e->event->repr->attribute("id"));
        
        if(id.size() == 0)
            return;
        
        stringstream uri;
        uri << "file://" << _doc->getURI() << "#" << id;
                
        Resource* shape = new Resource(uri.str());
        shape->setValue(rdf::_type, svg::Shape);
        
        activity->setValue(as::object, *shape);
        
        _log.addAnnotation(shape);
        */
    }

    AttributeValueMap*
    ArtivityLog::getChangedAttributes(string a, string b)
    {       
        AttributeValueMap* result = new AttributeValueMap();
        
        string buffer = "";
        string key = "";
        unsigned int i = 0;
        
        while(i < a.size())
        {
            char c = a[i];
            
            if(c == ':')
            {
                key = buffer;
                buffer = "";
            }
            else if(c == ';' || i == a.size() - 1)
            {
                if(key != "")
                {
                    ValueRecord* r = new ValueRecord();
                    r->oldval = string(buffer);
                    r->newval = "";
                    
                    result->operator[](key) = r;
                }
                
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
        
        while(i < b.size())
        {
            char c = b[i];
            
            if(c == ':')
            {
                key = buffer;
                buffer = "";
            }
            else if(c == ';' || i == b.size() - 1)
            {
                if(key != "")
                {
                    AttributeValueIterator it = result->find(key);
                    
                    if(it != result->end())
                    {
                        ValueRecord* r = result->operator[](key);
                        
                        if(r->oldval == buffer)
                        {
                            result->erase(it);
                            
                            delete r;
                        }
                        else
                        {
                            r->newval = string(buffer);
                        }
                    }
                    else
                    {
                        ValueRecord* r = new ValueRecord();
                        r->oldval = "";
                        r->newval = string(buffer);
                        
                        result->operator[](key) = r;
                    }
                }

                buffer = "";
            }
            else
            {
                buffer += c;
            }
            
            i++;
        }
        
        return result;
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
