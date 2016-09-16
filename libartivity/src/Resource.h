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

#ifndef _ART_RESOURCE_H
#define _ART_RESOURCE_H

#include <map>
#include <ctime>
#include <cstring>
#include <sstream>
#include <boost/shared_ptr.hpp>
#include "PropertyMap.h"

namespace artivity
{           
    class Property;

    class Serializer;

    class Resource;

    typedef boost::shared_ptr<Resource> ResourceRef;
    
    class Resource
    {
        friend class Serializer;
        friend class Influence;

    protected:
        void addProperty(const Property& property, PropertyValue value);

        void removeProperty(const Property& property, PropertyValue value);

        void setValue(const Property& property, PropertyValue value);

    public:
        std::string uri;

        PropertyMap properties;

		bool serialize;

        Resource(std::string uriref);
        Resource(const char* uriref);
        virtual ~Resource() {}

        bool is(const char* type);
        
        void clear();

		bool hasProperties();
        bool hasProperty(const Property& property, ResourceRef value);
        bool hasProperty(const Property& property, const time_t* value);
        bool hasProperty(const Property& property, const char* value);
        bool hasProperty(const Property& property, std::string value);
        bool hasProperty(const Property& property, int value);
        bool hasProperty(const Property& property, long value);
        bool hasProperty(const Property& property, float value);
        bool hasProperty(const Property& property, double value);

        
        void addProperty(const Property& property, ResourceRef value);
        void addProperty(const Property& property, const time_t* value);
        void addProperty(const Property& property, const char* value);
        void addProperty(const Property& property, std::string value);
        void addProperty(const Property& property, std::string value, const std::type_info& typeInfo);
        void addProperty(const Property& property, int value);
        void addProperty(const Property& property, long value);
        void addProperty(const Property& property, float value);
        void addProperty(const Property& property, double value);

        void removeProperty(const Property& property, ResourceRef value);        
        void removeProperty(const Property& property, const time_t* value);
        void removeProperty(const Property& property, const char* value);
        void removeProperty(const Property& property, std::string value);
        void removeProperty(const Property& property, std::string value, const std::type_info& typeInfo);
        void removeProperty(const Property& property, int value);
        void removeProperty(const Property& property, long value);
        void removeProperty(const Property& property, float value);
        void removeProperty(const Property& property, double value);
        void removeProperty(const Property& property);

        void setValue(const Property& property, ResourceRef value);        
        void setValue(const Property& property, const time_t* value);
        void setValue(const Property& property, const char* value);
        void setValue(const Property& property, std::string value);
        void setValue(const Property& property, std::string value, const std::type_info& typeInfo);
        void setValue(const Property& property, int value);
        void setValue(const Property& property, long value);
        void setValue(const Property& property, float value);
        void setValue(const Property& property, double value);
        
        void setType(const char* value);
        
        const char* getType();
        
        void setUri(std::string uriref);
        
        bool operator==(ResourceRef other);
        
        friend std::ostream& operator<<(std::ostream& out, ResourceRef resource);
    };
}

#endif // _ART_RESOURCE_H
