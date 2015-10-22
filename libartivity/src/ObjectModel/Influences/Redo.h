#ifndef REDO_H
#define REDO_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Generation.h"

namespace artivity
{
    class Redo : public Generation
    {
    public:
        Redo() : Generation()
        {
            setType(art::Redo);
        }
        
        Redo(const char* uriref) : Generation(uriref)
        {
            setType(art::Redo);
        }
    };
}

#endif // REDO_H
