#ifndef XMLATTRIBUTE_H
#define XMLATTRIBUTE_H

#include "../../Ontologies/rdf.h"
#include "../Entity.h"

namespace artivity
{
    class XmlAttribute: public Entity
    {
    private:        
        XmlElement* _ownerElement;
        
        const char* _localName;
        
    public:        
        XmlAttribute(const char* uri) : Entity(uri)
        {
            setType(xml::Attribute);
        }
        
        XmlElement* getOwnerElement()
        {
            return _ownerElement;
        }
        
        void setOwnerElement(XmlElement* ownerElement)
        {
            _ownerElement = ownerElement;
            
            setValue(xml::ownerElement, ownerElement);
        }
        
        const char* getLocalName()
        {
            return _localName;
        }
        
        void setLocalName(const char* localName)
        {
            _localName = localName;
            
            setValue(xml::localName, localName);
        }
    };
}

#endif // XMLATTRIBUTE_H
