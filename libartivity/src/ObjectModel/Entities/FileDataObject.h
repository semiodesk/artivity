#ifndef FILEDATAOBJECT_H
#define FILEDATAOBJECT_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/nfo.h"
#include "../Entity.h"
#include "InformationElement.h"
#include "../Geometry/Canvas.h"

namespace artivity
{

    class FileDataObject;
    typedef boost::shared_ptr<FileDataObject> FileDataObjectRef;

    class InformationElement;
    typedef boost::shared_ptr<InformationElement> InformationElementRef;

    class FileDataObject : public Entity
    {
    private:
        string _url;

        InformationElementRef _interpretedAs;
        
    public:
        FileDataObject() : Entity()
        {
            _url = "";
            
            setType(nfo::FileDataObject);
        }
        
        FileDataObject(const char* uriref) : Entity(uriref)
        {
            _url = "";
            
            setType(nfo::FileDataObject);
        }
        
        const char* getUrl()
        {
            return _url.c_str();
        }
        
        void setUrl(const char* url)
        {
            _url = string(url);
            
            setValue(nfo::fileUrl, _url.c_str());
        }

        void setInterpretedAs(InformationElementRef ie);

        InformationElementRef getInterpretedAs();
        
    };
}

#endif // FILEDATAOBJECT_H
