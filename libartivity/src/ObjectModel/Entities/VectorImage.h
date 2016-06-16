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
        public:
        VectorImage() : Image()
        {
            setType(nfo::RasterImage);
        }

        VectorImage(const char* uriref) : Image(uriref)
        {
            setType(nfo::RasterImage);
        }
    };
}

#endif // _ART_VECTORIMAGE_H
