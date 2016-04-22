#! /usr/bin/python

from gi.repository import Gio, Gdk, Gtk, GObject, GdkPixbuf, WebKit

class ArtivityJournal(Gtk.Window):
    """
    TODO
    """

    def __init__(self):
        Gtk.Window.__init__(self)

        self.set_title("Artivity Explorer")

        self.event_loader = None
        self.subject_tracker = None
        self.subject_uris = set()
        self.selected_session = 0

	self.__init_dark_theme()
        self.__init_layout()

        self.connect("delete-event", Gtk.main_quit)
        self.show_all()

	self.browser.load_uri("http://10.0.2.2:8262/artivity/app/journal/1.0/")	

    def __init_dark_theme(self):
        screen = Gdk.Screen.get_default()

        css = Gtk.CssProvider()
        css.load_from_path("/usr/share/themes/elementary/gtk-3.0/gtk-dark.css")

        context = self.get_style_context()
        context.add_provider_for_screen(screen, css, Gtk.STYLE_PROVIDER_PRIORITY_APPLICATION)

    def __init_layout(self):
        self.browser = WebKit.WebView()

        self.add(self.browser)

if __name__ == "__main__":
    window = ArtivityJournal()
    window.resize(800, 600)

    Gtk.main()
