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

#ifndef _ART_ENTITY_H
#define _ART_ENTITY_H

#include <list>

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"
#include "../Ontologies/nie.h"
#include "../Ontologies/dces.h"
#include "../Resource.h"
#include "../Property.h"

namespace artivity
{
    class Entity;

    typedef boost::shared_ptr<Entity> EntityRef;

    class Entity : public Resource
    {
    private:
        std::string _title;

        time_t _created;

        time_t _modified;

    public:
        Entity() : Resource(UriGenerator::getUri())
        {
            setType(prov::Entity);
        }
        
        Entity(const char* uriref) : Resource(uriref)
        {
            setType(prov::Entity);
        }

        time_t getCreated()
        {
            return _created;
        }

        void setCreated(time_t created)
        {
            _created = created;

            setValue(nie::created, &_created);
        }

        void resetCreated()
        {
            _created = 0;

            removeProperty(nie::created, &_created);
        }

        time_t getModified()
        {
            return _modified;
        }

        void setModified(time_t modified)
        {
            _modified = modified;

            setValue(nie::lastModified, &modified);
        }

        void resetModified()
        {
            _modified = 0;

            removeProperty(nie::lastModified, &_modified);
        }

        void setTitle(std::string title)
        {
            _title = title;

            setValue(dces::title, _title.c_str());
        }
    };
}

#endif // _ART_ENTITY_H
