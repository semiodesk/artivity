#ifndef AS_H
#define AS_H

#include "../Property.h"

#define AS(label) "http://www.w3.org/ns/activitystreams#"label;

namespace artivity
{
    namespace as
    {
        static const Resource Activity = AS("Activity")
        static const Resource Create = AS("Create")
        static const Resource Delete = AS("Delete")
        static const Resource Update = AS("Update")
        static const Resource Undo = AS("Undo")
        static const Resource View = AS("View")
        static const Resource Add = AS("Add")
        static const Resource Remove = AS("Remove")
        
        static const Property actor = AS("actor");
        static const Property author = AS("author");
        static const Property generator = AS("generator");
        static const Property instrument = AS("instrument");
        static const Property target = AS("target");
        static const Property object = AS("object");
        static const Property displayName = AS("displayName");
        static const Property endTime = AS("endTime");
        static const Property startTime = AS("startTime");
        static const Property updateTime = AS("updateTime");
        static const Property url = AS("url");
    }
}

#endif // AS_H
