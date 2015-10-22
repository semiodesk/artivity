#ifndef AGENT_H
#define AGENT_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

namespace artivity
{
    class Agent : public Resource
    {
    public:
        Agent() : Resource(UriGenerator::getUri())
        {
            setValue(rdf::_type, prov::Agent);
        }
        
        Agent(const char* uriref) : Resource(uriref)
        {
            setValue(rdf::_type, prov::Agent);
        }
    };
}

#endif // AGENT_H
