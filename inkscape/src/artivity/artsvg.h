#ifndef ARTINK_H
#define ARTINK_H

#include <artivity-client/ActivityLog.h>

#define ARTSVG(label) "http://www.semiodesk.com/artivity/1.0/inkscape/"label

namespace Inkscape
{
    namespace artsvg
    {
        static const artivity::Resource Viewbox = ARTSVG("Viewbox");
        
        static const artivity::Property hasViewbox = ART("hasViewbox");
        static const artivity::Property left = ART("left");
        static const artivity::Property right = ART("right");
        static const artivity::Property top = ART("top");
        static const artivity::Property bottom = ART("bottom");
    }
    
}

#endif // ARTINK_H
