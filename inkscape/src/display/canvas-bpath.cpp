/*
 * Simple bezier bpath CanvasItem for inkscape
 *
 * Authors:
 *   Lauris Kaplinski <lauris@ximian.com>
 *   Jon A. Cruz <jon@joncruz.org>
 *
 * Copyright (C) 2001 Lauris Kaplinski and Ximian, Inc.
 *
 * Released under GNU GPL
 *
 */

#ifdef HAVE_CONFIG_H
# include "config.h"
#endif
#include <sstream>
#include <string.h>
#include "desktop.h"

#include "color.h"
#include "display/sp-canvas-group.h"
#include "display/sp-canvas-util.h"
#include "display/canvas-bpath.h"
#include "display/curve.h"
#include "display/cairo-utils.h"
#include "helper/geom.h"
#include "display/sp-canvas.h"

static void sp_canvas_bpath_class_init (SPCanvasBPathClass *klass);
static void sp_canvas_bpath_init (SPCanvasBPath *path);
static void sp_canvas_bpath_destroy(SPCanvasItem *object);

static void sp_canvas_bpath_update (SPCanvasItem *item, Geom::Affine const &affine, unsigned int flags);
static void sp_canvas_bpath_render (SPCanvasItem *item, SPCanvasBuf *buf);
static double sp_canvas_bpath_point (SPCanvasItem *item, Geom::Point p, SPCanvasItem **actual_item);

static SPCanvasItemClass *parent_class;

GType
sp_canvas_bpath_get_type (void)
{
    static GType type = 0;
    if (!type) {
	GTypeInfo info = {
            sizeof (SPCanvasBPathClass),
	    NULL, NULL,
            (GClassInitFunc) sp_canvas_bpath_class_init,
	    NULL, NULL,
            sizeof (SPCanvasBPath),
	    0,
            (GInstanceInitFunc) sp_canvas_bpath_init,
	    NULL
	};
        type = g_type_register_static (SP_TYPE_CANVAS_ITEM, "SPCanvasBPath", &info, (GTypeFlags)0);
    }
    return type;
}

static void sp_canvas_bpath_class_init(SPCanvasBPathClass *klass)
{
    SPCanvasItemClass *item_class = (SPCanvasItemClass *) klass;

    parent_class = (SPCanvasItemClass*)g_type_class_peek_parent (klass);

    item_class->destroy = sp_canvas_bpath_destroy;
    item_class->update = sp_canvas_bpath_update;
    item_class->render = sp_canvas_bpath_render;
    item_class->point = sp_canvas_bpath_point;
}

static void
sp_canvas_bpath_init (SPCanvasBPath * bpath)
{
    bpath->fill_rgba = 0x00000000;
    bpath->fill_rule = SP_WIND_RULE_EVENODD;

    bpath->stroke_rgba = 0x00000000;
    bpath->stroke_width = 1.0;
    bpath->stroke_linejoin = SP_STROKE_LINEJOIN_MITER;
    bpath->stroke_linecap = SP_STROKE_LINECAP_BUTT;
    bpath->stroke_miterlimit = 11.0;
}

static void sp_canvas_bpath_destroy(SPCanvasItem *object)
{
    SPCanvasBPath *cbp = SP_CANVAS_BPATH (object);

    if (cbp->curve) {
        cbp->curve = cbp->curve->unref();
    }

    if (SP_CANVAS_ITEM_CLASS(parent_class)->destroy)
        (* SP_CANVAS_ITEM_CLASS(parent_class)->destroy) (object);
}

static void sp_canvas_bpath_update(SPCanvasItem *item, Geom::Affine const &affine, unsigned int flags)
{
    SPCanvasBPath *cbp = SP_CANVAS_BPATH(item);

    item->canvas->requestRedraw((int)item->x1, (int)item->y1, (int)item->x2, (int)item->y2);

    if (reinterpret_cast<SPCanvasItemClass *>(parent_class)->update) {
        reinterpret_cast<SPCanvasItemClass *>(parent_class)->update(item, affine, flags);
    }

    sp_canvas_item_reset_bounds (item);

    if (!cbp->curve) return;

    cbp->affine = affine;

    Geom::OptRect bbox = bounds_exact_transformed(cbp->curve->get_pathvector(), affine);

    if (bbox) {
        item->x1 = (int)bbox->min()[Geom::X] - 1;
        item->y1 = (int)bbox->min()[Geom::Y] - 1;
        item->x2 = (int)bbox->max()[Geom::X] + 1;
        item->y2 = (int)bbox->max()[Geom::Y] + 1;
    } else {
        item->x1 = 0;
        item->y1 = 0;
        item->x2 = 0;
        item->y2 = 0;
    }
    item->canvas->requestRedraw((int)item->x1, (int)item->y1, (int)item->x2, (int)item->y2);
}

