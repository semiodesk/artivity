#! /usr/bin/python

from gi.repository import Gtk
from datetime import datetime

from zeitgeist.client import ZeitgeistClient
from zeitgeist.mimetypes import *
from zeitgeist.datamodel import *

class ArtivityJournal(Gtk.Window):

	def __init__(self):
		Gtk.Window.__init__(self)
		
		self.set_title("Artivity Explorer")

		self.init_headerbar()
		self.init_log()
		self.init_log_view()

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
		self.load_events(self.open_button.get_filename())	
		
	def init_log(self):
		eventType = [Event.new_for_values(subject_interpretation=Interpretation.VISUAL)]

		self.log = ZeitgeistClient()
		self.log.install_monitor(TimeRange.always(), eventType, self.on_log_event_inserted, self.on_log_event_deleted)

	def init_log_view(self):
		self.logModel = Gtk.ListStore(str, str, str, str, str);

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

		self.logView = Gtk.TreeView()
		self.logView.set_model(self.logModel)
		self.logView.append_column(column1)
		self.logView.append_column(column3)
		self.logView.append_column(column4)
		self.logView.append_column(column5)
		self.logView.connect("row-activated", self.on_cell_clicked)

		self.logScroller = Gtk.ScrolledWindow()
		self.logScroller.add(self.logView)

		self.add(self.logScroller)

	def on_log_event_inserted(self, time_range, events):
		# insert into journal
		for e in events:
			time = datetime.fromtimestamp(int(e.timestamp) / 1e3).strftime("%Y-%m-%d %H:%M:%S")
			actor = e.actor
			type = self.abbreviate(e.interpretation)
			subject = e.subjects[0].uri
			payload = ''.join([chr(c) for c in e.payload])

			self.logModel.append([time, actor, type, subject, payload])
			
		self.logView.set_model(self.logModel)

	def abbreviate(self, uri):
		return uri.replace("http://www.zeitgeist-project.com/ontologies/2010/01/27/zg#", "zg:")

	def on_log_event_deleted(self, time_range, event_ids):
		# remove events from journal
		print event_ids

	def on_events_received(self, events):
		self.on_log_event_inserted(None, events)

	def on_cell_clicked(self, treeview, path, column):
		print self.logModel[path[0]][4]

	def load_events(self, filename):
		filename = "file://"+filename
		subj = Subject.new_for_values(uri=filename)
		template = Event.new_for_values(subjects=[subj])
		self.log.find_events_for_template(template, self.on_events_received)
	
if( __name__ == "__main__" ):
	window = ArtivityJournal()
	window.resize(1000, 700)

	Gtk.main()
