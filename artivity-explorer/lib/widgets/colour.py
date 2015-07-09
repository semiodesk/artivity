__author__ = 'sebastian'

from gi.repository import Gdk, Gtk, GObject

class FileColourWidget(Gtk.VBox):

    def __init__(self):

        Gtk.VBox.__init__(self)

        self.__init_layout()

    def __init_layout(self):
        self.__icon = Gtk.Image()
        self.__icon.set_from_file('/usr/share/icons/Artivity/16x16/colour.png')

        self.__title = Gtk.Label(halign=Gtk.Align.START)
        self.__title.set_markup('<span foreground="#666666"><b>Colour</b></span>')

        header = Gtk.HBox()
        header.pack_start(self.__icon, False, False, 0)
        header.pack_start(self.__title, True, True, 7)

        self.__colours_box = Gtk.Label(halign=Gtk.Align.END, hexpand=True)

        self.__palette_grid = Gtk.Grid()

        self.__layout = Gtk.Grid()
        self.__layout.set_row_spacing(7)
        self.__layout.attach(Gtk.Label("Colours", halign=Gtk.Align.START), 0, 0, 1, 1)
        self.__layout.attach(self.__colours_box, 1, 0, 1, 1)

        body = Gtk.Alignment()
        body.set_padding(0, 0, 23, 0)
        body.add(self.__layout)

        palette = Gtk.Alignment()
        palette.set_padding(0, 0, 23, 0)
        palette.add(self.__palette_grid)

        self.pack_start(header, False, False, 7)
        self.pack_start(body, True, False, 7)
        self.pack_start(palette, True, True, 7)

    def update(self, stats):
        self.__colours_box.set_text(stats.get_layer_count())

        # Clear the current palette items
        for swatch in self.__palette_grid.get_children():
            self.__palette_grid.remove(swatch)

        # Display the swatches in a tabular fashion
        row = 0
        col = 0

        for colour in stats.get_colour_palette():
            swatch = self.__get_swatch(colour)

            self.__palette_grid.attach(swatch, col, row, 1, 1)

            col += 1

            if col == 4:
                row += 1

            col %= 4

        self.__palette_grid.show_all()

    def __get_swatch(self, colour):
        c = Gdk.color_parse(colour)
        rgba = Gdk.RGBA.from_color(c)

        area = Gtk.ColorButton()
        area.set_size_request(24, 24)
        area.set_rgba(rgba)

        return area
