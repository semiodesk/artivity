#ifndef AS_H
#define AS_H

#include "../Property.h"

namespace artivity
{
    namespace client
    {
        namespace ontologies
        {
            namespace as
            {
                static const Resource Activity = "http://www.w3.org/ns/activitystreams#Activity";
                static const Property test = "http://www.w3.org/ns/activitystreams#test";
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
    }
}

#endif // AS_H
