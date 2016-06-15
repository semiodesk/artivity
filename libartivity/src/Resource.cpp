#include <typeinfo>

#include "Resource.h"
#include "Property.h"
#include "Serializer.h"

#include "Ontologies/rdf.h"

namespace artivity
{
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

    void Resource::setType(ResourceRef type)
    {
        Properties.setProperty(rdf::_type, type->Uri, typeid(Resource));
    }

    void Resource::setType(const char* value)
    {
        Properties.setProperty(rdf::_type, Serializer::toString(value), typeid(Resource));
    }
    
    const ResourceRef Resource::getType()
    {
        if(Properties.find(rdf::_type) != Properties.end())
        {
            return Properties.find(rdf::_type)->second.Value;
        }
        
        return NULL;
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
