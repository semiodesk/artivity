#! /usr/bin/python

from zeitgeist.client import ZeitgeistClient
from zeitgeist.mimetypes import *
from zeitgeist.datamodel import *

class EditingStatistics:

	subject = None

	zeitgeist = None

	def __init__(self, zeitgeist, filename):
		filename = "file://" + filename

		self.subject = Subject.new_for_values(uri=filename)
		self.zeitgeist = zeitgeist

	def get_cycle_count(self):
		return str(-1)

	def get_do_count(self):
		return str(-1)

	def get_undo_count(self):
		return str(-1)

	def get_redo_count(self):
		return str(-1)

	def get_confidence(self):
		return str(-1)

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
