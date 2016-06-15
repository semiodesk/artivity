#ifndef PERSON_H
#define PERSON_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

namespace artivity
{
    class Person;
    typedef boost::shared_ptr<Person> PersonRef;

    class Person : public Resource
    {
    public:
        Person() : Resource(UriGenerator::getUri())
        {
            setType(prov::Person);
        }
        
        Person(const char* uriref) : Resource(uriref)
        {
            setType(prov::Person);
        }
    };
}

#endif // PERSON_H
