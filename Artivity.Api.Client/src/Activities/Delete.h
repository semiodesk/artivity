#ifndef DELETE_H
#define DELETE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class Delete : public Activity
    {
    public:
        Delete() : Activity()
        {
            setValue(rdf::_type, art::Delete);
        }
        
        Delete(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::Delete);
        }
    };
}

#endif // DELETE_H
