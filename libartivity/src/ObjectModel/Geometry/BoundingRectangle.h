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
            setType(art::BoundingRectangle);
        }
        
        BoundingRectangle(const char* uriref) : Rectangle(uriref)
        {
            setType(art::BoundingRectangle);
        }
    };
}

#endif // BOUNDINGRECTANGLE_H
