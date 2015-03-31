/*
 * SVG <marker> implementation
 *
 * Authors:
 *   Lauris Kaplinski <lauris@kaplinski.com>
 *   Bryce Harrington <bryce@bryceharrington.org>
 *   Abhishek Sharma
 *   Jon A. Cruz <jon@joncruz.org>
 *
 * Copyright (C) 1999-2003 Lauris Kaplinski
 *               2004-2006 Bryce Harrington
 *               2008      Johan Engelen
 *
 * Released under GNU GPL, read the file 'COPYING' for more information
 */

#include <cstring>
#include <string>
#include "config.h"

#include <2geom/affine.h>
#include <2geom/transforms.h>
#include "svg/svg.h"
#include "display/drawing-group.h"
#include "xml/repr.h"
#include "attributes.h"
#include "marker.h"
#include "document.h"
#include "document-private.h"
#include "preferences.h"

struct SPMarkerView {
	SPMarkerView *next;
	unsigned int key;
  std::vector<Inkscape::DrawingItem *> items;
};

static void sp_marker_view_remove (SPMarker *marker, SPMarkerView *view, unsigned int destroyitems);

#include "sp-factory.h"

namespace {
	SPObject* createMarker() {
		return new SPMarker();
	}

	bool markerRegistered = SPFactory::instance().registerObject("svg:marker", createMarker);
}

SPMarker::SPMarker() : SPGroup(), SPViewBox() {

    this->markerUnits = 0;
    this->markerUnits_set = 0;

    this->orient_mode = MARKER_ORIENT_ANGLE;
    this->orient_set = 0;
    this->orient = 0;

    this->views = NULL;
}

/**
 * Initializes an SPMarker object.  This notes the marker's viewBox is
 * not set and initializes the marker's c2p identity matrix.
 */

SPMarker::~SPMarker() {
}

/**
 * Virtual build callback for SPMarker.
 *
 * This is to be invoked immediately after creation of an SPMarker.  This
 * method fills an SPMarker object with its SVG attributes, and calls the
 * parent class' build routine to attach the object to its document and
 * repr.  The result will be creation of the whole document tree.
 *
 * \see SPObject::build()
 */
void SPMarker::build(SPDocument *document, Inkscape::XML::Node *repr) {
    this->readAttr( "markerUnits" );
    this->readAttr( "refX" );
    this->readAttr( "refY" );
    this->readAttr( "markerWidth" );
    this->readAttr( "markerHeight" );
    this->readAttr( "orient" );
    this->readAttr( "viewBox" );
    this->readAttr( "preserveAspectRatio" );

    SPGroup::build(document, repr);
}

void SPMarker::release() {
    while (this->views) {
        // Destroy all DrawingItems etc.
        // Parent class ::hide method
        //reinterpret_cast<SPItemClass *>(parent_class)->hide(marker, marker->views->key);
    	// CPPIFY: correct one?
    	SPGroup::hide(this->views->key);


        sp_marker_view_remove (this, this->views, TRUE);
    }

    SPGroup::release();
}

/**
 * Removes, releases and unrefs all children of object
 *
 * This is the inverse of sp_marker_build().  It must be invoked as soon
 * as the marker is removed from the tree, even if it is still referenced
 * by other objects.  It hides and removes any views of the marker, then
 * calls the parent classes' release function to deregister the object
 * and release its SPRepr bindings.  The result will be the destruction
 * of the entire document tree.
 *
 * \see SPObject::release()
 */

