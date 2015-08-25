#ifndef ADD_H
#define ADD_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/as.h"
#include "../Activity.h"

namespace artivity
{
    class Add : public Activity
    {
    public:
        Add() : Activity()
        {
            setValue(rdf::_type, as::Add);
        }
        
        Add(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, as::Add);
        }
    };
}

#endif // ADD_H
