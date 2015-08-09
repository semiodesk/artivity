#ifndef AS_H
#define AS_H

#include "../Property.h"

namespace artivity
{
    namespace as
    {
        static const Resource Activity = "http://www.w3.org/ns/activitystreams#Activity";
        static const Resource Create = "http://www.w3.org/ns/activitystreams#Create";
        static const Resource Delete = "http://www.w3.org/ns/activitystreams#Delete";
        static const Resource Update = "http://www.w3.org/ns/activitystreams#Update";
        static const Resource Undo = "http://www.w3.org/ns/activitystreams#Undo";
        static const Resource View = "http://www.w3.org/ns/activitystreams#View";
        static const Resource Remove = "http://www.w3.org/ns/activitystreams#Remove";
        static const Resource Add = "http://www.w3.org/ns/activitystreams#Add";
        
        static const Property actor = "http://www.w3.org/ns/activitystreams#actor";
        static const Property author = "http://www.w3.org/ns/activitystreams#author";
        static const Property generator = "http://www.w3.org/ns/activitystreams#generator";
        static const Property instrument = "http://www.w3.org/ns/activitystreams#instrument";
        static const Property target = "http://www.w3.org/ns/activitystreams#target";
        static const Property object = "http://www.w3.org/ns/activitystreams#object";
        static const Property displayName = "http://www.w3.org/ns/activitystreams#displayName";
        static const Property endTime = "http://www.w3.org/ns/activitystreams#endTime";
        static const Property startTime = "http://www.w3.org/ns/activitystreams#startTime";
        static const Property updateTime = "http://www.w3.org/ns/activitystreams#updateTime";
        static const Property url = "http://www.w3.org/ns/activitystreams#url";
    }
}

#endif // AS_H
