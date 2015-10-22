#ifndef NFO_H
#define NFO_H

#include "../Property.h"

#define NFO(label) "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#"label;

namespace artivity
{
    namespace nfo
    {
        static const Resource FileDataObject = NFO("FileDataObject");
        static const Resource WebDataObject = NFO("WebDataObject");
        
        static const Property fileLastAccessed = NFO("fileLastAccessed");
        static const Property fileLastModified = NFO("fileLastModified");
        static const Property fileCreated = NFO("fileCreated");
        static const Property fileSize = NFO("fileSize");
        static const Property fileUrl = NFO("fileUrl");
    }
}

#endif // NFO_H
