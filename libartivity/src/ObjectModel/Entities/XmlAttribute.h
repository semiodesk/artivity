// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

#ifndef _ART_XMLATTRIBUTE_H
#define _ART_XMLATTRIBUTE_H

#include "../../Ontologies/xml.h"

#include "../Entity.h"

namespace artivity
{
    class XmlAttribute;

    typedef boost::shared_ptr<XmlAttribute> XmlAttributeRef;

    class XmlAttribute : public Entity
    {
    private:        
        XmlElementRef _ownerElement;
        
        const char* _localName;
        
    public:        
        XmlAttribute(const char* uri) : Entity(uri)
        {
            setType(xml::Attribute);
        }
        
        XmlElementRef getOwnerElement()
        {
            return _ownerElement;
        }
        
        void setOwnerElement(XmlElementRef ownerElement)
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

#endif // _ART_XMLATTRIBUTE_H
