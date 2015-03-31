/*
 * Horizontal/vertical but can also be angled line
 *
 * Authors:
 *   Lauris Kaplinski <lauris@kaplinski.com>
 *   Johan Engelen
 *   Maximilian Albert <maximilian.albert@gmail.com>
 *   Jon A. Cruz <jon@joncruz.org>
 *
 * Copyright (C) 2000-2002 Lauris Kaplinski
 * Copyright (C) 2007 Johan Engelen
 * Copyright (C) 2009 Maximilian Albert
 *
 * Released under GNU GPL, read the file 'COPYING' for more information
 */

#include <2geom/coord.h>
#include <2geom/transforms.h>
#include "sp-canvas-util.h"
#include "sp-ctrlpoint.h"
#include "guideline.h"
#include "display/cairo-utils.h"

#include "inkscape.h" // for inkscape_active_desktop()
#include "desktop.h"
#include "sp-namedview.h"
#include "display/sp-canvas.h"
#include "ui/control-manager.h"

using Inkscape::ControlManager;

static void sp_guideline_class_init(SPGuideLineClass *c);
static void sp_guideline_init(SPGuideLine *guideline);
static void sp_guideline_destroy(SPCanvasItem *object);

static void sp_guideline_update(SPCanvasItem *item, Geom::Affine const &affine, unsigned int flags);
static void sp_guideline_render(SPCanvasItem *item, SPCanvasBuf *buf);

static double sp_guideline_point(SPCanvasItem *item, Geom::Point p, SPCanvasItem **actual_item);

static void sp_guideline_drawline (SPCanvasBuf *buf, gint x0, gint y0, gint x1, gint y1, guint32 rgba);

static SPCanvasItemClass *parent_class;

GType sp_guideline_get_type()
{
    static GType guideline_type = 0;

    if (!guideline_type) {
        static GTypeInfo const guideline_info = {
            sizeof (SPGuideLineClass),
            NULL, NULL,
            (GClassInitFunc) sp_guideline_class_init,
            NULL, NULL,
            sizeof (SPGuideLine),
            16,
            (GInstanceInitFunc) sp_guideline_init,
            NULL,
        };

        guideline_type = g_type_register_static(SP_TYPE_CANVAS_ITEM, "SPGuideLine", &guideline_info, (GTypeFlags) 0);
    }

    return guideline_type;
}

static void sp_guideline_class_init(SPGuideLineClass *c)
{
    parent_class = SP_CANVAS_ITEM_CLASS(g_type_class_peek_parent(c));

    SPCanvasItemClass *item_class = SP_CANVAS_ITEM_CLASS(c);
    item_class->destroy = sp_guideline_destroy;
    item_class->update = sp_guideline_update;
    item_class->render = sp_guideline_render;
    item_class->point = sp_guideline_point;
}

static void sp_guideline_init(SPGuideLine *gl)
{
    gl->rgba = 0x0000ff7f;

    gl->normal_to_line = Geom::Point(0,1);
    gl->angle = 3.14159265358979323846/2;
    gl->point_on_line = Geom::Point(0,0);
    gl->sensitive = 0;

    gl->origin = NULL;
    gl->label = NULL;
}

static void sp_guideline_destroy(SPCanvasItem *object)
{
    g_return_if_fail (object != NULL);
    g_return_if_fail (SP_IS_GUIDELINE (object));
    //g_return_if_fail (SP_GUIDELINE(object)->origin != NULL);
    //g_return_if_fail (SP_IS_CTRLPOINT(SP_GUIDELINE(object)->origin));

    SPGuideLine *gl = SP_GUIDELINE(object);

    if (gl->origin != NULL && SP_IS_CTRLPOINT(gl->origin)) {
        sp_canvas_item_destroy(gl->origin);
    } else {
        // FIXME: This branch shouldn't be reached (although it seems to be harmless).
        //g_error("Why can it be that gl->origin is not a valid SPCtrlPoint?\n");
    }

    if (gl->label) {
        g_free(gl->label);
    }

    SP_CANVAS_ITEM_CLASS(parent_class)->destroy(object);
}

