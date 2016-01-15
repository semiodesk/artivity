#ifndef COORDINATESYSTEM_H
#define COORDINATESYSTEM_H

#include <string>
#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../../Resource.h"
#include "../../UriGenerator.h"

namespace artivity
{
    class CoordinateSystem : public Resource
    {
    private:
        int _coordinateDimension;
        
        const char* _transformationMatrix;
        
    public:
        CoordinateSystem() : Resource(UriGenerator::getUri())
        {
            setType(art::CoordinateSystem);
        }
        
        CoordinateSystem(const char* uriref) : Resource(uriref)
        {
            setType(art::CoordinateSystem);
        }

        int getCoordinateDimension()
        {
            return _coordinateDimension;
        }
        
        void setCoordinateDimension(int coordinateDimension)
        {
            _coordinateDimension = coordinateDimension;
            
            setValue(art::coordinateDimension, coordinateDimension);
        }
        
        const char* getTransformationMatrix()
        {
            return _transformationMatrix;
        }
        
        void setTransformationMatrix(const char* transformationMatrix)
        {
            _transformationMatrix = transformationMatrix;
            
            setValue(art::transformationMatrix, transformationMatrix);
        }
    };
}

#endif // COORDINATESYSTEM_H
