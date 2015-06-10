#! /usr/bin/python

from gi.repository import Gtk
from gi.repository import Gdk
from datetime import datetime

from zeitgeist.client import ZeitgeistClient
from zeitgeist.mimetypes import *
from zeitgeist.datamodel import *

from lib.stats import EditingStatistics
from lib.stats import CompositionStatistics
from lib.stats import ColourStatistics
import csv

import os, time
from stat import ST_ATIME, ST_MTIME, ST_CTIME

class ArtivityJournal(Gtk.Window):

	def __init__(self):
		Gtk.Window.__init__(self)
		
		self.subject_uris = set()
		self.set_title("Artivity Explorer")

		self.init_headerbar()
		self.init_log()
		self.init_log_view()
		self.init_stats()
		self.init_layout()

		self.connect("delete-event", Gtk.main_quit)
		self.show_all()

	def init_dark_theme(self):
		screen = Gdk.Screen.get_default()

		css = Gtk.CssProvider()
		css.load_from_path("/usr/share/themes/Adwaita/gtk-3.0/gtk-dark.css")

		context = self.get_style_context()
		context.add_provider_for_screen(screen, css, Gtk.STYLE_PROVIDER_PRIORITY_APPLICATION)

	def init_headerbar(self):
		self.open_button = Gtk.FileChooserButton()
		self.open_button.connect("file-set", self.on_file_selected)
		
		self.export_button = Gtk.Button(label="Export")
		self.export_button.connect("clicked", self.on_export_clicked)
			
		self.headerbar = Gtk.HeaderBar()
		self.headerbar.set_show_close_button(True)
		self.headerbar.props.title = self.get_title() 
		self.headerbar.pack_start(self.open_button)
		self.headerbar.pack_start(self.export_button)

		self.set_titlebar(self.headerbar)

	def on_file_selected(self, param):
		self.filename = self.open_button.get_filename()

		self.update_stats(self.filename)
		self.load_events(self.filename)	

	def init_layout(self):
		self.stack = Gtk.Stack()

		# NOTE: Deactivated transitions due to problems with some graphics drivers.
		#self.stack.set_transition_type(Gtk.StackTransitionType.SLIDE_UP_DOWN)
		#self.stack.set_transition_duration(500)

		self.stack.add_titled(self.stats_box, "stats", "Statistics")
		self.stack.add_titled(self.log_scroller, "log", "Protocol")

		self.stack_switcher = Gtk.StackSwitcher()
		self.stack_switcher.set_stack(self.stack)
		self.stack_switcher.props.halign = Gtk.Align.CENTER

		self.layout_root = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=7)
		self.layout_root.set_halign(Gtk.Align.FILL)
		self.layout_root.pack_start(self.stack_switcher, False, True, 7)
		self.layout_root.pack_start(self.stack, True, True, 0)

		self.add(self.layout_root)

	def init_stats(self):
		grid = Gtk.Grid()
		grid.set_halign(Gtk.Align.FILL)
		grid.set_row_spacing(30)
		grid.set_column_spacing(30)
		grid.set_orientation(Gtk.Orientation.VERTICAL)
		grid.attach(self.get_file_info_widget(), 0, 0, 3, 1)	
		grid.attach(self.get_file_editing_widget(), 0, 1, 1, 1)
		grid.attach(self.get_file_composition_widget(), 1, 1, 1, 1)
		grid.attach(self.get_file_colour_widget(), 2, 1, 1, 1)

		self.stats_box = Gtk.Box()
		self.stats_box.pack_start(grid, True, True, 14)

	def get_file_info_widget(self):
		file_label = Gtk.Label()
		file_label.set_halign(Gtk.Align.START)
		file_label.set_markup("<b>File:</b>")

		self.file_name = Gtk.Label()

		file_created_label = Gtk.Label()
		file_created_label.set_halign(Gtk.Align.START)
		file_created_label.set_markup("<b>Created:</b>")

		self.file_created = Gtk.Label()

		file_modified_label = Gtk.Label()
		file_modified_label.set_halign(Gtk.Align.START)
		file_modified_label.set_markup("<b>Last modified:</b>")

		self.file_modified = Gtk.Label()

		file_accessed_label = Gtk.Label()
		file_accessed_label.set_halign(Gtk.Align.START)
		file_accessed_label.set_markup("<b>Last accessed:</b>")

		self.file_accessed = Gtk.Label()

		grid = Gtk.Grid()
		grid.set_column_spacing(7)
		grid.set_row_spacing(7)

		grid.add(file_label)
		grid.attach(self.file_name, 1, 0, 6, 1)
		grid.attach(file_created_label, 0, 1, 1, 1)
		grid.attach(self.file_created, 1, 1, 1, 1)
		grid.attach(file_modified_label, 0, 2, 1, 1)
		grid.attach(self.file_modified, 1, 2, 1, 1)
		grid.attach(file_accessed_label, 0, 3, 1, 1)
		grid.attach(self.file_accessed, 1, 3, 1, 1)

		return grid

	def get_file_editing_widget(self):
		self.editing_store = Gtk.ListStore(str, str)

		label = Gtk.Label()
		label.set_halign(Gtk.Align.START)
		label.set_markup("<b>EDITING</b>")

		col0 = Gtk.TreeViewColumn("", Gtk.CellRendererText(), text=0)
		col0.set_expand(True)
		col0.set_min_width(150)
		col1 = Gtk.TreeViewColumn("", Gtk.CellRendererText(xalign=1.0), text=1)
		col1.set_min_width(50)

		view = Gtk.TreeView(self.editing_store)
		view.append_column(col0)
		view.append_column(col1)

		box = Gtk.Box(spacing=7)
		box.set_orientation(Gtk.Orientation.VERTICAL)
		box.pack_start(label, False, True, 0)
		box.pack_start(view, True, True, 0)

		return box;

	def get_file_composition_widget(self):
		self.composition_store = Gtk.ListStore(str, str)

		label = Gtk.Label()
		label.set_halign(Gtk.Align.START)
		label.set_markup("<b>COMPOSITION</b>")

		col0 = Gtk.TreeViewColumn("", Gtk.CellRendererText(), text=0)
		col0.set_expand(True)
		col0.set_min_width(150)
		col1 = Gtk.TreeViewColumn("", Gtk.CellRendererText(xalign=1.0), text=1)
		col1.set_min_width(50)

		view = Gtk.TreeView(self.composition_store)
		view.append_column(col0)
		view.append_column(col1)

		box = Gtk.Box(spacing=7)
		box.set_orientation(Gtk.Orientation.VERTICAL)
		box.pack_start(label, False, True, 0)
		box.pack_start(view, True, True, 0)

		return box;

	def get_file_colour_widget(self):
		self.colour_store = Gtk.ListStore(str, str)

		label = Gtk.Label()
		label.set_halign(Gtk.Align.START)
		label.set_markup("<b>COLOUR</b>")

		col0 = Gtk.TreeViewColumn("", Gtk.CellRendererText(), text=0)
		col0.set_expand(True)
		col0.set_min_width(150)
		col1 = Gtk.TreeViewColumn("", Gtk.CellRendererText(xalign=1.0), text=1)
		col1.set_min_width(50)

		view = Gtk.TreeView(self.colour_store)
		view.append_column(col0)
		view.append_column(col1)

		self.colour_grid = Gtk.Grid()

		box = Gtk.Box(spacing=7)
		box.set_orientation(Gtk.Orientation.VERTICAL)
		box.pack_start(label, False, True, 0)
		box.pack_start(view, True, True, 0)
		box.pack_start(self.colour_grid, True, True, 0)

		return box;

	def update_stats(self, filename):
		try:
			stat = os.stat(filename)

			timeformat = "%d %b %Y %H:%M:%S"

			self.file_name.set_text(filename)
			self.file_created.set_text(time.strftime(timeformat, time.localtime(stat[ST_CTIME])))
			self.file_modified.set_text(time.strftime(timeformat, time.localtime(stat[ST_MTIME])))
			self.file_accessed.set_text(time.strftime(timeformat, time.localtime(stat[ST_ATIME])))
		except IOError:
			print "Error: Failed to get information about", filename

		stats = EditingStatistics(self.log, self.subject_uris, self.update_editing_stats)
		stats.update()

		stats = CompositionStatistics()
		stats.parse(filename)
		
		self.composition_store.clear()
		self.composition_store.append(["Layers:", stats.get_layer_count()])
		self.composition_store.append(["Groups:", stats.get_group_count()])
		self.composition_store.append(["Objects:", stats.get_element_count()])
		self.composition_store.append(["  Masked:", stats.get_element_masked_count()])
		self.composition_store.append(["  Clipped:", stats.get_element_clipped_count()])
		self.composition_store.append(["  Copied:", stats.get_element_copied_count()])
		self.composition_store.append(["  Duplicated:", stats.get_element_duplicated_count()])

		self.colour_store.clear()
		self.colour_store.append(["Colours:", stats.get_colour_count()])

		# Clear the current palette items
		for button in self.colour_grid.get_children():
			self.colour_grid.remove(button)

		# Display the swatches in a tabular fashion
		row = 0
		col = 0

		for colour in stats.get_colour_palette():
			swatch = self.get_colour_swatch(colour)

			self.colour_grid.attach(swatch, col, row, 1, 1)

			col += 1

			if col == 4:
				row += 1

			col %= 4

		self.colour_grid.show_all()

	def get_colour_swatch(self, colour):
		c = Gdk.color_parse(colour)
		rgba = Gdk.RGBA.from_color(c)

		button = Gtk.ColorButton()
		button.set_size_request(24, 24)
		button.set_rgba(rgba)

		return button

	def update_editing_stats(self, stats):
		self.editing_store.clear()
		self.editing_store.append(["Cycles:", str(stats.begin_edit_count)])
		self.editing_store.append(["Steps:", str(stats.edit_count)])
		self.editing_store.append(["  Undos:", str(stats.undo_count)])
		self.editing_store.append(["  Redos:", str(stats.redo_count)])
		self.editing_store.append(["Confidence:", str(round(stats.confidence, 3))])
	
	def init_log(self):
		event_type = [Event.new_for_values(subject_interpretation=Interpretation.VISUAL)]

		self.log = ZeitgeistClient()
		self.log.install_monitor(TimeRange.always(), event_type, self.on_log_event_inserted, self.on_log_event_deleted)

	def init_log_view(self):
		self.log_store = Gtk.ListStore(str, str, str, str, str);

		log_model = Gtk.TreeModelSort(model=self.log_store)
		log_model.set_sort_column_id(0, Gtk.SortType.DESCENDING)

		column1 = Gtk.TreeViewColumn('Time', Gtk.CellRendererText(), text=0)
		column1.set_min_width(150)
		column1.set_resizable(True)
		column1.set_sort_column_id(0)

		column3 = Gtk.TreeViewColumn('Event', Gtk.CellRendererText(), text=2)
		column3.set_min_width(150)
		column3.set_resizable(True)
		column3.set_sort_column_id(2)

		renderer4 = Gtk.CellRendererText()
		renderer4.wrap_width = 400

		column4 = Gtk.TreeViewColumn('Data', renderer4, text=4)
		column4.set_min_width(75)
		column4.set_fixed_width(400)
		column4.set_resizable(True)
		column4.set_sort_column_id(4)

		self.log_view = Gtk.TreeView()
		self.log_view.set_model(log_model)
		self.log_view.append_column(column1)
		self.log_view.append_column(column3)
		self.log_view.append_column(column4)
		self.log_view.connect("row-activated", self.on_cell_clicked)
		self.log_view.connect("size-allocate", self.on_log_view_size_allocate)

		self.log_scroller = Gtk.ScrolledWindow()
		self.log_scroller.add(self.log_view)

	def on_log_view_size_allocate(self, widget, event, data=None):
		adjustment = self.log_scroller.get_vadjustment()
		adjustment.set_value(adjustment.get_lower())

	def on_log_event_inserted(self, time_range, events):
		# insert into journal
		for e in events:
			time = datetime.fromtimestamp(int(e.timestamp) / 1e3).strftime("%Y-%m-%d %H:%M:%S")
			actor = e.actor
			type = self.abbreviate(e.interpretation)
			subject = e.subjects[0].uri
			payload = ''.join([chr(c) for c in e.payload])

			self.log_store.append([time, actor, type, subject, payload])
			
		self.update_stats(self.filename)

	def abbreviate(self, uri):
		u = uri.replace("http://www.zeitgeist-project.com/ontologies/2010/01/27/zg#", "zg:")
		u = u.replace("http://purl.org/ontologies/art/1.0/terms#", "art:")
		return u;

	def on_log_event_deleted(self, time_range, event_ids):
		# remove events from journal
		print event_ids

	def on_cell_clicked(self, treeview, path, column):
		print self.log_store[path[0]][4]

	def load_events(self, filename):
		# Names the file may have previously been given.
		uri = "file://" + filename

		self.subject_uris.clear()
		self.subject_uris.add(uri)

		self.get_subject_move_events([uri])
	
	def get_subject_move_events(self, uris):
		templates = []

		for u in uris:
			subject = Subject.new_for_values(current_uri = u)

			templates.append(Event.new_for_values(subjects=[subject], interpretation=Interpretation.MOVE_EVENT))

		self.log.find_events_for_templates(templates, self.on_subjects_received, num_events=1000)

	def on_subjects_received(self, events):
		if len(events) == 0:
			self.on_subjects_resolved()
		else:
			uris = set()

			for e in events:
				u = str(e.subjects[0].uri)

				if not u in self.subject_uris:
					uris.add(u)

				self.subject_uris.add(u)

			self.get_subject_move_events(uris)

	def on_subjects_resolved(self):
		templates = []

		for u in self.subject_uris:
			subject = Subject.new_for_values(uri=u)

			templates.append(Event.new_for_values(subjects=[subject]))

		self.log.find_events_for_templates(templates, self.on_events_received, num_events=10000)

	def on_events_received(self, events):
		self.on_log_event_inserted(None, events)

	def on_export_clicked(self, event):
		with open('export.csv', 'wb') as csvfile:
			writer = csv.writer(csvfile, delimiter=";")
			for x in self.log_store:
				writer.writerow([x[0], x[1], x[2], x[3], x[4]])	

	
if( __name__ == "__main__" ):
	window = ArtivityJournal()
	window.resize(1000, 700)

	Gtk.main()
