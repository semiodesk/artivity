#ifndef UPDATE_H
#define UPDATE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class Update : public Activity
    {
    public:
        Update() : Activity()
        {
            setValue(rdf::_type, art::Update);
        }
        
        Update(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::Update);
        }
    };
}

#endif // UPDATE_H
