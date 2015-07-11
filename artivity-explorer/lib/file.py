#! /usr/bin/python

from gi.repository import Gtk

from timer import Timer

from ontology import art

from zeitgeist.datamodel import *

import sys, math

class FileTracker:
    """
    Helper class for finding all relevant URIs for moved files.
    """

    def __init__(self, zeitgeist, on_complete_handler):
        self.uris = set()
        self.zeitgeist = zeitgeist
        self.on_complete_handler = on_complete_handler

    def get_file_uris(self, filename):
        uri = "file://" + filename

        self.uris.clear()
        self.uris.add(uri)

        self.__get_file_move_events(self.uris)

    def __get_file_move_events(self, uris):
        templates = []

        for u in uris:
            subject = Subject.new_for_values(current_uri=u)

            templates.append(Event.new_for_values(subjects=[subject], interpretation=Interpretation.MOVE_EVENT))

        self.zeitgeist.find_events_for_templates(templates, self.__on_file_uris_received, num_events=50)

    def __on_file_uris_received(self, events):
        if len(events) == 0:
            self.__on_file_uris_resolved()
        else:
            uris = set()

            for e in events:
                u = str(e.subjects[0].uri)

                if u not in self.uris:
                    uris.add(u)

                self.uris.add(u)

            self.__get_file_move_events(uris)

    def __on_file_uris_resolved(self):
        if self.on_complete_handler:
            self.on_complete_handler(self)

