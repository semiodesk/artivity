#ifndef UNDO_H
#define UNDO_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/as.h"
#include "../Activity.h"

using namespace artivity::client::ontologies;

namespace artivity
{
    namespace client
    {
        class Undo : public Activity
        {
        public:
            Undo(const char* uriref) : Activity(uriref)
            {
                setValue(rdf::_type, as::Undo);
            }
        };
    }
}

#endif // UNDO_H
