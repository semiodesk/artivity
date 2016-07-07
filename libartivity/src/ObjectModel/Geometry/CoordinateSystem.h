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

#ifndef _ART_COORDINATESYSTEM_H
#define _ART_COORDINATESYSTEM_H

#include "../../Ontologies/art.h"
#include "../../UriGenerator.h"
#include "../../Resource.h"

namespace artivity
{
    class CoordinateSystem;

    typedef boost::shared_ptr<CoordinateSystem> CoordinateSystemRef;

    class CoordinateSystem : public Resource
    {
    private:
        int _coordinateDimension;
        
        const char* _transformationMatrix;
        
    public:
        CoordinateSystem() : Resource(UriGenerator::getUri())
        {
            setType(art::CoordinateSystem);
        }
        
        CoordinateSystem(const char* uriref) : Resource(uriref)
        {
            setType(art::CoordinateSystem);
        }

        int getCoordinateDimension()
        {
            return _coordinateDimension;
        }
        
        void setCoordinateDimension(int coordinateDimension)
        {
            _coordinateDimension = coordinateDimension;
            
            setValue(art::coordinateDimension, coordinateDimension);
        }
        
        const char* getTransformationMatrix()
        {
            return _transformationMatrix;
        }
        
        void setTransformationMatrix(const char* transformationMatrix)
        {
            _transformationMatrix = transformationMatrix;
            
            setValue(art::transformationMatrix, transformationMatrix);
        }
    };
}

#endif // _ART_COORDINATESYSTEM_H
