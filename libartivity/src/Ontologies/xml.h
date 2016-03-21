#ifndef XML_H
#define XML_H

#define XML(label) "http://www.w3.org/2001/04/infoset#" label;

namespace artivity
{
    namespace xml
    {
        static const char* Element = XML("Element");
        static const char* Attribute = XML("Attribute");
        static const char* ownerElement = XML("ownerElement");
        static const char* localName = XML("localName");
    }
}

#endif // XML_H
