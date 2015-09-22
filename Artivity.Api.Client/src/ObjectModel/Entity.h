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
        Invalidation* _invalidation;
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
        
        Invalidation* getInvalidation()
        {
            return _invalidation;
        }
        
        void setInvalidation(Invalidation* invalidation)
        {
            _invalidation = invalidation;
            
            setValue(prov::qualifiedInvalidation, invalidation);
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
