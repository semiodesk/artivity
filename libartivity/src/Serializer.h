#ifndef SERIALIZER_H
#define SERIALIZER_H

#include <map>
#include <string>
#include "Resource.h"
#include "XsdTypeMap.h"

using namespace std;

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

        static string toString(const Resource& value);
        static string toString(const char* value);
		static string toString(int value);
		static string toString(long value);
		static string toString(float value);
		static string toString(double value);
		static string toString(const time_t* value);
        
        string serialize(Resource& resource, RdfSerializationFormat format);
        stringstream& serialize(stringstream& out, Resource& resource, RdfSerializationFormat format);
    };
}

#endif // SERIALIZER_H
