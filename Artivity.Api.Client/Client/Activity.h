#ifndef ACTIVITY_H
#define ACTIVITY_H

#include "Resource.h"

namespace artivity
{
    namespace client
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
            Activity(const char* uriref);
            ~Activity();
            
            void setDisplayName(string name);
            string getDisplayName();
            
            void setUrl(const char* url);
            const char* getUrl();
            
            void setTime(time_t time);
            time_t getTime();
            
            void setStartTime(time_t time);
            time_t getStartTime();
            
            void setEndTime(time_t time);
            time_t getEndTime();
            
            void setUpdateTime(time_t time);
            time_t getUpdateTime();
            
            void setActor(Resource& actor);
            Resource* getActor();
            
            void setInstrument(Resource& instrument);
            Resource* getInstrument();
            
            void setObject(Resource& object);
            Resource* getObject();
            
            void setTarget(Resource& target);
            Resource* getTarget();
        };
    }
}

#endif // ACTIVITY_H
