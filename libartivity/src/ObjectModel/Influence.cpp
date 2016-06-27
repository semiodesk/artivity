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
#include "../PropertyMap.h"

#include "Activity.h"
#include "Influence.h"

namespace artivity
{
	using namespace std;

    list<EntityRef> Influence::getEntities()
    {
        return entities;
    }

    void Influence::addEntity(EntityRef entity)
    {
        entities.push_back(entity);
    }

    ActivityRef Influence::getActivity()
    {
        return activity;
    }

    void Influence::removeEntity(EntityRef entity)
    {
        entities.remove(entity);
    }

    void Influence::addChange(ChangeRef change)
    {
        changes.push_back(change);

        addProperty(art::hadChange, change);
    }

    void Influence::addChange(EntityRef entity, const char* property, ResourceRef resource)
    {
        ChangeRef change = ChangeRef(new Change());
        change->setEntity(entity);
        change->addProperty(art::property, property, typeid(Resource));
        change->addProperty(art::value, resource);

        changes.push_back(change);

        addProperty(art::hadChange, change);
    }

    void Influence::addChange(EntityRef entity, const char* property, double value)
    {
        ChangeRef change = ChangeRef(new Change());
        change->setEntity(entity);
        change->addProperty(art::property, property, typeid(Resource));
        change->addProperty(art::value, value);

        changes.push_back(change);

        addProperty(art::hadChange, change);
    }

    void Influence::removeChange(ChangeRef change)
    {
        changes.remove(change);

        removeProperty(art::hadChange, change);
    }

    void Influence::clearChanges()
    {
        changes.clear();
    }

    ViewportRef Influence::getViewport()
    {
        return _viewport;
    }

    void Influence::setViewport(ViewportRef viewport)
    {
        _viewport = viewport;

        setValue(art::hadViewport, viewport);
    }

    GeometryRef Influence::getBoundaries()
    {
        return _boundaries;
    }

    void Influence::setBoundaries(GeometryRef boundaries)
    {
        _boundaries = boundaries;

        setValue(art::hadBoundaries, boundaries);
    }

    RenderingDataObjectRef Influence::getRendering()
    {
        return _rendering;
    }

    void Influence::setRendering(RenderingDataObjectRef render)
    {
        _rendering = render;

        setValue(art::renderedAs, _rendering);
    }
}

