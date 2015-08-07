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

#define CURL_STATICLIB 1

#include <curl/curl.h>
#include "undo-stack-observer.h"
#include "desktop.h"
#include "document.h"
#include <zeitgeist.h>
#include "event.h"
#include "xml/event.h"
#include "xml/node.h"
#include "xml/attribute-record.h"
#include <ctime>

class SPDesktop;

namespace Inkscape
{
    /**
     * Inkscape::ArtivityObserver - observer for tracing calls to
     * SPDocumentUndo::undo, SPDocumentUndo::redo, SPDocumentUndo::maybe_done.
     *
     */

    struct EventRecord
    {
        ZeitgeistSubject* subject;
        
        const char* eventType;

        Event* event;

        gint64 timestamp;
    };
    
    class ArtivityLog : public UndoStackObserver
    {
    private:
        
        SPDocument* _doc;

        SPDesktop* _desktop;
        
        ZeitgeistLog* _log;
        
        std::vector<EventRecord>* _queue;

        template<class TEvent> bool is(Inkscape::XML::Event* event);
            
        template<class TEvent> TEvent as(Inkscape::XML::Event* event);
   

    protected:
            
        ZeitgeistSubject* newSubject();

        void logEvent(Event* e, const char* typeUri);
        
        void logSubject(ZeitgeistSubject* s, const char* typeUri, Event* e, const gchar* subjectUri, const gchar* originUri, gint64 timestamp);

        void processEventQueue();
        
        GByteArray* getEventPayload(Event* e);

        GString* serializeEvent(Inkscape::XML::Event* event);

    public:
        
        ArtivityLog(SPDocument* doc, SPDesktop* desktop);
        
        virtual ~ArtivityLog();
        
        void notifyUndoEvent(Event* e);
        
        void notifyRedoEvent(Event* e);
        
        void notifyUndoCommitEvent(Event* e);
        
        void notifyClearUndoEvent();
        
        void notifyClearRedoEvent();
    };

    static void logSubjectComplete(GObject* source, GAsyncResult* result, gpointer userData);
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
