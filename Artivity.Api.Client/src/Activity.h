#ifndef ACTIVITY_H
#define ACTIVITY_H

#include <list>
#include "Resource.h"
#include "Association.h"
#include "Entities/Entity.h"

namespace artivity
{
    class Activity : public Resource
    {
    private:        
        list<Association*>* _associations;
    
        list<Entity*>* _usedEntities;
                    
        list<Entity*>* _invalidatedEntities;
                                
        list<Entity*>* _generatedEntities;

        time_t _startTime;
        
        time_t _endTime;
        
    public:
        Activity();
        Activity(const char* uriref);
        virtual ~Activity();
        
        bool isValid();
        
        void setTime(time_t time);
        time_t getTime();
        
        void setStartTime(time_t time);
        time_t getStartTime();
        
        void setEndTime(time_t time);
        time_t getEndTime();
        
        list<Association*> getAssociations();
        void addAssociation(Association* association);
        void removeAssociation(Association* association);
        
        list<Entity*> getUsedEntities();
        void addUsedEntity(Entity* entity);
        void removeUsedEntity(Entity* entity);
        
        list<Entity*> getInvalidatedEntities();
        void addInvalidatedEntity(Entity* entity);
        void removeInvalidatedEntity(Entity* entity);
        
        list<Entity*> getGeneratedEntities();
        void addGeneratedEntity(Entity* entity);
        void removeGeneratedEntity(Entity* entity);
    };
}

#endif // ACTIVITY_H