class FileEventLoader:
    """
    Asynchronously loads all events for a given set of subjects.
    """

    def __init__(self, zeitgeist, uris, on_received_handler, on_completed_handler, page_size=100):
        self.timer = Timer()
        self.zeitgeist = zeitgeist
        self.page_size= page_size
        self.page_size_max = page_size
        self.on_received_handler = on_received_handler
        self.on_completed_handler = on_completed_handler
        self.completed = False
        self.uris = uris
        self.templates = []
        self.editing_sessions = []
        self.first_event = 0
        self.first_edit_event = 0
        self.last_event = 0
        self.last_edit_event = 0

        for u in uris:
            subject = Subject.new_for_values(uri=u)

            self.templates.append(Event.new_for_values(subjects=[subject]))

        self.templates.append(Event.new_for_values(subject_interpretation=Interpretation.WEBSITE))

    def load_events(self):
        print
        print 'FileEventLoader: Loading events..'

        self.timer.reset()
        self.timer.start()

        # Query for the first file access event to have a reliable timerange for retrieving file events + websites.
        templates = []

        for u in self.uris:
            subject = Subject.new_for_values(uri=u)

            templates.append(Event.new_for_values(subjects=[subject]))

        self.zeitgeist.find_events_for_templates(templates, self.on_first_event_received, result_type=ResultType.LeastRecentEvents, num_events=1)
        self.zeitgeist.find_events_for_templates(templates, self.on_last_event_received, result_type=ResultType.MostRecentEvents, num_events=1)

        templates = []

        for u in self.uris:
            subject = Subject.new_for_values(uri=u)

            templates.append(Event.new_for_values(subjects=[subject], interpretation=art.EditEvent))
            templates.append(Event.new_for_values(subjects=[subject], interpretation=art.UndoEvent))
            templates.append(Event.new_for_values(subjects=[subject], interpretation=art.RedoEvent))

        self.zeitgeist.find_events_for_templates(templates, self.on_first_edit_event_received, result_type=ResultType.LeastRecentEvents, num_events=1)
        self.zeitgeist.find_events_for_templates(templates, self.on_last_edit_event_received, result_type=ResultType.MostRecentEvents, num_events=1)

    def load_editing_sessions(self):
        templates = []

        for u in self.uris:
            subject = Subject.new_for_values(uri=u)

            templates.append(Event.new_for_values(subjects=[subject], interpretation=art.EndEditingEvent))
            templates.append(Event.new_for_values(subjects=[subject], interpretation=art.BeginEditingEvent))

        self.zeitgeist.find_events_for_templates(templates, self.on_editing_sessions_received)

    def on_editing_sessions_received(self, events):
        """
        Stores the editing cycles in format (begin (min), duration (sec)).

        :param events: List of editing cycle events received from Zeitgeist.
        :return: void
        """
        end = None
        begin = None

        E = sorted(events, key=lambda event: int(event.timestamp))

        for e in E:
            if begin is None and e.interpretation == art.BeginEditingEvent:
                begin = int(e.timestamp)
            elif end is None and e.interpretation == art.EndEditingEvent:
                end = int(e.timestamp)

            if begin is not None and end is not None:
                x = (begin - self.first_event) / 60000
                y = (end - begin) / 60000

                self.editing_sessions.append((x, x + y))

                end = None
                begin = None

        if begin is not None and end is None:
            # Session without end event is interpreted as an ongoing session..
            x = int(e.timestamp)
            y = sys.maxint

            self.editing_sessions.insert(0, (x, y))

        print 'FileEventLoader: Editing cycles:', len(self.editing_sessions)

    def on_first_event_received(self, events):
        self.first_event = int(events[0].timestamp)

    def on_first_edit_event_received(self, events):
        self.first_edit_event = int(events[0].timestamp)

    def on_last_event_received(self, events):
        self.last_event = int(events[0].timestamp)

    def on_last_edit_event_received(self, events):
        self.last_edit_event = events[0]

        # Load the editing cycles when the entire time range is known.
        self.load_editing_sessions()

        zeitgeist = self.zeitgeist
        templates = self.templates
        handler = self.__on_events_received
        limit = self.page_size
        type = ResultType.MostRecentEvents

        # Save the time range to reproduce the query in case of errors..
        self.timerange = TimeRange(self.first_event, self.last_event)

        zeitgeist.find_events_for_templates(templates, handler, result_type=type, timerange=self.timerange, num_events=limit)

    def __on_events_received(self, events):
        print 'FileEventLoader:', len(events), 'received.'

        if not len(events):
            if self.on_completed_handler:
                self.timer.stop()
                print 'FileEventLoader: Done.'
                print 'FileEventLoader: Elapsed time: %ss' % self.timer.total_seconds

                self.completed = True
                self.on_completed_handler()

            return

        filtered_events = list(self.__filter_websites(events))

        if len(filtered_events) > 0 and self.on_received_handler:
            self.on_received_handler(filtered_events)

        last = int(events[len(events) - 1].timestamp)

        if self.page_size < self.page_size_max:
            self.page_size = min(self.page_size_max, self.page_size * 2)

        zeitgeist = self.zeitgeist
        templates = self.templates
        handler = self.__on_events_received
        limit = self.page_size
        type = ResultType.MostRecentEvents

        # Save the time range to reproduce the query in case of errors..
        self.timerange = TimeRange(self.first_event, last - 1)

        zeitgeist.find_events_for_templates(templates, handler, result_type=type, timerange=self.timerange, num_events=limit, error_handler=self.__on_error)

    def __on_error(self, exception):
        if self.page_size == 1:
            # We cannot load a single event, it's time to give up.
            dialog = Gtk.MessageDialog(self, 0, Gtk.MessageType.INFO, Gtk.ButtonsType.OK, exception)
            dialog.run()
            dialog.destroy()

            return

        zeitgeist = self.zeitgeist
        templates = self.templates
        handler = self.__on_events_received
        type = ResultType.MostRecentEvents

        self.page_size = math.ceil(self.page_size / 2)

        # Re-try the query with a single number of events (in case the query exceeded its size limit of 4mb)
        zeitgeist.find_events_for_templates(templates, handler, result_type=type, timerange=self.timerange, num_events=self.page_size, error_handler=self.__on_error)

    def __filter_websites(self, events):
        for e in events:
            t = (int(e.timestamp) - self.first_event) / 60000

            if self.completed:
                if e.interpretation == art.EndEditingEvent:
                    # Close an opened session..
                    self.editing_sessions[len(self.editing_sessions) - 1][1] = int(e.timestamp)
                elif e.interpretation == art.BeginEditingEvent:
                    # Open a new session..
                    self.editing_sessions.insert(0, (int(e.timestamp, sys.maxint)))

            if e.subjects[0].interpretation != Interpretation.WEBSITE:
                yield e
            else:
                for c in self.editing_sessions:
                    if c[0] <= t <= c[1]:
                        yield e
