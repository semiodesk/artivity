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
        
        logEvent(art::Open, NULL);
    }

    ArtivityLog::~ArtivityLog()
    {
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
        // Do nothing.
    }

    void
    ArtivityLog::notifyClearRedoEvent()
    {
        // Do nothing.
    }

    void
    ArtivityLog::logEvent(const Resource& type, Event* e)
    {
        if(_desktop == NULL) return;
                                
        Association* association = new Association();
        association->setAgent(_agent);

        _log.addResource(association);
                
        time_t now;
        time(&now);
        
        Activity* activity = new Activity();
        activity->setValue(rdf::_type, type);
        activity->setTime(&now);
        activity->addAssociation(association);
        
        if(activity->is(art::Update) || activity->is(art::Undo) || activity->is(art::Redo))
        {
            Geom::Rect vbox = _desktop->get_display_area();

            Viewbox* viewbox = new Viewbox();
            viewbox->setLeft(vbox.left());
            viewbox->setRight(vbox.right());
            viewbox->setTop(vbox.top());
            viewbox->setBottom(vbox.bottom());
            viewbox->setZoomFactor(_desktop->current_zoom());
        
            _log.addResource(viewbox);
            
            Generation* generation = new Generation();
            generation->setViewbox(viewbox);
            
            _log.addResource(generation);
            
            FileDataObject* generated = new FileDataObject("_:UNSET");
            //generated->addProperty(rdf::_type, prov::Collection);
            generated->addProperty(prov::generatedAtTime, activity->getTime());
            generated->setGeneration(generation);
            generated->setLastModificationTime(activity->getTime());
            
            activity->addGeneratedEntity(generated);
                            
            _log.addResource(generated);
            
            Invalidation* invalidation = new Invalidation();
            invalidation->setViewbox(viewbox);
            
            _log.addResource(invalidation);
            
            FileDataObject* invalidated = new FileDataObject("_:UNSET");
            //invalidated->addProperty(rdf::_type, prov::Collection);
            invalidated->addProperty(prov::invalidatedAtTime, activity->getTime());
            invalidated->setInvalidation(invalidation);
            
            activity->addInvalidatedEntity(invalidated);
                
            _log.addResource(invalidated);
        }
        
        // NOTE: setType() requires the Generation and Invalidation to be initialized.
        setType(activity, e);
        
        _log.push_back(activity);
        
        processEventQueue();
    }

    void
    ArtivityLog::processEventQueue()
    {
        if(_log.empty() || !_log.isConnected()) return;
        
        if(_doc == NULL)
        {
            g_error("processEventQueue: _doc is NULL");
            return;
        }

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
        }
        
        ActivityLogIterator it = _log.begin();
        
        while(it != _log.end())
        {
            Activity* activity = *it;
            
            activity->addUsedEntity(_entity);
                            
            if(activity->is(art::Update) || activity->is(art::Undo) || activity->is(art::Redo))
            {
                if(!activity->getGeneratedEntities().empty())
                {
                    Entity* e = *(activity->getGeneratedEntities().begin());
            
                    FileDataObject* generated = dynamic_cast<FileDataObject*>(e);
                    
                    if(generated != NULL)
                    {
                        stringstream generatedUri;
                        generatedUri << uri << "#" << UriGenerator::getRandomId(10);
                    
                        generated->setUri(generatedUri.str());
                        generated->setUrl(uri.c_str());
                        generated->addGenericEntity(_entity);
                    }
                }
                
                if(!activity->getInvalidatedEntities().empty())
                {
                    Entity* e = *(activity->getInvalidatedEntities().begin());
            
                    FileDataObject* invalidated = dynamic_cast<FileDataObject*>(e);
                    
                    if(invalidated != NULL)
                    {
                        stringstream invalidatedUri;
                        invalidatedUri << uri << "#" << UriGenerator::getRandomId(10);
                    
                        invalidated->setUri(invalidatedUri.str());
                        invalidated->setUrl(uri.c_str());
                        invalidated->addGenericEntity(_entity);
                    }
                }
            }
            
            it++;
        }
        
        _log.transmit();
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
            if(activity->getGeneratedEntities().empty() && activity->getInvalidatedEntities().empty())
                return;
            
            XmlElement* element = getXmlElement(e);
            
            _log.addResource(element);
               
            Inkscape::XML::EventChgAttr* x = as<Inkscape::XML::EventChgAttr*>(e->event);
            
            string newval = string(x->newval);
            string oldval = string(x->oldval);
                
            AttributeValueMap* values = getChangedAttributes(oldval, newval);
            AttributeValueIterator it = values->begin();
            
            while(it != values->end())
            {                     
                const char* name = it->first.c_str();
                
                XmlAttribute* attribute = new XmlAttribute(element, name);
                
                _log.addResource(attribute);
                
                setGeneratedValue(activity->getGeneratedEntities(), it->second->newval.c_str(), attribute);
                setInvalidatedValue(activity->getInvalidatedEntities(), it->second->oldval.c_str(), attribute);
            
                delete it->second;
                
                it++;
            }
            
            delete values;
        }
        else if(is<Inkscape::XML::EventChgContent*>(e->event))
        {
            if(activity->getGeneratedEntities().empty() && activity->getInvalidatedEntities().empty())
                return;
                
            XmlElement* element = getXmlElement(e);
            
            _log.addResource(element);
            
            Inkscape::XML::EventChgContent* x = as<Inkscape::XML::EventChgContent*>(e->event);
            
            string newval = string(x->newval);
            string oldval = string(x->oldval);
            
            setGeneratedValue(activity->getGeneratedEntities(), newval, element);
            setInvalidatedValue(activity->getInvalidatedEntities(), oldval, element);
        }
        else if(is<Inkscape::XML::EventChgOrder*>(e->event))
        {
            //Inkscape::XML::EventChgOrder* x = as<Inkscape::XML::EventChgOrder*>(e->event);
            
            activity->setValue(rdf::_type, art::Update);
        }
    }
    
    void
    ArtivityLog::setGeneratedValue(list<Entity*> entities, string value, Resource* location)
    {
        if(entities.empty())
            return;
        
        Entity* e = *(entities.begin());
        
        FileDataObject* file = dynamic_cast<FileDataObject*>(e);
        
        if(file == NULL)
            return;
        
        Generation* generation = file->getGeneration();
        
        if(generation == NULL)
            return;
        
        generation->setValue(value);
        generation->setLocation(location);
    }
    
    void
    ArtivityLog::setInvalidatedValue(list<Entity*> entities, string value, Resource* location)
    {
        if(entities.empty())
            return;
        
        Entity* e = *(entities.begin());
        
        FileDataObject* file = dynamic_cast<FileDataObject*>(e);
        
        if(file == NULL)
            return;
        
        Invalidation* invalidation = file->getInvalidation();
        
        if(invalidation == NULL)
            return;
        
        invalidation->setValue(value);
        invalidation->setLocation(location);
    }
    
    XmlElement*
    ArtivityLog::getXmlElement(Event* e)
    {
        const char* id = getXmlElementId(e);
        const char* url = _doc->getURI();
        
        if(id == NULL || url == NULL)
            return NULL;
        
        return new XmlElement(url, id);
    }
    
    const char*
    ArtivityLog::getXmlElementId(Event* e)
    {
        if(e->event->repr == NULL)
            return NULL;
        
        Node* node = e->event->repr;
        
        const gchar* id = node->attribute("id");
        
        if(id == NULL)
        {
            node = node->parent();
            
            while(id == NULL && node != NULL)
            {
                id = node->attribute("id");
            }
        }
        
        if(id != NULL)
        {
            return string(id).c_str();
        }
        
        return NULL;
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
