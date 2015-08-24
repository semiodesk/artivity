#ifndef CREATE_H
#define CREATE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/as.h"
#include "../Activity.h"

namespace artivity
{
    class Create : public Activity
    {
    public:
        Create(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, as::Create);
        }
    };
}

#endif // CREATE_H
