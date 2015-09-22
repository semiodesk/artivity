#ifndef PROPERTY_H
#define PROPERTY_H

#include "Resource.h"

namespace artivity
{
    class Property : public Resource
    {
    public:
        Property(const char* uriref) : Resource(uriref) {}
        virtual ~Property() {}
    };
}

#endif // PROPERTY_H
