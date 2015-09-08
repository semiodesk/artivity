#include "Activity.h"
#include "Ontologies/prov.h"
#include "Ontologies/rdf.h"
#include "UriGenerator.h"
#include <algorithm>

namespace artivity
{
    Activity::Activity() : Resource(UriGenerator::getUri())
    {
        _associations = new list<Association*>();
        _usedEntities = new list<Entity*>();
        _invalidatedEntities = new list<Entity*>();
        _generatedEntities = new list<Entity*>();
        
        setValue(rdf::_type, prov::Activity);
    }
    
    Activity::Activity(const char* uriref) : Resource(uriref)
    {
        _associations = new list<Association*>();
        _usedEntities = new list<Entity*>();
        _invalidatedEntities = new list<Entity*>();
        _generatedEntities = new list<Entity*>();
        
        setValue(rdf::_type, prov::Activity);
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
        
        setValue(prov::endedAtTime, &time);
    }
    
    time_t Activity::getEndTime()
    {
        return _endTime;
    }
    
    void Activity::setStartTime(time_t time)
    {
        _startTime = time;
        
        setValue(prov::endedAtTime, &time);
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
        _associations->push_back(association);
    }
    
    void Activity::removeAssociation(Association* association)
    {        
        _associations->remove(association);
    }
    
    list<Entity*> Activity::getUsedEntities()
    {
        return *_usedEntities;
    }
    
    void Activity::addUsedEntity(Entity* entity)
    {
        _usedEntities->push_back(entity);
    }
    
    void Activity::removeUsedEntity(Entity* entity)
    {        
        _usedEntities->remove(entity);
    }
    
    list<Entity*> Activity::getInvalidatedEntities()
    {
        return *_invalidatedEntities;
    }
    
    void Activity::addInvalidatedEntity(Entity* entity)
    {
        _invalidatedEntities->push_back(entity);
    }
    
    void Activity::removeInvalidatedEntity(Entity* entity)
    {       
        _invalidatedEntities->remove(entity);
    }
    
    list<Entity*> Activity::getGeneratedEntities()
    {
        return *_generatedEntities;
    }
    
    void Activity::addGeneratedEntity(Entity* entity)
    {
        _generatedEntities->push_back(entity);
    }
    
    void Activity::removeGeneratedEntity(Entity* entity)
    {        
        _generatedEntities->remove(entity);
    }
}

