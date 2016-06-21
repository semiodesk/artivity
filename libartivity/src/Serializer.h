#ifndef __ART_SERIALIZER_H
#define __ART_SERIALIZER_H

#include <map>
#include <queue>
#include <hash_set>
#include <string>
#include "Resource.h"
#include "XsdTypeMap.h"

namespace artivity
{
    enum RdfSerializationFormat { N3 };
    
	class SerializerContext
	{
	public:
		// Reference to the target string stream.
		std::stringstream out;

		// Prefixes that need to be declared.
		std::hash_set<std::string> prefixes;

		// Resources which have already been serialized.
		std::hash_set<std::string> track;

		// Resources which need to be serialized.
		std::queue<ResourceRef> queue;

		SerializerContext() {}
	};
    
    class Serializer
    {  
	protected:
		void serializeN3(SerializerContext& context, ResourceRef resource);
        void serializeN3(SerializerContext& context, std::string property, PropertyValue x);
		void serializeN3(SerializerContext& context, std::string uri);

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
