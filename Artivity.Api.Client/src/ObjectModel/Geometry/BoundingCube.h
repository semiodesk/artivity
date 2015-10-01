#ifndef BOUNDINGCUBE_H
#define BOUNDINGCUBE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "Cube.h"

namespace artivity
{
    class BoundingCube : public Cube
    {
    public:
        BoundingCube() : Cube()
        {
            setValue(rdf::_type, art::BoundingCube);
        }
        
        BoundingCube(const char* uriref) : Cube(uriref)
        {
            setValue(rdf::_type, art::BoundingCube);
        }
    };
}

#endif // BOUNDINGCUBE_H
