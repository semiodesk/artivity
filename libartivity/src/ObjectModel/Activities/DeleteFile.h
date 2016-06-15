#ifndef DELETEFILE_H
#define DELETEFILE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class DeleteFile;
    typedef boost::shared_ptr<DeleteFile> DeleteFileRef;

    class DeleteFile : public Activity
    {
    public:
        DeleteFile() : Activity()
        {
            setType(art::DeleteFile);
        }
        
        DeleteFile(const char* uriref) : Activity(uriref)
        {
            setType(art::DeleteFile);
        }
    };
}

#endif // DELETEFILE_H
