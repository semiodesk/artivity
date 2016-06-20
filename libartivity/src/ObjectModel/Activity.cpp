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

#include <algorithm>

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

#include "Activity.h"

namespace artivity
{
	using namespace std;

    Activity::Activity() : Resource(UriGenerator::getUri())
    {
		setType(prov::Activity);

		_usages = new list<EntityRef>();
        _associations = new list<AssociationRef>();
		_generations = new list<GenerationRef>();
		_invalidations = new list<InvalidationRef>();
		_influences = new list<EntityInfluenceRef>();
    }
    
    Activity::Activity(const char* uriref) : Resource(uriref)
    {
		setType(prov::Activity);

		_usages = new list<EntityRef>();
		_associations = new list<AssociationRef>();
		_generations = new list<GenerationRef>();
		_invalidations = new list<InvalidationRef>();
		_influences = new list<EntityInfluenceRef>();
    }

    Activity::~Activity()
    {
		delete _usages;
		delete _associations;
        delete _generations;
		delete _invalidations;
		delete _influences;
    }
    
	bool Activity::empty()
	{
		return Properties.empty();
	}

    void Activity::clear()
    {
        Resource::clear();
        
		_usages->clear();
		_associations->clear();
		_generations->clear();
		_invalidations->clear();
		_influences->clear();
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
    
    list<AssociationRef> Activity::getAssociations()
    {
        return *_associations;
    }
    
    void Activity::addAssociation(AssociationRef association)
    {
        if(hasProperty(prov::qualifiedAssociation, association)) return;
        
        _associations->push_back(association);
        
        addProperty(prov::qualifiedAssociation, association);
    }
    
    void Activity::removeAssociation(AssociationRef association)
    {        
        _associations->remove(association);
        
        removeProperty(prov::qualifiedAssociation, association);
    }
    
    list<EntityRef> Activity::getUsedEntities()
    {
        return *_usages;
    }
    
    void Activity::addUsage(EntityRef entity)
    {
        if(hasProperty(prov::used, entity)) return;
        
		_usages->push_back(entity);
        
        addProperty(prov::used, entity);
    }
    
    void Activity::removeUsage(EntityRef entity)
    {
		_usages->remove(entity);
        
        removeProperty(prov::used, entity);
    }
    
    list<InvalidationRef> Activity::getInvalidations()
    {
        return *_invalidations;
    }
    
    void Activity::addInfluence(InvalidationRef invalidation)
    {
		if (hasProperty(prov::qualifiedInvalidation, invalidation)) return;
        
		_invalidations->push_back(invalidation);
        
		addProperty(prov::qualifiedInvalidation, invalidation);
    }
    
	void Activity::removeInfluence(InvalidationRef invalidation)
    {       
		_invalidations->remove(invalidation);
        
		removeProperty(prov::qualifiedInvalidation, invalidation);
    }
    
	list<GenerationRef> Activity::getGenerations()
    {
		return *_generations;
    }
    
    void Activity::addInfluence(GenerationRef generation)
    {
		if (hasProperty(prov::qualifiedGeneration, generation)) return;
        
		_generations->push_back(generation);
        
		addProperty(prov::qualifiedGeneration, generation);
    }
    
	void Activity::removeInfluence(GenerationRef generation)
    {        
		_generations->remove(generation);
        
		removeProperty(prov::qualifiedGeneration, generation);
    }

	list<EntityInfluenceRef> Activity::getEntityInfluences()
	{
		return *_influences;
	}

	void Activity::addInfluence(EntityInfluenceRef influence)
	{
		if (hasProperty(prov::qualifiedInfluence, influence)) return;

		_influences->push_back(influence);

		addProperty(prov::qualifiedInfluence, influence);
	}

	void Activity::removeInfluence(EntityInfluenceRef influence)
	{
		_influences->remove(influence);

		removeProperty(prov::qualifiedInfluence, influence);
	}
}

