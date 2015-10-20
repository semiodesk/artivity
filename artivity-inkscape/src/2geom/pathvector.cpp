/*
 * PathVector - std::vector containing Geom::Path
 * This file provides a set of operations that can be performed on PathVector,
 * e.g. an affine transform.
 *
 * Authors:
 *  Johan Engelen <goejendaagh@zonnet.nl>
 * 
 * Copyright 2008  authors
 *
 * This library is free software; you can redistribute it and/or
 * modify it either under the terms of the GNU Lesser General Public
 * License version 2.1 as published by the Free Software Foundation
 * (the "LGPL") or, at your option, under the terms of the Mozilla
 * Public License Version 1.1 (the "MPL"). If you do not alter this
 * notice, a recipient may use your version of this file under either
 * the MPL or the LGPL.
 *
 * You should have received a copy of the LGPL along with this library
 * in the file COPYING-LGPL-2.1; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
 * You should have received a copy of the MPL along with this library
 * in the file COPYING-MPL-1.1
 *
 * The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * This software is distributed on an "AS IS" basis, WITHOUT WARRANTY
 * OF ANY KIND, either express or implied. See the LGPL or the MPL for
 * the specific language governing rights and limitations.
 */

#ifndef SEEN_GEOM_PATHVECTOR_CPP
#define SEEN_GEOM_PATHVECTOR_CPP

#include <2geom/pathvector.h>

#include <2geom/path.h>
#include <2geom/affine.h>

namespace Geom {

// TODO: see which of these functions can be inlined for optimization

/**
 * Reverses all Paths and the order of paths in the vector as well
 **/
PathVector reverse_paths_and_order (PathVector const & path_in)
{
    PathVector path_out;
    for (PathVector::const_reverse_iterator it = path_in.rbegin(); it != path_in.rend(); ++it) {
        path_out.push_back( (*it).reverse() );
    }
    return path_out;
}

OptRect bounds_fast( PathVector const& pv )
{
    typedef PathVector::const_iterator const_iterator;
    
    OptRect bound;
    if (pv.empty()) return bound;
    
    bound = (pv.begin())->boundsFast();
    for (const_iterator it = ++(pv.begin()); it != pv.end(); ++it)
    {
        bound.unionWith(it->boundsFast());
    }
    return bound;
}

OptRect bounds_exact( PathVector const& pv )
{
    typedef PathVector::const_iterator const_iterator;
    
    OptRect bound;
    if (pv.empty()) return bound;
    
    bound = (pv.begin())->boundsExact();
    for (const_iterator it = ++(pv.begin()); it != pv.end(); ++it)
    {
        bound.unionWith(it->boundsExact());
    }
    return bound;
}

/* Note: undefined for empty pathvectors or pathvectors with empty paths.
 * */
boost::optional<PathVectorPosition> nearestPoint(PathVector const & path_in, Point const& _point, double *distance_squared)
{
    boost::optional<PathVectorPosition> retval;

    double mindsq = infinity();
    unsigned int i = 0;
    for (Geom::PathVector::const_iterator pit = path_in.begin(); pit != path_in.end(); ++pit) {
        double dsq;
        double t = pit->nearestPoint(_point, &dsq);
        //std::cout << t << "," << dsq << std::endl;
        if (dsq < mindsq) {
            mindsq = dsq;
            retval = PathVectorPosition(i, t);
        }

        ++i;
    }

    if (distance_squared) {
        *distance_squared = mindsq;
    }
    return retval;
}

std::vector<PathVectorPosition> allNearestPoints(PathVector const & path_in, Point const& _point, double *distance_squared)
{
    std::vector<PathVectorPosition> retval;

    double mindsq = infinity();
    unsigned int i = 0;
    for (Geom::PathVector::const_iterator pit = path_in.begin(); pit != path_in.end(); ++pit) {
        double dsq;
        double t = pit->nearestPoint(_point, &dsq);
        if (dsq < mindsq) {
            mindsq = dsq;
            retval.push_back(PathVectorPosition(i, t));
        }

        ++i;
    }

    if (distance_squared) {
        *distance_squared = mindsq;
    }
    return retval;

}

} // namespace Geom

#endif // SEEN_GEOM_PATHVECTOR_CPP

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
