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

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"
#include "Generation.h"
#include "Invalidation.h"

namespace artivity
{
    class Entity;
    typedef boost::shared_ptr<Entity> EntityRef;

    class Entity : public Resource
    {
    private:
        GenerationRef _generation;
        
        InvalidationRef _invalidation;
        
        std::list<EntityRef>* _genericEntities;
        
    public:
        Entity() : Resource(UriGenerator::getUri())
        {
            _genericEntities = new std::list<EntityRef>();
            
            setType(prov::Entity);
        }
        
        Entity(const char* uriref) : Resource(uriref)
        {
            _genericEntities = new std::list<EntityRef>();
            
            setType(prov::Entity);
        }
        
        virtual ~Entity()
        {
            delete _genericEntities;
        }
        
        void clear()
        {
            Resource::clear();
            
            _genericEntities->clear();
        }
        
        GenerationRef getGeneration()
        {
            return _generation;
        }
        
        void setGeneration(GenerationRef generation)
        {
            _generation = generation;
            
            setValue(prov::qualifiedGeneration, generation);
        }
        
        InvalidationRef getInvalidation()
        {
            return _invalidation;
        }
        
        void setInvalidation(InvalidationRef invalidation)
        {
            _invalidation = invalidation;
            
            setValue(prov::qualifiedInvalidation, invalidation);
        }
        
        std::list<EntityRef> getGenericEntities()
        {
            return *_genericEntities;
        }
        
        void addGenericEntity(EntityRef entity)
        {
            _genericEntities->push_back(entity);
            
            addProperty(prov::specializationOf, entity);
        }
        
        void removeGenericEntity(EntityRef entity)
        {
            _genericEntities->remove(entity);
            
            removeProperty(prov::specializationOf, entity);
        }
    };
}

#endif // _ART_ENTITY_H
