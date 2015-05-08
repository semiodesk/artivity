#! /usr/bin/python

from zeitgeist.client import ZeitgeistClient
from zeitgeist.mimetypes import *
from zeitgeist.datamodel import *

class art:
	BeginEditingEvent = "http://purl.org/ontologies/art/1.0/terms#BeginEditingEvent"
	EndEditingEvent = "http://purl.org/ontologies/art/1.0/terms#EndEditingEvent"
	EditEvent = "http://purl.org/ontologies/art/1.0/terms#EditEvent"
	UndoEvent = "http://purl.org/ontologies/art/1.0/terms#UndoEvent"
	RedoEvent = "http://purl.org/ontologies/art/1.0/terms#RedoEvent"

class EditingStatistics:

	begin_edit_count = 0;

	edit_count = 0;

	redo_count = 0;

	undo_count = 0;

	confidence = 0.0;

	def __init__(self, zeitgeist, filename, on_property_changed):
		filename = "file://" + filename

		self.subject = Subject.new_for_values(uri=filename)
		self.zeitgeist = zeitgeist
		self.raise_property_changed = on_property_changed

	def update(self):
		begin_edit = Event.new_for_values(interpretation=art.BeginEditingEvent, subjects=[self.subject])
		self.zeitgeist.find_event_ids_for_templates([begin_edit], self.on_begin_edit_ids_received, num_events=0)

		edit = Event.new_for_values(interpretation=art.EditEvent, subjects=[self.subject])
		self.zeitgeist.find_event_ids_for_templates([edit], self.on_edit_ids_received, num_events=0)

		undo = Event.new_for_values(interpretation=art.UndoEvent, subjects=[self.subject])
		self.zeitgeist.find_event_ids_for_templates([undo], self.on_undo_ids_received, num_events=0)

		redo = Event.new_for_values(interpretation=art.RedoEvent, subjects=[self.subject])
		self.zeitgeist.find_event_ids_for_templates([redo], self.on_redo_ids_received, num_events=0)

	def on_begin_edit_ids_received(self, ids):
		self.begin_edit_count = len(ids)
		self.raise_property_changed(self)
		
	def on_edit_ids_received(self, ids):
		self.edit_count = len(ids)
		self.update_confidence()
		self.raise_property_changed(self)

	def on_undo_ids_received(self, ids):
		self.undo_count = len(ids)
		self.update_confidence()
		self.raise_property_changed(self)

	def on_redo_ids_received(self, ids):
		self.redo_count = len(ids)
		self.update_confidence()
		self.raise_property_changed(self)

	def update_confidence(self):
		if not self.edit_count == 0:
			self.confidence = float(self.edit_count - self.undo_count + self.redo_count) / self.edit_count

class CompositionStatistics:
	subject = None

	zeitgeist = None

	def __init__(self, zeitgeist, filename):
		filename = "file://" + filename

		self.subject = Subject.new_for_values(uri=filename)
		self.zeitgeist = zeitgeist

	def get_layer_count(self):
		return str(-1)

	def get_group_count(self):
		return str(-1)

	def get_object_count(self):
		return str(-1)

	def get_object_masked_count(self):
		return str(-1)

	def get_object_clipped_count(self):
		return str(-1)

	def get_object_copied_count(self):
		return str(-1)

	def get_object_duplicated_count(self):
		return str(-1)

class ColourStatistics:
	subject = None

	zeitgeist = None

	def __init__(self, zeitgeist, filename):
		filename = "file://" + filename

		self.subject = Subject.new_for_values(uri=filename)
		self.zeitgeist = zeitgeist

	def get_colour_count(self):
		return str(-1)

	def get_colour_palette(self):
		return str(-1)
