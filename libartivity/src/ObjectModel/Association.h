// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

#ifndef _ART_ASSOCIATION_H
#define _ART_ASSOCIATION_H

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

		Association(std::string uriref) : Resource(uriref)
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

#endif // _ART_ASSOCIATION_H
