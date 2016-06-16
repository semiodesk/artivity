#ifndef _ART_IMAGE_H
#define _ART_IMAGE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/nfo.h"
#include "Media.h"

namespace artivity
{
    class Image;
    typedef boost::shared_ptr<Image> ImageRef;

    class Image : public Media
    {
    private:
        string _url;
        
    public:
        Image() : Media()
        {
            _url = "";
            
            setType(nfo::Image);
        }
        
        Image(const char* uriref) : Media(uriref)
        {
            _url = "";
            
            setType(nfo::Image);
        }
    };
}

#endif // _ART_IMAGE_H
