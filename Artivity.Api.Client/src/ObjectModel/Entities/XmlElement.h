#ifndef XMLELEMENT_H
#define XMLELEMENT_H

#include "../../Ontologies/rdf.h"
#include "../Entity.h"

namespace artivity
{
    class XmlElement : public Entity
    {
    public:        
        XmlElement(const char* uri) : Entity(uri)
        {                            
            setType(xml::Element);
        }
    };
}

#endif // XMLELEMENT_H
