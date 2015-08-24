#ifndef VIEW_H
#define VIEW_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/as.h"
#include "../Activity.h"

namespace artivity
{
    class View : public Activity
    {
    public:
        View() : Activity()
        {
            setValue(rdf::_type, as::View);
        }
        
        View(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, as::View);
        }
    };
}

#endif // VIEW_H
