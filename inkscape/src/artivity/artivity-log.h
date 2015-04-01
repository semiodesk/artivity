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
#include "document.h"
#include <zeitgeist.h>

namespace Inkscape
{
	/**
	 * Inkscape::ArtivityObserver - observer for tracing calls to
	 * SPDocumentUndo::undo, SPDocumentUndo::redo, SPDocumentUndo::maybe_done.
	 *
	 */
	class ArtivityLog : public UndoStackObserver
	{
	private:
			
		SPDocument* _doc;

		ZeitgeistLog* _log;

		std::vector<ZeitgeistSubject*>* _queue;
			
	public:
			
		ArtivityLog(SPDocument* doc);
			
		virtual ~ArtivityLog() { }

		void notifyUndoEvent(Event* log);
		void notifyRedoEvent(Event* log);
		void notifyUndoCommitEvent(Event* log);
		void notifyClearUndoEvent();
		void notifyClearRedoEvent();
	};

	static void zeitgeist_complete(GObject *source_object, GAsyncResult *res, gpointer user_data);
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
