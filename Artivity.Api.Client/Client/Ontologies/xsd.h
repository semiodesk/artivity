#ifndef XSD_H
#define XSD_H

#include "../Resource.h"
#include "../Property.h"

namespace artivity
{
    namespace client
    {
        namespace ontologies
        {
            namespace xsd
            {
                static const Resource _int = Resource("http://www.w3.org/2001/XMLSchema#int");
                static const Resource _long = "http://www.w3.org/2001/XMLSchema#long";
                static const Resource _float = "http://www.w3.org/2001/XMLSchema#float";
                static const Resource _double = "http://www.w3.org/2001/XMLSchema#double";
                static const Resource dateTime = "http://www.w3.org/2001/XMLSchema#dateTime";
            }
        }
    }
}

#endif // XSD_H
