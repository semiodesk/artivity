#ifndef GEOMETRY_H
#define GEOMETRY_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../../UriGenerator.h"

namespace artivity
{
    class Geometry;
    typedef boost::shared_ptr<Geometry> GeometryRef;

    class Geometry : public Resource
    {        
    public:
        Geometry() : Resource(UriGenerator::getUri())
        {
            setType(art::Geometry);
        }
        
        Geometry(const char* uriref) : Resource(uriref)
        {
            setType(art::Geometry);
        }
    };
}

#endif // GEOMETRY_H
