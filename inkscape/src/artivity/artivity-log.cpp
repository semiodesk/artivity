/*
 * Authors:
 * Sebastian Faubel <sebastian@semiodesk.com>
 * Moritz Eberl <moritz@semiodesk.com>
 *
 * Copyright (c) 2015 Authors
 *
 * Released under GNU GPL, see the file 'COPYING' for more information.
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
        _desktop = desktop;
        _document = document;
        
        _log = ActivityLog();
        _log.addAgent(new SoftwareAgent("application://inkscape.desktop"));
        
        double width = _document->getWidth().quantity;
        double height = _document->getHeight().quantity;
        const Resource* unit = getLengthUnit(_document->getWidth());
            
        if(_document->getURI() != NULL)
        {
            _filePath = string(_document->getURI());
                        
            _log.loadFile(_filePath.c_str(), width, height, unit);
            
            FileDataObject* file = _log.getFile();
            
            _activity = _log.createActivity<EditFile>();
            _activity->addUsedEntity(file);
        
            transmitQueue();
        }
        else
        {
            _filePath = "";
            
            _log.createFile(width, height, unit);
            
            _activity = _log.createActivity<CreateFile>();
        }
    }

    ArtivityLog::~ArtivityLog()
    {
        if(_document->getURI() != NULL)
        {
            time_t now;
            time(&now);
            
            _activity->setEndTime(now);
            
            _log.transmit();
        }
    }
    
    void
    ArtivityLog::notifyUndoCommitEvent(Event* e)
    {        
        logEvent(e, art::Edit);
    }
    
    void
    ArtivityLog::notifyUndoEvent(Event* e)
    {
        logEvent(e, art::Undo);
    }
    
    void
    ArtivityLog::notifyRedoEvent(Event* e)
    {        
        logEvent(e, art::Redo);
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
    ArtivityLog::logEvent(Event* e, const Resource& type)
    {
        if(e->event == NULL)
        {
            return;
        }
            
        try
        {
            time_t now;
            time(&now);
                         
            Canvas* canvas = getCanvas();
            
            Viewport* viewport = createViewport();
            
            if(is<Inkscape::XML::EventAdd*>(e->event))
            {
                logAdd(now, e, canvas, viewport);
            }
            else if(is<Inkscape::XML::EventDel*>(e->event))
            {
                logDelete(now, e, canvas, viewport);
            }
            else if(is<Inkscape::XML::EventChgContent*>(e->event))
            {
                logChangeContent(now, e, canvas, viewport);
            }
            else if(is<Inkscape::XML::EventChgAttr*>(e->event))
            {
                logChangeAttributes(now, e, canvas, viewport);
            }
            else if(is<Inkscape::XML::EventChgOrder*>(e->event))
            {
                logChangeOrder(now, e, canvas, viewport);
            }
            else if(type.Uri == art::Undo.Uri || type.Uri == art::Redo.Uri)
            {
                logUndoOrRedo(now, e, canvas, viewport, type);
            }
        
            transmitQueue();
        }
        catch(exception e)
        {
            // TODO: log error.
        }
    }
   
    void
    ArtivityLog::logUndoOrRedo(time_t time, Event* e, Canvas* canvas, Viewport* viewport, const Resource& type)
    {
        FileDataObject* file = _log.getFile();
        FileDataObject* newver = createGeneratedVersion(file, canvas);
        FileDataObject* oldver = createInvalidatedVersion(file, canvas);
        
        Generation* generation = createGeneration(newver, time, viewport, type);
        Invalidation* invalidation = createInvalidation(oldver, time, viewport, type);
    }
    
    void
    ArtivityLog::logAdd(time_t time, Event* e, Canvas* canvas, Viewport* viewport)
    {
        Inkscape::XML::EventAdd* x = as<Inkscape::XML::EventAdd*>(e->event);
        
        BoundingRectangle* bounds = createBoundingRectangle(e);
        
        FileDataObject* file = _log.getFile();
        FileDataObject* newver = createGeneratedVersion(file, canvas);
        FileDataObject* oldver = createInvalidatedVersion(file, canvas);
        
        stringstream uri;
        uri << newver->Uri << "#" << getXmlNodeId(x->child);
        
        XmlElement* element = _log.createResource<XmlElement>(uri.str().c_str());
        
        Generation* generation = createGeneration(newver, time, viewport, art::Add);
        generation->setLocation(element);
        generation->setBoundaries(bounds);
        
        Invalidation* invalidation = createInvalidation(oldver, time, viewport, art::Add);
        invalidation->setBoundaries(bounds);
    }
    
    void
    ArtivityLog::logDelete(time_t time, Event* e, Canvas* canvas, Viewport* viewport)
    {
        // Inkscape::XML::EventDel* x = as<Inkscape::XML::EventDel*>(e->event);
            
        FileDataObject* file = _log.getFile();
        FileDataObject* newver = createGeneratedVersion(file, canvas);
        FileDataObject* oldver = createInvalidatedVersion(file, canvas);
        
        Generation* generation = createGeneration(newver, time, viewport, art::Remove);
        Invalidation* invalidation = createInvalidation(oldver, time, viewport, art::Remove);
    }
    
    void
    ArtivityLog::logChangeContent(time_t time, Event* e, Canvas* canvas, Viewport* viewport)
    {
        if(e->event->repr == NULL)
        {
            // TODO: Log error.
            return;
        }
        
        Inkscape::XML::EventChgContent* x = as<Inkscape::XML::EventChgContent*>(e->event);
        
        BoundingRectangle* bounds = createBoundingRectangle(e);
        
        FileDataObject* file = _log.getFile();
        FileDataObject* newver = createGeneratedVersion(file, canvas);
        FileDataObject* oldver = createInvalidatedVersion(file, canvas);
        
        stringstream uri;
        uri << newver->Uri << "#" << getXmlNodeId(e->event->repr);
        
        XmlElement* element = _log.createResource<XmlElement>(uri.str().c_str());
         
        // NEW VALUE
        string newval = x->newval != 0 ? string(x->newval) : "";
        
        Generation* generation = createGeneration(newver, time, viewport, art::Edit);
        generation->setValue(newval);
        generation->setLocation(element);
        generation->setBoundaries(bounds);
        
        // OLD VALUE
        string oldval = x->oldval != 0 ? string(x->oldval) : "";
        
        Invalidation* invalidation = createInvalidation(oldver, time, viewport, art::Edit);
        invalidation->setValue(oldval);
        invalidation->setLocation(element);
        invalidation->setBoundaries(bounds);
    }
    
    void
    ArtivityLog::logChangeAttributes(time_t time, Event* e, Canvas* canvas, Viewport* viewport)
    {
        if(e->event->repr == NULL)
        {
            // TODO: Log error.
            return;
        }
        
        Inkscape::XML::EventChgAttr* x = as<Inkscape::XML::EventChgAttr*>(e->event);
             
        BoundingRectangle* bounds = createBoundingRectangle(e);
               
        FileDataObject* file = _log.getFile();
        FileDataObject* newver = createGeneratedVersion(file, canvas);
        FileDataObject* oldver = createInvalidatedVersion(file, canvas);
        
        string newval = x->newval != 0 ? string(x->newval) : "";
        string oldval = x->oldval != 0 ? string(x->oldval) : "";
            
        AttributeValueMap* values = getChangedAttributes(oldval, newval);
        AttributeValueIterator it = values->begin();
        
        stringstream uri;
        uri << newver->Uri << "#" << getXmlNodeId(e->event->repr);
        
        XmlElement* element = _log.createResource<XmlElement>(uri.str().c_str());
        
        while(it != values->end())
        {
            string name = it->first;
            
            stringstream atturi;
            atturi << uri.str() << "@" << name;
            
            XmlAttribute* attribute = _log.createResource<XmlAttribute>(atturi.str().c_str());
            attribute->setOwnerElement(element);
            attribute->setLocalName(name.c_str());
            
            Generation* generation = createGeneration(newver, time, viewport, art::Edit);
            generation->setValue(it->second->newval.c_str());
            generation->setLocation(attribute);
            generation->setBoundaries(bounds);
        
            Invalidation* invalidation = createInvalidation(oldver, time, viewport, art::Edit);
            invalidation->setValue(it->second->oldval.c_str());
            invalidation->setLocation(attribute);
            invalidation->setBoundaries(bounds);
        
            delete it->second;
            
            it++;
        }
        
        delete values;
    }
    
    void
    ArtivityLog::logChangeOrder(time_t time, Event* e, Canvas* canvas, Viewport* viewport)
    {                    
        // Inkscape::XML::EventChgOrder* x = as<Inkscape::XML::EventChgOrder*>(e->event);
        
        // TODO: Set the element and location.
    }
    
    FileDataObject*
    ArtivityLog::createGeneratedVersion(Entity* entity, Canvas* canvas)
    {
        FileDataObject* version = _log.createResource<FileDataObject>();
        version->setCanvas(canvas);
        version->addGenericEntity(entity);
        
        _activity->addGeneratedEntity(version);
        
        return version;
    }
    
    FileDataObject*
    ArtivityLog::createInvalidatedVersion(Entity* entity, Canvas* canvas)
    {
        FileDataObject* version = _log.createResource<FileDataObject>();
        version->setCanvas(canvas);
        version->addGenericEntity(entity);
        
        _activity->addInvalidatedEntity(version);
        
        return version;
    }
    
    Generation*
    ArtivityLog::createGeneration(Entity* entity, time_t time, Viewport* viewport, const Resource& type)
    {
        Generation* generation = _log.createResource<Generation>();
        generation->setType(type);
        generation->setTime(time);
        generation->setViewport(viewport);
        
        entity->setGeneration(generation);
        
        return generation;
    }

    Invalidation*
    ArtivityLog::createInvalidation(Entity* entity, time_t time, Viewport* viewport, const Resource& type)
    {
        Invalidation* invalidation = _log.createResource<Invalidation>();
        invalidation->setType(type);
        invalidation->setTime(time);
        invalidation->setViewport(viewport);
        
        entity->setInvalidation(invalidation);
        
        return invalidation;
    }
    
    Viewport*
    ArtivityLog::createViewport()
    {
        Rect vbox = _desktop->get_display_area();
        
        Point* p = _log.createResource<Point>();
        p->setX(vbox.left());
        p->setY(vbox.top());
        
        Viewport* viewport = _log.createResource<Viewport>();
        viewport->setPosition(p);
        viewport->setWidth(vbox.right() - vbox.left());
        viewport->setHeight(vbox.bottom() - vbox.top());
        viewport->setZoomFactor(_desktop->current_zoom());
        
        return viewport;
    }
    
    BoundingRectangle*
    ArtivityLog::createBoundingRectangle(Event* e)
    {
        const char* id = getXmlNodeId(e->event->repr);
        
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
        
        Point* p = _log.createResource<Point>();
        p->setX(x.value(doc));
        p->setY(y.value(doc));
        
        BoundingRectangle* bounds = _log.createResource<BoundingRectangle>();
        bounds->setPosition(p);
        bounds->setWidth(w.value(doc));
        bounds->setHeight(h.value(doc));
        
        return bounds;
    }
    
    Canvas*
    ArtivityLog::getCanvas()
    {
        double width = _document->getWidth().quantity;
        double height = _document->getHeight().quantity;
        const Resource* lengthUnit = getLengthUnit(_document->getWidth());
                
        _log.updateCanvas(width, height, lengthUnit);
        
        return _log.getCanvas();
    }
    
    const char*
    ArtivityLog::getXmlNodeId(Node* n)
    {
        if(n != NULL)
        {        
            const gchar* id = n->attribute("id");
            
            if(id == NULL)
            {
                n = n->parent();
                
                while(id == NULL && n != NULL)
                {
                    id = n->attribute("id");
                }
            }
            
            if(id != NULL)
            {
                return string(id).c_str();
            }
        }
        
        return NULL;
    }

    const Resource*
    ArtivityLog::getLengthUnit(const Quantity& quantity)
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
    
    void
    ArtivityLog::transmitQueue()
    { 
        // If the document has not been saved yet, we keep all 
        // pending changes in the event queue.
        if(_document->getURI() == NULL || _log.empty())
        {
            return;
        }
        
        FileDataObject* file = _log.getFile();
        
        if(file == NULL)
        {
            g_error("ERROR: File data object is not initialized.");
            return;
        }
        
        string url = string("file://") + _document->getURI();
        
        if(_filePath == "")
        {
            _filePath = string(_document->getURI());
            
            file->setUrl(url.c_str());
            
            _activity->addUsedEntity(file);
        }
        
        if(!_activity->getGeneratedEntities().empty())
        {
            Entity* e = *(_activity->getGeneratedEntities().begin());
    
            FileDataObject* newver = dynamic_cast<FileDataObject*>(e);
            
            if(newver != NULL)
            {
                stringstream uri;
                uri << url << "#" << UriGenerator::getRandomId(10);
            
                newver->setUri(uri.str());
                newver->addGenericEntity(file);
            }
        }
        
        if(!_activity->getInvalidatedEntities().empty())
        {
            Entity* e = *(_activity->getInvalidatedEntities().begin());
    
            FileDataObject* oldver = dynamic_cast<FileDataObject*>(e);
            
            if(oldver != NULL)
            {
                stringstream uri;
                uri << url << "#" << UriGenerator::getRandomId(10);
            
                oldver->setUri(uri.str());
                oldver->addGenericEntity(file);
            }
        }
        
        _log.transmit();
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
