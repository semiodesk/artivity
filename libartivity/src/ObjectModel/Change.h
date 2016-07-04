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

#ifndef _ART_CHANGE_H
#define _ART_CHANGE_H

#include "../Ontologies/art.h"

#include "Entity.h"

namespace artivity
{
    class Change;

    typedef boost::shared_ptr<Change> ChangeRef;

    class Change : public Resource
    {
    private:
        EntityRef _entity;

        const char* _property;

    public:
        Change() : Resource(UriGenerator::getUri())
        {
            setType(art::Change);
        }
        
        Change(const char* uriref) : Resource(uriref)
        {
            setType(art::Change);
        }

        Change(EntityRef entity, const char* property, ResourceRef value) : Resource(UriGenerator::getUri())
        {
            setType(art::Change);
            addProperty(art::entity, entity);
            addProperty(art::property, property, typeid(Resource));
            addProperty(art::value, value);
        }

        Change(EntityRef entity, const char* property, std::string value) : Resource(UriGenerator::getUri())
        {
            setType(art::Change);
            addProperty(art::entity, entity);
            addProperty(art::property, property, typeid(Resource));
            addProperty(art::value, value);
        }

        Change(EntityRef entity, const char* property, int value) : Resource(UriGenerator::getUri())
        {
            setType(art::Change);
            addProperty(art::entity, entity);
            addProperty(art::property, property, typeid(Resource));
            addProperty(art::value, value);
        }

        Change(EntityRef entity, const char* property, double value) : Resource(UriGenerator::getUri())
        {
            setType(art::Change);
            addProperty(art::entity, entity);
            addProperty(art::property, property, typeid(Resource));
            addProperty(art::value, value);
        }

        void setEntity(EntityRef entity);

        void setProperty(std::string property);

        void setValue(PropertyValue value);
    };
}

#endif // _ART_CHANGE_H
