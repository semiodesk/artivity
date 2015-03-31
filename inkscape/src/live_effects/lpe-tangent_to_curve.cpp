/** \file
 * Implementation of tangent-to-curve LPE.
 */

/*
 * Authors:
 *   Johan Engelen
 *   Maximilian Albert
 *
 * Copyright (C) Johan Engelen 2007 <j.b.c.engelen@utwente.nl>
 * Copyright (C) Maximilian Albert 2008 <maximilian.albert@gmail.com>
 *
 * Released under GNU GPL, read the file 'COPYING' for more information
 */

#include <glibmm/i18n.h>

#include "live_effects/lpe-tangent_to_curve.h"
// FIXME: The following are only needed to convert the path's SPCurve* to pwd2.
//        There must be a more convenient way to achieve this.
#include "sp-path.h"
#include "display/curve.h"

#include <2geom/path.h>
#include <2geom/transforms.h>

#include "knot-holder-entity.h"
#include "knotholder.h"

namespace Inkscape {
namespace LivePathEffect {

namespace TtC {

class KnotHolderEntityAttachPt : public LPEKnotHolderEntity {
public:
    KnotHolderEntityAttachPt(LPETangentToCurve *effect) : LPEKnotHolderEntity(effect) {};
    virtual void knot_set(Geom::Point const &p, Geom::Point const &origin, guint state);
    virtual Geom::Point knot_get() const;
};

class KnotHolderEntityLeftEnd : public LPEKnotHolderEntity {
public:
    KnotHolderEntityLeftEnd(LPETangentToCurve *effect) : LPEKnotHolderEntity(effect) {};
    virtual void knot_set(Geom::Point const &p, Geom::Point const &origin, guint state);
    virtual Geom::Point knot_get() const;
};

class KnotHolderEntityRightEnd : public LPEKnotHolderEntity
{
public:
    KnotHolderEntityRightEnd(LPETangentToCurve *effect) : LPEKnotHolderEntity(effect) {};
    virtual void knot_set(Geom::Point const &p, Geom::Point const &origin, guint state);
    virtual Geom::Point knot_get() const;
};

} // namespace TtC

LPETangentToCurve::LPETangentToCurve(LivePathEffectObject *lpeobject) :
    Effect(lpeobject),
    angle(_("Angle:"), _("Additional angle between tangent and curve"), "angle", &wr, this, 0.0),
    t_attach(_("Location along curve:"), _("Location of the point of attachment along the curve (between 0.0 and number-of-segments)"), "t_attach", &wr, this, 0.5),
    length_left(_("Length left:"), _("Specifies the left end of the tangent"), "length-left", &wr, this, 150),
    length_right(_("Length right:"), _("Specifies the right end of the tangent"), "length-right", &wr, this, 150)
{
    show_orig_path = true;
    _provides_knotholder_entities = true;

    registerParameter( dynamic_cast<Parameter *>(&angle) );
    registerParameter( dynamic_cast<Parameter *>(&t_attach) );
    registerParameter( dynamic_cast<Parameter *>(&length_left) );
    registerParameter( dynamic_cast<Parameter *>(&length_right) );
}

LPETangentToCurve::~LPETangentToCurve()
{
}

Geom::Piecewise<Geom::D2<Geom::SBasis> >
LPETangentToCurve::doEffect_pwd2 (Geom::Piecewise<Geom::D2<Geom::SBasis> > const & pwd2_in)
{
    using namespace Geom;
    Piecewise<D2<SBasis> > output;

    ptA = pwd2_in.valueAt(t_attach);
    derivA = unit_vector(derivative(pwd2_in).valueAt(t_attach));

    // TODO: Why are positive angles measured clockwise, not counterclockwise?
    Geom::Rotate rot(Geom::Rotate::from_degrees(-angle));
    derivA = derivA * rot;

    C = ptA - derivA * length_left;
    D = ptA + derivA * length_right;

    output = Piecewise<D2<SBasis> >(D2<SBasis>(Linear(C[X], D[X]), Linear(C[Y], D[Y])));

    return output;
}

void
LPETangentToCurve::addKnotHolderEntities(KnotHolder *knotholder, SPDesktop *desktop, SPItem *item) {
    {
        KnotHolderEntity *e = new TtC::KnotHolderEntityAttachPt(this);
        e->create( desktop, item, knotholder, Inkscape::CTRL_TYPE_UNKNOWN,
                   _("Adjust the point of attachment of the tangent") );
        knotholder->add(e);
    }
    {
        KnotHolderEntity *e = new TtC::KnotHolderEntityLeftEnd(this);
        e->create( desktop, item, knotholder, Inkscape::CTRL_TYPE_UNKNOWN,
                    _("Adjust the \"left\" end of the tangent") );
        knotholder->add(e);
    }
    {
        KnotHolderEntity *e = new TtC::KnotHolderEntityRightEnd(this);
        e->create( desktop, item, knotholder, Inkscape::CTRL_TYPE_UNKNOWN,
                   _("Adjust the \"right\" end of the tangent") );
        knotholder->add(e);
    }
};

namespace TtC {

void
KnotHolderEntityAttachPt::knot_set(Geom::Point const &p, Geom::Point const &/*origin*/, guint state)
{
    using namespace Geom;

    LPETangentToCurve* lpe = dynamic_cast<LPETangentToCurve *>(_effect);

    Geom::Point const s = snap_knot_position(p, state);

    // FIXME: There must be a better way of converting the path's SPCurve* to pwd2.
    SPCurve *curve = SP_PATH(item)->get_curve_for_edit();
    Geom::PathVector pathv = curve->get_pathvector();
    Piecewise<D2<SBasis> > pwd2;
    for (unsigned int i=0; i < pathv.size(); i++) {
        pwd2.concat(pathv[i].toPwSb());
    }

    double t0 = nearest_point(s, pwd2);
    lpe->t_attach.param_set_value(t0);

    // FIXME: this should not directly ask for updating the item. It should write to SVG, which triggers updating.
    sp_lpe_item_update_patheffect (SP_LPE_ITEM(item), false, true);
}

void
KnotHolderEntityLeftEnd::knot_set(Geom::Point const &p, Geom::Point const &/*origin*/, guint state)
{
    LPETangentToCurve *lpe = dynamic_cast<LPETangentToCurve *>(_effect);

    Geom::Point const s = snap_knot_position(p, state);

    double lambda = Geom::nearest_point(s, lpe->ptA, lpe->derivA);
    lpe->length_left.param_set_value(-lambda);

    sp_lpe_item_update_patheffect (SP_LPE_ITEM(item), false, true);
}

void
KnotHolderEntityRightEnd::knot_set(Geom::Point const &p, Geom::Point const &/*origin*/, guint state)
{
    LPETangentToCurve *lpe = dynamic_cast<LPETangentToCurve *>(_effect);
    
    Geom::Point const s = snap_knot_position(p, state);

    double lambda = Geom::nearest_point(s, lpe->ptA, lpe->derivA);
    lpe->length_right.param_set_value(lambda);

    sp_lpe_item_update_patheffect (SP_LPE_ITEM(item), false, true);
}

Geom::Point
KnotHolderEntityAttachPt::knot_get() const
{
    LPETangentToCurve const *lpe = dynamic_cast<LPETangentToCurve const*>(_effect);
    return lpe->ptA;
}

Geom::Point
KnotHolderEntityLeftEnd::knot_get() const
{
    LPETangentToCurve const *lpe = dynamic_cast<LPETangentToCurve const*>(_effect);
    return lpe->C;
}

Geom::Point
KnotHolderEntityRightEnd::knot_get() const
{
    LPETangentToCurve const *lpe = dynamic_cast<LPETangentToCurve const*>(_effect);
    return lpe->D;
}

} // namespace TtC

} //namespace LivePathEffect
} /* namespace Inkscape */

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
