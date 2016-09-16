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

#include <typeinfo>

#include "Resource.h"
#include "Property.h"
#include "Serializer.h"

#include "Ontologies/rdf.h"

namespace artivity
{
	using namespace std;

    Resource::Resource(string uriref)
    {
        uri = uriref;        
		serialize = true;
    }
    
    Resource::Resource(const char* uriref)
    {
        uri = string(uriref);      
		serialize = true;
    }

    bool Resource::is(const char* type)
    {
        return properties.hasProperty(rdf::_type, type, typeid(Resource));
    }
    
    void Resource::clear()
    {
        properties.clear();
    }
    
	bool Resource::hasProperties()
	{
		return !properties.empty();
	}

    bool Resource::hasProperty(const Property& property, ResourceRef value)
    {
        return properties.hasProperty(property.uri, value);
    }
    
    bool Resource::hasProperty(const Property& property, const char* value)
    {            
        return properties.hasProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    bool Resource::hasProperty(const Property& property, int value)
    {
        return properties.hasProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    bool Resource::hasProperty(const Property& property, long value)
    {
        return properties.hasProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    bool Resource::hasProperty(const Property& property, float value)
    {
        return properties.hasProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    bool Resource::hasProperty(const Property& property, double value)
    {
        return properties.hasProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    bool Resource::hasProperty(const Property& property, const time_t* value)
    {
        return properties.hasProperty(property.uri, Serializer::toString(value), typeid(value));
    }

    void Resource::addProperty(const Property& property, PropertyValue value)
    {
        properties.addProperty(property.uri, value);
    }

    void Resource::addProperty(const Property& property, ResourceRef value)
    {
        properties.addProperty(property.uri, value);
    } 

    void Resource::addProperty(const Property& property, std::string value)
    {
        properties.addProperty(property.uri, value, typeid(value));
    }

    void Resource::addProperty(const Property& property, std::string value, const type_info& typeInfo)
    {
        properties.addProperty(property.uri, value, typeInfo);
    }

    void Resource::addProperty(const Property& property, const char* value)
    {
        properties.addProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::addProperty(const Property& property, int value)
    {
        properties.addProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::addProperty(const Property& property, long value)
    {
        properties.addProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::addProperty(const Property& property, float value)
    {
        properties.addProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::addProperty(const Property& property, double value)
    {
        properties.addProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::addProperty(const Property& property, const time_t* value)
    {
        properties.addProperty(property.uri, Serializer::toString(value), typeid(value));
    }

    void Resource::removeProperty(const Property& property, PropertyValue value)
    {
        properties.removeProperty(property.uri, value);
    }

    void Resource::removeProperty(const Property& property, ResourceRef value)
    {
        properties.removeProperty(property.uri, value);
    }
   
    void Resource::removeProperty(const Property& property, const char* value)
    {
        properties.removeProperty(property.uri, Serializer::toString(value), typeid(value));
    }

    void Resource::removeProperty(const Property& property, std::string value)
    {
        properties.removeProperty(property.uri, value, typeid(value));
    }
    
    void Resource::removeProperty(const Property& property, std::string value, const type_info& typeInfo)
    {
        properties.removeProperty(property.uri, value, typeInfo);
    }

    void Resource::removeProperty(const Property& property, int value)
    {
        properties.removeProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::removeProperty(const Property& property, long value)
    {
        properties.removeProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::removeProperty(const Property& property, float value)
    {
        properties.removeProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::removeProperty(const Property& property, double value)
    {
        properties.removeProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::removeProperty(const Property& property, const time_t* value)
    {
        properties.removeProperty(property.uri, Serializer::toString(value), typeid(value));
    }

    void Resource::removeProperty(const Property& property)
    {
        properties.removeProperty(property.uri);
    }

    void Resource::setValue(const Property& property, PropertyValue value)
    {
        properties.setProperty(property.uri, value);
    }

    void Resource::setValue(const Property& property, ResourceRef value)
    {
        properties.setProperty(property.uri, value);
    }  

    void Resource::setValue(const Property& property, const time_t* value)
    {
        properties.setProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::setValue(const Property& property, const char* value)
    {
        properties.setProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::setValue(const Property& property, std::string value)
    {
        properties.setProperty(property.uri, value, typeid(value));
    }

    void Resource::setValue(const Property& property, std::string value, const type_info& typeInfo)
    {
        properties.setProperty(property.uri, value, typeInfo);
    }

    void Resource::setValue(const Property& property, int value)
    {
        properties.setProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::setValue(const Property& property, long value)
    {
        properties.setProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::setValue(const Property& property, float value)
    {
        properties.setProperty(property.uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::setValue(const Property& property, double value)
    {
        properties.setProperty(property.uri, Serializer::toString(value), typeid(value));
    }

    void Resource::setType(const char* value)
    {
        properties.setProperty(rdf::_type, Serializer::toString(value), typeid(Resource));
    }
    
    const char* Resource::getType()
    {
        if(properties.find(rdf::_type) != properties.end())
        {
            return properties.find(rdf::_type)->second.LiteralValue.c_str();
        }

        return NULL;
    }
    
    void Resource::setUri(string uriref)
    {
        uri = string(uriref);
    }
 
    bool Resource::operator==(ResourceRef other)
    {
        return uri == other->uri;
    }
    
    ostream& operator<<(ostream& out, ResourceRef resource)
    {           
        out << "<" << resource->uri << ">";
        
        return out;
    }
}
