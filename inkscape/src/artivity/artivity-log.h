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
#include <ctime>

// TODO: Create artivity.h
#include <artivity-client/ActivityLog.h>
#include <artivity-client/SoftwareAgent.h>
#include <artivity-client/SoftwareAssociation.h>
#include <artivity-client/Viewbox.h>
#include <artivity-client/Entities/FileDataObject.h>

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
        
        SPDocument* _doc;

        SPDesktop* _desktop;

        artivity::ActivityLog _log;	

        template<class TEvent> bool is(Inkscape::XML::Event* event);
            
        template<class TEvent> TEvent as(Inkscape::XML::Event* event);
        
        artivity::SoftwareAgent* _agent = NULL;
        
        artivity::FileDataObject* _entity = NULL;
        
        std::vector<artivity::Resource*> _resourcePointers;
   
    protected:
           
        void logEvent(const artivity::Resource& type, Event* e);
 
        void logEvent(artivity::Activity* activity, Event* e);
        
        void setType(artivity::Activity* activity, Event* e);
        
        void setObject(artivity::Activity* activity, Event* e);
        
        void processEventQueue();
        
        artivity::SoftwareAgent* getSoftwareAgent();
        
        artivity::SoftwareAssociation* getSoftwareAssociation();
        
        artivity::Viewbox* getViewbox();
                
        AttributeValueMap* getChangedAttributes(string a, string b);
        
        GByteArray* getEventPayload(Event* e);

        GString* serializeEvent(Inkscape::XML::Event* event);

        void clearPointers();
    
    public:
        
        ArtivityLog(SPDocument* doc, SPDesktop* desktop);
        
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
