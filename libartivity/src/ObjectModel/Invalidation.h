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

#ifndef _ART_INVALIDATION_H
#define _ART_INVALIDATION_H

#include "ActivityInfluence.h"

namespace artivity
{
    class Invalidation;

    typedef boost::shared_ptr<Invalidation> InvalidationRef;

    class Invalidation : public ActivityInfluence
    {   
        private:
        std::list<EntityRef> _entities;

        public:
        Invalidation() : ActivityInfluence()
        {
            setType(prov::Invalidation);
        }
        
        Invalidation(const char* uriref) : ActivityInfluence(uriref)
        {
            setType(prov::Invalidation);
        }

        void addInvalidated(EntityRef entity)
        {
            addProperty(prov::invalidated, entity);
            _entities.push_back(entity);
        }

        void removeInvalidated(EntityRef entity)
        {
            removeProperty(prov::invalidated, entity);
            _entities.remove(entity);
        }

        void clearInvalidated(EntityRef entity)
        {
            properties.erase(prov::invalidated);
            _entities.clear();
        }
    };
}

#endif // _ART_INVALIDATION_H
