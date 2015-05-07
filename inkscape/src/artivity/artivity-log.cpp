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

namespace art
{
	const char* BeginEditingEvent = "http://purl.org/ontologies/art/1.0/terms#BeginEditingEvent";
	
	const char* EndEditingEvent = "http://purl.org/ontologies/art/1.0/terms#EndEditingEvent";
	
	const char* EditEvent = "http://purl.org/ontologies/art/1.0/terms#EditEvent";
	
	const char* UndoEvent = "http://purl.org/ontologies/art/1.0/terms#UndoEvent";
	
	const char* RedoEvent = "http://purl.org/ontologies/art/1.0/terms#RedoEvent";
}

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
		_queue = new std::vector<EventRecord>();

		ZeitgeistSubject* subject = newSubject();
		
		_queue->push_back({ subject, art::BeginEditingEvent, NULL });

		processEventQueue();
	}

	ArtivityLog::~ArtivityLog()
	{
		g_message("~ArtivityLog() called;");
	}
	
	void
	ArtivityLog::notifyUndoEvent(Event* e)
	{
		logEvent(e, art::UndoEvent);
	}

	void
	ArtivityLog::notifyRedoEvent(Event* e)
	{
		logEvent(e, art::RedoEvent);
	}

	void
	ArtivityLog::notifyUndoCommitEvent(Event* e)
	{
		logEvent(e, art::EditEvent);
	}

	void
	ArtivityLog::notifyClearUndoEvent()
	{
		ZeitgeistSubject* subject = newSubject();
		
		_queue->push_back({ subject, art::EndEditingEvent, NULL });

		processEventQueue();
	}

	void
	ArtivityLog::notifyClearRedoEvent()
	{
		g_message("notifyClearRedoEvent() called");
	}

	void
	ArtivityLog::logEvent(Event* e, const char* typeUri)
	{
		// NOTE: The current document may have not been saved yet. Therefore,
		// we create an event without a URI and push it into the queue.
		ZeitgeistSubject* subject = newSubject();
		
		_queue->push_back({ subject, typeUri, e });

		g_message("Queued subject=%p desc=%s", subject, e->description.data());
		
		processEventQueue();
	}

	void
	ArtivityLog::processEventQueue()
	{
		if(_doc == NULL)
		{
			g_error("processEventQueue: _doc is NULL");
			return;
		}
		
		if(!zeitgeist_log_is_connected(_log))
		{
			g_warning("processEventQueue: No connection to Zeitgeist log.");
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
			EventRecord r = _queue->back();
			
			logSubject(r.subject, r.eventType, r.event, uri, uri);

			_queue->pop_back();
			
			g_message("processEventQueue: Logged subject; Queue size=%lu", _queue->size());
		}
	}
	
	ZeitgeistSubject*
	ArtivityLog::newSubject()
	{		
		return zeitgeist_subject_new_full(
          NULL,
		  ZEITGEIST_NFO_VISUAL,
          ZEITGEIST_NFO_FILE_DATA_OBJECT,
          "image/svg+xml", NULL, "", "net");
	}
	
	void
	ArtivityLog::logSubject(ZeitgeistSubject* s, const char* typeUri, Event* e, const gchar* subjectUri, const gchar* originUri)
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
			typeUri,
			ZEITGEIST_ZG_USER_ACTIVITY,
			"application://inkscape.desktop", s, NULL);
		
		if(event != NULL)
		{
			if(e != NULL)
			{
				GByteArray* data = getEventPayload(e);

				if(data != NULL)
				{
					zeitgeist_event_set_payload(event, data);
				}
			}
			
			zeitgeist_log_insert_events_no_reply(_log, event, NULL);

			// TODO: Free data, xml and event in completed event handler.
			// zeitgeist_log_insert_events(_log, NULL, logSubjectComplete, event, NULL);
		}
		else
		{
			g_error("logSubject: Failed to create Zeitgeist event.");
		}
	}

	static void
	logSubjectComplete(GObject* source, GAsyncResult* result, gpointer userData)
	{
		g_message("logSubjectComplete() called");

		// TODO: Free event and subject here, if necessary.
	}
	
	GByteArray*
	ArtivityLog::getEventPayload(Event* e)
	{
		if(e == NULL) return NULL;

		if(e->event == NULL)
		{
			g_warning("getEventPayload: e->event was NULL");
			return NULL;
		}
		
		GString* xml = g_string_new("");
		
		Inkscape::XML::Event* event = e->event;
		
		do
		{
			g_string_append_printf(xml, "%s", serializeEvent(event)->str);
			
			event = event->next;
		}
		while(event != NULL);
		
		//g_message("getEventPayload: Data=%s", xml->str);

		GByteArray* result = g_byte_array_new_take((guint8*)xml->str, xml->len);
		
		return result;
	}

	GString* 
	ArtivityLog::serializeEvent(Inkscape::XML::Event* event)
	{
		GString* xml = g_string_new("");
		
		if(is<Inkscape::XML::EventAdd*>(event))
		{
			Inkscape::XML::EventAdd* e = as<Inkscape::XML::EventAdd*>(event);

			if(e->child != NULL)
			{
				const gchar* name = e->child->name();
				
				g_string_append_printf(xml, "<artsvg:addEvent>%s</artsvg:addEvent>", name);
			}
			else
			{
				g_warning("handleEvent: e->child of Inkscape::XML::EventAdd* was NULL.");
			}
		}
		else if(is<Inkscape::XML::EventDel*>(event))
		{
			Inkscape::XML::EventDel* e = as<Inkscape::XML::EventDel*>(event);

			if(e->child != NULL)
			{
				const gchar* name = e->child->name();
				
				g_string_append_printf(xml, "<artsvg:delEvent>%s</artsvg:delEvent>", name);
			}
			else
			{
				g_warning("handleEvent: e->child of Inkscape::XML::EventDel* was NULL.");
			}
		}
		else if(is<Inkscape::XML::EventChgAttr*>(event))
		{
			Inkscape::XML::EventChgAttr* e = as<Inkscape::XML::EventChgAttr*>(event);
			
			const gchar* attributeName = g_quark_to_string(e->key);

			g_string_append_printf(xml, "<artsvg:changeAttribute key=\"%s\" ", attributeName);
			
			if(e->oldval.pointer() != NULL)
			{
				g_string_append_printf(xml, "old=\"%s\" ", e->oldval);
			}
			
			if(e->newval.pointer() != NULL)
			{
				g_string_append_printf(xml, "new=\"%s\" ", e->newval);
			}
			
			g_string_append_printf(xml, "/>");
		}
		else if(is<Inkscape::XML::EventChgContent*>(event))
		{
			Inkscape::XML::EventChgContent* e = as<Inkscape::XML::EventChgContent*>(event);
			
			g_string_append_printf(xml, "<artsvg:changeContent");
			
			if(e->oldval.pointer() != NULL)
			{
				g_string_append_printf(xml, "old=\"%s\" ", e->oldval);
			}
			
			if(e->newval.pointer() != NULL)
			{
				g_string_append_printf(xml, "new=\"%s\" ", e->newval);
			}
			
			g_string_append_printf(xml, "/>");
		}
		else if(is<Inkscape::XML::EventChgOrder*>(event))
		{
			Inkscape::XML::EventChgOrder* e = as<Inkscape::XML::EventChgOrder*>(event);

			if(e->child != NULL)
			{
				const gchar* content = e->child->content();
				
				g_message("Change order. node %s ", content);
			}
			else
			{
				g_warning("handleEvent: e->child of Inkscape::XML::EventChgOrder* was NULL.");
			}
		}
		else
		{
			g_warning("handleEvent: Handled unknown event type.");
		}
		
		return xml;
	}

	template<class TEvent> bool
	ArtivityLog::is(Inkscape::XML::Event* event)
	{
		return dynamic_cast<TEvent>(event) != NULL;
	}

	template<class TEvent> TEvent
	ArtivityLog::as(Inkscape::XML::Event* event)
	{
		return dynamic_cast<TEvent>(event);
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
