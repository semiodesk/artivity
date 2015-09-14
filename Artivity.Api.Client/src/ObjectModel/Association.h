#ifndef ASSOCIATION_H
#define ASSOCIATION_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

#include "Agent.h"
#include "Role.h"

namespace artivity
{
    class Association : public Resource
    {
    private:
        const Agent* _agent;
        const Role* _role;
        
    public:
        Association() : Resource(UriGenerator::getUri())
        {
            _agent = NULL;
            _role = NULL;
            
            setValue(rdf::_type, prov::Association);
        }
        
        Association(const char* uriref) : Resource(uriref)
        {
            _agent = NULL;
            _role = NULL;
            
            setValue(rdf::_type, prov::Association);
        }
        
        virtual ~Association()
        {
            //if(_agent != NULL) delete _agent;
            //if(_role != NULL) delete _role;
        }
        
        const Agent* getAgent()
        {
            return _agent;
        }
        
        void setAgent(const Agent* agent)
        {
            _agent = agent;
            
            setValue(prov::agent, agent);
        }
        
        const Role* getRole()
        {
            return _role;
        }
        
        void setRole(const Role* role)
        {
            _role = role;
            
            setValue(prov::hadRole, role);
        }
    };
}

#endif // ASSOCIATION_H
