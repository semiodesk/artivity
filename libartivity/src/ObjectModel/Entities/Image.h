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
// Copyright (c) Semiodesk GmbH 2016

#ifndef _ART_IMAGE_H
#define _ART_IMAGE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/nfo.h"
#include "../Geometry/Canvas.h"
#include "Media.h"

namespace artivity
{
    class Image;

    typedef boost::shared_ptr<Image> ImageRef;

    class Image : public Media
    {
		std::list<CanvasRef>* _canvases;

    public:
        Image() : Media()
        {
            setType(nfo::Image);

			_canvases = new std::list<CanvasRef>();
        }
        
        Image(const char* uriref) : Media(uriref)
        {
            setType(nfo::Image);

			_canvases = new std::list<CanvasRef>();
        }

		std::list<CanvasRef> getCanvases()
		{
			return *_canvases;
		}

		void addCanvas(CanvasRef canvas)
		{
			_canvases->push_back(canvas);

			addProperty(art::hadCanvas, canvas);
		}

		void removeCanvas(CanvasRef canvas)
		{
			_canvases->remove(canvas);

			removeProperty(art::hadCanvas, canvas);
		}
    };
}

#endif // _ART_IMAGE_H