static void
sp_canvas_bpath_render (SPCanvasItem *item, SPCanvasBuf *buf)
{
    SPCanvasBPath *cbp = SP_CANVAS_BPATH (item);

    Geom::Rect area = buf->rect;

    if ( !cbp->curve  || 
         ((cbp->stroke_rgba & 0xff) == 0 && (cbp->fill_rgba & 0xff) == 0 ) || 
         cbp->curve->get_segment_count() < 1)
        return;

    if (!buf->ct)
        return;

    bool dofill = ((cbp->fill_rgba & 0xff) != 0);
    bool dostroke = ((cbp->stroke_rgba & 0xff) != 0);

    cairo_set_tolerance(buf->ct, 0.5);
    cairo_new_path(buf->ct);

    feed_pathvector_to_cairo (buf->ct, cbp->curve->get_pathvector(), cbp->affine, area,
        /* optimized_stroke = */ !dofill, 1);

    if (dofill) {
        // RGB / BGR
        ink_cairo_set_source_rgba32(buf->ct, cbp->fill_rgba);
        cairo_set_fill_rule(buf->ct, cbp->fill_rule == SP_WIND_RULE_EVENODD? CAIRO_FILL_RULE_EVEN_ODD
                            : CAIRO_FILL_RULE_WINDING);
        cairo_fill_preserve(buf->ct);
    }

    if (dostroke) {
        ink_cairo_set_source_rgba32(buf->ct, cbp->stroke_rgba);
        cairo_set_line_width(buf->ct, 1);
        if (cbp->dashes[0] != 0 && cbp->dashes[1] != 0) {
            cairo_set_dash (buf->ct, cbp->dashes, 2, 0);
        }
        cairo_stroke(buf->ct);
    } else {
        cairo_new_path(buf->ct);
    }
}

static double
sp_canvas_bpath_point (SPCanvasItem *item, Geom::Point p, SPCanvasItem **actual_item)
{
    SPCanvasBPath *cbp = SP_CANVAS_BPATH (item);

    if ( !cbp->curve  || 
         ((cbp->stroke_rgba & 0xff) == 0 && (cbp->fill_rgba & 0xff) == 0 ) || 
         cbp->curve->get_segment_count() < 1)
        return Geom::infinity();

    double width = 0.5;
    Geom::Rect viewbox = item->canvas->getViewbox();
    viewbox.expandBy (width);
    double dist = Geom::infinity();
    pathv_matrix_point_bbox_wind_distance(cbp->curve->get_pathvector(), cbp->affine, p, NULL, NULL, &dist, 0.5, &viewbox);

    if (dist <= 1.0) {
        *actual_item = item;
    }

    return dist;
}

SPCanvasItem *
sp_canvas_bpath_new (SPCanvasGroup *parent, SPCurve *curve)
{
    g_return_val_if_fail (parent != NULL, NULL);
    g_return_val_if_fail (SP_IS_CANVAS_GROUP (parent), NULL);

    SPCanvasItem *item = sp_canvas_item_new (parent, SP_TYPE_CANVAS_BPATH, NULL);

    sp_canvas_bpath_set_bpath (SP_CANVAS_BPATH (item), curve);

    return item;
}

void
sp_canvas_bpath_set_bpath (SPCanvasBPath *cbp, SPCurve *curve)
{
    g_return_if_fail (cbp != NULL);
    g_return_if_fail (SP_IS_CANVAS_BPATH (cbp));

    if (cbp->curve) {
        cbp->curve = cbp->curve->unref();
    }

    if (curve) {
        cbp->curve = curve->ref();
    }

    sp_canvas_item_request_update (SP_CANVAS_ITEM (cbp));
}

void
sp_canvas_bpath_set_fill (SPCanvasBPath *cbp, guint32 rgba, SPWindRule rule)
{
    g_return_if_fail (cbp != NULL);
    g_return_if_fail (SP_IS_CANVAS_BPATH (cbp));

    cbp->fill_rgba = rgba;
    cbp->fill_rule = rule;

    sp_canvas_item_request_update (SP_CANVAS_ITEM (cbp));
}

void
sp_canvas_bpath_set_stroke (SPCanvasBPath *cbp, guint32 rgba, gdouble width, SPStrokeJoinType join, SPStrokeCapType cap, double dash, double gap)
{
    g_return_if_fail (cbp != NULL);
    g_return_if_fail (SP_IS_CANVAS_BPATH (cbp));

    cbp->stroke_rgba = rgba;
    cbp->stroke_width = MAX (width, 0.1);
    cbp->stroke_linejoin = join;
    cbp->stroke_linecap = cap;
    cbp->dashes[0] = dash;
    cbp->dashes[1] = gap;

    sp_canvas_item_request_update (SP_CANVAS_ITEM (cbp));
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
