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
    using Geom::Rect;
    using Geom::OptRect;
    using Inkscape::Util::List;
    using Inkscape::Util::rest;
    using Inkscape::Util::Unit;
    using Inkscape::Util::Quantity;
    using Inkscape::XML::AttributeRecord;
    using Inkscape::XML::Node;
    using Inkscape::XML::NodeType;
    
    ArtivityLog::ArtivityLog(SPDocument* document, SPDesktop* desktop) : UndoStackObserver()
    {
        _document = document;
        _desktop = desktop;

        _log = ActivityLog();
        _log.addAgent(new SoftwareAgent("application://inkscape.desktop"));
    
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
        if(_desktop == NULL)
            return;
        
        try
        {
            Activity* activity = _log.createActivity(type);
                    
            if(activity->is(art::Update) || activity->is(art::Undo) || activity->is(art::Redo))
            {
                Quantity width = _document->getWidth();
                Quantity height = _document->getHeight();
                
                CoordinateSystem* coordinateSystem = new CoordinateSystem();
                coordinateSystem->setCoordinateDimension(2);
                coordinateSystem->setTransformationMatrix("[1 0 0; 0 1 0; 0 0 0]");
                
                _log.addResource(coordinateSystem);
                
                // TODO: Get the one referencing to the actual file to reduce data overhead.
                Canvas* canvas = new Canvas();
                canvas->setWidth(width.quantity);
                canvas->setHeight(height.quantity);
                canvas->setLengthUnit(getUnit(width));
                canvas->setCoordinateSystem(coordinateSystem);
                
                _log.addResource(canvas);
                
                Rect vbox = _desktop->get_display_area();
      
                Point* p = new Point();
                p->setX(vbox.left());
                p->setY(vbox.top());
                
                _log.addResource(p);
                
                Viewport* viewport = new Viewport();
                viewport->setPosition(p);
                viewport->setWidth(vbox.right() - vbox.left());
                viewport->setHeight(vbox.bottom() - vbox.top());
                viewport->setZoomFactor(_desktop->current_zoom());
            
                _log.addResource(viewport);
                
                Generation* generation = new Generation();
                generation->setViewport(viewport);
                
                _log.addResource(generation);
                
                FileDataObject* generated = new FileDataObject("_:UNSET");
                //generated->addProperty(rdf::_type, prov::Collection);
                generated->setCanvas(canvas);
                generated->setGeneration(generation);
                generated->setLastModificationTime(activity->getTime());
                generated->setGenerationTime(activity->getTime());
                
                activity->addGeneratedEntity(generated);
                                
                _log.addResource(generated);
                
                Invalidation* invalidation = new Invalidation();
                invalidation->setViewport(viewport);
                
                _log.addResource(invalidation);
                
                FileDataObject* invalidated = new FileDataObject("_:UNSET");
                //invalidated->addProperty(rdf::_type, prov::Collection);
                invalidated->setCanvas(canvas);
                invalidated->setInvalidation(invalidation);
                invalidated->setInvalidationTime(activity->getTime());
                
                activity->addInvalidatedEntity(invalidated);
                    
                _log.addResource(invalidated);
            }
            
            // NOTE: setType() requires the Generation and Invalidation to be initialized.
            setType(activity, e);
            
            processEventQueue();
        }
        catch(exception e)
        {
            g_error("exception caught.");
        }
    }

    void
    ArtivityLog::processEventQueue()
    {
        if(_log.empty() || !_log.connected()) return;
        
        if(_document == NULL)
        {
            g_error("processEventQueue: _doc is NULL");
            return;
        }

        if(_document->getURI() == NULL)
        {
            // The document is not yet persisted. Therefore we keep our events
            // in the queue until we have a valid file system path.
            return;
        }
        
        string uri = "file://" + string(_document->getURI());
        
        if(_entity == NULL)
        {
            // NOTE: We do not know the actual URI of the file data object here.
            // Therefore we add it as a variable and let the server replace
            // the URI with the one it knows. This is hacky, but will eventually
            // not be necessary once the client API is being stabilized.
            string var = "?:" + uri;
            
            FileDataObject* file = new FileDataObject(var.c_str());
            file->setUrl(uri.c_str());
            
            _log.addResource(file);
            
            _entity = file;
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
                        
            Inkscape::XML::EventChgAttr* x = as<Inkscape::XML::EventChgAttr*>(e->event);
            
            string newval = x->newval != 0 ? string(x->newval) : "";
            string oldval = x->oldval != 0 ? string(x->oldval) : "";
                
            AttributeValueMap* values = getChangedAttributes(oldval, newval);
            AttributeValueIterator it = values->begin();
            
            XmlElement* element = getXmlElement(e);
               
            BoundingRectangle* bounds = getBoundingRectangle(e);
            
            while(it != values->end())
            {                     
                const char* name = it->first.c_str();
                
                XmlAttribute* attribute = new XmlAttribute(element, name);
                
                _log.addResource(attribute);
                
                setGeneratedValue(activity->getGeneratedEntities(), it->second->newval.c_str(), attribute, bounds);
                setInvalidatedValue(activity->getInvalidatedEntities(), it->second->oldval.c_str(), attribute, bounds);
            
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
            
            BoundingRectangle* bounds = getBoundingRectangle(e);
                    
            Inkscape::XML::EventChgContent* x = as<Inkscape::XML::EventChgContent*>(e->event);
            
            string newval = x->newval != 0 ? string(x->newval) : "";
            string oldval = x->oldval != 0 ? string(x->oldval) : "";
            
            setGeneratedValue(activity->getGeneratedEntities(), newval, element, bounds);
            setInvalidatedValue(activity->getInvalidatedEntities(), oldval, element, bounds);
        }
        else if(is<Inkscape::XML::EventChgOrder*>(e->event))
        {
            //Inkscape::XML::EventChgOrder* x = as<Inkscape::XML::EventChgOrder*>(e->event);
            
            activity->setValue(rdf::_type, art::Update);
        }
    }
    
    void
    ArtivityLog::setGeneratedValue(list<Entity*> entities, string value, Resource* location, BoundingRectangle* bounds)
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
        
        if(bounds)
        {
            generation->setBoundaries(bounds);
        }
    }
    
    void
    ArtivityLog::setInvalidatedValue(list<Entity*> entities, string value, Resource* location, BoundingRectangle* bounds)
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
        
        if(bounds)
        {
            invalidation->setBoundaries(bounds);
        }
    }
    
    BoundingRectangle*
    ArtivityLog::getBoundingRectangle(Event* e)
    {
        const char* id = getXmlElementId(e);
        
        SPObject* obj = _document->getObjectById(id);
        
        if(!obj) return NULL;
            
        SPItem* item = dynamic_cast<SPItem*>(obj);

        if(!item) return NULL;
                
        OptRect rect = item->documentVisualBounds();
        
        if(!rect) return NULL;
        
        const Unit* svg = &(_document->getSVGUnit());
        
        Quantity x = Quantity(rect->min()[0], svg);
        Quantity y = Quantity(rect->min()[1], svg);
        Quantity w = Quantity(rect->dimensions()[0], svg);
        Quantity h = Quantity(rect->dimensions()[1], svg);
        
        const Unit* doc = _document->getWidth().unit;
        
        Point* p = new Point();
        p->setX(x.value(doc));
        p->setY(y.value(doc));
        
        _log.addResource(p);
        
        BoundingRectangle* bounds = new BoundingRectangle();
        bounds->setPosition(p);
        bounds->setWidth(w.value(doc));
        bounds->setHeight(h.value(doc));
        
        _log.addResource(bounds);
        
        return bounds;
    }
    
    XmlElement*
    ArtivityLog::getXmlElement(Event* e)
    {
        const char* id = getXmlElementId(e);
        const char* url = _document->getURI();
        
        if(id == NULL || url == NULL)
            return NULL;
        
        XmlElement* element = new XmlElement(url, id);
        
        _log.addResource(element);
                        
        return element;
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
    
    Node*
    ArtivityLog::getXmlElementNode(Event* e)
    {
        if(e->event->repr == NULL)
            return NULL;
        
        return e->event->repr;
    }

    const Resource*
    ArtivityLog::getUnit(const Quantity& quantity)
    {        
        const Unit* unit = quantity.unit;
                    
        if(unit->abbr == "px")
        {
            return &(art::px);
        }
        else if(unit->abbr == "m")
        {
            return &(art::m);
        }
        else if(unit->abbr == "cm")
        {
            return &(art::cm);
        }
        else if(unit->abbr == "mm")
        {
            return &(art::mm);
        }
        else if(unit->abbr == "in")
        {
            return &(art::in);
        }
        else if(unit->abbr == "ft")
        {
            return &(art::ft);
        }
        else
        {
            return NULL;
        }
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
