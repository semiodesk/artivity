#ifndef __SP_MARKER_H__
#define __SP_MARKER_H__

/*
 * SVG <marker> implementation
 *
 * Authors:
 *   Lauris Kaplinski <lauris@kaplinski.com>
 *
 * Copyright (C) 1999-2003 Lauris Kaplinski
 * Copyright (C) 2008      Johan Engelen
 *
 * Released under GNU GPL, read the file 'COPYING' for more information
 */

/*
 * This is quite similar in logic to <svg>
 * Maybe we should merge them somehow (Lauris)
 */

#define SP_TYPE_MARKER (sp_marker_get_type ())
#define SP_MARKER(obj) (dynamic_cast<SPMarker*>((SPObject*)obj))
#define SP_IS_MARKER(obj) (dynamic_cast<const SPMarker*>((SPObject*)obj) != NULL)

struct SPMarkerView;

#include <2geom/rect.h>
#include <2geom/affine.h>
#include "svg/svg-length.h"
#include "enums.h"
#include "sp-item-group.h"
#include "uri-references.h"
#include "viewbox.h"

enum markerOrient {
  MARKER_ORIENT_ANGLE,
  MARKER_ORIENT_AUTO,
  MARKER_ORIENT_AUTO_START_REVERSE
};

class SPMarker : public SPGroup, public SPViewBox {
public:
	SPMarker();
	virtual ~SPMarker();

	/* units */
	unsigned int markerUnits_set : 1;
	unsigned int markerUnits : 1;

	/* reference point */
	SVGLength refX;
	SVGLength refY;

	/* dimensions */
	SVGLength markerWidth;
	SVGLength markerHeight;

	/* orient */
	unsigned int orient_set : 1;
	markerOrient orient_mode : 2;
	float orient;

	/* Private views */
	SPMarkerView *views;

	virtual void build(SPDocument *document, Inkscape::XML::Node *repr);
	virtual void release();
	virtual void set(unsigned int key, gchar const* value);
	virtual void update(SPCtx *ctx, guint flags);
	virtual Inkscape::XML::Node* write(Inkscape::XML::Document *xml_doc, Inkscape::XML::Node *repr, guint flags);

	virtual Inkscape::DrawingItem* show(Inkscape::Drawing &drawing, unsigned int key, unsigned int flags);
	virtual Inkscape::DrawingItem* private_show(Inkscape::Drawing &drawing, unsigned int key, unsigned int flags);
	virtual void hide(unsigned int key);

	virtual Geom::OptRect bbox(Geom::Affine const &transform, SPItem::BBoxType type) const;
	virtual void print(SPPrintContext *ctx);
};

class SPMarkerReference : public Inkscape::URIReference {
	SPMarkerReference(SPObject *obj) : URIReference(obj) {}
	SPMarker *getObject() const {
		return static_cast<SPMarker *>(URIReference::getObject());
	}
protected:
	virtual bool _acceptObject(SPObject *obj) const {
		return SP_IS_MARKER(obj);
	}
};

void sp_marker_show_dimension (SPMarker *marker, unsigned int key, unsigned int size);
Inkscape::DrawingItem *sp_marker_show_instance (SPMarker *marker, Inkscape::DrawingItem *parent,
				      unsigned int key, unsigned int pos,
				      Geom::Affine const &base, float linewidth);
void sp_marker_hide (SPMarker *marker, unsigned int key);
const gchar *generate_marker (GSList *reprs, Geom::Rect bounds, SPDocument *document, Geom::Point center, Geom::Affine move);
SPObject *sp_marker_fork_if_necessary(SPObject *marker);

#endif
