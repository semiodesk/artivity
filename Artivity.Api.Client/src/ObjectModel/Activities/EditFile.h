#ifndef EDITFILE_H
#define EDITFILE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class EditFile : public Activity
    {
    public:
        EditFile() : Activity()
        {
            setValue(rdf::_type, art::EditFile);
        }
        
        EditFile(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::EditFile);
        }
    };
}

#endif // EDITFILE_H
