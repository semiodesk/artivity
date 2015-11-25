#ifndef SVG_H
#define SVG_H

#define SVG(label) "http://www.mathiswebs.com/ontology/svg_ont.owl#"label;

namespace artivity
{
    namespace svg
    {
        static const char* Shape = SVG("Shape");
        
        static const char* Curved = SVG("Curved");
        static const char* Circle = SVG("Circle");
        static const char* Ellipse = SVG("Ellipse");
        
        static const char* Polygon = SVG("Polygon");
        static const char* Triangle = SVG("Triangle");
        static const char* Quadrate = SVG("Quadrate");
        static const char* Trapezoid = SVG("Trapezoid");
        static const char* Parallelogram = SVG("Parallelogram");
        static const char* Diamond = SVG("Diamond");
        static const char* Rectangle = SVG("Rectangle");
        static const char* Square = SVG("Square");
        static const char* Pentagon = SVG("Pentagon");
        static const char* Hexagon = SVG("Hexagon");
        
        static const char* Predefined = SVG("Predefined");
        static const char* Star = SVG("Star");
        static const char* Heart = SVG("Heart");
        static const char* Crescent = SVG("Crescent");
        static const char* Sun = SVG("Sun");
        
        static const char* id = SVG("id");
    }
}

#endif // SVG_H
