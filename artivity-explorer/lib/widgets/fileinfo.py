#! /usr/bin/python

import os
import time

from gi.repository import Gdk, Gtk, GObject
from stat import ST_ATIME, ST_MTIME, ST_CTIME
from datetime import datetime

class FileInfoWidget(Gtk.HBox):

    def __init__(self):

        self.__file_name = None
        self.__file_name_box = Gtk.Label()
        self.__file_name_box.set_halign(Gtk.Align.START)
        self.__file_created = None
        self.__file_created_box = Gtk.Label()
        self.__file_created_box.set_halign(Gtk.Align.START)
        self.__file_modified = None
        self.__file_modified_box = Gtk.Label()
        self.__file_modified_box.set_halign(Gtk.Align.START)
        self.__file_accessed = None
        self.__file_accessed_box = Gtk.Label()
        self.__file_accessed_box.set_halign(Gtk.Align.START)
        self.__file_age = None
        self.__file_age_box = Gtk.Label()
        self.__file_age_box.set_halign(Gtk.Align.START)

        Gtk.HBox.__init__(self, spacing=7)

        self.__init_layout()

    def __init_layout(self):
        self.pack_start(self.__file_name_box, True, True, 7)
        self.pack_start(self.__file_created_box, False, False, 7)
        self.pack_start(self.__file_modified_box, False, False, 7)
        self.pack_start(self.__file_accessed_box, False, False, 7)
        self.pack_start(self.__file_age_box, False, False, 7)

    def set_file(self, filename):
        try:
            stat = os.stat(filename)

            self.__file_name = filename
            self.__file_created = time.localtime(stat[ST_CTIME])
            self.__file_modified = time.localtime(stat[ST_MTIME])
            self.__file_accessed = time.localtime(stat[ST_ATIME])
            self.__file_age = datetime.now() - datetime.fromtimestamp(time.mktime(self.__file_created))

            self.update()
        except IOError:
            print "FileInfoWidget: Error: Failed to get information about", filename

    def update(self):
            timeformat = "%d.%m.%Y %H:%M"

            self.__file_name_box.set_text(self.__file_name)
            self.__file_created_box.set_text(time.strftime(timeformat, self.__file_created))
            self.__file_modified_box.set_text(time.strftime(timeformat, self.__file_modified))
            self.__file_accessed_box.set_text(time.strftime(timeformat, self.__file_accessed))
