#ifndef NFO_H
#define NFO_H



#define NFO(label) "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#"label;

namespace artivity
{
    namespace nfo
    {
		static const char* FileDataObject = NFO("FileDataObject");
        static const char* WebDataObject = NFO("WebDataObject");

        static const char* fileLastAccessed = NFO("fileLastAccessed");
        static const char* fileLastModified = NFO("fileLastModified");
        static const char* fileCreated = NFO("fileCreated");
        static const char* fileSize = NFO("fileSize");
        static const char* fileUrl = NFO("fileUrl");
    }
}

#endif // NFO_H
