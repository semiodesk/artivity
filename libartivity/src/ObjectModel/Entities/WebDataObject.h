#ifndef WEBDATAOBJECT_H
#define WEBDATAOBJECT_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/nfo.h"
#include "../Entity.h"

namespace artivity
{
    class WebDataObject;
    typedef boost::shared_ptr<WebDataObject> WebDataObjectRef;

    class WebDataObject : public Entity
    {
    public:
        WebDataObject() : Entity()
        {
            setType(nfo::WebDataObject);
        }
        
        WebDataObject(const char* uriref) : Entity(uriref)
        {
            setType(nfo::WebDataObject);
        }
    };
}

#endif // WEBDATAOBJECT_H
