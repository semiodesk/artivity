#! /usr/bin/python

import gtk
from datetime import datetime

from zeitgeist.client import ZeitgeistClient
from zeitgeist.mimetypes import *
from zeitgeist.datamodel import *

class ArtivityJournal(gtk.Window):

	vbox = None;

	def __init__(self):
		gtk.Window.__init__(self)
		
		self.set_title("Artivity Explorer")

		self.init_menubar()

		self.init_log()
		self.init_log_view()

		self.connect("delete-event", gtk.main_quit)
		self.show_all()

	def init_menubar(self):
		mb = gtk.MenuBar();

		filemenu = gtk.Menu()
        	filemenuItem = gtk.MenuItem("File")
	        filemenuItem.set_submenu(filemenu)
       
        	exit = gtk.MenuItem("Exit")
	        exit.connect("activate", gtk.main_quit)
        	filemenu.append(exit)

	        mb.append(filemenuItem)


		querymenu = gtk.Menu()
		querymenuItem = gtk.MenuItem("Query")
		querymenuItem.set_submenu(querymenu)
		
		timespan = gtk.MenuItem("Select File")
		timespan.connect("activate", self.set_file)
		querymenu.append(timespan)

		mb.append(querymenuItem)


        	self.vbox = gtk.VBox(False, 2)
	        self.vbox.pack_start(mb, False, False, 0)

        	self.add(self.vbox)	

		

	def init_log(self):
		eventType = [Event.new_for_values(subject_interpretation=Interpretation.VISUAL)]

		self.log = ZeitgeistClient()
		self.log.install_monitor(TimeRange.always(), eventType, self.on_log_event_inserted, self.on_log_event_deleted)

	def init_log_view(self):
		self.logModel = gtk.ListStore(str, str, str, str, str);

		column1 = gtk.TreeViewColumn('Time', gtk.CellRendererText(), text=0)
    		column1.set_sizing(gtk.TREE_VIEW_COLUMN_FIXED)
		column1.set_min_width(150)
		column1.set_resizable(True)
		column1.set_sort_column_id(0)

		column2 = gtk.TreeViewColumn('Subject', gtk.CellRendererText(), text=3)
    		column2.set_sizing(gtk.TREE_VIEW_COLUMN_FIXED)
		column2.set_min_width(400)
		column2.set_resizable(True)
		column2.set_sort_column_id(3)

		column3 = gtk.TreeViewColumn('Event', gtk.CellRendererText(), text=2)
    		column3.set_sizing(gtk.TREE_VIEW_COLUMN_FIXED)
		column3.set_min_width(150)
		column3.set_resizable(True)
		column3.set_sort_column_id(2)

		renderer4 = gtk.CellRendererText()
		renderer4.wrap_width = 400
		renderer4.wrap_mode = gtk.WRAP_WORD

		column4 = gtk.TreeViewColumn('Data', renderer4, text=4)
    		column4.set_sizing(gtk.TREE_VIEW_COLUMN_FIXED)
		column4.set_min_width(75)
		column4.set_fixed_width(400)
		column4.set_resizable(True)
		column4.set_sort_column_id(4)

		column5 = gtk.TreeViewColumn()

		self.logView = gtk.TreeView()
		self.logView.set_model(self.logModel)
		self.logView.append_column(column1)
		self.logView.append_column(column2)
		self.logView.append_column(column3)
		self.logView.append_column(column4)
		self.logView.append_column(column5)
		self.logView.connect("row-activated", self.cell_clicked)

		self.logScroller = gtk.ScrolledWindow()
		self.logScroller.set_policy(gtk.POLICY_AUTOMATIC, gtk.POLICY_AUTOMATIC)
		self.logScroller.add(self.logView)

		self.vbox.pack_start(self.logScroller, True, True, 0 )

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

	def cell_clicked(self, treeview, path, column):
		print self.logModel[path[0]][4]


	def load_events(self, filename):
		filename = "file://"+filename
		subj = Subject.new_for_values(uri=filename)
		template = Event.new_for_values(subjects=[subj])
		self.log.find_events_for_template(template, self.on_events_received)
	

	def get_artivity_files(self):
		chooser = gtk.FileChooserDialog(title=None, action=gtk.FILE_CHOOSER_ACTION_OPEN, buttons=(gtk.STOCK_CANCEL,gtk.RESPONSE_CANCEL,gtk.STOCK_OPEN,gtk.RESPONSE_OK))
		response = chooser.run()
		if( response == gtk.RESPONSE_OK ):
			self.load_events(chooser.get_filename())	
		chooser.destroy()

	def set_file(self, thing):
		self.get_artivity_files()


if( __name__ == "__main__" ):
	window = ArtivityJournal()
	window.resize(1350, 500)

	gtk.gdk.threads_init()
	gtk.main()
