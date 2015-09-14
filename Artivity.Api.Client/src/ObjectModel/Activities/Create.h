#ifndef CREATE_H
#define CREATE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class Create : public Activity
    {
    public:
        Create() : Activity()
        {
            setValue(rdf::_type, art::Create);
        }
        
        Create(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::Create);
        }
    };
}

#endif // CREATE_H
