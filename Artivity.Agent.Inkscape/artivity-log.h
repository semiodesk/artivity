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

#include <libartivity/artivity.h>

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
        
        artivity::Activity* _activity;
        
        string _filePath;
        
        template<class TEvent> bool is(Inkscape::XML::Event* event);
            
        template<class TEvent> TEvent as(Inkscape::XML::Event* event);
        
        sigc::connection _modifiedConnection;
        
        sigc::connection _savedConnection;
        
    protected:
           
        void onSaved();
        
        void onModified();
        
        void logEvent(Event* e, const artivity::Resource& type);
        
        void logUndoOrRedo(time_t time, Event* e, artivity::Canvas* canvas, artivity::Viewport* viewport, const artivity::Resource& type);
    
        void logAdd(time_t time, Event* e, artivity::Canvas* canvas, artivity::Viewport* viewport);
        
        void logDelete(time_t time, Event* e, artivity::Canvas* canvas, artivity::Viewport* viewport);
        
        void logChangeContent(time_t time, Event* e, artivity::Canvas* canvas, artivity::Viewport* viewport);
    
        void logChangeAttributes(time_t time, Event* e, artivity::Canvas* canvas, artivity::Viewport* viewport);
        
        void logChangeOrder(time_t time, Event* e, artivity::Canvas* canvas, artivity::Viewport* viewport);
    
        artivity::Viewport* createViewport();
        
        artivity::BoundingRectangle* createBoundingRectangle(Event* e);
        
        artivity::Canvas* getCanvas();
        
        const char* getXmlNodeId(Inkscape::XML::Node* n);
    
        const artivity::Resource* getLengthUnit(const Util::Quantity& quantity);
    
        AttributeValueMap* getChangedAttributes(Event* e);
    
        void transmitQueue();
        
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
