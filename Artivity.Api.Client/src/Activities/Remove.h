#ifndef REMOVE_H
#define REMOVE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/as.h"
#include "../Activity.h"

namespace artivity
{
    class Remove : public Activity
    {
    public:
        Remove() : Activity()
        {
            setValue(rdf::_type, as::Remove);
        }
        
        Remove(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, as::Remove);
        }
    };
}

#endif // REMOVE_H
