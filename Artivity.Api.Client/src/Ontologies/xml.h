#ifndef XML_H
#define XML_H

#include "../Property.h"

#define XML(label) "http://www.w3.org/2001/04/infoset#"label;

namespace artivity
{
    namespace xml
    {
        static const Resource Element = XML("Element");
        static const Resource Attribute = XML("Attribute");
        static const Property ownerElement = XML("ownerElement");
        static const Property localName = XML("localName");
    }
}

#endif // XML_H
