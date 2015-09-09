#ifndef PERSON_H
#define PERSON_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

namespace artivity
{
    class Person : public Resource
    {
    public:
        Person() : Resource(UriGenerator::getUri())
        {
            setValue(rdf::_type, prov::Person);
        }
        
        Person(const char* uriref) : Resource(uriref)
        {
            setValue(rdf::_type, prov::Person);
        }
    };
}

#endif // PERSON_H
