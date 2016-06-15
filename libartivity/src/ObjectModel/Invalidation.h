#ifndef INVALIDATION_H
#define INVALIDATION_H

#include "ActivityInfluence.h"

namespace artivity
{
    class Invalidation;
    typedef boost::shared_ptr<Invalidation> InvalidationRef;

    class Invalidation : public ActivityInfluence
    {        
    public:
        Invalidation() : ActivityInfluence()
        {
            setType(prov::Invalidation);
        }
        
        Invalidation(const char* uriref) : ActivityInfluence(uriref)
        {
            setType(prov::Invalidation);
        }
    };
}


#endif // INVALIDATION_H
