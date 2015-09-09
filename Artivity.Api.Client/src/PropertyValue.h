#ifndef PROPERTYVALUE_H
#define PROPERTYVALUE_H

#include <string>
#include <typeinfo>

using namespace std;

namespace artivity
{
    class Resource;
    
    class PropertyValue
    {
    public:
        const Resource* Value;
        
        string LiteralValue;
        
        const char* LiteralType;
        
        PropertyValue(const Resource* resource)
        {
            Value = resource;
            LiteralValue = string();
            LiteralType = NULL;
        }
        
        PropertyValue(const string& value, const type_info& literalType)
        {
            Value = NULL;
            LiteralValue = string(value);
            LiteralType = literalType.name();
        }
        
        virtual ~PropertyValue() {}
    };
}

#endif // PROPERTYVALUE_H
