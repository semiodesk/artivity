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

#include "../Resource.h"
#include "../Property.h"

#include "Influences/Revision.h"
#include "Influences/Derivation.h"
#include "Influences/EntityInfluence.h"

#include "Entity.h"

namespace artivity
{
	using namespace std;

    time_t Entity::getCreated()
    {
        return _created;
    }

    void Entity::setCreated(time_t created)
    {
        _created = created;

        setValue(nie::created, &_created);
    }

    void Entity::resetCreated()
    {
        _created = 0;

        removeProperty(nie::created, &_created);
    }

    time_t Entity::getModified()
    {
        return _modified;
    }

    void Entity::setModified(time_t modified)
    {
        _modified = modified;

        setValue(nie::lastModified, &modified);
    }

    void Entity::resetModified()
    {
        _modified = 0;

        removeProperty(nie::lastModified, &_modified);
    }

    void Entity::setTitle(string title)
    {
        _title = title;

        setValue(dces::title, _title.c_str());
    }

    list<EntityInfluenceRef> Entity::getInfluences()
    {
        return _influences;
    }

    void Entity::addInfluence(EntityInfluenceRef influence)
    {
        if (hasProperty(prov::qualifiedInfluence, influence)) return;

        _influences.push_back(influence);

        addProperty(prov::qualifiedInfluence, influence);
    }

    void Entity::addInfluence(DerivationRef derivation)
    {
        if (hasProperty(prov::qualifiedDerivation, derivation)) return;

        _influences.push_back(derivation);

        addProperty(prov::qualifiedDerivation, derivation);
    }

    void Entity::addInfluence(RevisionRef revision)
    {
        if (hasProperty(prov::qualifiedRevision, revision)) return;

        _influences.push_back(revision);

        addProperty(prov::qualifiedRevision, revision);
    }
}

