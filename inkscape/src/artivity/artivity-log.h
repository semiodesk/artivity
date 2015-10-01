/*
 * Authors:
 * Sebastian Faubel <sebastian@semiodesk.com>
 * Moritz Eberl <moritz@semiodesk.com>
 *
 * Copyright (c) 2015 Authors
 *
 * Released under GNU GPL, see the file 'COPYING' for more information
 */

#ifndef SEEN_INKSCAPE_ARTIVITY_OBSERVER_H
#define SEEN_INKSCAPE_ARTIVITY_OBSERVER_H

#include "undo-stack-observer.h"
#include "desktop.h"
#include "document.h"
#include "event.h"
#include "xml/event.h"
#include "xml/node.h"
#include "xml/attribute-record.h"
#include "util/units.h"
#include "sp-item.h"
#include "2geom/coord.h"
#include <ctime>
#include <signal.h>
#include <stdio.h>
#include <unistd.h>

#include <artivity-client/artivity.h>

class SPDesktop;

namespace Inkscape
{
    struct EventRecord
    {
        const char* eventType;

        Event* event;

        gint64 timestamp;
    };
    
    struct ValueRecord
    {
        string oldval;
        string newval;
    };
    
    typedef map<string, ValueRecord*> AttributeValueMap;
    
    typedef map<string, ValueRecord*>::iterator AttributeValueIterator;
 
    /**
     * Inkscape::ArtivityObserver - observer for tracing calls to
     * SPDocumentUndo::undo, SPDocumentUndo::redo, SPDocumentUndo::maybe_done.
     *
     */   
    class ArtivityLog : public UndoStackObserver, public sigc::trackable
    {
    private:
        
        SPDocument* _document;

        SPDesktop* _desktop;

        artivity::ActivityLog _log;	

        artivity::SoftwareAgent* _agent = NULL;
        
        artivity::FileDataObject* _entity = NULL;
        
        artivity::Canvas* _canvas = NULL;
        
        template<class TEvent> bool is(Inkscape::XML::Event* event);
            
        template<class TEvent> TEvent as(Inkscape::XML::Event* event);
   
    protected:
           
        void logEvent(const artivity::Resource& type, Event* e);
        
        void setType(artivity::Activity* activity, Event* e);

        void setGeneratedValue(list<artivity::Entity*> entities, string value, artivity::Resource* location, artivity::BoundingRectangle* bounds);
        
        void setInvalidatedValue(list<artivity::Entity*> entities, string value, artivity::Resource* location, artivity::BoundingRectangle* bounds);
        
        void processEventQueue();
        
        artivity::BoundingRectangle* getBoundingRectangle(Event* e);
    
        artivity::XmlElement* getXmlElement(Event* e);
        
        const char* getXmlElementId(Event* e);
        
        Inkscape::XML::Node* getXmlElementNode(Event* e);
    
        const artivity::Resource* getUnit(const Util::Quantity& quantity);
    
        AttributeValueMap* getChangedAttributes(string a, string b);
    
    public:
        
        ArtivityLog(SPDocument* document, SPDesktop* desktop);
        
        virtual ~ArtivityLog();
        
        void notifyUndoEvent(Event* e);
        
        void notifyRedoEvent(Event* e);
        
        void notifyUndoCommitEvent(Event* e);
        
        void notifyClearUndoEvent();
        
        void notifyClearRedoEvent();
    };
}

#endif // SEEN_INKSCAPE_ARTIVITY_OBSERVER_H

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