static void sp_guideline_render(SPCanvasItem *item, SPCanvasBuf *buf)
{
    //TODO: the routine that renders the label of a specific guideline sometimes
    // ends up erasing the labels of the other guidelines.
    // Maybe we should render all labels everytime.

    SPGuideLine const *gl = SP_GUIDELINE (item);

    cairo_save(buf->ct);
    cairo_translate(buf->ct, -buf->rect.left(), -buf->rect.top());
    ink_cairo_set_source_rgba32(buf->ct, gl->rgba);
    cairo_set_line_width(buf->ct, 1);
    cairo_set_line_cap(buf->ct, CAIRO_LINE_CAP_SQUARE);
    cairo_set_font_size(buf->ct, 10);

    Geom::Point normal_dt = /*unit_vector*/(gl->normal_to_line * gl->affine.withoutTranslation()); // note that normal_dt does not have unit length
    Geom::Point point_on_line_dt = gl->point_on_line * gl->affine;

    if (gl->label) {
        int px = round(point_on_line_dt[Geom::X]);
        int py = round(point_on_line_dt[Geom::Y]);
        cairo_save(buf->ct);
        cairo_translate(buf->ct, px, py);
        cairo_rotate(buf->ct, atan2(normal_dt.cw()));
        cairo_translate(buf->ct, 0, -5);
        cairo_move_to(buf->ct, 0, 0);
        cairo_show_text(buf->ct, gl->label);
        cairo_restore(buf->ct);
    }

    if ( Geom::are_near(normal_dt[Geom::Y], 0.) ) { // is vertical?
        int position = round(point_on_line_dt[Geom::X]);
        cairo_move_to(buf->ct, position + 0.5, buf->rect.top() + 0.5);
        cairo_line_to(buf->ct, position + 0.5, buf->rect.bottom() - 0.5);
        cairo_stroke(buf->ct);
    } else if ( Geom::are_near(normal_dt[Geom::X], 0.) ) { // is horizontal?
        int position = round(point_on_line_dt[Geom::Y]);
        cairo_move_to(buf->ct, buf->rect.left() + 0.5, position + 0.5);
        cairo_line_to(buf->ct, buf->rect.right() - 0.5, position + 0.5);
        cairo_stroke(buf->ct);
    } else {
        // render angled line. Once intersection has been detected, draw from there.
        Geom::Point parallel_to_line( normal_dt.ccw() );

        //try to intersect with left vertical of rect
        double y_intersect_left = (buf->rect.left() - point_on_line_dt[Geom::X]) * parallel_to_line[Geom::Y] / parallel_to_line[Geom::X] + point_on_line_dt[Geom::Y];
        if ( (y_intersect_left >= buf->rect.top()) && (y_intersect_left <= buf->rect.bottom()) ) {
            // intersects with left vertical!
            double y_intersect_right = (buf->rect.right() - point_on_line_dt[Geom::X]) * parallel_to_line[Geom::Y] / parallel_to_line[Geom::X] + point_on_line_dt[Geom::Y];
            sp_guideline_drawline (buf, buf->rect.left(), static_cast<gint>(round(y_intersect_left)), buf->rect.right(), static_cast<gint>(round(y_intersect_right)), gl->rgba);
            goto end;
        }

        //try to intersect with right vertical of rect
        double y_intersect_right = (buf->rect.right() - point_on_line_dt[Geom::X]) * parallel_to_line[Geom::Y] / parallel_to_line[Geom::X] + point_on_line_dt[Geom::Y];
        if ( (y_intersect_right >= buf->rect.top()) && (y_intersect_right <= buf->rect.bottom()) ) {
            // intersects with right vertical!
            sp_guideline_drawline (buf, buf->rect.right(), static_cast<gint>(round(y_intersect_right)), buf->rect.left(), static_cast<gint>(round(y_intersect_left)), gl->rgba);
            goto end;
        }

        //try to intersect with top horizontal of rect
        double x_intersect_top = (buf->rect.top() - point_on_line_dt[Geom::Y]) * parallel_to_line[Geom::X] / parallel_to_line[Geom::Y] + point_on_line_dt[Geom::X];
        if ( (x_intersect_top >= buf->rect.left()) && (x_intersect_top <= buf->rect.right()) ) {
            // intersects with top horizontal!
            double x_intersect_bottom = (buf->rect.bottom() - point_on_line_dt[Geom::Y]) * parallel_to_line[Geom::X] / parallel_to_line[Geom::Y] + point_on_line_dt[Geom::X];
            sp_guideline_drawline (buf, static_cast<gint>(round(x_intersect_top)), buf->rect.top(), static_cast<gint>(round(x_intersect_bottom)), buf->rect.bottom(), gl->rgba);
            goto end;
        }

        //try to intersect with bottom horizontal of rect
        double x_intersect_bottom = (buf->rect.bottom() - point_on_line_dt[Geom::Y]) * parallel_to_line[Geom::X] / parallel_to_line[Geom::Y] + point_on_line_dt[Geom::X];
        if ( (x_intersect_top >= buf->rect.left()) && (x_intersect_top <= buf->rect.right()) ) {
            // intersects with bottom horizontal!
            sp_guideline_drawline (buf, static_cast<gint>(round(x_intersect_bottom)), buf->rect.bottom(), static_cast<gint>(round(x_intersect_top)), buf->rect.top(), gl->rgba);
            goto end;
        }
    }
    end:
    cairo_restore(buf->ct);
}

