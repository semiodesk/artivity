#include <algorithm>

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

#include "Activity.h"

namespace artivity
{
    Activity::Activity() : Resource(UriGenerator::getUri())
    {
        _associations = new list<Association*>();
        _usedEntities = new list<Entity*>();
        _invalidatedEntities = new list<Entity*>();
        _generatedEntities = new list<Entity*>();
        
        setType(prov::Activity);
    }
    
    Activity::Activity(const char* uriref) : Resource(uriref)
    {
        _associations = new list<Association*>();
        _usedEntities = new list<Entity*>();
        _invalidatedEntities = new list<Entity*>();
        _generatedEntities = new list<Entity*>();
        
        setType(prov::Activity);
    }

    Activity::~Activity()
    {
        delete _associations;
        delete _usedEntities;
        delete _invalidatedEntities;
        delete _generatedEntities;
    }
    
    bool Activity::isValid()
    {
        // TODO: Check if type, actor, object and time is set.
        // See: http://www.w3.org/TR/activitystreams-core/#example-1
        return true;
    }
    
    void Activity::clear()
    {
        Resource::clear();
        
        _associations->clear();
        _usedEntities->clear();
        _invalidatedEntities->clear();
        _generatedEntities->clear();
    }
    
    void Activity::setTime(time_t time)
    {
        setStartTime(time);
        setEndTime(time);
    }
    
    time_t Activity::getTime()
    {
        return _startTime;
    }
    
    void Activity::setEndTime(time_t time)
    {
        _endTime = time;
        
        setValue(prov::endedAtTime, &_endTime);
    }
    
    time_t Activity::getEndTime()
    {
        return _endTime;
    }
    
    void Activity::setStartTime(time_t time)
    {
        _startTime = time;
        
        setValue(prov::startedAtTime, &_startTime);
    }
    
    time_t Activity::getStartTime()
    {
        return _startTime;
    }
    
    list<Association*> Activity::getAssociations()
    {
        return *_associations;
    }
    
    void Activity::addAssociation(Association* association)
    {
        if(hasProperty(prov::qualifiedAssociation, association)) return;
        
        _associations->push_back(association);
        
        addProperty(prov::qualifiedAssociation, association);
    }
    
    void Activity::removeAssociation(Association* association)
    {        
        _associations->remove(association);
        
        removeProperty(prov::qualifiedAssociation, association);
    }
    
    list<Entity*> Activity::getUsedEntities()
    {
        return *_usedEntities;
    }
    
    void Activity::addUsedEntity(Entity* entity)
    {
        if(hasProperty(prov::used, entity)) return;
        
        _usedEntities->push_back(entity);
        
        addProperty(prov::used, entity);
    }
    
    void Activity::removeUsedEntity(Entity* entity)
    {
        _usedEntities->remove(entity);
        
        removeProperty(prov::used, entity);
    }
    
    list<Entity*> Activity::getInvalidatedEntities()
    {
        return *_invalidatedEntities;
    }
    
    void Activity::addInvalidatedEntity(Entity* entity)
    {
        if(hasProperty(prov::invalidated, entity)) return;
        
        _invalidatedEntities->push_back(entity);
        
        addProperty(prov::invalidated, entity);
    }
    
    void Activity::removeInvalidatedEntity(Entity* entity)
    {       
        _invalidatedEntities->remove(entity);
        
        removeProperty(prov::invalidated, entity);
    }
    
    list<Entity*> Activity::getGeneratedEntities()
    {
        return *_generatedEntities;
    }
    
    void Activity::addGeneratedEntity(Entity* entity)
    {
        if(hasProperty(prov::generated, entity)) return;
        
        _generatedEntities->push_back(entity);
        
        addProperty(prov::generated, entity);
    }
    
    void Activity::removeGeneratedEntity(Entity* entity)
    {        
        _generatedEntities->remove(entity);
        
        removeProperty(prov::generated, entity);
    }
}

