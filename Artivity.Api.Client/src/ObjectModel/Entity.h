#ifndef ENTITY_H
#define ENTITY_H

#include <list>

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"
#include "Generation.h"
#include "Invalidation.h"

using namespace std;

namespace artivity
{
    class Entity : public Resource
    {
    private:
        Generation* _generation;
        
        time_t _generationTime;
        
        Invalidation* _invalidation;
        
        time_t _invalidationTime;
        
        list<Entity*>* _genericEntities;
        
    public:
        Entity() : Resource(UriGenerator::getUri())
        {
            _genericEntities = new list<Entity*>();
            
            setValue(rdf::_type, prov::Entity);
        }
        
        Entity(const char* uriref) : Resource(uriref)
        {
            _genericEntities = new list<Entity*>();
            
            setValue(rdf::_type, prov::Entity);
        }
        
        virtual ~Entity()
        {
            delete _genericEntities;
        }
        
        Generation* getGeneration()
        {
            return _generation;
        }
        
        void setGeneration(Generation* generation)
        {
            _generation = generation;
            
            setValue(prov::qualifiedGeneration, generation);
        }
        
        time_t getGenerationTime()
        {
            return _generationTime;
        }
        
        void setGenerationTime(time_t time)
        {
            _generationTime = time;
            
            Resource::setValue(prov::generatedAtTime, &time);
        }
        
        Invalidation* getInvalidation()
        {
            return _invalidation;
        }
        
        void setInvalidation(Invalidation* invalidation)
        {
            _invalidation = invalidation;
            
            setValue(prov::qualifiedInvalidation, invalidation);
        }
        
        time_t getInvalidationTime()
        {
            return _invalidationTime;
        }
        
        void setInvalidationTime(time_t time)
        {
            _invalidationTime = time;
            
            Resource::setValue(prov::invalidatedAtTime, &time);
        }
        
        list<Entity*> getGenericEntities()
        {
            return *_genericEntities;
        }
        
        void addGenericEntity(Entity* entity)
        {
            _genericEntities->push_back(entity);
            
            addProperty(prov::specializationOf, entity);
        }
        
        void removeGenericEntity(Entity* entity)
        {
            _genericEntities->remove(entity);
            
            removeProperty(prov::specializationOf, entity);
        }
    };
}

#endif // ENTITY_H
