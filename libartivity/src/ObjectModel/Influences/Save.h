#ifndef SAVE_H
#define SAVE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Generation.h"

namespace artivity
{
    class Save;
    typedef boost::shared_ptr<Save> SaveRef;


    class Save : public Generation
    {
    public:
        Save() : Generation()
        {
            setType(art::Save);
        }
        
        Save(char* uriref) : Generation(uriref)
        {
            setType(art::Save);
        }
    };
}

#endif // SAVE_H
