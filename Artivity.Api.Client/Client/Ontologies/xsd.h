#ifndef XSD_H
#define XSD_H

#include "../Resource.h"
#include "../Property.h"

#define XSD(label) "http://www.w3.org/2001/XMLSchema#"label;

namespace artivity
{
    namespace xsd
    {
        static const Resource _int = XSD("int");
        static const Resource _long = XSD("long");
        static const Resource _float = XSD("float");
        static const Resource _double = XSD("double");
        static const Resource dateTime = XSD("dateTime");
    }
}

#endif // XSD_H
