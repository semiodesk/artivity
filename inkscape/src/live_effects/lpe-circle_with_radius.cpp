/**
 * @file
 * LPE effect that draws a circle based on two points and a radius.
 * - implementation
 */
/* Authors:
 *   Johan Engelen <j.b.c.engelen@utwente.nl>
 *
 * Copyright (C) 2007 Authors
 *
 * Released under GNU GPL, read the file 'COPYING' for more information
 */

#include "live_effects/lpe-circle_with_radius.h"
#include "display/curve.h"

// You might need to include other 2geom files. You can add them here:
#include <2geom/path.h>
#include <2geom/sbasis.h>
#include <2geom/bezier-to-sbasis.h>
#include <2geom/d2.h>
#include <2geom/circle.h>

using namespace Geom;

namespace Inkscape {
namespace LivePathEffect {

LPECircleWithRadius::LPECircleWithRadius(LivePathEffectObject *lpeobject) :
    Effect(lpeobject)//,
    // initialise your parameters here:
    //radius(_("Float parameter"), _("just a real number like 1.4!"), "svgname", &wr, this, 50)
{
    // register all your parameters here, so Inkscape knows which parameters this effect has:
    //registerParameter( dynamic_cast<Parameter *>(&radius) );
}

LPECircleWithRadius::~LPECircleWithRadius()
{

}

std::vector<Geom::Path>
LPECircleWithRadius::doEffect_path (std::vector<Geom::Path> const & path_in)
{
    std::vector<Geom::Path> path_out = std::vector<Geom::Path>();

    Geom::Point center = path_in[0].initialPoint();
    Geom::Point pt = path_in[0].finalPoint();

    double radius = Geom::L2(pt - center);

    Geom::Circle c(center, radius);
    c.getPath(path_out);

    return path_out;
}

/*

Geom::Piecewise<Geom::D2<Geom::SBasis> >
LPECircleWithRadius::doEffect_pwd2 (Geom::Piecewise<Geom::D2<Geom::SBasis> > & pwd2_in)
{
    Geom::Piecewise<Geom::D2<Geom::SBasis> > output;

    output = pwd2_in;   // spice this up to make the effect actually *do* something!

    return output;
}

*/

/* ######################## */

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
// vim: filetype=cpp:expandtab:shiftwidth=4:tabstop=8:softtabstop=4:fileencoding=utf-8:textwidth=99 :
