#ifndef XSD_H
#define XSD_H

#include "../Resource.h"
#include "../Property.h"

#define XSD(label) "http://www.w3.org/2001/XMLSchema#" label;

namespace artivity
{
    namespace xsd
    {
        static const char* _int = XSD("int");
		static const char* _long = XSD("long");
		static const char* _float = XSD("float");
		static const char* _double = XSD("double");
		static const char* dateTime = XSD("dateTime");
    }
}

#endif // XSD_H
