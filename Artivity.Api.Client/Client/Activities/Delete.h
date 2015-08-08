#ifndef DELETE_H
#define DELETE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/as.h"
#include "../Activity.h"

using namespace artivity::client::ontologies;

namespace artivity
{
    namespace client
    {
        class Delete : public Activity
        {
        public:
            Delete(const char* uriref) : Activity(uriref)
            {
                setValue(rdf::_type, as::Delete);
            }
        };
    }
}

#endif // DELETE_H
