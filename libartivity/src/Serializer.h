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
        XsdTypeMap TYPE_MAP;
                
        Serializer() {}
        ~Serializer() {}

		static string toString(ResourceRef value);
        static string toString(const char* value);
		static string toString(int value);
		static string toString(long value);
		static string toString(float value);
		static string toString(double value);
		static string toString(const time_t* value);
        
        string serialize(ResourceRef resource, RdfSerializationFormat format);
        stringstream& serialize(stringstream& out, ResourceRef resource, RdfSerializationFormat format);
    };
}

#endif // SERIALIZER_H
