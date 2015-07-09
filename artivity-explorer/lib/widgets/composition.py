__author__ = 'sebastian'

from gi.repository import Gdk, Gtk, GObject

class FileCompositionWidget(Gtk.VBox):

    def __init__(self):

        Gtk.VBox.__init__(self)

        self.__init_layout()

    def __init_layout(self):
        self.__icon = Gtk.Image()
        self.__icon.set_from_file('/usr/share/icons/Artivity/16x16/composition.png')

        self.__title = Gtk.Label(halign=Gtk.Align.START)
        self.__title.set_markup('<span foreground="#666666"><b>Composition</b></span>')

        header = Gtk.HBox()
        header.pack_start(self.__icon, False, False, 0)
        header.pack_start(self.__title, True, True, 7)

        self.__layers_box = Gtk.Label(halign=Gtk.Align.END, hexpand=True)
        self.__groups_box = Gtk.Label(halign=Gtk.Align.END, hexpand=True)
        self.__objects_box = Gtk.Label(halign=Gtk.Align.END, hexpand=True)
        self.__masked_box = Gtk.Label(halign=Gtk.Align.END, hexpand=True)
        self.__clipped_box = Gtk.Label(halign=Gtk.Align.END, hexpand=True)

        self.__layout = Gtk.Grid()
        self.__layout.set_row_spacing(7)
        self.__layout.attach(Gtk.Label("Layers", halign=Gtk.Align.START), 0, 0, 1, 1)
        self.__layout.attach(self.__layers_box, 1, 0, 1, 1)
        self.__layout.attach(Gtk.Label("Groups", halign=Gtk.Align.START), 0, 1, 1, 1)
        self.__layout.attach(self.__groups_box, 1, 1, 1, 1)
        self.__layout.attach(Gtk.Label("Objects", halign=Gtk.Align.START), 0, 2, 1, 1)
        self.__layout.attach(self.__objects_box, 1, 2, 1, 1)
        self.__layout.attach(Gtk.Label("  Masked", halign=Gtk.Align.START), 0, 3, 1, 1)
        self.__layout.attach(self.__masked_box, 1, 3, 1, 1)
        self.__layout.attach(Gtk.Label("  Clipped", halign=Gtk.Align.START), 0, 4, 1, 1)
        self.__layout.attach(self.__clipped_box, 1, 4, 1, 1)

        body = Gtk.Alignment()
        body.set_padding(0, 0, 23, 0)
        body.add(self.__layout)

        self.pack_start(header, False, False, 7)
        self.pack_start(body, True, True, 7)

    def update(self, stats):
        self.__layers_box.set_text(stats.get_layer_count())
        self.__groups_box.set_text(stats.get_group_count())
        self.__objects_box.set_text(stats.get_element_count())
        self.__masked_box.set_text(stats.get_element_masked_count())
        self.__clipped_box.set_text(stats.get_element_clipped_count())
