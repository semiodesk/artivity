/*
 * Authors:
 *   MenTaLguY <mental@rydia.net>
 *   Jon A. Cruz <jon@joncruz.org>
 *
 * Copyright (C) 2003 MenTaLguY
 *
 * Released under GNU GPL, read the file 'COPYING' for more information
 */

#ifndef SEEN_INKSCAPE_VERSION_H
#define SEEN_INKSCAPE_VERSION_H

#include <glib.h>

#define SVG_VERSION "1.1"

namespace Inkscape {

class Version {
public:

    Version() : _major(0), _minor(0) {}

    // Note: somebody pollutes our namespace with major() and minor()
    Version(unsigned mj, unsigned mn) : _major(mj), _minor(mn) {}

    bool operator>(Version const &other) const {
        return _major > other._major ||
            ( _major == other._major && _minor > other._minor );
    }

    bool operator==(Version const &other) const {
        return _major == other._major && _minor == other._minor;
    }

    bool operator!=(Version const &other) const {
        return _major != other._major || _minor != other._minor;
    }

    bool operator<(Version const &other) const {
        return _major < other._major ||
            ( _major == other._major && _minor < other._minor );
    }

    unsigned int _major;
    unsigned int _minor;
};

}

gboolean sp_version_from_string(const gchar *string, Inkscape::Version *version);

gchar *sp_version_to_string(Inkscape::Version version);

gboolean sp_version_inside_range(Inkscape::Version version,
                                 unsigned major_min, unsigned minor_min,
                                 unsigned major_max, unsigned minor_max);

#endif // SEEN_INKSCAPE_VERSION_H
/*
  Local Variables:
  mode:c++
  c-file-style:"stroustrup"
  c-file-offsets:((innamespace . 0)(inline-open . 0))
  indent-tabs-mode:nil
  fill-column:75
  End:
*/
// vim: filetype=cpp:expandtab:shiftwidth=4:tabstop=8:softtabstop=4 :
