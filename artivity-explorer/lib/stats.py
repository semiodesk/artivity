#! /usr/bin/python

from xml.dom import minidom

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

	def __init__(self, zeitgeist, uris, on_property_changed):
		self.uris = uris 
		self.zeitgeist = zeitgeist
		self.raise_property_changed = on_property_changed

	def get_templates(self, subjects, interpretation):
		templates = []

		for uri in self.uris:
			subject = Subject.new_for_values(uri=uri)

			templates.append(Event.new_for_values(subjects=[subject], interpretation=interpretation))

		return templates

	def update(self):
		begin_edit = self.get_templates(self.uris, art.BeginEditingEvent)
		self.zeitgeist.find_event_ids_for_templates(begin_edit, self.on_begin_edit_ids_received, num_events=0)

		edit = self.get_templates(self.uris, art.EditEvent)
		self.zeitgeist.find_event_ids_for_templates(edit, self.on_edit_ids_received, num_events=0)

		undo = self.get_templates(self.uris, art.UndoEvent)
		self.zeitgeist.find_event_ids_for_templates(undo, self.on_undo_ids_received, num_events=0)

		redo = self.get_templates(self.uris, art.RedoEvent)
		self.zeitgeist.find_event_ids_for_templates(redo, self.on_redo_ids_received, num_events=0)

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

	def __init__(self):
		self.layer_count = 0
		self.group_count = 0
		self.element_count = 0
		self.masked_count = 0
		self.clipped_count = 0
		self.palette = set()

	def parse(self, filename):
		dom = minidom.parse(filename)

		for node in dom.childNodes:
			# Parse only the child nodes of the SVG root node
			self.parse_nodes(node.childNodes)

	def parse_nodes(self, nodelist):
		skip = ['metadata','sodipodi:namedview']

		for node in nodelist:
			# Only element and text nodes have a tag name
			if not node.nodeType == node.ELEMENT_NODE: continue

			# Skip tags that are no SVG elements.
			if node.tagName in skip: continue

			count = node.tagName != 'defs'
			
			self.parse_node(node, count)

	def parse_node(self, node, count):
		if node.tagName == 'g':
			mode = getattr(node, 'inkscape:groupmode')

			if mode == 'layer':
				self.layer_count += 1
			else:
				self.group_count += 1
		
		else:
			if hasattr(node, 'style'):
				style = getattr(node, 'style')

				if style.endswith(';'):
					style = style[:-1]

				try:
					attrs = dict(a.split(':') for a in style.split(';'))
				
					if 'fill' in attrs:
						self.add_colour(attrs['fill'])
					if 'stroke' in attrs:
						self.add_colour(attrs['stroke'])
					if 'stop-color' in attrs:
						self.add_colour(attrs['stop-color'])
				except ValueError:
					print style

			clip = getattr(node, 'clip-path')
			mask = getattr(node, 'mask')

			if clip:
				self.clipped_count += 1
			elif mask and mask != 'none':
				self.masked_count += 1

			if count:
				self.element_count += 1

		for child in node.childNodes:
			# Only element and text nodes have a tag name
			if not child.nodeType == node.ELEMENT_NODE: continue

			self.parse_node(child, count)

	def add_colour(self, colour):
		if colour and colour.startswith('#'):
			self.palette.add(colour)

	def get_layer_count(self):
		return str(self.layer_count)

	def get_group_count(self):
		return str(self.group_count)

	def get_element_count(self):
		return str(self.element_count)

	def get_element_masked_count(self):
		return str(self.masked_count)

	def get_element_clipped_count(self):
		return str(self.clipped_count)

	def get_element_copied_count(self):
		return str('#')

	def get_element_duplicated_count(self):
		return str('#')

	def get_colour_count(self):
		return str(len(self.palette))

	def get_colour_palette(self):
		return self.palette

def getattr(node, name):
	if name in node.attributes.keys():
		return node.attributes[name].value
	else:	
		return None

def hasattr(node, name):
	return name in node.attributes.keys()

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
