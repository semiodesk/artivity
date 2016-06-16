#ifndef _ART_INFORMATIONELEMENT_H
#define _ART_INFORMATIONELEMENT_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/nfo.h"
#include "../../Ontologies/nie.h"
#include "../Entity.h"
#include "FileDataObject.h"

namespace artivity
{
    class InformationElement;
    typedef boost::shared_ptr<InformationElement> InformationElementRef;

    class FileDataObject;
    typedef boost::shared_ptr<FileDataObject> FileDataObjectRef;

    class InformationElement : public Entity
    {
    private:
        string _url;
        FileDataObjectRef _fileDataObject;
        
    public:
        InformationElement() : Entity()
        {
            _url = "";
            
            setType(nie::InformationElement);
        }
        
        InformationElement(const char* uriref) : Entity(uriref)
        {
            _url = "";
            
            setType(nie::InformationElement);
        }
        
        const char* getUrl()
        {
            return _url.c_str();
        }
        
        void setUrl(const char* url)
        {
            _url = string(url);
        }

        void setStoredAs(FileDataObjectRef fdo);

        FileDataObjectRef getStoredAs();
    };
}

#endif // _ART_INFORMATIONELEMENT_H
