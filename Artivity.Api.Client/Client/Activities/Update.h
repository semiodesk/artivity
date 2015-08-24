#ifndef UPDATE_H
#define UPDATE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/as.h"
#include "../Activity.h"

namespace artivity
{
    class Update : public Activity
    {
    public:
        Update(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, as::Update);
        }
    };
}

#endif // UPDATE_H