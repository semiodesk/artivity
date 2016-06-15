#ifndef GENERATION_H
#define GENERATION_H

#include "ActivityInfluence.h"

namespace artivity
{
    class Generation;
    typedef boost::shared_ptr<Generation> GenerationRef;

    class Generation : public ActivityInfluence
    {        
    public:
        Generation() : ActivityInfluence()
        {
            Resource::setType(prov::Generation);
        }
        
        Generation(const char* uriref) : ActivityInfluence(uriref)
        {
            Resource::setType(prov::Generation);
        }
    };
}

#endif // GENERATION_H
