#ifndef BROWSE_H
#define BROWSE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class Browse : public Activity
    {
    public:
        Browse() : Activity()
        {
            setValue(rdf::_type, art::Browse);
        }
        
        Browse(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::Browse);
        }
    };
}

#endif // BROWSE_H
