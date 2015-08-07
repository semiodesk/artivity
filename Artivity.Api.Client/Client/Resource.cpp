#include <typeinfo>

#include "Resource.h"
#include "Property.h"
#include "Serializer.h"

namespace artivity
{
    namespace client
    {
        Resource::Resource(const char* uriref)
        {
            Uri = string(uriref);
                        
            Properties = PropertyMap();
        }
        
        bool Resource::hasProperty(const Property& property, const Resource& value)
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
        
        void Resource::addProperty(const Property& property, const Resource& value)
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
        
        void Resource::removeProperty(const Property& property, const Resource& value)
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
        
        void Resource::setValue(const Property& property, const Resource& value)
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
            
        bool Resource::operator==(const Resource& other)
        {
            return Uri == other.Uri;
        }
        
        ostream& operator<<(ostream& out, const Resource& resource)
        {           
            out << "<" << resource.Uri << ">";
            
            return out;
        }
    }
}
