#ifndef CARTESIANCOORDINATESYSTEM_H
#define CARTESIANCOORDINATESYSTEM_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "CoordinateSystem.h"

namespace artivity
{
    class CartesianCoordinateSystem : public CoordinateSystem
    {        
    public:
        CartesianCoordinateSystem() : CoordinateSystem()
        {
            setType(art::CartesianCoordinateSystem);
        }
        
        CartesianCoordinateSystem(const char* uriref) : CoordinateSystem(uriref)
        {
            setType(art::CartesianCoordinateSystem);
        }
    };
}

#endif // CARTESIANCOORDINATESYSTEM_H
