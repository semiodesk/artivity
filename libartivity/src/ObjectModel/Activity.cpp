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
#include "../Resource.h"
#include "../Property.h"

#include "Entity.h"
#include "Influence.h"
#include "Association.h"
#include "Activity.h"

namespace artivity
{
	using namespace std;

    Activity::Activity() : Resource(UriGenerator::getUri())
    {
		setType(prov::Activity);
    }
    
    Activity::Activity(const char* uriref) : Resource(uriref)
    {
		setType(prov::Activity);
    }

    Activity::~Activity()
    {
    }
    
	bool Activity::empty()
	{
		return properties.empty();
	}

    void Activity::clear()
    {
        Resource::clear();
        
        _influences.clear();
        // Note: The association can be cleared here because it only needs to be transmitted once.
		_associations.clear();
        _usedEntities.clear();
        _generatedEntities.clear();
        _invalidatedEntities.clear();
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
    
    list<InfluenceRef> Activity::getInfluences()
    {
        return _influences;
    }
    
    list<AssociationRef> Activity::getAssociations()
    {
        return _associations;
    }
    
    void Activity::addAssociation(AssociationRef association)
    {
        if(hasProperty(prov::qualifiedAssociation, association)) return;
        
        _associations.push_back(association);
        
        addProperty(prov::qualifiedAssociation, association);
    }
    
    void Activity::removeAssociation(AssociationRef association)
    {        
        _associations.remove(association);
        
        removeProperty(prov::qualifiedAssociation, association);
    }
    
    list<EntityRef> Activity::getUsedEntities()
    {
        return _usedEntities;
    }

    void Activity::clearUsedEntities()
    {
        for (auto entity : _usedEntities)
        {
            removeProperty(prov::used, entity);

            list<InfluenceRef> influences = entity->getInfluences();

            for (auto influence = influences.begin(); influence != influences.end(); influence++)
            {
                _influences.remove(*influence);
            }
        }
        _usedEntities.clear();
    }
    
    void Activity::addUsed(EntityRef entity)
    {
        if (hasProperty(prov::used, entity)) return;
        
        _usedEntities.push_back(entity);
        
        addProperty(prov::used, entity);
        
        list<InfluenceRef> influences = entity->getInfluences();
        
        _influences.insert(_influences.end(), influences.begin(), influences.end());
    }
    
    void Activity::removeUsed(EntityRef entity)
    {
        _usedEntities.remove(entity);
        
        removeProperty(prov::used, entity);
        
        list<InfluenceRef> influences = entity->getInfluences();
        
        for(auto influence = influences.begin(); influence != influences.end(); influence++)
        {
            _influences.remove(*influence);
        }
    }
    
    list<EntityRef> Activity::getInvalidatedEntities()
    {
        return _invalidatedEntities;
    }
    
    void Activity::clearInvalidatedEntities()
    {
        for(auto entity = _invalidatedEntities.begin(); entity != _invalidatedEntities.end(); entity++)
        {
            removeInvalidated(*entity);
        }
    }
    
    void Activity::addInvalidated(EntityRef entity)
    {
        if (hasProperty(prov::invalidated, entity)) return;
        
        _invalidatedEntities.push_back(entity);
        
        addProperty(prov::invalidated, entity);
        
        list<InfluenceRef> influences = entity->getInfluences();
        
        _influences.insert(_influences.end(), influences.begin(), influences.end());
    }
    
    void Activity::removeInvalidated(EntityRef entity)
    {       
        _invalidatedEntities.remove(entity);
        
        removeProperty(prov::invalidated, entity);
        
        list<InfluenceRef> influences = entity->getInfluences();
        
        for(auto influence = influences.begin(); influence != influences.end(); influence++)
        {
            _influences.remove(*influence);
        }
    }
    
	list<EntityRef> Activity::getGeneratedEntities()
    {
		return _generatedEntities;
    }
    
    void Activity::clearGeneratedEntities()
    {
        for(auto entity = _generatedEntities.begin(); entity != _generatedEntities.end(); entity++)
        {
            removeGenerated(*entity);
        }
    }
    
    void Activity::addGenerated(EntityRef entity)
    {
        if (hasProperty(prov::generated, entity)) return;
        
        _generatedEntities.push_back(entity);
        
        addProperty(prov::generated, entity);
        
        list<InfluenceRef> influences = entity->getInfluences();
        
        _influences.insert(_influences.end(), influences.begin(), influences.end());
    }
    
    void Activity::removeGenerated(EntityRef entity)
    {        
        _generatedEntities.remove(entity);
        
        removeProperty(prov::generated, entity);
        
        list<InfluenceRef> influences = entity->getInfluences();
        
        for(auto influence = influences.begin(); influence != influences.end(); influence++)
        {
            _influences.remove(*influence);
        }
    }

    struct by_Uri
    {
        by_Uri(std::string x) : x(x) {}
        
        bool operator()(ResourceRef const & r) const
        {
            return x == r->uri;
        }
        
        const std::string x;
    };

    EntityRef Activity::getEntity(std::string uri)
    {
        std::list<EntityRef>::iterator e = std::find_if(_usedEntities.begin(), _usedEntities.end(), by_Uri(uri));
        
        if (e != _usedEntities.end())
        {
            return *e;
        }
        
        e = std::find_if(_generatedEntities.begin(), _generatedEntities.end(), by_Uri(uri));
        
        if (e != _generatedEntities.end())
        {
            return *e;
        }
        
        e = std::find_if(_invalidatedEntities.begin(), _invalidatedEntities.end(), by_Uri(uri));
        
        if (e != _invalidatedEntities.end())
        {
            return *e;
        }
        
        return EntityRef();
    }
}

