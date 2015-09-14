#ifndef ROLE_H
#define ROLE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

namespace artivity
{
    class Role : public Resource
    {
    public:
        Role() : Resource(UriGenerator::getUri())
        {
            setValue(rdf::_type, prov::Role);
        }
        
        Role(const char* uriref) : Resource(uriref)
        {
            setValue(rdf::_type, prov::Role);
        }
    };
}

#endif // ROLE_H
