__author__ = 'sebastian'

from gi.repository import Gdk, Gtk, GObject

class FileEditingWidget(Gtk.VBox):

    def __init__(self):

        Gtk.VBox.__init__(self)

        self.__init_layout()

    def __init_layout(self):
        self.__icon = Gtk.Image()
        self.__icon.set_from_file('/usr/share/icons/Artivity/16x16/editing.png')

        self.__title = Gtk.Label(halign=Gtk.Align.START)
        self.__title.set_markup('<span foreground="#666666"><b>Editing</b></span>')

        header = Gtk.HBox()
        header.pack_start(self.__icon, False, False, 0)
        header.pack_start(self.__title, True, True, 7)

        self.__sessions_box = Gtk.Label(halign=Gtk.Align.END, hexpand=True)
        self.__steps_box = Gtk.Label(halign=Gtk.Align.END, hexpand=True)
        self.__undos_box = Gtk.Label(halign=Gtk.Align.END, hexpand=True)
        self.__redos_box = Gtk.Label(halign=Gtk.Align.END, hexpand=True)
        self.__confidence_box = Gtk.Label(halign=Gtk.Align.END, hexpand=True)

        self.__layout = Gtk.Grid()
        self.__layout.set_row_spacing(7)
        self.__layout.attach(Gtk.Label("Sessions", halign=Gtk.Align.START), 0, 0, 1, 1)
        self.__layout.attach(self.__sessions_box, 1, 0, 1, 1)
        self.__layout.attach(Gtk.Label("Steps", halign=Gtk.Align.START), 0, 1, 1, 1)
        self.__layout.attach(self.__steps_box, 1, 1, 1, 1)
        self.__layout.attach(Gtk.Label("  Undos", halign=Gtk.Align.START), 0, 2, 1, 1)
        self.__layout.attach(self.__undos_box, 1, 2, 1, 1)
        self.__layout.attach(Gtk.Label("  Redos", halign=Gtk.Align.START), 0, 3, 1, 1)
        self.__layout.attach(self.__redos_box, 1, 3, 1, 1)
        self.__layout.attach(Gtk.Label("Confidence", halign=Gtk.Align.START), 0, 4, 1, 1)
        self.__layout.attach(self.__confidence_box, 1, 4, 1, 1)

        body = Gtk.Alignment()
        body.set_padding(0, 0, 23, 0)
        body.add(self.__layout)

        self.pack_start(header, False, False, 7)
        self.pack_start(body, True, True, 7)

    def update(self, stats):
        self.__sessions_box.set_text(str(stats.end_edit_count))
        self.__steps_box.set_text(str(stats.edit_count))
        self.__undos_box.set_text(str(stats.undo_count))
        self.__redos_box.set_text(str(stats.redo_count))
        self.__confidence_box.set_text(str(round(stats.confidence, 3)))
