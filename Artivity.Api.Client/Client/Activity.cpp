#include "Activity.h"

#include "Ontologies/as.h"
#include "Ontologies/rdf.h"

namespace artivity
{
    Activity::Activity(const char* uriref) : Resource(uriref)
    {
        _actor = NULL;
        _instrument = NULL;
        _target = NULL;
        _object = NULL;
        
        setValue(rdf::_type, as::Activity);
    }

    Activity::~Activity() {}
    
    bool Activity::isValid()
    {
        // TODO: Check if type, actor, object and time is set.
        // See: http://www.w3.org/TR/activitystreams-core/#example-1
        return true;
    }
    
    void Activity::setDisplayName(string name)
    {
        _displayName = name;
        
        setValue(as::displayName, name.c_str());
    }
    
    string Activity::getDisplayName()
    {
        return _displayName;
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
        
        setValue(as::endTime, &time);
    }
    
    time_t Activity::getEndTime()
    {
        return _endTime;
    }
    
    void Activity::setStartTime(time_t time)
    {
        _startTime = time;
        
        setValue(as::startTime, &time);
    }
    
    time_t Activity::getStartTime()
    {
        return _startTime;
    }
    
    void Activity::setUpdateTime(time_t time)
    {
        _updateTime = time;
        
        setValue(as::updateTime, &time);
    }
    
    time_t Activity::getUpdateTime()
    {
        return _updateTime;
    }
    
    void Activity::setActor(Resource& actor)
    {
        _actor = &actor;
        
        setValue(as::actor, actor);
    }
    
    Resource* Activity::getActor()
    {
        return _actor;
    }
    
    void Activity::setInstrument(Resource& instrument)
    {
        _instrument = &instrument;
        
        setValue(as::instrument, instrument);
    }
    
    Resource* Activity::getInstrument()
    {
        return _instrument;
    }
    
    void Activity::setObject(Resource& object)
    {
        _object = &object;
        
        setValue(as::object, object);
    }
    
    Resource* Activity::getObject()
    {
        return _object;
    }
    
    void Activity::setTarget(Resource& target)
    {
        _target = &target;
        
        setValue(as::target, target);
    }
    
    Resource* Activity::getTarget()
    {
        return _target;
    }
}

