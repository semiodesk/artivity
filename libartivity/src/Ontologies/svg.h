#ifndef SVG_H
#define SVG_H

#include "../Property.h"

#define SVG(label) "http://www.mathiswebs.com/ontology/svg_ont.owl#"label;

namespace artivity
{
    namespace svg
    {
        static const Resource Shape = SVG("Shape");
        
        static const Resource Curved = SVG("Curved");
        static const Resource Circle = SVG("Circle");
        static const Resource Ellipse = SVG("Ellipse");
        
        static const Resource Polygon = SVG("Polygon");
        static const Resource Triangle = SVG("Triangle");
        static const Resource Quadrate = SVG("Quadrate");
        static const Resource Trapezoid = SVG("Trapezoid");
        static const Resource Parallelogram = SVG("Parallelogram");
        static const Resource Diamond = SVG("Diamond");
        static const Resource Rectangle = SVG("Rectangle");
        static const Resource Square = SVG("Square");
        static const Resource Pentagon = SVG("Pentagon");
        static const Resource Hexagon = SVG("Hexagon");
        
        static const Resource Predefined = SVG("Predefined");
        static const Resource Star = SVG("Star");
        static const Resource Heart = SVG("Heart");
        static const Resource Crescent = SVG("Crescent");
        static const Resource Sun = SVG("Sun");
        
        static const Property id = SVG("id");
    }
}

#endif // SVG_H
