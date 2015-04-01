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
		
		_queue = new std::vector<ZeitgeistEvent*>();
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

		// Create an event without an origin and push it into the queue.
		ZeitgeistEvent* event = createEvent();

		_queue->push_back(event);

		// Determine if the document has been saved..
		const gchar* uri = _doc->getURI();

		if(uri == NULL) return;	

		// ..if so, push all events in the queue into Zeitgeist.
		while(!_queue->empty())
		{
			ZeitgeistEvent* event = _queue->back();

			// TODO: Set the origin of species..
			
			// TODO: Add to zeitgeist log..
			
			_queue->pop_back();
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

	ZeitgeistEvent*
	ArtivityLog::createEvent()
	{
		return NULL;
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
