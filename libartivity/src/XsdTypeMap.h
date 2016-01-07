#ifndef TYPEMAP_H
#define TYPEMAP_H

#include <ctime>
#include <typeinfo>
#include "Resource.h"
#include "Ontologies/xsd.h"

namespace artivity
{
    typedef map<const char*, const char*>::iterator XsdTypeMapIterator;
    
    class XsdTypeMap : public map<const char*, const char*>
    {
    public:
        XsdTypeMap()
        {
            insert(pair<const char*, const char*>(typeid(int).name(), xsd::_int));
            insert(pair<const char*, const char*>(typeid(long).name(), xsd::_long));
            insert(pair<const char*, const char*>(typeid(float).name(), xsd::_float));
            insert(pair<const char*, const char*>(typeid(double).name(), xsd::_double));
            insert(pair<const char*, const char*>(typeid(const time_t*).name(), xsd::dateTime));
        }
        
        ~XsdTypeMap() {}
    };
}

#endif // TYPEMAP_H
