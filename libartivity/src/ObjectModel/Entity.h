#ifndef ENTITY_H
#define ENTITY_H

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

#endif // ENTITY_H
