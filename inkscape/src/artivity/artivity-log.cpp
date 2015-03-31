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
	void
	ArtivityLog::notifyUndoEvent(Event* log)
	{
		g_message("notifyUndoEvent(SPDocumentUndo::undo) called; log=%p\n", log->event);
	}

	void
	ArtivityLog::notifyRedoEvent(Event* log)
	{
		g_message("notifyRedoEvent(SPDocumentUndo::redo) called; log=%p\n", log->event);
	}

	void
	ArtivityLog::notifyUndoCommitEvent(Event* log)
	{
		g_message("notifyUndoCommitEvent(SPDocumentUndo::maybe_done) called; log=%p\n", log->event);

		// The log was not initialized properly..
		if(_doc == NULL) return;
		
		const gchar* uri = _doc->getURI();

		// The document has not yet been saved..
		if(uri == NULL) return;
		
		g_message("   %s\n", uri);
	}

	void
	ArtivityLog::notifyClearUndoEvent()
	{
		g_message("notifyClearUndoEvent(sp_document_clear_undo) called");
	}

	void
	ArtivityLog::notifyClearRedoEvent()
	{
		g_message("notifyClearRedoEvent(sp_document_clear_redo) called");
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
