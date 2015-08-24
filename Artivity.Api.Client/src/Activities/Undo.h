#ifndef UNDO_H
#define UNDO_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/as.h"
#include "../Activity.h"

namespace artivity
{
    class Undo : public Activity
    {
    public:
        Undo() : Activity()
        {
            setValue(rdf::_type, as::Undo);
        }
        
        Undo(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, as::Undo);
        }
    };
}

#endif // UNDO_H
