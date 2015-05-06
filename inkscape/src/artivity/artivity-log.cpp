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
		logEvent(e, Undo);
	}

	void
	ArtivityLog::notifyRedoEvent(Event* e)
	{
		logEvent(e, Redo);
	}

	void
	ArtivityLog::notifyUndoCommitEvent(Event* e)
	{
		logEvent(e, UndoCommit);
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
	ArtivityLog::logEvent(Event* e, UndoType type)
	{
		if(_doc == NULL)
		{
			g_error("%s: _doc is NULL", type );
			return;
		}

		// NOTE: The current document may have not been saved yet. Therefore,
		// we create an event without a URI and push it into the queue.
		ZeitgeistSubject* subject = newSubject(e);
		
		_queue->push_back(subject);

		g_message("Queued subject=%p desc=%s", subject, e->description.data());
		
		if(!zeitgeist_log_is_connected(_log))
		{
			g_warning("No connection to Zeitgeist log. Cannot empty queue.");
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
			
			logSubject(e, s, uri, uri, type);

			_queue->pop_back();
			
			g_message("Logged subject; Queue size=%d", _queue->size());
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
	ArtivityLog::logSubject(Event* e, ZeitgeistSubject* s, const gchar* subjectUri, const gchar* originUri, UndoType type)
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
			GByteArray* data = getEventPayload(e, type);
			
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

	GString* 
	ArtivityLog::handleEvent(Inkscape::XML::Event* event)
	{

		GString* xml = g_string_new("");
		Inkscape::XML::EventAdd* eventAdd = dynamic_cast<Inkscape::XML::EventAdd*>(event);
		if( eventAdd != NULL)
		{
			g_string_append_printf(xml, "<artsvg:addEvent>%s</artsvg:addEvent>", eventAdd->child->name());
		}


		Inkscape::XML::EventDel* eventDel = dynamic_cast<Inkscape::XML::EventDel*>(event);
		if( eventDel != NULL)
		{
			g_string_append_printf(xml, "<artsvg:delEvent>%s</artsvg:delEvent>", eventDel->child->name());
		}


		Inkscape::XML::EventChgAttr* eventAttr = dynamic_cast<Inkscape::XML::EventChgAttr*>(event);
		if( eventAttr != NULL)
		{
			const gchar* attributeName = g_quark_to_string(eventAttr->key);

			g_string_append_printf(xml, "<artsvg:changeAttribute key=%s ", attributeName );
			if( eventAttr->oldval.pointer() != NULL )
			{
				g_string_append_printf(xml, "old=%s ", eventAttr->oldval);
			}
			if( eventAttr->newval.pointer() != NULL )
			{
				g_string_append_printf(xml, "new=%s ", eventAttr->newval);
			}
			g_string_append_printf(xml, "/>");
		}

		Inkscape::XML::EventChgContent* eventContent = dynamic_cast<Inkscape::XML::EventChgContent*>(event);
		if( eventContent != NULL)
		{
			g_string_append_printf(xml, "<artsvg:changeContent");
			if( eventContent->oldval.pointer() != NULL )
			{
				g_string_append_printf(xml, "old=%s ", eventContent->oldval);
			}
			if( eventAttr->newval.pointer() != NULL )
			{
				g_string_append_printf(xml, "new=%s ", eventContent->newval);
			}
			g_string_append_printf(xml, "/>");
		}

		Inkscape::XML::EventChgOrder* eventOrder = dynamic_cast<Inkscape::XML::EventChgOrder*>(event);
		if( eventOrder != NULL)
		{
			g_message("Change order. node %s ", eventOrder->child->content());
		}
		return xml;
	}
	
	GByteArray*
	ArtivityLog::getEventPayload(Event* e, UndoType type)
	{
		GString* xml = g_string_new("<");
		Node* node = e->event->repr;

		const char* eventType;

		if( type == Do )
			eventType = "art:do";
		else if( type == Undo )
			eventType = "art:undo";
		else if( type == Redo )
			eventType = "art:redo";

		g_string_append_printf(xml, "%s>", eventType );

		Inkscape::XML::Event* event = e->event;
		do
		{
			g_string_append_printf(xml, "%s", handleEvent(event)->str);
			event = event->next;
		} while(event != NULL );
		
		
		g_string_append_printf(xml,"</%s>", eventType);

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
