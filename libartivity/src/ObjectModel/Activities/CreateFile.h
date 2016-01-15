#ifndef CREATEFILE_H
#define CREATEFILE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class CreateFile : public Activity
    {
    public:
        CreateFile() : Activity()
        {
            setType(art::CreateFile);
        }
        
        CreateFile(const char* uriref) : Activity(uriref)
        {
            setType(art::CreateFile);
        }
    };
}

#endif // CREATEFILE_H
