#ifndef ASSOCIATION_H
#define ASSOCIATION_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

#include "Agent.h"
#include "Role.h"

namespace artivity
{
    class Association;
    typedef boost::shared_ptr<Association> AssociationRef;

    class Association : public Resource
    {
    private:
        AgentRef _agent;
        RoleRef _role;
        
    public:
        Association() : Resource(UriGenerator::getUri())
        {
            _agent = NULL;
            _role = NULL;
            
            setType(prov::Association);
        }
        
        Association(const char* uriref) : Resource(uriref)
        {
            _agent = NULL;
            _role = NULL;
            
            setType(prov::Association);
        }
        
        virtual ~Association() {}
        
        AgentRef getAgent()
        {
            return _agent;
        }
        
        void setAgent(AgentRef agent)
        {
            _agent = agent;
            
            setValue(prov::agent, agent);
        }
        
        RoleRef getRole()
        {
            return _role;
        }
        
        void setRole(RoleRef role)
        {
            _role = role;
            
            setValue(prov::hadRole, role);
        }
    };
}

#endif // ASSOCIATION_H
