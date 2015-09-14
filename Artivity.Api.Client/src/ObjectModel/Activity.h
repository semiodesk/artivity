#ifndef ACTIVITY_H
#define ACTIVITY_H

#include <list>

#include "../Resource.h"

#include "Entity.h"
#include "Association.h"

namespace artivity
{
    class Activity : public Resource
    {
    private:        
        list<const Association*>* _associations;
    
        list<const Entity*>* _usedEntities;
                    
        list<const Entity*>* _invalidatedEntities;
                                
        list<const Entity*>* _generatedEntities;

        const time_t* _startTime;
        
        const time_t* _endTime;
        
    public:
        Activity();
        Activity(const char* uriref);
        virtual ~Activity();
        
        bool isValid();
        
        void setTime(const time_t* time);
        const time_t* getTime();
        
        void setStartTime(const time_t* time);
        const time_t* getStartTime();
        
        void setEndTime(const time_t* time);
        const time_t* getEndTime();
        
        list<const Association*> getAssociations();
        void addAssociation(const Association* association);
        void removeAssociation(const Association* association);
        
        list<const Entity*> getUsedEntities();
        void addUsedEntity(const Entity* entity);
        void removeUsedEntity(const Entity* entity);
        
        list<const Entity*> getInvalidatedEntities();
        void addInvalidatedEntity(const Entity* entity);
        void removeInvalidatedEntity(const Entity* entity);
        
        list<const Entity*> getGeneratedEntities();
        void addGeneratedEntity(const Entity* entity);
        void removeGeneratedEntity(const Entity* entity);
    };
}

#endif // ACTIVITY_H
