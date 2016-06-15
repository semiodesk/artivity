#ifndef PROPERTYVALUE_H
#define PROPERTYVALUE_H

#include <string>
#include <typeinfo>
#include <boost/shared_ptr.hpp>


namespace artivity
{
    class Resource;
    typedef boost::shared_ptr<Resource> ResourceRef;
    
    class PropertyValue
    {
    public:
        ResourceRef Value;
        
        std::string LiteralValue;
        
        const char* LiteralType;

        PropertyValue(ResourceRef resource)
        {
            Value = resource;
            LiteralValue = std::string();
            LiteralType = NULL;
        }

        
        PropertyValue(const std::string& value, const type_info& literalType)
        {
            Value = NULL;
            LiteralValue = std::string(value);
            LiteralType = literalType.name();

        }
        
        virtual ~PropertyValue() {}
    };
}

#endif // PROPERTYVALUE_H
