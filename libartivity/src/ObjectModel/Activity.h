// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

#ifndef _ART_ACTIVITY_H
#define _ART_ACTIVITY_H

#include <list>

#include "../Resource.h"

#include "Entity.h"
#include "Association.h"
#include "Generation.h"
#include "Invalidation.h"
#include "EntityInfluence.h"

namespace artivity
{
    class Activity;

    typedef boost::shared_ptr<Activity> ActivityRef;

    class Activity : public Resource
    {
    private:        
		std::list<EntityRef>* _usages;

        std::list<AssociationRef>* _associations;
                                                      
		std::list<GenerationRef>* _generations;

		std::list<InvalidationRef>* _invalidations;

		std::list<EntityInfluenceRef>* _influences;

        time_t _startTime;
        
        time_t _endTime;
        
    public:
        Activity();
        Activity(const char* uriref);
        virtual ~Activity();
               
		bool empty();
        void clear();
        
        void setTime(time_t time);
        time_t getTime();
        
        void setStartTime(time_t time);
        time_t getStartTime();
        
        void setEndTime(time_t time);
        time_t getEndTime();
        
		std::list<EntityRef> getUsedEntities();
		void addUsage(EntityRef entity);
		void removeUsage(EntityRef entity);

        std::list<AssociationRef> getAssociations();
        void addAssociation(AssociationRef association);
        void removeAssociation(AssociationRef association);
               
		std::list<GenerationRef> getGenerations();
		void addInfluence(GenerationRef generation);
		void removeInfluence(GenerationRef generation);

		std::list<InvalidationRef> getInvalidations();
		void addInfluence(InvalidationRef invalidation);
		void removeInfluence(InvalidationRef invalidation);

		std::list<EntityInfluenceRef> getEntityInfluences();
		void addInfluence(EntityInfluenceRef influence);
		void removeInfluence(EntityInfluenceRef influence);
    };
}

#endif // _ART_ACTIVITY_H
