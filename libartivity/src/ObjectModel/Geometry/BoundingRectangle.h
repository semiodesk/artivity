#ifndef BOUNDINGRECTANGLE_H
#define BOUNDINGRECTANGLE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "Rectangle.h"

namespace artivity
{
    class BoundingRectangle : public Rectangle
    {
    public:
        BoundingRectangle() : Rectangle()
        {
            setValue(rdf::_type, art::BoundingRectangle);
        }
        
        BoundingRectangle(const char* uriref) : Rectangle(uriref)
        {
            setValue(rdf::_type, art::BoundingRectangle);
        }
    };
}

#endif // BOUNDINGRECTANGLE_H
