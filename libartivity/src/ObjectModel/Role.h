#ifndef ROLE_H
#define ROLE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

namespace artivity
{
    class Role;
    typedef boost::shared_ptr<Role> RoleRef;

    class Role : public Resource
    {
    public:
        Role() : Resource(UriGenerator::getUri())
        {
            setType(prov::Role);
        }
        
        Role(const char* uriref) : Resource(uriref)
        {
            setType(prov::Role);
        }
    };
}

#endif // ROLE_H
