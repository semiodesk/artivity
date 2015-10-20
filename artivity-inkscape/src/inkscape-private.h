#ifndef SEEN_INKSCAPE_PRIVATE_H
#define SEEN_INKSCAPE_PRIVATE_H

/*
 * Some forward declarations
 *
 * Authors:
 *   Lauris Kaplinski <lauris@kaplinski.com>
 *
 * Copyright (C) 1999-2002 Lauris Kaplinski
 * Copyright (C) 2001-2002 Ximian, Inc.
 *
 * Released under GNU GPL, read the file 'COPYING' for more information
 */
#include <glib-object.h>

#define SP_TYPE_INKSCAPE (inkscape_get_type ())
#define SP_INKSCAPE(obj) (G_TYPE_CHECK_INSTANCE_CAST ((obj), SP_TYPE_INKSCAPE, Inkscape))
#define SP_INKSCAPE_CLASS(klass) (G_TYPE_CHECK_CLASS_CAST ((klass), SP_TYPE_INKSCAPE, InkscapeClass))
#define SP_IS_INKSCAPE(obj) (G_TYPE_CHECK_INSTANCE_TYPE ((obj), SP_TYPE_INKSCAPE))
#define SP_IS_INKSCAPE_CLASS(klass) (G_TYPE_CHECK_CLASS_TYPE ((klass), SP_TYPE_INKSCAPE))

#include "inkscape.h"

struct SPColor;
namespace Inkscape { class Selection; }

GType inkscape_get_type (void);

void inkscape_ref (void);
void inkscape_unref (void);

guint inkscape_mapalt();
void inkscape_mapalt(guint);

guint inkscape_trackalt();
void inkscape_trackalt(guint);

/*
 * These are meant solely for desktop, document etc. implementations
 */

void inkscape_selection_modified (Inkscape::Selection *selection, guint flags);
void inkscape_selection_changed (Inkscape::Selection * selection);
void inkscape_selection_set (Inkscape::Selection * selection);
void inkscape_eventcontext_set (Inkscape::UI::Tools::ToolBase * eventcontext);
void inkscape_add_desktop (SPDesktop * desktop);
void inkscape_remove_desktop (SPDesktop * desktop);
void inkscape_activate_desktop (SPDesktop * desktop);
void inkscape_reactivate_desktop (SPDesktop * desktop);

void inkscape_set_color (SPColor *color, float opacity);

#endif // SEEN_INKSCAPE_PRIVATE_H



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
