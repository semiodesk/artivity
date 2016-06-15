#ifndef ACTIVITY_H
#define ACTIVITY_H

#include <list>

#include "../Resource.h"

#include "Entity.h"
#include "Association.h"

namespace artivity
{
    class Activity;
    typedef boost::shared_ptr<Activity> ActivityRef;

    class Activity : public Resource
    {
    private:        
        std::list<AssociationRef>* _associations;
    
        std::list<EntityRef>* _usedEntities;
                    
        std::list<EntityRef>* _invalidatedEntities;
                                
        std::list<EntityRef>* _generatedEntities;

        time_t _startTime;
        
        time_t _endTime;
        
    public:
        Activity();
        Activity(const char* uriref);
        virtual ~Activity();
        
        bool isValid();
        
        void clear();
        
        void setTime(time_t time);
        time_t getTime();
        
        void setStartTime(time_t time);
        time_t getStartTime();
        
        void setEndTime(time_t time);
        time_t getEndTime();
        
        std::list<AssociationRef> getAssociations();
        void addAssociation(AssociationRef association);
        void removeAssociation(AssociationRef association);
        
        std::list<EntityRef> getUsedEntities();
        void addUsedEntity(EntityRef entity);
        void removeUsedEntity(EntityRef entity);
        
        std::list<EntityRef> getInvalidatedEntities();
        void addInvalidatedEntity(EntityRef entity);
        void removeInvalidatedEntity(EntityRef entity);
        
        std::list<EntityRef> getGeneratedEntities();
        void addGeneratedEntity(EntityRef entity);
        void removeGeneratedEntity(EntityRef entity);
    };
}

#endif // ACTIVITY_H
