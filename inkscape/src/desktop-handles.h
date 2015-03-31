#ifndef SEEN_SP_DESKTOP_HANDLES_H
#define SEEN_SP_DESKTOP_HANDLES_H

/*
 * Frontends
 *
 * Authors:
 *   Lauris Kaplinski <lauris@kaplinski.com>
 *
 * Copyright (C) 1999-2002 Lauris Kaplinski
 * Copyright (C) 2000-2001 Ximian, Inc.
 *
 * Released under GNU GPL, read the file 'COPYING' for more information
 */


class SPDesktop;
class SPDocument;

namespace Inkscape {
namespace UI {
namespace Tools {

class ToolBase;

}
}
}

class SPNamedView;
struct SPCanvas;
struct SPCanvasGroup;
struct SPCanvasItem;

namespace Inkscape { 
    class MessageStack;
    class Selection; 
}

#define SP_DESKTOP_ZOOM_MAX 256.0
#define SP_DESKTOP_ZOOM_MIN 0.01

#define SP_COORDINATES_UNDERLINE_NONE (0)
#define SP_COORDINATES_UNDERLINE_X (1 << Geom::X)
#define SP_COORDINATES_UNDERLINE_Y (1 << Geom::Y)

//ToolBase * sp_desktop_event_context (SPDesktop const * desktop);
Inkscape::Selection * sp_desktop_selection (SPDesktop const * desktop);
SPDocument * sp_desktop_document (SPDesktop const * desktop);
SPCanvas * sp_desktop_canvas (SPDesktop const * desktop);
SPCanvasItem * sp_desktop_acetate (SPDesktop const * desktop);
SPCanvasGroup * sp_desktop_main (SPDesktop const * desktop);
SPCanvasGroup * sp_desktop_gridgroup (SPDesktop const * desktop);
SPCanvasGroup * sp_desktop_guides (SPDesktop const * desktop);
SPCanvasItem *sp_desktop_drawing (SPDesktop const *desktop);
SPCanvasGroup * sp_desktop_sketch (SPDesktop const * desktop);
SPCanvasGroup * sp_desktop_controls (SPDesktop const * desktop);
SPCanvasGroup * sp_desktop_tempgroup (SPDesktop const * desktop);
Inkscape::MessageStack * sp_desktop_message_stack (SPDesktop const * desktop);
SPNamedView * sp_desktop_namedview (SPDesktop const * desktop);

#endif // SEEN_SP_DESKTOP_HANDLES_H

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
