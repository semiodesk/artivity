#ifndef REMOVE_H
#define REMOVE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Generation.h"

namespace artivity
{
    class Remove : public Generation
    {
    public:
        Remove() : Generation()
        {
            setType(art::Remove);
        }
        
        Remove(const char* uriref) : Generation(uriref)
        {
            setType(art::Remove);
        }
    };
}

#endif // REMOVE_H
