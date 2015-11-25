#ifndef UO_H
#define UO_H

#include "../Property.h"

// Source: http://qudt.org/1.1/vocab/OVG_units-qudt-(v1.1).ttl
#define UNIT(label) "http://qudt.org/vocab/unit#"label;

namespace artivity
{
    namespace unit
    {
        static const char* Millimeter = UNIT("Millimeter");
        static const char* Centimeter = UNIT("Centimeter");
        static const char* Meter = UNIT("Meter");
        static const char* Point = UNIT("Point");
        static const char* Inch = UNIT("Inch");
        static const char* Foot = UNIT("Foot");
    }
}

#endif // UO_H
