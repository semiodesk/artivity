#ifndef AS_H
#define AS_H

#include "../Property.h"

#define ART(label) "http://www.w3.org/ns/activitystreams#"label;

namespace artivity
{
    namespace as
    {
        static const Resource Activity = ART("Activity")
        static const Resource Create = ART("Create")
        static const Resource Delete = ART("Delete")
        static const Resource Update = ART("Update")
        static const Resource Undo = ART("Undo")
        static const Resource View = ART("View")
        static const Resource Remove = ART("Remove")
        static const Resource Add = ART("Add")
        
        static const Property actor = ART("actor");
        static const Property author = ART("author");
        static const Property generator = ART("generator");
        static const Property instrument = ART("instrument");
        static const Property target = ART("target");
        static const Property object = ART("object");
        static const Property displayName = ART("displayName");
        static const Property endTime = ART("endTime");
        static const Property startTime = ART("startTime");
        static const Property updateTime = ART("updateTime");
        static const Property url = ART("url");
    }
}

#endif // AS_H
