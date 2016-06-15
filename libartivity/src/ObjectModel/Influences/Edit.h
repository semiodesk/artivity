#ifndef EDIT_H
#define EDIT_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "../Generation.h"

namespace artivity
{
    class Edit;
    typedef boost::shared_ptr<Edit> EditRef;

    class Edit : public Generation
    {
    public:
        Edit() : Generation()
        {
            setType(art::Edit);
        }
        
        Edit(const char* uriref) : Generation(uriref)
        {
            setType(art::Edit);
        }
    };
}

#endif // EDIT_H
