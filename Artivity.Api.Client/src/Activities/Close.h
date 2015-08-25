#ifndef CLOSE_H
#define CLOSE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/art.h"
#include "../Activity.h"

namespace artivity
{
    class Close : public Activity
    {
    public:
        Close() : Activity()
        {
            setValue(rdf::_type, art::Close);
        }
        
        Close(const char* uriref) : Activity(uriref)
        {
            setValue(rdf::_type, art::Close);
        }
    };
}

#endif // CLOSE_H
