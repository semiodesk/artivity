#ifndef AGENT_H
#define AGENT_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

namespace artivity
{
    class Agent;
    typedef boost::shared_ptr<Agent> AgentRef;

    class Agent : public Resource
    {
    public:
        Agent() : Resource(UriGenerator::getUri())
        {
            setType(prov::Agent);
        }
        
        Agent(const char* uriref) : Resource(uriref)
        {
            setType(prov::Agent);
        }
    };
}

#endif // AGENT_H
