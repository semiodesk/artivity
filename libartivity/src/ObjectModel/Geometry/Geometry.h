#ifndef GEOMETRY_H
#define GEOMETRY_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../../UriGenerator.h"

namespace artivity
{
    class Geometry : public Resource
    {        
    public:
        Geometry() : Resource(UriGenerator::getUri())
        {
            setValue(rdf::_type, art::Geometry);
        }
        
        Geometry(const char* uriref) : Resource(uriref)
        {
            setValue(rdf::_type, art::Geometry);
        }
    };
}

#endif // GEOMETRY_H
