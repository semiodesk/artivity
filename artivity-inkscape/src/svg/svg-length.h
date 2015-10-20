#ifndef SEEN_SP_SVG_LENGTH_H
#define SEEN_SP_SVG_LENGTH_H

/**
 *  \file src/svg/svg-length.h
 *  \brief SVG length type
 */
/*
 * Authors:
 *   Lauris Kaplinski <lauris@kaplinski.com>
 *   Carl Hetherington <inkscape@carlh.net>
 *
 * Copyright (C) 1999-2002 Lauris Kaplinski
 * Copyright (C) 2000-2001 Ximian, Inc.
 *
 * Released under GNU GPL, read the file 'COPYING' for more information
 */

#include <glib.h>

class SVGLength
{
public:
    SVGLength();

    enum Unit {
        NONE,
        PX,
        PT,
        PC,
        MM,
        CM,
        INCH,
        FOOT,
        MITRE,
        EM,
        EX,
        PERCENT,
        LAST_UNIT = PERCENT
    };

    // The object's value is valid / exists in SVG.
    bool _set;

    // The unit of value.
    Unit unit;

    // The value of this SVGLength as found in the SVG.
    float value;

    // The value in pixels (value * pixels/unit).
    float computed;

    float operator=(float v) {
        _set = true;
        unit = NONE;
        value = computed = v;
        return v;
    }

    bool read(gchar const *str);
    void readOrUnset(gchar const *str, Unit u = NONE, float v = 0, float c = 0);
    bool readAbsolute(gchar const *str);
    void set(Unit u, float v, float c);
    void unset(Unit u = NONE, float v = 0, float c = 0);
    void update(double em, double ex, double scale);
};

#endif // SEEN_SP_SVG_LENGTH_H

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
