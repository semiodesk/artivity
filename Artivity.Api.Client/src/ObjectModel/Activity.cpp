#include <algorithm>

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

#include "Activity.h"

namespace artivity
{
    Activity::Activity() : Resource(UriGenerator::getUri())
    {
        _associations = new list<const Association*>();
        _usedEntities = new list<const Entity*>();
        _invalidatedEntities = new list<const Entity*>();
        _generatedEntities = new list<const Entity*>();
        
        setValue(rdf::_type, prov::Activity);
    }
    
    Activity::Activity(const char* uriref) : Resource(uriref)
    {
        _associations = new list<const Association*>();
        _usedEntities = new list<const Entity*>();
        _invalidatedEntities = new list<const Entity*>();
        _generatedEntities = new list<const Entity*>();
        
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
    
    void Activity::setTime(const time_t* time)
    {
        setStartTime(time);
        setEndTime(time);
    }
    
    const time_t* Activity::getTime()
    {
        return _startTime;
    }
    
    void Activity::setEndTime(const time_t* time)
    {
        _endTime = time;
        
        setValue(prov::endedAtTime, _endTime);
    }
    
    const time_t* Activity::getEndTime()
    {
        return _endTime;
    }
    
    void Activity::setStartTime(const time_t* time)
    {
        _startTime = time;
        
        setValue(prov::startedAtTime, _startTime);
    }
    
    const time_t* Activity::getStartTime()
    {
        return _startTime;
    }
    
    list<const Association*> Activity::getAssociations()
    {
        return *_associations;
    }
    
    void Activity::addAssociation(const Association* association)
    {
        _associations->push_back(association);
        
        addProperty(prov::qualifiedAssociation, association);
    }
    
    void Activity::removeAssociation(const Association* association)
    {        
        _associations->remove(association);
        
        removeProperty(prov::qualifiedAssociation, association);
    }
    
    list<const Entity*> Activity::getUsedEntities()
    {
        return *_usedEntities;
    }
    
    void Activity::addUsedEntity(const Entity* entity)
    {
        _usedEntities->push_back(entity);
        
        addProperty(prov::used, entity);
    }
    
    void Activity::removeUsedEntity(const Entity* entity)
    {        
        _usedEntities->remove(entity);
        
        removeProperty(prov::used, entity);
    }
    
    list<const Entity*> Activity::getInvalidatedEntities()
    {
        return *_invalidatedEntities;
    }
    
    void Activity::addInvalidatedEntity(const Entity* entity)
    {
        _invalidatedEntities->push_back(entity);
        
        addProperty(prov::invalidated, entity);
    }
    
    void Activity::removeInvalidatedEntity(const Entity* entity)
    {       
        _invalidatedEntities->remove(entity);
        
        removeProperty(prov::invalidated, entity);
    }
    
    list<const Entity*> Activity::getGeneratedEntities()
    {
        return *_generatedEntities;
    }
    
    void Activity::addGeneratedEntity(const Entity* entity)
    {
        _generatedEntities->push_back(entity);
        
        addProperty(prov::generated, entity);
    }
    
    void Activity::removeGeneratedEntity(const Entity* entity)
    {        
        _generatedEntities->remove(entity);
        
        removeProperty(prov::generated, entity);
    }
}

