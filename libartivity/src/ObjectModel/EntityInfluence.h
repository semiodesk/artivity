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

#ifndef _ART_ENTITYINFLUENCE_H
#define _ART_ENTITYINFLUENCE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../Ontologies/dces.h"
#include "../UriGenerator.h"
#include "../Resource.h"
#include "Entity.h"
#include "Influence.h"
#include "Change.h"

namespace artivity
{
	class EntityInfluence;

	typedef boost::shared_ptr<EntityInfluence> EntityInfluenceRef;

	class EntityInfluence : public Influence
	{
        private:
        std::list<ChangeRef> _changes;
        std::list<EntityRef> _entities;
        bool _isSave;

	    public:
        EntityInfluence() : Influence()
		{
            _isSave = false;
			setType(prov::EntityInfluence);
		}

		EntityInfluence(const char* uriref) : Influence(uriref)
		{
            _isSave = false;
			setType(prov::EntityInfluence);
		}

        void addChange(ChangeRef change)
        {
            _changes.push_back(change);
            addProperty(art::qualifiedChange, change);
        }

        void removeChange(ChangeRef change)
        {
            _changes.remove(change);
            removeProperty(art::qualifiedChange, change);
        }

        void clearChanges()
        {
            _changes.clear();
            
        }

        void addEntity(EntityRef entity)
        {
            _entities.push_back(entity);
            addProperty(prov::entity, entity);
        }

        void removeChange(EntityRef entity)
        {
            _entities.remove(entity);
            removeProperty(prov::entity, entity);
        }

        void clearEntities()
        {
            _entities.clear();
        }

        void setIsSave(bool val)
        {
            _isSave = val;
        }

        bool getIsSave()
        {
            return _isSave;
        }
	};
}

#endif // _ART_ENTITYINFLUENCE_H
