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

#include "../Ontologies/prov.h"

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
            setType(prov::Association);

            _agent = AgentRef();
            _role = RoleRef();
        }

		Association(std::string uriref) : Resource(uriref)
		{
			setType(prov::Association);

            _agent = AgentRef();
            _role = RoleRef();
		}

        Association(const char* uriref) : Resource(uriref)
        {          
            setType(prov::Association);

            _agent = AgentRef();
            _role = RoleRef();
        }
        
        virtual ~Association() {}
        
        AgentRef getAgent();
        
        void setAgent(AgentRef agent);
        
        RoleRef getRole();
        
        void setRole(RoleRef role);
    };
}

#endif // _ART_ASSOCIATION_H
