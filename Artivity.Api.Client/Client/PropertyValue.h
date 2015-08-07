#ifndef PROPERTYVALUE_H
#define PROPERTYVALUE_H

#include <string>
#include <typeinfo>

using namespace std;

namespace artivity
{
    namespace client
    {
        class Resource;
        
        class PropertyValue
        {
        public:
            const Resource* Value;
            
            string LiteralValue;
            
            const char* LiteralType;
            
            PropertyValue(const Resource& resource)
            {
                Value = &resource;
                LiteralValue = "";
                LiteralType = NULL;
            }
            
            PropertyValue(string value, const type_info& literalType)
            {
                Value = NULL;
                LiteralValue = value;
                LiteralType = literalType.name();
            }
            
            ~PropertyValue() {}
        };
    }
}

#endif // PROPERTYVALUE_H
