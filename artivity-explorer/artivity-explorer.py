#! /usr/bin/python

from gi.repository import Gtk
from datetime import datetime

from zeitgeist.client import ZeitgeistClient
from zeitgeist.mimetypes import *
from zeitgeist.datamodel import *

import os, time
from stat import ST_ATIME, ST_MTIME, ST_CTIME

class ArtivityJournal(Gtk.Window):

	def __init__(self):
		Gtk.Window.__init__(self)
		
		self.set_title("Artivity Explorer")

		self.init_headerbar()
		self.init_log()
		self.init_log_view()
		self.init_stats_view()
		self.init_layout()

		self.connect("delete-event", Gtk.main_quit)
		self.show_all()

	def init_headerbar(self):
		self.open_button = Gtk.FileChooserButton()
		self.open_button.connect("file-set", self.on_file_selected)
		
		self.export_button = Gtk.Button(label="Export")
		self.export_button.set_sensitive(False)

		self.headerbar = Gtk.HeaderBar()
		self.headerbar.set_show_close_button(True)
		self.headerbar.props.title = self.get_title() 
		self.headerbar.pack_start(self.open_button)
		self.headerbar.pack_start(self.export_button)

		self.set_titlebar(self.headerbar)

	def on_file_selected(self, param):
		self.update_stats_view()

		self.load_events(self.open_button.get_filename())	

	def init_layout(self):
		self.stack = Gtk.Stack()
		self.stack.set_transition_type(Gtk.StackTransitionType.SLIDE_LEFT_RIGHT)
		self.stack.set_transition_duration(500)

		self.stack.add_titled(self.log_scroller, "log", "Protocol")
		self.stack.add_titled(self.stats_box, "stats", "Statistics")

		self.stack_switcher = Gtk.StackSwitcher()
		self.stack_switcher.set_stack(self.stack)
		self.stack_switcher.props.halign = Gtk.Align.CENTER

		self.layout_root = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=7)
		self.layout_root.pack_start(self.stack_switcher, False, True, 7)
		self.layout_root.pack_start(self.stack, True, True, 0)

		self.add(self.layout_root)

	def init_stats_view(self):
		self.file_label = Gtk.Label()
		self.file_label.set_halign(Gtk.Align.START)
		self.file_label.set_markup("<b>File:</b>")

		self.file_name = Gtk.Label()

		self.file_created_label = Gtk.Label()
		self.file_created_label.set_halign(Gtk.Align.START)
		self.file_created_label.set_markup("<b>Created:</b>")

		self.file_created = Gtk.Label()

		self.file_modified_label = Gtk.Label()
		self.file_modified_label.set_halign(Gtk.Align.START)
		self.file_modified_label.set_markup("<b>Last modified:</b>")

		self.file_modified = Gtk.Label()

		self.file_accessed_label = Gtk.Label()
		self.file_accessed_label.set_halign(Gtk.Align.START)
		self.file_accessed_label.set_markup("<b>Last accessed:</b>")

		self.file_accessed = Gtk.Label()

		self.stats_view = Gtk.Grid()
		self.stats_view.set_column_spacing(7)
		self.stats_view.set_row_spacing(7)

		self.stats_view.add(self.file_label)
		self.stats_view.attach(self.file_name, 1, 0, 6, 1)
		self.stats_view.attach(self.file_created_label, 0, 1, 1, 1)
		self.stats_view.attach(self.file_created, 1, 1, 1, 1)
		self.stats_view.attach(self.file_modified_label, 0, 2, 1, 1)
		self.stats_view.attach(self.file_modified, 1, 2, 1, 1)
		self.stats_view.attach(self.file_accessed_label, 0, 3, 1, 1)
		self.stats_view.attach(self.file_accessed, 1, 3, 1, 1)

		self.stats_box = Gtk.Box()
		self.stats_box.pack_start(self.stats_view, True, True, 14)

	def update_stats_view(self):
		filename = self.open_button.get_filename()

		try:
			stat = os.stat(filename)

			timeformat = "%d %b %Y %H:%M:%S"

			self.file_name.set_text(filename)
			self.file_created.set_text(time.strftime(timeformat, time.localtime(stat[ST_CTIME])))
			self.file_modified.set_text(time.strftime(timeformat, time.localtime(stat[ST_MTIME])))
			self.file_accessed.set_text(time.strftime(timeformat, time.localtime(stat[ST_ATIME])))
		except IOError:
			print "Error: Failed to get information about", filename
		
	def init_log(self):
		event_type = [Event.new_for_values(subject_interpretation=Interpretation.VISUAL)]

		self.log = ZeitgeistClient()
		self.log.install_monitor(TimeRange.always(), event_type, self.on_log_event_inserted, self.on_log_event_deleted)

	def init_log_view(self):
		self.log_model = Gtk.ListStore(str, str, str, str, str);

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

		column5 = Gtk.TreeViewColumn()

		self.log_view = Gtk.TreeView()
		self.log_view.set_model(self.log_model)
		self.log_view.append_column(column1)
		self.log_view.append_column(column3)
		self.log_view.append_column(column4)
		self.log_view.append_column(column5)
		self.log_view.connect("row-activated", self.on_cell_clicked)

		self.log_scroller = Gtk.ScrolledWindow()
		self.log_scroller.add(self.log_view)

	def on_log_event_inserted(self, time_range, events):
		# insert into journal
		for e in events:
			time = datetime.fromtimestamp(int(e.timestamp) / 1e3).strftime("%Y-%m-%d %H:%M:%S")
			actor = e.actor
			type = self.abbreviate(e.interpretation)
			subject = e.subjects[0].uri
			payload = ''.join([chr(c) for c in e.payload])

			self.log_model.append([time, actor, type, subject, payload])
			
		self.log_view.set_model(self.log_model)

	def abbreviate(self, uri):
		return uri.replace("http://www.zeitgeist-project.com/ontologies/2010/01/27/zg#", "zg:")

	def on_log_event_deleted(self, time_range, event_ids):
		# remove events from journal
		print event_ids

	def on_events_received(self, events):
		self.on_log_event_inserted(None, events)

	def on_cell_clicked(self, treeview, path, column):
		print self.log_model[path[0]][4]

	def load_events(self, filename):
		filename = "file://"+filename
		subj = Subject.new_for_values(uri=filename)
		template = Event.new_for_values(subjects=[subj])
		self.log.find_events_for_template(template, self.on_events_received)
	
if( __name__ == "__main__" ):
	window = ArtivityJournal()
	window.resize(1000, 700)

	Gtk.main()
