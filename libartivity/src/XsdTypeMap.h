#ifndef __ART_TYPEMAP_H
#define __ART_TYPEMAP_H

#include <ctime>
#include <typeinfo>
#include "Resource.h"
#include "Ontologies/xsd.h"

namespace artivity
{
    typedef std::map<const char*, const char*>::iterator XsdTypeMapIterator;
    
    class XsdTypeMap : public std::map<const char*, const char*>
    {
    public:
        XsdTypeMap()
        {
            insert(std::pair<const char*, const char*>(typeid(int).name(), xsd::_int));
            insert(std::pair<const char*, const char*>(typeid(long).name(), xsd::_long));
            insert(std::pair<const char*, const char*>(typeid(float).name(), xsd::_float));
            insert(std::pair<const char*, const char*>(typeid(double).name(), xsd::_double));
            insert(std::pair<const char*, const char*>(typeid(const time_t*).name(), xsd::dateTime));
        }
        
        ~XsdTypeMap() {}
    };
}

#endif // TYPEMAP_H
