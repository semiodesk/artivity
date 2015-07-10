#! /usr/bin/python

from gi.repository import Gio, Gdk, Gtk, GObject, GdkPixbuf
from datetime import datetime, timedelta

from zeitgeist.client import ZeitgeistClient
from zeitgeist.datamodel import *

from matplotlib.figure import Figure
from matplotlib.font_manager import FontProperties
from matplotlib.backends.backend_gtk3agg import FigureCanvasGTK3Agg as FigureCanvas

from lib.widgets.fileinfo import FileInfoWidget
from lib.widgets.editing import FileEditingWidget
from lib.widgets.composition import FileCompositionWidget
from lib.widgets.colour import FileColourWidget

from lib.stats import EditingStatistics
from lib.stats import CompositionStatistics
from lib.file import FileTracker, FileEventLoader
from lib.ontology import abbreviate, app, art, zg

import csv, re

class ArtivityJournal(Gtk.Window):
    """
    TODO
    """

    def __init__(self):
        Gtk.Window.__init__(self)

        self.set_title("Artivity Explorer")

        self.__filename = None
        self.__fileinfo = FileInfoWidget()
        self.__editinfo = FileEditingWidget()
        self.__compositioninfo = FileCompositionWidget()
        self.__colourinfo = FileColourWidget()

        self.__actor_palette = {
           app.Inkscape: '#f02cab',
           app.Chromium: '#00ccff',
           app.Firefox: '#00ccff',
           app.Opera: '#00ccff',
           app.Nautilus: '#ee7101',
           app.EyeOfGnome: '#ee7101'}

        self.__activity_icons = {
            app.Inkscape: {
                zg.AccessEvent: '/usr/share/icons/Artivity/16x16/save.png',
                art.BeginEditingEvent: '/usr/share/icons/Artivity/16x16/editing_begin.png',
                art.EndEditingEvent: '/usr/share/icons/Artivity/16x16/editing_end.png',
                art.EditEvent: '/usr/share/icons/Artivity/16x16/edit.png',
                art.UndoEvent: '/usr/share/icons/Artivity/16x16/undo.png',
            },
            app.Chromium: {
                zg.AccessEvent: '/usr/share/icons/Artivity/16x16/globe.png'
            },
            app.Firefox: {
                zg.AccessEvent: '/usr/share/icons/Artivity/16x16/globe.png'
            },
            app.Opera: {
                zg.AccessEvent: '/usr/share/icons/Artivity/16x16/globe.png'
            },
            app.Nautilus: {
                zg.MoveEvent: '/usr/share/icons/Artivity/16x16/move.png'
            },
            app.EyeOfGnome: {
                zg.AccessEvent: '/usr/share/icons/Artivity/16x16/eye.png'
            }}

        self.event_loader = None
        self.subject_tracker = None
        self.subject_uris = set()
        self.selected_session = 0

        self.__init_header()
        self.__init_log_monitor()
        self.__init_log_view()
        self.__init_log_histogram()
        self.__init_layout()

        self.connect("delete-event", Gtk.main_quit)
        self.show_all()

    def __init_dark_theme(self):
        screen = Gdk.Screen.get_default()

        css = Gtk.CssProvider()
        css.load_from_path("/usr/share/themes/Adwaita/gtk-3.0/gtk-dark.css")

        context = self.get_style_context()
        context.add_provider_for_screen(screen, css, Gtk.STYLE_PROVIDER_PRIORITY_APPLICATION)

    def __init_header(self):
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

    def __init_layout(self):
        sidebar = Gtk.VBox()
        sidebar.set_hexpand(True)
        sidebar.set_vexpand(False)
        sidebar.set_size_request(200, 500)
        sidebar.pack_start(self.__editinfo, False, False, 14)
        sidebar.pack_start(self.__compositioninfo, False, False, 0)
        sidebar.pack_start(self.__colourinfo, False, False, 14)

        alignment = Gtk.Alignment()
        alignment.align = 0
        alignment.set_padding(0, 0, 14, 14)
        alignment.add(sidebar)

        paned = Gtk.HPaned()
        paned.pack1(alignment, False, False)
        paned.pack2(self.log_page, True, True)

        self.layout_root = Gtk.VBox()
        self.layout_root.pack_start(paned, True, True, 0)
        self.layout_root.pack_start(Gtk.VSeparator(), False, False, 0)
        self.layout_root.pack_start(self.__fileinfo, False, False, 7)

        self.add(self.layout_root)

    def __init_log_monitor(self):
        event_type = [Event.new_for_values(subject_interpretation=Interpretation.VISUAL)]

        self.log = ZeitgeistClient()
        self.log.install_monitor(TimeRange.always(), event_type, self.on_log_event_inserted, self.on_log_event_deleted)

    def __init_log_view(self):
        self.log_store = Gtk.ListStore(str, str, str, str, str, str, GdkPixbuf.Pixbuf, str, int)

        log_model = Gtk.TreeModelSort(model=self.log_store)

        column0 = Gtk.TreeViewColumn('', Gtk.CellRendererText(), cell_background=5)
        column0.set_min_width(4)
        column0.set_fixed_width(4)
        column0.set_resizable(False)

        column1 = Gtk.TreeViewColumn('Time', Gtk.CellRendererText(), text=0)
        column1.set_min_width(150)
        column1.set_resizable(True)
        column1.set_sort_column_id(0)

        renderer4 = Gtk.CellRendererText()
        renderer4.wrap_width = 400

        column4 = Gtk.TreeViewColumn('Data', renderer4, text=4)
        column4.set_min_width(300)
        column4.set_fixed_width(600)
        column4.set_expand(True)
        column4.set_resizable(True)
        column4.set_sort_column_id(4)

        self.__expandcolumn = column4

        column5 = Gtk.TreeViewColumn('', Gtk.CellRendererPixbuf(), pixbuf=6)
        column5.set_min_width(30)
        column5.set_resizable(False)

        column6 = Gtk.TreeViewColumn('Zoom', Gtk.CellRendererText(), text=7)
        column6.set_min_width(100)
        column6.set_resizable(False)
        column6.set_sort_column_id(7)

        self.log_view = Gtk.TreeView()
        self.log_view.set_model(log_model)
        self.log_view.set_vexpand(True)
        self.log_view.append_column(column0)
        self.log_view.append_column(column1)
        self.log_view.append_column(column5)
        self.log_view.append_column(column4)
        self.log_view.append_column(column6)
        self.log_view.connect('size-allocate', self.on_log_view_size_allocate)
        self.log_view.connect('row-activated', self.on_log_view_row_clicked)
        self.log_view.connect('key_press_event', self.on_log_view_key_pressed)

        self.log_view_range = None

        self.log_histbox = Gtk.VBox()
        self.log_histbox.set_size_request(800, 150)

        self.log_scroller = Gtk.ScrolledWindow()
        self.log_scroller.add(self.log_view)

        cycle_list = Gtk.TreeView()
        cycle_list.set_size_request(200, 0)

        cycle_view = Gtk.VBox()
        cycle_view.pack_start(self.log_histbox, False, False, 0)
        cycle_view.pack_start(self.log_scroller, True, True, 0)

        self.log_page = Gtk.HBox()
        self.log_page.set_hexpand(True)
        self.log_page.set_vexpand(True)
        self.log_page.pack_start(Gtk.VSeparator(), False, False, 0)
        self.log_page.pack_start(cycle_view, True, True, 0)

    def __init_log_histogram(self):
        self.log_figure = Figure(dpi=72, facecolor='#444444')
        self.log_figure.subplots_adjust(left=0.035, right=1.0, bottom=0.07, top=0.95)
        self.log_figure_data = dict()

        self.log_canvas = FigureCanvas(self.log_figure)
        self.log_canvas.set_size_request(300, 350)

        self.log_histbox.add(self.log_canvas)
        self.log_histbox.show_all()

    def update_stats(self, filename):
        stats = CompositionStatistics()
        stats.parse(filename)

        self.__compositioninfo.update(stats)
        self.__colourinfo.update(stats)

        stats = EditingStatistics()
        stats.calculate(self.log_store)

        self.update_histogram()

    def update_histogram(self):
        print
        print "update_histogram():", len(self.log_store)

        if len(self.log_store) > 0:
            timeformat = "%Y-%m-%d %H:%M:%S"

            begin_time = datetime.strptime(self.log_store[len(self.log_store) - 1][0], timeformat)
            begin_time -= timedelta(seconds=begin_time.second)

            maxv = 0

            # Build the data rows for the diagrams from the data in the log store.
            self.log_figure_data = dict()

            for row in self.log_store:
                actor = row[1]

                x, y = [], []

                if actor in self.log_figure_data.keys():
                    x, y = self.log_figure_data[actor]

                d = datetime.strptime(row[0], timeformat)
                d -= timedelta(seconds=d.second)
                v = int((d - begin_time).total_seconds() / 60)

                maxv = max(maxv, v)

                if len(x) == 0:
                    x.insert(0, v)
                    y.insert(0, 1)
                elif x[0] == v:
                    # Increase the counter for the current value.
                    y[0] += 1
                else:
                    # Fill the up the missing minutes with 0.
                    n = x[0] - v

                    while n > 1:
                        x.insert(0, x[0] - 1)
                        y.insert(0, 0)

                        n -= 1

                    # Now insert the current value
                    x.insert(0, v)
                    y.insert(0, 1)

                self.log_figure_data[actor] = (x, y)

            # Prepend and append 0s to the values so that the lines in the diagram start and end at 0.
            for actor in self.log_figure_data.keys():
                x, y = self.log_figure_data[actor]

                x.insert(0, x[0] - 1)
                x.append(x[len(x) - 1] + 1)
                y.insert(0, 0)
                y.append(0)

            # Prepend and append 0s to the values so that the lines in the diagram start and end at 0.
            for actor in self.log_figure_data.keys():
                x, y = self.log_figure_data[actor]

                x.insert(0, x[0] - 1)
                x.append(x[len(x) - 1] + 1)
                y.insert(0, 0)
                y.append(0)

            self.log_figure.clear()

            a = self.log_figure.add_subplot(111, axisbg='#444444')
            a.set_title('ACTIVITIES / MIN', horizontalalignment='left', y=1.06)
            a.title.set_color('white')
            a.grid(True)
            a.yaxis.grid(color='#999999')
            a.yaxis.label.set_color('white')
            a.tick_params(axis='y', colors='#eeeeee')
            a.xaxis.grid(color='#999999')
            a.xaxis.label.set_color('white')
            a.tick_params(axis='x', colors='#eeeeee')
            a.spines['top'].set_visible(False)
            a.spines['right'].set_visible(False)
            a.spines['bottom'].set_visible(False)
            a.spines['left'].set_visible(False)
            a.set_xlim([maxv * -0.05, maxv * 1.1])
            #a.set_xlabel('min')
            #a.set_ylabel('Activities')

            # Draw the editing cycles
            i = 0

            for cycle in self.event_loader.editing_sessions:
                pink = '#f02cab'

                # Only set the label for the first plot.
                if i > 0:
                    a.axvspan(cycle[0], cycle[1], facecolor=pink, edgecolor=pink, alpha=0.3)
                else:
                    a.axvspan(cycle[0], cycle[1], facecolor=pink, edgecolor=pink, alpha=0.3, label='Editing Session')

                i += 1

            # Plot the data
            for actor in self.log_figure_data.keys():
                x, y = self.log_figure_data[actor]
                label = self.get_display_name(actor)
                colour = self.get_colour(actor)

                #offset = transforms.ScaledTranslation(i * (4 / 72), 0, self.log_figure.dpi_scale_trans)
                #transform = a.transData + offset

                a.vlines(x, [0], y, lw=3, edgecolor=colour, alpha=0.75, label=label)

            # Add the legend to the plot _after_ all subplots have been made.
            font = FontProperties()
            font.set_size(12)

            # Bounding box of the axis.
            b = a.get_position()

            # Set the figure size.
            a.set_position([b.x0, b.y0 + b.height * 0.15, b.width, b.height * .75])

            # Add the legend at the bottom of the plot.
            l = a.legend(ncol=10, loc='upper left', borderaxespad=0, bbox_to_anchor=(0, -.16), framealpha=0, prop=font)

            for t in l.get_texts():
                t.set_color('white')
        else:
            self.log_figure.clear()

        self.log_canvas.draw()

    def get_display_name(self, actor):
        app_info = Gio.DesktopAppInfo.new(actor.replace("application://", ""))

        return app_info.get_display_name()

    def get_colour(self, actor):
        if actor in self.__actor_palette:
            return self.__actor_palette[actor]

        return '#eeeeee'

    def get_pixbuf(self, actor, interpretation):
        if actor in self.__activity_icons:
            icons = self.__activity_icons[actor]

            if interpretation in icons:
                icon = Gtk.Image()
                icon.set_from_file(icons[interpretation])

                return icon.get_pixbuf()

        return None

    def on_log_view_key_pressed(self, widget, event):
        # 0xFFFF is the delete key
        # See: https://git.gnome.org/browse/gtk+/plain/gdk/gdkkeysyms.h
        if event.keyval == 65535:
            selection = self.log_view.get_selection()

            model, iter = selection.get_selected()

            id = model[iter][8]

            if id > 0:
                self.log.delete_events([id])
                self.log_store.remove(iter)

                self.update_histogram()

    def on_log_view_size_allocate(self, widget, event, data=None):
        adjustment = self.log_scroller.get_vadjustment()
        adjustment.set_value(adjustment.get_lower())

    def on_log_view_row_clicked(self, widget, path, column):
        iter = self.log_store.get_iter(path)

        value = self.log_store.get_value(iter, 3)

        if value.startswith('http'):
            import webbrowser

            webbrowser.open(value)

    def on_log_event_inserted(self, time_range, events):
        self.log_view.freeze_child_notify()
        #self.log_view.set_model(None)

        for e in events:
            # Modify events correspond with editing cycles which are already tracked..
            if e.interpretation == zg.ModifyEvent:
                continue

            id = e.id
            time = datetime.fromtimestamp(int(e.timestamp) / 1e3).strftime("%Y-%m-%d %H:%M:%S")
            actor = e.actor
            subject = e.subjects[0].uri
            type = abbreviate(e.interpretation)
            payload = ''.join([chr(c) for c in e.payload])

            zoom = self.__get_zoom(payload)

            if e.interpretation == zg.MoveEvent:
                payload = e.subjects[0].uri + u' \u2192 ' + e.subjects[0].current_uri
            elif e.interpretation == art.EditEvent:
                payload = 'Edit ' + self.__get_display_string(payload)
            elif e.interpretation == art.UndoEvent:
                payload = 'Undo'
            elif e.interpretation == art.RedoEvent:
                payload = 'Redo'
            elif e.interpretation == art.BeginEditingEvent:
                payload = 'Open file'
            elif e.interpretation == art.EndEditingEvent:
                payload = 'Close file'
            elif payload == '':
                payload = subject

            row = [time, actor, type, subject, payload, self.get_colour(actor), self.get_pixbuf(actor, e.interpretation), zoom, id]

            if not self.event_loader.completed:
                self.log_store.append(row)
            else:
                self.log_store.insert(0, row)

        #self.log_view.set_model(self.log_store)
        self.log_view.thaw_child_notify()

        stats = EditingStatistics()
        stats.calculate(self.log_store)

        self.__editinfo.update(stats)

        self.update_histogram()

    def __get_zoom(self, payload):
        if payload is None or payload == '':
            return ''

        match = re.match('.*zoom=\"([0-9]+(,[0-9]+)*)\"', payload)

        if match:
            z = match.group(1).replace(',','.')

            return '{0:.0f}%'.format(float(z) * 100)
        else:
            return ''

    def __get_display_string(self, payload):
        if payload is None or payload == '':
            return ''

        matches = re.findall('<artsvg:change.*?>', payload)

        if len(matches) > 0:
            return ''.join(matches)
        else:
            return ''

    def on_log_event_deleted(self, time_range, event_ids):
        # remove events from journal
        print event_ids

    def on_file_selected(self, param):
        self.__filename = self.open_button.get_filename()

        self.__fileinfo.set_file(self.__filename)

        self.log_store.clear()

        self.update_stats(self.__filename)

        self.load_file_events(self.__filename)

    def load_file_events(self, filename):
        self.subject_tracker = FileTracker(self.log, self.on_file_uris_resolved)
        self.subject_tracker.get_file_uris(filename)

    def on_file_uris_resolved(self, tracker):
        self.event_loader = FileEventLoader(self.log, tracker.uris, self.on_file_events_received, self.on_file_events_loaded)
        self.event_loader.load_events()

    def on_file_events_received(self, events):
        self.on_log_event_inserted(None, events)

    def on_file_events_loaded(self):
        self.update_histogram()

    def on_export_clicked(self, event):
        with open('export.csv', 'wb') as csvfile:
            writer = csv.writer(csvfile, delimiter=";")
            for x in self.log_store:
                writer.writerow([x[0], x[1], x[2], x[3], x[4]])

if __name__ == "__main__":
    window = ArtivityJournal()
    window.resize(1200, 700)

    Gtk.main()
