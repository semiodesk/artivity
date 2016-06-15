#ifndef RESROUCE_H
#define RESROUCE_H

#include <map>
#include <ctime>
#include <cstring>
#include <sstream>
#include "PropertyMap.h"
#include <boost/shared_ptr.hpp>


namespace artivity
{           
    class Property;
    class Serializer;

    class Resource;
    typedef boost::shared_ptr<Resource> ResourceRef;
    
    class Resource
    {
        friend class Serializer;
        
    protected:
        PropertyMap Properties;
                
    public:                    
        std::string Uri;
        
        Resource(std::string uriref);
        Resource(const char* uriref);
        virtual ~Resource() {}

        bool is(ResourceRef type);
        
        void clear();

        bool hasProperty(const Property& property, ResourceRef value);
        bool hasProperty(const Property& property, const time_t* value);
        bool hasProperty(const Property& property, const char* value);
        bool hasProperty(const Property& property, int value);
        bool hasProperty(const Property& property, long value);
        bool hasProperty(const Property& property, float value);
        bool hasProperty(const Property& property, double value);
        
        void addProperty(const Property& property, ResourceRef value);
        void addProperty(const Property& property, const time_t* value);
        void addProperty(const Property& property, const char* value);
        void addProperty(const Property& property, int value);
        void addProperty(const Property& property, long value);
        void addProperty(const Property& property, float value);
        void addProperty(const Property& property, double value);

        void removeProperty(const Property& property, ResourceRef value);        
        void removeProperty(const Property& property, const time_t* value);
        void removeProperty(const Property& property, const char* value);
        void removeProperty(const Property& property, int value);
        void removeProperty(const Property& property, long value);
        void removeProperty(const Property& property, float value);
        void removeProperty(const Property& property, double value);

        void setValue(const Property& property, ResourceRef value);        
        void setValue(const Property& property, const time_t* value);
        void setValue(const Property& property, const char* value);
        void setValue(const Property& property, int value);
        void setValue(const Property& property, long value);
        void setValue(const Property& property, float value);
        void setValue(const Property& property, double value);
        
        void setType(ResourceRef type);
        void setType(const Resource& type);
        void setType(const char* value);
        
        const ResourceRef getType();
        
        void setUri(std::string uriref);
        
        bool operator==(ResourceRef other);
        
        friend std::ostream& operator<<(std::ostream& out, ResourceRef resource);
    };
}

#endif // RESROUCE_H
