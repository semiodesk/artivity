#ifndef RESROUCE_H
#define RESROUCE_H

#include <map>
#include <ctime>
#include <cstring>
#include <sstream>
#include "PropertyMap.h"

using namespace std;

namespace artivity
{           
    class Property;
    class Serializer;
    
    class Resource
    {
        friend class Serializer;
        
    protected:
        PropertyMap Properties;
                
    public:                    
        string Uri;
        
        Resource(string uriref);
        Resource(const char* uriref);
        virtual ~Resource() {}

        bool is(const Resource& type);
        
        void clear();

        bool hasProperty(const Property& property, const Resource* value);
        bool hasProperty(const Property& property, const Resource& value);
        bool hasProperty(const Property& property, const time_t* value);
        bool hasProperty(const Property& property, const char* value);
        bool hasProperty(const Property& property, int value);
        bool hasProperty(const Property& property, long value);
        bool hasProperty(const Property& property, float value);
        bool hasProperty(const Property& property, double value);
        
        void addProperty(const Property& property, const Resource* value);
        void addProperty(const Property& property, const Resource& value);
        void addProperty(const Property& property, const time_t* value);
        void addProperty(const Property& property, const char* value);
        void addProperty(const Property& property, int value);
        void addProperty(const Property& property, long value);
        void addProperty(const Property& property, float value);
        void addProperty(const Property& property, double value);

        void removeProperty(const Property& property, const Resource* value);        
        void removeProperty(const Property& property, const Resource& value);
        void removeProperty(const Property& property, const time_t* value);
        void removeProperty(const Property& property, const char* value);
        void removeProperty(const Property& property, int value);
        void removeProperty(const Property& property, long value);
        void removeProperty(const Property& property, float value);
        void removeProperty(const Property& property, double value);

        void setValue(const Property& property, const Resource* value);        
        void setValue(const Property& property, const Resource& value);
        void setValue(const Property& property, const time_t* value);
        void setValue(const Property& property, const char* value);
        void setValue(const Property& property, int value);
        void setValue(const Property& property, long value);
        void setValue(const Property& property, float value);
        void setValue(const Property& property, double value);
        
        void setType(const Resource* type);
        void setType(const Resource& type);
        
        const Resource* getType();
        
        void setUri(string uriref);
        
        bool operator==(const Resource& other);
        
        friend ostream& operator<<(ostream& out, const Resource& resource);
    };
}

#endif // RESROUCE_H