void SPMarker::set(unsigned int key, const gchar* value) {
	switch (key) {
	case SP_ATTR_MARKERUNITS:
		this->markerUnits_set = FALSE;
		this->markerUnits = SP_MARKER_UNITS_STROKEWIDTH;

		if (value) {
			if (!strcmp (value, "strokeWidth")) {
				this->markerUnits_set = TRUE;
			} else if (!strcmp (value, "userSpaceOnUse")) {
				this->markerUnits = SP_MARKER_UNITS_USERSPACEONUSE;
				this->markerUnits_set = TRUE;
			}
		}

		this->requestDisplayUpdate(SP_OBJECT_MODIFIED_FLAG | SP_OBJECT_VIEWPORT_MODIFIED_FLAG);
		break;

	case SP_ATTR_REFX:
	    this->refX.readOrUnset(value);
		this->requestDisplayUpdate(SP_OBJECT_MODIFIED_FLAG);
		break;

	case SP_ATTR_REFY:
	    this->refY.readOrUnset(value);
		this->requestDisplayUpdate(SP_OBJECT_MODIFIED_FLAG);
		break;

	case SP_ATTR_MARKERWIDTH:
	    this->markerWidth.readOrUnset(value, SVGLength::NONE, 3.0, 3.0);
		this->requestDisplayUpdate(SP_OBJECT_MODIFIED_FLAG);
		break;

	case SP_ATTR_MARKERHEIGHT:
	    this->markerHeight.readOrUnset(value, SVGLength::NONE, 3.0, 3.0);
		this->requestDisplayUpdate(SP_OBJECT_MODIFIED_FLAG);
		break;

	case SP_ATTR_ORIENT:
		this->orient_set = FALSE;
		this->orient_mode = MARKER_ORIENT_ANGLE;
		this->orient = 0.0;

		if (value) {
                    if (!strcmp (value, "auto")) {
                        this->orient_mode = MARKER_ORIENT_AUTO;
                        this->orient_set = TRUE;
                    } else if (!strcmp (value, "auto-start-reverse")) {
                        this->orient_mode = MARKER_ORIENT_AUTO_START_REVERSE;
                        this->orient_set = TRUE;
                    } else if (sp_svg_number_read_f (value, &this->orient)) {
                        this->orient_mode = MARKER_ORIENT_ANGLE;
                        this->orient_set = TRUE;
                    }
		}

		this->requestDisplayUpdate(SP_OBJECT_MODIFIED_FLAG);
		break;

	case SP_ATTR_VIEWBOX:
            set_viewBox( value );
            this->requestDisplayUpdate(SP_OBJECT_MODIFIED_FLAG | SP_OBJECT_VIEWPORT_MODIFIED_FLAG);
            break;

	case SP_ATTR_PRESERVEASPECTRATIO:
            set_preserveAspectRatio( value );
            this->requestDisplayUpdate(SP_OBJECT_MODIFIED_FLAG | SP_OBJECT_VIEWPORT_MODIFIED_FLAG);
            break;

	default:
		SPGroup::set(key, value);
		break;
	}
}

void SPMarker::update(SPCtx *ctx, guint flags) {

    SPItemCtx ictx;

    // Copy parent context
    ictx.flags = ctx->flags;

    // Initialize transformations
    ictx.i2doc = Geom::identity();
    ictx.i2vp = Geom::identity();

    // Set up viewport
    ictx.viewport = Geom::Rect::from_xywh(0, 0, this->markerWidth.computed, this->markerHeight.computed);

    SPItemCtx rctx = get_rctx( &ictx );

    // Shift according to refX, refY
    Geom::Point ref( this->refX.computed, this->refY.computed );
    ref *= c2p;
    this->c2p = this->c2p * Geom::Translate( -ref );

    // And invoke parent method
    SPGroup::update((SPCtx *) &rctx, flags);

    // As last step set additional transform of drawing group
    for (SPMarkerView *v = this->views; v != NULL; v = v->next) {
        for (unsigned i = 0 ; i < v->items.size() ; i++) {
            if (v->items[i]) {
                Inkscape::DrawingGroup *g = dynamic_cast<Inkscape::DrawingGroup *>(v->items[i]);
                g->setChildTransform(this->c2p);
            }
        }
    }
}

Inkscape::XML::Node* SPMarker::write(Inkscape::XML::Document *xml_doc, Inkscape::XML::Node *repr, guint flags) {
	if ((flags & SP_OBJECT_WRITE_BUILD) && !repr) {
		repr = xml_doc->createElement("svg:marker");
	}

	if (this->markerUnits_set) {
		if (this->markerUnits == SP_MARKER_UNITS_STROKEWIDTH) {
			repr->setAttribute("markerUnits", "strokeWidth");
		} else {
			repr->setAttribute("markerUnits", "userSpaceOnUse");
		}
	} else {
		repr->setAttribute("markerUnits", NULL);
	}

	if (this->refX._set) {
		sp_repr_set_svg_double(repr, "refX", this->refX.computed);
	} else {
		repr->setAttribute("refX", NULL);
	}

	if (this->refY._set) {
		sp_repr_set_svg_double (repr, "refY", this->refY.computed);
	} else {
		repr->setAttribute("refY", NULL);
	}

	if (this->markerWidth._set) {
		sp_repr_set_svg_double (repr, "markerWidth", this->markerWidth.computed);
	} else {
		repr->setAttribute("markerWidth", NULL);
	}

	if (this->markerHeight._set) {
		sp_repr_set_svg_double (repr, "markerHeight", this->markerHeight.computed);
	} else {
		repr->setAttribute("markerHeight", NULL);
	}

	if (this->orient_set) {
            if (this->orient_mode == MARKER_ORIENT_AUTO) {
                repr->setAttribute("orient", "auto");
            } else if (this->orient_mode == MARKER_ORIENT_AUTO_START_REVERSE) {
                repr->setAttribute("orient", "auto-start-reverse");
            } else {
                sp_repr_set_css_double(repr, "orient", this->orient);
            }
	} else {
            repr->setAttribute("orient", NULL);
	}
        
	/* fixme: */
	//XML Tree being used directly here while it shouldn't be....
	repr->setAttribute("viewBox", this->getRepr()->attribute("viewBox"));
	//XML Tree being used directly here while it shouldn't be....
	repr->setAttribute("preserveAspectRatio", this->getRepr()->attribute("preserveAspectRatio"));

	SPGroup::write(xml_doc, repr, flags);

	return repr;
}

