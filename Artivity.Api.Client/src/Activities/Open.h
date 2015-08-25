#ifndef OPEN_H
#define OPEN_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class Open : public Activity
    {
    public:
        Open() : Activity()
        {
            setValue(rdf::_type, art::Open);
        }
        
        Open(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::Open);
        }
    };
}

#endif // OPEN_H
