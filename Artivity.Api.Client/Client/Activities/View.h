#ifndef VIEW_H
#define VIEW_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/as.h"
#include "../Activity.h"

using namespace artivity::client::ontologies;

namespace artivity
{
    namespace client
    {
        class View : public Activity
        {
        public:
            View(const char* uriref) : Activity(uriref)
            {
                setValue(rdf::_type, as::View);
            }
        };
    }
}

#endif // VIEW_H
