#ifndef _ART_VECTORIMAGE_H
#define _ART_VECTORIMAGE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/nfo.h"
#include "Image.h"

namespace artivity
{
    class VectorImage;
    typedef boost::shared_ptr<VectorImage> VectorImageRef;

    class VectorImage : public Image
    {
        private:
        string _url;

        public:
        VectorImage() : Image()
        {
            _url = "";

            setType(nfo::RasterImage);
        }

        VectorImage(const char* uriref) : Image(uriref)
        {
            _url = "";

            setType(nfo::RasterImage);
        }
    };
}

#endif // _ART_VECTORIMAGE_H