Inkscape::DrawingItem* SPMarker::show(Inkscape::Drawing &/*drawing*/, unsigned int /*key*/, unsigned int /*flags*/) {
    // Markers in tree are never shown directly even if outside of <defs>.
    return  0;
}

Inkscape::DrawingItem* SPMarker::private_show(Inkscape::Drawing &drawing, unsigned int key, unsigned int flags) {
    return SPGroup::show(drawing, key, flags);
}

void SPMarker::hide(unsigned int key) {
	// CPPIFY: correct?
	SPGroup::hide(key);
}

Geom::OptRect SPMarker::bbox(Geom::Affine const &/*transform*/, SPItem::BBoxType /*type*/) const {
	return Geom::OptRect();
}

void SPMarker::print(SPPrintContext* /*ctx*/) {

}

/* fixme: Remove link if zero-sized (Lauris) */

/**
 * Removes any SPMarkerViews that a marker has with a specific key.
 * Set up the DrawingItem array's size in the specified SPMarker's SPMarkerView.
 * This is called from sp_shape_update() for shapes that have markers.  It
 * removes the old view of the marker and establishes a new one, registering
 * it with the marker's list of views for future updates.
 *
 * \param marker Marker to create views in.
 * \param key Key to give each SPMarkerView.
 * \param size Number of DrawingItems to put in the SPMarkerView.
 */
void
sp_marker_show_dimension (SPMarker *marker, unsigned int key, unsigned int size)
{
    SPMarkerView *view;

    for (view = marker->views; view != NULL; view = view->next) {
        if (view->key == key) break;
    }
    if (view && (view->items.size() != size)) {
        /* Free old view and allocate new */
        /* Parent class ::hide method */
    	marker->hide(key);

        sp_marker_view_remove (marker, view, TRUE);
        view = NULL;
    }
    if (!view) {
        view = new SPMarkerView();
        view->items.clear();
        for (unsigned int i = 0; i < size; i++) {
            view->items.push_back(NULL);
        }
        view->next = marker->views;
        marker->views = view;
        view->key = key;
    }
}

/**
 * Shows an instance of a marker.  This is called during sp_shape_update_marker_view()
 * show and transform a child item in the drawing for all views with the given key.
 */
Inkscape::DrawingItem *
sp_marker_show_instance ( SPMarker *marker, Inkscape::DrawingItem *parent,
                          unsigned int key, unsigned int pos,
                          Geom::Affine const &base, float linewidth)
{
    // do not show marker if linewidth == 0 and markerUnits == strokeWidth
    // otherwise Cairo will fail to render anything on the tile
    // that contains the "degenerate" marker
    if (marker->markerUnits == SP_MARKER_UNITS_STROKEWIDTH && linewidth == 0) {
        return NULL;
    }

    for (SPMarkerView *v = marker->views; v != NULL; v = v->next) {
        if (v->key == key) {
            if (pos >= v->items.size()) {
                return NULL;
            }
            if (!v->items[pos]) {
                /* Parent class ::show method */
            	v->items[pos] = marker->private_show(parent->drawing(), key, SP_ITEM_REFERENCE_FLAGS);

                if (v->items[pos]) {
                    /* fixme: Position (Lauris) */
                    parent->prependChild(v->items[pos]);
                    Inkscape::DrawingGroup *g = dynamic_cast<Inkscape::DrawingGroup *>(v->items[pos]);
                    if (g) g->setChildTransform(marker->c2p);
                }
            }
            if (v->items[pos]) {
                Geom::Affine m;
                if (marker->orient_mode == MARKER_ORIENT_AUTO) {
                    m = base;
                } else if (marker->orient_mode == MARKER_ORIENT_AUTO_START_REVERSE) {
                    m = Geom::Rotate::from_degrees( 180.0 ) * base;
                    m = base;
                } else {
                    /* fixme: Orient units (Lauris) */
                    m = Geom::Rotate::from_degrees(marker->orient);
                    m *= Geom::Translate(base.translation());
                }
                if (marker->markerUnits == SP_MARKER_UNITS_STROKEWIDTH) {
                    m = Geom::Scale(linewidth) * m;
                }
                v->items[pos]->setTransform(m);
            }
            return v->items[pos];
        }
    }

    return NULL;
}

