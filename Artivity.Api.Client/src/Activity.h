#ifndef ACTIVITY_H
#define ACTIVITY_H

#include "Resource.h"

namespace artivity
{
    class Activity : public Resource
    {
    private:
        string _displayName;
        
        const char* _url;
        
        time_t _startTime;
        
        time_t _endTime;
        
        time_t _updateTime;
        
        Resource* _actor;
        
        Resource* _instrument;
                    
        Resource* _object;
                                
        Resource* _target;

    public:
        Activity();
        Activity(const char* uriref);
        ~Activity();
        
        bool isValid();
        
        void setDisplayName(string name);
        string getDisplayName();
        
        void setTime(time_t time);
        time_t getTime();
        
        void setStartTime(time_t time);
        time_t getStartTime();
        
        void setEndTime(time_t time);
        time_t getEndTime();
        
        void setUpdateTime(time_t time);
        time_t getUpdateTime();
        
        // Set the actor in the activity, i.e. a person modifying an image.
        void setActor(Resource& actor);
        
        // Get the actor in the activity, i.e. a person modifying an image.
        Resource* getActor();
        
        // Set the target of the activity, i.e. a file being modified.
        void setTarget(Resource& target);
        
        // Get the target of the activity, i.e. a file being modified.
        Resource* getTarget();
        
        // Set the tool being used in the activity, i.e. an image manipulation program.
        void setInstrument(Resource& instrument);
        
        // Get the tool being used in the activity, i.e. an image manipulation program.
        Resource* getInstrument();
        
        // Set the object of the activity, i.e. an image element being modified.
        void setObject(Resource& object);
        
        // Get the object of the activity, i.e. an image element being modified.
        Resource* getObject();
    };
}

#endif // ACTIVITY_H
