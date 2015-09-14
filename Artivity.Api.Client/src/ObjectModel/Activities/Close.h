#ifndef CLOSE_H
#define CLOSE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class CloseFile : public Activity
    {
    public:
        CloseFile() : Activity()
        {
            setValue(rdf::_type, art::Close);
        }
        
        CloseFile(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::Close);
        }
    };
}

#endif // CLOSE_H