/**
 * Hides/removes all views of the given marker that have key 'key'.
 * This replaces SPItem implementation because we have our own views
 * \param key SPMarkerView key to hide.
 */
void
sp_marker_hide (SPMarker *marker, unsigned int key)
{
	SPMarkerView *v;

	v = marker->views;
	while (v != NULL) {
		SPMarkerView *next;
		next = v->next;
		if (v->key == key) {
			/* Parent class ::hide method */
			marker->hide(key);

			sp_marker_view_remove (marker, v, TRUE);
			return;
		}
		v = next;
	}
}

/**
 * Removes a given view.  Also will destroy sub-items in the view if destroyitems
 * is set to a non-zero value.
 */
static void
sp_marker_view_remove (SPMarker *marker, SPMarkerView *view, unsigned int destroyitems)
{
	if (view == marker->views) {
		marker->views = view->next;
	} else {
		SPMarkerView *v;
		for (v = marker->views; v->next != view; v = v->next) if (!v->next) return;
		v->next = view->next;
	}
	if (destroyitems) {
      for (unsigned int i = 0; i < view->items.size(); i++) {
			/* We have to walk through the whole array because there may be hidden items */
			delete view->items[i];
		}
	}
    view->items.clear();
    delete view;
}

const gchar *generate_marker(GSList *reprs, Geom::Rect bounds, SPDocument *document, Geom::Point center, Geom::Affine move)
{
    Inkscape::XML::Document *xml_doc = document->getReprDoc();
    Inkscape::XML::Node *defsrepr = document->getDefs()->getRepr();

    Inkscape::XML::Node *repr = xml_doc->createElement("svg:marker");

    // Uncommenting this will make the marker fixed-size independent of stroke width.
    // Commented out for consistency with standard markers which scale when you change
    // stroke width:
    //repr->setAttribute("markerUnits", "userSpaceOnUse");

    sp_repr_set_svg_double(repr, "markerWidth", bounds.dimensions()[Geom::X]);
    sp_repr_set_svg_double(repr, "markerHeight", bounds.dimensions()[Geom::Y]);
    sp_repr_set_svg_double(repr, "refX", center[Geom::X]);
    sp_repr_set_svg_double(repr, "refY", center[Geom::Y]);

    repr->setAttribute("orient", "auto");

    defsrepr->appendChild(repr);
    const gchar *mark_id = repr->attribute("id");
    SPObject *mark_object = document->getObjectById(mark_id);

    for (GSList *i = reprs; i != NULL; i = i->next) {
        Inkscape::XML::Node *node = (Inkscape::XML::Node *)(i->data);
        SPItem *copy = SP_ITEM(mark_object->appendChildRepr(node));

        Geom::Affine dup_transform;
        if (!sp_svg_transform_read (node->attribute("transform"), &dup_transform))
            dup_transform = Geom::identity();
        dup_transform *= move;

        copy->doWriteTransform(copy->getRepr(), dup_transform);
    }

    Inkscape::GC::release(repr);
    return mark_id;
}

SPObject *sp_marker_fork_if_necessary(SPObject *marker)
{
    if (marker->hrefcount < 2) {
        return marker;
    }

    Inkscape::Preferences *prefs = Inkscape::Preferences::get();
    gboolean colorStock = prefs->getBool("/options/markers/colorStockMarkers", true);
    gboolean colorCustom = prefs->getBool("/options/markers/colorCustomMarkers", false);
    const gchar *stock = marker->getRepr()->attribute("inkscape:isstock");
    gboolean isStock = (!stock || !strcmp(stock,"true"));

    if (isStock ? !colorStock : !colorCustom) {
        return marker;
    }

    SPDocument *doc = marker->document;
    Inkscape::XML::Document *xml_doc = doc->getReprDoc();
    // Turn off garbage-collectable or it might be collected before we can use it
    marker->getRepr()->setAttribute("inkscape:collect", NULL);
    Inkscape::XML::Node *mark_repr = marker->getRepr()->duplicate(xml_doc);
    doc->getDefs()->getRepr()->addChild(mark_repr, NULL);
    if (!mark_repr->attribute("inkscape:stockid")) {
        mark_repr->setAttribute("inkscape:stockid", mark_repr->attribute("id"));
    }
    marker->getRepr()->setAttribute("inkscape:collect", "always");

    SPObject *marker_new = static_cast<SPObject *>(doc->getObjectByRepr(mark_repr));
    Inkscape::GC::release(mark_repr);
    return marker_new;
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
// vim: filetype=cpp:expandtab:shiftwidth=4:tabstop=8:softtabstop=4 :
