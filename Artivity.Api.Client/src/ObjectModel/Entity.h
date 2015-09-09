#ifndef ENTITY_H
#define ENTITY_H

#include <list>

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

using namespace std;

namespace artivity
{
    class Entity : public Resource
    {
    private:
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
