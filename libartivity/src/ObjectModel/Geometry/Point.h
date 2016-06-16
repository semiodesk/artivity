#ifndef POINT_H
#define POINT_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../../UriGenerator.h"

namespace artivity
{
    class Point;
    typedef boost::shared_ptr<Point> PointRef;

    class Point : public Resource
    {
    private:
        double _x;
        
        double _y;
        
        double _z;
        
    public:
        Point() : Resource(UriGenerator::getUri())
        {
            setType(art::Point);
        }
        
        Point(const char* uriref) : Resource(uriref)
        {
            setType(art::Point);
        }

        Point(double x, double y) : Resource(UriGenerator::getUri())
        {
            _x = x;
            _y = y;
            _z = 0;
        }

        Point(double x, double y, double z) : Resource(UriGenerator::getUri())
        {
            _x = x;
            _y = y;
            _z = z;
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
    };
}

#endif // POINT_H
