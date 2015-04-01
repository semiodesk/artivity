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
#include "event.h"

namespace Inkscape
{
	ArtivityLog::ArtivityLog(SPDocument* doc) : UndoStackObserver(), _doc(doc)
	{
		g_message("ArtivityLog(SPDocument*) called; doc=%p", doc);

		_log = zeitgeist_log_new();
		_queue = new std::vector<ZeitgeistSubject*>();
	}
	
	void
	ArtivityLog::notifyUndoEvent(Event* log)
	{
		g_message("notifyUndoEvent(Event*) called; log=%p", log->event);
	}

	void
	ArtivityLog::notifyRedoEvent(Event* log)
	{
		g_message("notifyRedoEvent(Event*) called; log=%p", log->event);
	}

	void
	ArtivityLog::notifyUndoCommitEvent(Event* log)
	{
		g_message("notifyUndoCommitEvent(Event*) called; log=%p", log->event);

		if(_doc == NULL) return;

		const char* xml = "";
		
		// Create an event without an origin and push it into the queue.
		ZeitgeistSubject* subject = zeitgeist_subject_new_full(
          NULL,
		  ZEITGEIST_ZG_MODIFY_EVENT,
          ZEITGEIST_ZG_USER_ACTIVITY,
          "image/svg+xml",
          NULL,
          xml,
          "net");
		
		_queue->push_back(subject);

		if(!zeitgeist_log_is_connected(_log)) return;
		
		g_message("notifyUndoCommitEvent(Event*) queued subject; subject=%p", subject);

		// Determine if the document has been saved..
		const gchar* uri = _doc->getURI();

		if(uri == NULL) return;	

		uri = g_strconcat("file:/", uri, NULL);
		
		g_message("notifyUndoCommitEvent(Event*) uri=%s", uri);
		
		// ..if so, push all events in the queue into Zeitgeist.
		while(!_queue->empty())
		{					
			ZeitgeistSubject* s = _queue->back();
			zeitgeist_subject_set_uri(s, uri);
			zeitgeist_subject_set_current_uri(s, uri);
			zeitgeist_subject_set_origin(s, uri);

			g_message("notifyUndoCommitEvent(Event*): uri=%s", zeitgeist_subject_get_uri(s));

			ZeitgeistEvent* event = zeitgeist_event_new_full(
    			ZEITGEIST_ZG_MODIFY_EVENT,
  				ZEITGEIST_ZG_USER_ACTIVITY,
    			"application://inkscape.desktop",
    			s,
				NULL);
			
			zeitgeist_log_insert_events(_log, NULL, zeitgeist_complete, NULL, event, NULL);
			
			_queue->pop_back();

			g_message("notifyUndoCommitEvent(Event*): logged event; queue.size(): %d", _queue->size());
		}
	}

	void
	ArtivityLog::notifyClearUndoEvent()
	{
		g_message("notifyClearUndoEvent(Event*) called");
	}

	void
	ArtivityLog::notifyClearRedoEvent()
	{
		g_message("notifyClearRedoEvent(Event*) called");
	}

	static void
	zeitgeist_complete(GObject *source_object, GAsyncResult *res, gpointer user_data)
	{
		g_message("zeitgeist_complete(..) called");
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
