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
        Uri = uriref;
                    
        Properties = PropertyMap();
    }
    
    Resource::Resource(const char* uriref)
    {
        Uri = string(uriref);
                    
        Properties = PropertyMap();
    }

    bool Resource::is(ResourceRef type)
    {
        return Properties.hasProperty(rdf::_type, type);
    }
    
    void Resource::clear()
    {
        Properties.clear();
    }
    
    bool Resource::hasProperty(const Property& property, ResourceRef value)
    {
        return Properties.hasProperty(property.Uri, value);
    }
    
    bool Resource::hasProperty(const Property& property, const char* value)
    {            
        return Properties.hasProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    bool Resource::hasProperty(const Property& property, int value)
    {
        return Properties.hasProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    bool Resource::hasProperty(const Property& property, long value)
    {
        return Properties.hasProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    bool Resource::hasProperty(const Property& property, float value)
    {
        return Properties.hasProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    bool Resource::hasProperty(const Property& property, double value)
    {
        return Properties.hasProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    bool Resource::hasProperty(const Property& property, const time_t* value)
    {
        return Properties.hasProperty(property.Uri, Serializer::toString(value), typeid(value));
    }

    void Resource::addProperty(const Property& property, ResourceRef value)
    {
        Properties.addProperty(property.Uri, value);
    } 

    void Resource::addProperty(const Property& property, const char* value)
    {
        Properties.addProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::addProperty(const Property& property, int value)
    {
        Properties.addProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::addProperty(const Property& property, long value)
    {
        Properties.addProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::addProperty(const Property& property, float value)
    {
        Properties.addProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::addProperty(const Property& property, double value)
    {
        Properties.addProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::addProperty(const Property& property, const time_t* value)
    {
        Properties.addProperty(property.Uri, Serializer::toString(value), typeid(value));
    }

    void Resource::removeProperty(const Property& property, ResourceRef value)
    {
        Properties.removeProperty(property.Uri, value);
    }
   
    void Resource::removeProperty(const Property& property, const char* value)
    {
        Properties.removeProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::removeProperty(const Property& property, int value)
    {
        Properties.removeProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::removeProperty(const Property& property, long value)
    {
        Properties.removeProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::removeProperty(const Property& property, float value)
    {
        Properties.removeProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::removeProperty(const Property& property, double value)
    {
        Properties.removeProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::removeProperty(const Property& property, const time_t* value)
    {
        Properties.removeProperty(property.Uri, Serializer::toString(value), typeid(value));
    }

    void Resource::setValue(const Property& property, ResourceRef value)
    {
        Properties.setProperty(property.Uri, value);
    }  

    void Resource::setValue(const Property& property, const time_t* value)
    {
        Properties.setProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::setValue(const Property& property, const char* value)
    {
        Properties.setProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::setValue(const Property& property, int value)
    {
        Properties.setProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::setValue(const Property& property, long value)
    {
        Properties.setProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::setValue(const Property& property, float value)
    {
        Properties.setProperty(property.Uri, Serializer::toString(value), typeid(value));
    }
    
    void Resource::setValue(const Property& property, double value)
    {
        Properties.setProperty(property.Uri, Serializer::toString(value), typeid(value));
    }


    void Resource::setType(const char* value)
    {
        Properties.setProperty(rdf::_type, Serializer::toString(value), typeid(Resource));
    }
    
    const char* Resource::getType()
    {
        if(Properties.find(rdf::_type) != Properties.end())
        {
            return Properties.find(rdf::_type)->second.LiteralValue.c_str();
        }
    }
    
    void Resource::setUri(string uriref)
    {
        Uri = string(uriref);
    }
 
    bool Resource::operator==(ResourceRef other)
    {
        return Uri == other->Uri;
    }
    
    ostream& operator<<(ostream& out, ResourceRef resource)
    {           
        out << "<" << resource->Uri << ">";
        
        return out;
    }
}