static void sp_guideline_update(SPCanvasItem *item, Geom::Affine const &affine, unsigned int flags)
{
    SPGuideLine *gl = SP_GUIDELINE(item);

    if ((SP_CANVAS_ITEM_CLASS(parent_class))->update) {
        (SP_CANVAS_ITEM_CLASS(parent_class))->update(item, affine, flags);
    }

    gl->affine = affine;

    sp_ctrlpoint_set_coords(gl->origin, gl->point_on_line);
    sp_canvas_item_request_update(SP_CANVAS_ITEM (gl->origin));

    Geom::Point pol_transformed = gl->point_on_line*affine;
    if (gl->is_horizontal()) {
        sp_canvas_update_bbox (item, -1000000, round(pol_transformed[Geom::Y] - 16), 1000000, round(pol_transformed[Geom::Y] + 1));
    } else if (gl->is_vertical()) {
        sp_canvas_update_bbox (item, round(pol_transformed[Geom::X]), -1000000, round(pol_transformed[Geom::X] + 16), 1000000);
    } else {
        //TODO: labels in angled guidelines are not showing up for some reason.
        sp_canvas_update_bbox (item, -1000000, -1000000, 1000000, 1000000);
    }
}

// Returns 0.0 if point is on the guideline
static double sp_guideline_point(SPCanvasItem *item, Geom::Point p, SPCanvasItem **actual_item)
{
    SPGuideLine *gl = SP_GUIDELINE (item);

    if (!gl->sensitive) {
        return Geom::infinity();
    }

    *actual_item = item;

    Geom::Point vec = gl->normal_to_line * gl->affine.withoutTranslation();
    double distance = Geom::dot((p - gl->point_on_line * gl->affine), unit_vector(vec));
    return MAX(fabs(distance)-1, 0);
}

SPCanvasItem *sp_guideline_new(SPCanvasGroup *parent, char* label, Geom::Point point_on_line, Geom::Point normal)
{
    SPCanvasItem *item = sp_canvas_item_new(parent, SP_TYPE_GUIDELINE, NULL);
    SPCanvasItem *origin = ControlManager::getManager().createControl(parent, Inkscape::CTRL_TYPE_ORIGIN);
    ControlManager::getManager().track(origin);

    SPGuideLine *gl = SP_GUIDELINE(item);
    SPCtrlPoint *cp = SP_CTRLPOINT(origin);
    gl->origin = cp;

    normal.normalize();
    gl->label = label;
    gl->normal_to_line = normal;
    gl->angle = tan( -gl->normal_to_line[Geom::X] / gl->normal_to_line[Geom::Y]);
    sp_guideline_set_position(gl, point_on_line);

    sp_ctrlpoint_set_coords(cp, point_on_line);

    return item;
}

void sp_guideline_set_label(SPGuideLine *gl, const char* label)
{
    if (gl->label) {
        g_free(gl->label);
    }
    gl->label = g_strdup(label);

    sp_canvas_item_request_update(SP_CANVAS_ITEM (gl));
}

void sp_guideline_set_position(SPGuideLine *gl, Geom::Point point_on_line)
{
    gl->point_on_line = point_on_line;
    sp_canvas_item_request_update(SP_CANVAS_ITEM (gl));
}

void sp_guideline_set_normal(SPGuideLine *gl, Geom::Point normal_to_line)
{
    gl->normal_to_line = normal_to_line;
    gl->angle = tan( -normal_to_line[Geom::X] / normal_to_line[Geom::Y]);

    sp_canvas_item_request_update(SP_CANVAS_ITEM (gl));
}

void sp_guideline_set_color(SPGuideLine *gl, unsigned int rgba)
{
    gl->rgba = rgba;
    sp_ctrlpoint_set_color(gl->origin, rgba);

    sp_canvas_item_request_update(SP_CANVAS_ITEM(gl));
}

void sp_guideline_set_sensitive(SPGuideLine *gl, int sensitive)
{
    gl->sensitive = sensitive;
}

void sp_guideline_delete(SPGuideLine *gl)
{
    //gtk_object_destroy(GTK_OBJECT(gl->origin));
    sp_canvas_item_destroy(SP_CANVAS_ITEM(gl));
}

static void
sp_guideline_drawline (SPCanvasBuf *buf, gint x0, gint y0, gint x1, gint y1, guint32 /*rgba*/)
{
    cairo_move_to(buf->ct, x0 + 0.5, y0 + 0.5);
    cairo_line_to(buf->ct, x1 + 0.5, y1 + 0.5);
    cairo_stroke(buf->ct);
}

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
