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

#ifndef _ART_VIEWPORT_H
#define _ART_VIEWPORT_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "Rectangle.h"

namespace artivity
{
    class Viewport;

    typedef boost::shared_ptr<Viewport> ViewportRef;

    class Viewport : public Rectangle
    {
    private:
        double _zoomFactor;
        
    public:
        Viewport() : Rectangle()
        {
            setType(art::Viewport);
        }
        
        Viewport(const char* uriref) : Rectangle(uriref)
        {
            setType(art::Viewport);
        }

        Viewport(double top, double left, double right, double bottom) : Rectangle(top, left, right, bottom)
        {
        }

        Viewport(PointRef pos, double width, double height) : Rectangle(pos, width, height)
        {
        }
        
        double getZoomFactor()
        {
            return _zoomFactor;
        }
        
        void setZoomFactor(double zoomFactor)
        {
            _zoomFactor = zoomFactor;
            
            setValue(art::zoomFactor, zoomFactor);
        }
    };
}

#endif // _ART_VIEWPORT_H
