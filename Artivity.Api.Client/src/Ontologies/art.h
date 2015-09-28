#ifndef ART_H
#define ART_H

#include "../Property.h"

#define ART(label) "http://semiodesk.com/artivity/1.0/"label;

namespace artivity
{
    namespace art
    {
        static const Resource Add = ART("Add");
        static const Resource Remove = ART("Remove");
        static const Resource Create = ART("Create");
        static const Resource Delete = ART("Delete");
        static const Resource Undo = ART("Undo");
        static const Resource Redo = ART("Redo");
        static const Resource Update = ART("Update");
        static const Resource Open = ART("Open");
        static const Resource Close = ART("Close");
        static const Resource Save = ART("Save");
        static const Resource View = ART("View");
        
        static const Resource Viewbox = ART("Viewbox");
        static const Property hadViewbox = ART("hadViewbox");
        static const Property left = ART("left");
        static const Property right = ART("right");
        static const Property top = ART("top");
        static const Property bottom = ART("bottom");
        static const Property zoomFactor = ART("zoomFactor");
        
        static const Resource Dimensions = ART("Dimensions");
        static const Property dimensions = ART("dimensions");
        static const Property x = ART("x");
        static const Property y = ART("y");
        static const Property z = ART("z");
        static const Property unit = ART("unit");
        
        // There is no Pixel unit in the QUDT unit ontology.. :/
        static const Resource Pixel = ART("Pixel");
        
        static const Property isCaptureEnabled = ART("isCaptureEnabled");

        static const Property selectedLayer = ART("selectedLayer");
    }
}

#endif // ART_H
