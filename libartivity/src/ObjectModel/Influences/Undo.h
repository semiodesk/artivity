#ifndef UNDO_H
#define UNDO_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Invalidation.h"

namespace artivity
{
    class Undo : public Invalidation
    {
    public:
        Undo() : Invalidation()
        {
            setType(art::Undo);
        }
        
        Undo(const char* uriref) : Invalidation(uriref)
        {
            setType(art::Undo);
        }
    };
}

#endif // UNDO_H
