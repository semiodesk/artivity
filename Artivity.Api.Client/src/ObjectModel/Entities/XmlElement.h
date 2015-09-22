#ifndef XMLELEMENT_H
#define XMLELEMENT_H

#include "../../Ontologies/rdf.h"
#include "../Entity.h"

namespace artivity
{
    class XmlElement : public Entity
    {
    public:        
        XmlElement(const char* url, const char* id)
        {
            stringstream uri;
                            
            if(strncmp(url, "file:", 5) == 0)
            {
                uri << "file://";
            }
            
            uri << url << "#" << id;
            
            setUri(uri.str());
                            
            setValue(rdf::_type, xml::Element);
        }
    };
}

#endif // XMLELEMENT_H
