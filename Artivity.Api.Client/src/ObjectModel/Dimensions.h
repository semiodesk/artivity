#ifndef DIMENSIONS_H
#define DIMENSIONS_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/art.h"
#include "../UriGenerator.h"

namespace artivity
{
    class Dimensions : public Resource
    {
    private:
        double _x;
        double _y;
        double _z;
        Resource* _unit;
        
    public:
        Dimensions() : Resource(UriGenerator::getUri())
        {
            setValue(rdf::_type, art::Dimensions);
        }
        
        Dimensions(const char* uriref) : Resource(uriref)
        {
            setValue(rdf::_type, art::Dimensions);
        }
        
        double getX()
        {
            return _x;
        }
        
        void setX(double x)
        {
            _x = x;
            
            setValue(art::x, x);
        }
        
        double getY()
        {
            return _y;
        }
        
        void setY(double y)
        {
            _y = y;
            
            setValue(art::y, y);
        }
        
        double getZ()
        {
            return _z;
        }
        
        void setZ(double z)
        {
            _z = z;
            
            setValue(art::z, z);
        }
        
        Resource* getUnit()
        {
            return _unit;
        }
        
        void setUnit(Resource* unit)
        {
            _unit = unit;
            
            setValue(art::unit, unit);
        }
    };
}

#endif // DIMENSIONS_H
