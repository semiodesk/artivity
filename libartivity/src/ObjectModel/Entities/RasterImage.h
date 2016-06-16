#ifndef _ART_RASTERIMAGE_H
#define _ART_RASTERIMAGE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/nfo.h"
#include "Image.h"

namespace artivity
{
    class RasterImage;
    typedef boost::shared_ptr<RasterImage> RasterImageRef;

    class RasterImage : public Image
    {
        private:
        string _url;

        public:
        RasterImage() : Image()
        {
            _url = "";

            setType(nfo::RasterImage);
        }

        RasterImage(const char* uriref) : Image(uriref)
        {
            _url = "";

            setType(nfo::RasterImage);
        }
    };
}

#endif // _ART_RASTERIMAGE_H
