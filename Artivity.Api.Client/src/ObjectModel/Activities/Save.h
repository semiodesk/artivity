#ifndef SAVE_H
#define SAVE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class SaveFile : public Activity
    {
    public:
        SaveFile() : Activity()
        {
            setValue(rdf::_type, art::Save);
        }
        
        SaveFile(char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::Save);
        }
    };
}

#endif // SAVE_H
