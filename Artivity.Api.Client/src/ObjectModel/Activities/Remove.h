#ifndef REMOVE_H
#define REMOVE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class Remove : public Activity
    {
    public:
        Remove() : Activity()
        {
            setValue(rdf::_type, art::Remove);
        }
        
        Remove(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::Remove);
        }
    };
}

#endif // REMOVE_H
