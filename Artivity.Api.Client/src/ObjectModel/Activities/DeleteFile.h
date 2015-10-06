#ifndef DELETEFILE_H
#define DELETEFILE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class DeleteFile : public Activity
    {
    public:
        DeleteFile() : Activity()
        {
            setValue(rdf::_type, art::DeleteFile);
        }
        
        DeleteFile(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::DeleteFile);
        }
    };
}

#endif // DELETEFILE_H
