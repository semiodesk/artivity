#ifndef __ART_SERIALIZER_H
#define __ART_SERIALIZER_H

#include <map>
#include <string>
#include "Resource.h"
#include "XsdTypeMap.h"

namespace artivity
{
    enum RdfSerializationFormat { N3 };
    
    class SerializerContext;
    
    class Serializer
    {  
    public:
		std::map<std::string, std::string> PREFIX_MAP;
        XsdTypeMap TYPE_MAP;
                
		Serializer();
        ~Serializer() {}

        static std::string toString(ResourceRef value);
        static std::string toString(const char* value);
        static std::string toString(int value);
        static std::string toString(long value);
        static std::string toString(float value);
        static std::string toString(double value);
        static std::string toString(const time_t* value);
        
        std::string serialize(ResourceRef resource, RdfSerializationFormat format);
        std::stringstream& serialize(std::stringstream& out, ResourceRef resource, RdfSerializationFormat format);
    };
}

#endif // SERIALIZER_H
