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
            setValue(rdf::_type, prov::SoftwareAgent);
        }
        
        SoftwareAgent(const char* uriref) : Agent(uriref)
        {
            setValue(rdf::_type, prov::SoftwareAgent);
        }
    };
}

#endif // SOFTWAREAGENT_H
