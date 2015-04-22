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
#include "xml/event.h"
#include "xml/node.h"
#include "xml/attribute-record.h"

namespace Inkscape
{
	using namespace std;
	using Inkscape::Util::List;
	using Inkscape::Util::rest;
	using Inkscape::XML::AttributeRecord;
	using Inkscape::XML::Node;
	using Inkscape::XML::NodeType;
	
	ArtivityLog::ArtivityLog(SPDocument* doc) : UndoStackObserver(), _doc(doc)
	{
		g_message("ArtivityLog(SPDocument*) called; doc=%p", doc);

		_log = zeitgeist_log_new();
		_queue = new std::vector<ZeitgeistSubject*>();
	}
	
	void
	ArtivityLog::notifyUndoEvent(Event* e)
	{
		logEvent(e, "notifyUndo");
	}

	void
	ArtivityLog::notifyRedoEvent(Event* e)
	{
		logEvent(e, "notifyRedo");
	}

	void
	ArtivityLog::notifyUndoCommitEvent(Event* e)
	{
		logEvent(e, "notifyUndoCommit");
	}

	void
	ArtivityLog::notifyClearUndoEvent()
	{
		g_message("notifyClearUndoEvent() called");
	}

	void
	ArtivityLog::notifyClearRedoEvent()
	{
		g_message("notifyClearRedoEvent() called");
	}

	void
	ArtivityLog::logEvent(Event* e, const gchar* source)
	{
		if(_doc == NULL)
		{
			g_error("%s: _doc is NULL", source);
			return;
		}

		// NOTE: The current document may have not been saved yet. Therefore,
		// we create an event without a URI and push it into the queue.
		ZeitgeistSubject* subject = newSubject(e);
		
		_queue->push_back(subject);

		g_message("%s: Queued subject=%p desc=%s", source, subject, e->description.data());
		
		if(!zeitgeist_log_is_connected(_log))
		{
			g_warning("%s: No connection to Zeitgeist log. Cannot empty queue.", source);
			return;
		}

		const gchar* uri = _doc->getURI();

		if(uri == NULL)
		{
			// The document is not yet persisted. Therefore we keep our events
			// in the queue until we have a valid file system path.
			return;
		}

		uri = g_strconcat("file://", uri, NULL);
		
		// We got a saved document. Now we can empty the event queue.
		while(!_queue->empty())
		{					
			ZeitgeistSubject* s = _queue->back();
			
			logSubject(e, s, uri, uri);

			_queue->pop_back();
			
			g_message("%s: Logged subject; Queue size=%d", source, _queue->size());
		}
	}
	
	ZeitgeistSubject*
	ArtivityLog::newSubject(Event* e)
	{		
		return zeitgeist_subject_new_full(
          NULL,
		  ZEITGEIST_NFO_VISUAL,
          ZEITGEIST_NFO_FILE_DATA_OBJECT,
          "image/svg+xml", NULL, "", "net");
	}
	
	void
	ArtivityLog::logSubject(Event* e, ZeitgeistSubject* s, const gchar* subjectUri, const gchar* originUri)
	{
		if(_doc == NULL)
		{
			g_error("logSubject: _doc is NULL");
			return;
		}
		
		if(subjectUri != NULL)
		{
			zeitgeist_subject_set_uri(s, subjectUri);
			zeitgeist_subject_set_current_uri(s, subjectUri);
		}

		if(originUri != NULL)
		{
			zeitgeist_subject_set_origin(s, originUri);
		}
		
		ZeitgeistEvent* event = zeitgeist_event_new_full(
			ZEITGEIST_ZG_MODIFY_EVENT,
			ZEITGEIST_ZG_USER_ACTIVITY,
			"application://inkscape.desktop", s, NULL);
		
		if(event != NULL)
		{
			GByteArray* data = getEventPayload(e);
			
			zeitgeist_event_set_payload(event, data);

			zeitgeist_log_insert_events_no_reply(_log, event, NULL);

			// TODO: Free data, xml and event in completed event handler.
			// zeitgeist_log_insert_events(_log, NULL, logSubjectComplete, event, NULL);
		}
		else
		{
			g_error("logSubject: Failed to create Zeitgeist event.");
		}
	}
	
	GByteArray*
	ArtivityLog::getEventPayload(Event* e)
	{
		GString* xml = g_string_new("");
		
		Node* node = e->event->repr;

		g_string_append_printf(xml, "<%s", node->name());

		NodeType nodeType = node->type();
		
		if(nodeType == Inkscape::XML::ELEMENT_NODE)
		{
			List<const AttributeRecord> record = node->attributeList();

			while(record)
			{
				const gchar* key = g_quark_to_string(record->key);
				const char* value = record->value;

				g_string_append_printf(xml, " %s=\"%s\"", key, value);

				record++;
			}

			g_string_append_printf(xml, "/>");
		}
		else if(nodeType == Inkscape::XML::TEXT_NODE)
		{
			g_string_append_printf(xml, ">%s</%s>", node->content(), node->name());
		}

		g_message("getEventPayload: Type=%u ; Data=%s", e->type, xml->str);

		GByteArray* result = g_byte_array_new_take((guint8*)xml->str, xml->len);
		
		return result;
	}

	static void
	logSubjectComplete(GObject* source, GAsyncResult* result, gpointer userData)
	{
		g_message("logSubjectComplete() called");

		// TODO: Free event and subject here, if necessary.
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
