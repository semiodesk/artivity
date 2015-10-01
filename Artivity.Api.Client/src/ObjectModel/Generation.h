#ifndef GENERATION_H
#define GENERATION_H

#include "ActivityInfluence.h"

namespace artivity
{
    class Generation : public ActivityInfluence
    {        
    public:
        Generation() : ActivityInfluence()
        {
            Resource::setValue(rdf::_type, prov::Generation);
        }
        
        Generation(const char* uriref) : ActivityInfluence(uriref)
        {
            Resource::setValue(rdf::_type, prov::Generation);
        }
    };
}

#endif // GENERATION_H
