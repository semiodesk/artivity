/**
 * \file
 * \brief  Various utility functions.
 *//*
 * Copyright 2007 Johan Engelen <goejendaagh@zonnet.nl>
 * Copyright 2006 Michael G. Sloan <mgsloan@gmail.com>
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
 *
 */

#ifndef SEEN_LIB2GEOM_UTILS_H
#define SEEN_LIB2GEOM_UTILS_H

#include <cstddef>
#include <vector>

namespace Geom {

// proper logical xor
inline bool logical_xor (bool a, bool b) { return (a || b) && !(a && b); }

void binomial_coefficients(std::vector<size_t>& bc, std::size_t n);

struct EmptyClass {};

/**
 * @brief Noncommutative multiplication helper.
 * Generates operator*(T, U) from operator*=(T, U). Does not generate operator*(U, T)
 * like boost::multipliable does. This makes it suitable for noncommutative cases,
 * such as transforms.
 */
template <class T, class U, class B = EmptyClass>
struct MultipliableNoncommutative : B
{
    friend T operator*(T const &lhs, U const &rhs) {
        T nrv(lhs); nrv *= rhs; return nrv;
    }
};

} // end namespace Geom

#endif // SEEN_LIB2GEOM_UTILS_H

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
