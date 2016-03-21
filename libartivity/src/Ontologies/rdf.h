#ifndef RDF_H
#define RDF_H

#include "../Property.h"

#define RDF(label) "http://www.w3.org/1999/02/22-rdf-syntax-ns#" label;

namespace artivity
{
    namespace rdf
    {
		static const char* _type = RDF("type");
    }
}

#endif // RDF_H
