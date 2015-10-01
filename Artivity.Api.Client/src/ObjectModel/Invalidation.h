#ifndef INVALIDATION_H
#define INVALIDATION_H

#include "ActivityInfluence.h"

namespace artivity
{
    class Invalidation : public ActivityInfluence
    {        
    public:
        Invalidation() : ActivityInfluence()
        {
            Resource::setValue(rdf::_type, prov::Invalidation);
        }
        
        Invalidation(const char* uriref) : ActivityInfluence(uriref)
        {
            Resource::setValue(rdf::_type, prov::Invalidation);
        }
    };
}


#endif // INVALIDATION_H
