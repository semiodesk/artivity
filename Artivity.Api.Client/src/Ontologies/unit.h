#ifndef UO_H
#define UO_H

#include "../Property.h"

// Source: http://qudt.org/1.1/vocab/OVG_units-qudt-(v1.1).ttl
#define UNIT(label) "http://qudt.org/vocab/unit#"label;

namespace artivity
{
    namespace unit
    {
        static const Resource Millimeter = UNIT("Millimeter");
        static const Resource Centimeter = UNIT("Centimeter");
        static const Resource Meter = UNIT("Meter");
        static const Resource Point = UNIT("Point");
        static const Resource Inch = UNIT("Inch");
        static const Resource Foot = UNIT("Foot");
    }
}

#endif // UO_H
