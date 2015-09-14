#ifndef WEBDATAOBJECT_H
#define WEBDATAOBJECT_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/nfo.h"
#include "../Entity.h"

namespace artivity
{
    class WebDataObject : public Entity
    {
    public:
        WebDataObject() : Entity()
        {
            setValue(rdf::_type, nfo::WebDataObject);
        }
        
        WebDataObject(const char* uriref) : Entity(uriref)
        {
            setValue(rdf::_type, nfo::WebDataObject);
        }
    };
}

#endif // WEBDATAOBJECT_H
