#ifndef ART_H
#define ART_H

#include "../Property.h"

#define ART(label) "http://semiodesk.com/artivity/1.0/"label;

namespace artivity
{
    namespace art
    {
        static const Resource OpenFile = ART("OpenFile");
        static const Resource CloseFile = ART("CloseFile");
        static const Resource SaveFile = ART("SaveFile");
        
        static const Resource Viewbox = ART("Viewbox");
        static const Property viewbox = ART("viewbox");
        static const Property left = ART("left");
        static const Property right = ART("right");
        static const Property top = ART("top");
        static const Property bottom = ART("bottom");
        static const Property zoomFactor = ART("zoomFactor");
        
        static const Resource Modification = ART("Modification");
        static const Property modification = ART("modification");
        static const Property fromValue = ART("fromValue");
        static const Property toValue = ART("toValue");
        
        static const Resource ContentModification = ART("ContentModification");
        
        static const Resource AttributeModification = ART("AttributeModification");
        static const Property attributeName = ART("attributeName");
    }
}

#endif // ART_H
