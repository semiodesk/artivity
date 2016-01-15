#ifndef SOFTWAREAGENT_H
#define SOFTWAREAGENT_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"

#include "Agent.h"

namespace artivity
{
    class SoftwareAgent : public Agent
    {
    public:
        SoftwareAgent() : Agent()
        {
            setType(prov::SoftwareAgent);
        }
        
        SoftwareAgent(const char* uriref) : Agent(uriref)
        {
            setType(prov::SoftwareAgent);
        }
    };
}

#endif // SOFTWAREAGENT_H
