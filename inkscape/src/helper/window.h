#ifndef __SP_WINDOW_H__
#define __SP_WINDOW_H__

/*
 * Generic window implementation
 *
 * Author:
 *   Lauris Kaplinski <lauris@kaplinski.com>
 *
 * This code is in public domain
 */

struct _GtkWidget;
typedef _GtkWidget GtkWidget;

namespace Gtk {
class Window;
}

/*
 * This function is deprecated. Use Inkscape::UI::window_new instead.
 */
GtkWidget *sp_window_new (const gchar *title, unsigned int resizeable);

namespace Inkscape {
namespace UI {

Gtk::Window *window_new (const gchar *title, unsigned int resizeable);

}
}

#endif

/*
  Local Variables:
  mode:c++
  c-file-style:"stroustrup"
  c-file-offsets:((innamespace . 0)(inline-open . 0)(case-label . +))
  indent-tabs-mode:nil
  fill-column:99
  End:
*/
// vim: filetype=cpp:expandtab:shiftwidth=4:tabstop=8:softtabstop=4:fileencoding=utf-8:textwidth=99 :
