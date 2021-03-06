#ifndef DCES_H
#define DCES_H

#include "../Property.h"

#define DCES(label) "http://purl.org/dc/elements/1.1/"label;

namespace artivity
{
    namespace dces
    {
        static const Property title = DCES("title");
        static const Property description = DCES("description");
    }
}

#endif // DCES_H
