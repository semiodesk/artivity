#ifndef ADD_H
#define ADD_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Generation.h"

namespace artivity
{
    class Add : public Generation
    {
    public:
        Add() : Generation()
        {
            setType(art::Add);
        }
        
        Add(const char* uriref) : Generation(uriref)
        {
            setType(art::Add);
        }
    };
}

#endif // ADD_H
