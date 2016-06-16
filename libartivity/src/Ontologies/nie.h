#ifndef _ART_NIE_H
#define _ART_NIE_H

#define NIE(label) "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#" label;

namespace artivity
{
    namespace nie
    {
        static const char* InformationElement = NIE("InformationElement");

        static const char* isStoredAs = NIE("isStoredAs");
        static const char* interpretedAs = NIE("interpretedAs");
        

    }
}

#endif // _ART_NIE_H
