#ifndef _ART_MEDIA_H
#define _ART_MEDIA_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/nfo.h"
#include "InformationElement.h"

namespace artivity
{
    class Media;
    typedef boost::shared_ptr<Media> MediaRef;

    class Media : public InformationElement
    {
    private:
        string _url;
        
    public:
        Media() : InformationElement()
        {
            _url = "";
            
            setType(nfo::Media);
        }
        
        Media(const char* uriref) : InformationElement(uriref)
        {
            _url = "";
            
            setType(nfo::Media);
        }

    };
}

#endif // _ART_MEDIA_H
