#! /usr/bin/python

import gtk
from datetime import datetime

from zeitgeist.client import ZeitgeistClient
from zeitgeist.mimetypes import *
from zeitgeist.datamodel import *

class ArtivityJournal(gtk.Window):

	def __init__(self):
		gtk.Window.__init__(self)

		self.init_log()
		self.init_log_view()

	def init_log(self):
		eventType = [Event.new_for_values(subject_uri='file:///home/sebastian/test.svg', subject_interpretation=Interpretation.VISUAL)]

		self.log = ZeitgeistClient()
		self.log.install_monitor(TimeRange.always(), eventType, self.on_log_event_inserted, self.on_log_event_deleted)

	def init_log_view(self):
		self.logModel = gtk.ListStore(str, str, str, str, str);

		column1 = gtk.TreeViewColumn('Time', gtk.CellRendererText(), text=0)
    		column1.set_sizing(gtk.TREE_VIEW_COLUMN_FIXED)
		column1.set_min_width(150)
		column1.set_resizable(True)
		column1.set_sort_column_id(0)

		column2 = gtk.TreeViewColumn('Actor', gtk.CellRendererText(), text=1)
    		column2.set_sizing(gtk.TREE_VIEW_COLUMN_FIXED)
		column2.set_min_width(220)
		column2.set_resizable(True)
		column2.set_sort_column_id(1)

		column3 = gtk.TreeViewColumn('Event', gtk.CellRendererText(), text=2)
    		column3.set_sizing(gtk.TREE_VIEW_COLUMN_FIXED)
		column3.set_min_width(150)
		column3.set_resizable(True)
		column3.set_sort_column_id(2)

		column4 = gtk.TreeViewColumn('Subject', gtk.CellRendererText(), text=3)
    		column4.set_sizing(gtk.TREE_VIEW_COLUMN_FIXED)
		column4.set_min_width(400)
		column4.set_resizable(True)
		column4.set_sort_column_id(3)

		renderer6 = gtk.CellRendererText()
		renderer6.wrap_width = 400
		renderer6.wrap_mode = gtk.WRAP_WORD

		column5 = gtk.TreeViewColumn('Data', renderer6, text=4)
    		column5.set_sizing(gtk.TREE_VIEW_COLUMN_FIXED)
		column5.set_min_width(75)
		column5.set_fixed_width(400)
		column5.set_resizable(True)
		column5.set_sort_column_id(4)

		column6 = gtk.TreeViewColumn()

		self.logView = gtk.TreeView()
		self.logView.set_model(self.logModel)
		self.logView.append_column(column1)
		self.logView.append_column(column2)
		self.logView.append_column(column3)
		self.logView.append_column(column4)
		self.logView.append_column(column5)
		self.logView.append_column(column6)

		self.logScroller = gtk.ScrolledWindow()
		self.logScroller.set_policy(gtk.POLICY_AUTOMATIC, gtk.POLICY_AUTOMATIC)
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

window = ArtivityJournal()
window.resize(1350, 500)
window.connect("delete-event", gtk.main_quit)
window.show_all()

gtk.gdk.threads_init()
gtk.main()
