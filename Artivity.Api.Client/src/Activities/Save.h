#ifndef SAVE_H
#define SAVE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class Save : public Activity
    {
    public:
        Save() : Activity()
        {
            setValue(rdf::_type, art::Save);
        }
        
        Save(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::Save);
        }
    };
}

#endif // SAVE_H
