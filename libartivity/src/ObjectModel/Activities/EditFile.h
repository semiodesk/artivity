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
            setType(art::EditFile);
        }
        
        EditFile(const char* uriref) : Activity(uriref)
        {
            setType(art::EditFile);
        }
    };
}

#endif // EDITFILE_H
