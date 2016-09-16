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

#ifndef _ART_ENTITY_H
#define _ART_ENTITY_H

#include <list>

#include "../Ontologies/dces.h"
#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../Ontologies/nie.h"

#include "../UriGenerator.h"
#include "../Resource.h"
#include "../Property.h"

namespace artivity
{
    class Influence;
    
    typedef boost::shared_ptr<Influence> InfluenceRef;
    
    class Generation;
    
    typedef boost::shared_ptr<Generation> GenerationRef;
    
    class Invalidation;
    
    typedef boost::shared_ptr<Invalidation> InvalidationRef;
    
    class Derivation;
    
    typedef boost::shared_ptr<Derivation> DerivationRef;
    
    class EntityInfluence;

    typedef boost::shared_ptr<EntityInfluence> EntityInfluenceRef;

    class Derivation;

    typedef boost::shared_ptr<Derivation> DerivationRef;

    class Revision;

    typedef boost::shared_ptr<Revision> RevisionRef;

    class Entity;

    typedef boost::shared_ptr<Entity> EntityRef;
    
    class Entity : public Resource
    {
    private:
        std::string _title;

        std::list<InfluenceRef> _influences;

        time_t _created;

        time_t _modified;

    public:
        Entity() : Resource(UriGenerator::getUri())
        {
            setType(prov::Entity);
        }
        
        Entity(const char* uriref) : Resource(uriref)
        {
            setType(prov::Entity);
        }

        virtual ~Entity() {}

        void setTitle(std::string title);

        time_t getCreated();

        void setCreated(time_t created);

        void resetCreated();

        time_t getModified();

        void setModified(time_t modified);

        void resetModified();

        std::list<InfluenceRef> getInfluences();

        void addInfluence(GenerationRef generation);
        
        void addInfluence(InvalidationRef invalidation);
        
        void addInfluence(EntityInfluenceRef influence);

        void addInfluence(DerivationRef derivation);

        void addInfluence(RevisionRef revision);
        
        void removeInfluence(GenerationRef generation);
        
        void removeInfluence(InvalidationRef invalidation);
        
        void removeInfluence(EntityInfluenceRef influence);
        
        void removeInfluence(DerivationRef derivation);
        
        void removeInfluence(RevisionRef revision);

        void clearInfluences();
    };
}

#endif // _ART_ENTITY_H
