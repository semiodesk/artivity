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

#ifndef ACTIVITYINFLUENCE_H
#define ACTIVITYINFLUENCE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../Ontologies/dces.h"
#include "../UriGenerator.h"
#include "Geometry/Viewport.h"

namespace artivity
{
    class ActivityInfluence;

    typedef boost::shared_ptr<ActivityInfluence> ActivityInfluenceRef;

    class ActivityInfluence : public Resource
    {
    private:
        ViewportRef _viewport;
        
        GeometryRef _boundaries;
        
        ResourceRef _location;
        
        time_t _time;
                
        string _content;
        
        string _description;
        
    public:
        ActivityInfluence() : Resource(UriGenerator::getUri())
        {
            setType( prov::ActivityInfluence);
        }
        
        ActivityInfluence(const char* uriref) : Resource(uriref)
        {
            setType(prov::ActivityInfluence);
        }
        
        ViewportRef getViewport()
        {
            return _viewport;
        }
        
        void setViewport(ViewportRef viewport)
        {
            _viewport = viewport;
            
            Resource::setValue(art::hadViewport, viewport);
        }
        
        GeometryRef getBoundaries()
        {
            return _boundaries;
        }
        
        void setBoundaries(GeometryRef boundaries)
        {
            _boundaries = boundaries;
            
            Resource::setValue(art::hadBoundaries, boundaries);
        }
        
        ResourceRef getLocation()
        {
            return _location;
        }
        
        void setLocation(ResourceRef location)
        {
            _location = location;
            
            Resource::setValue(prov::atLocation, location);
        }
        
        time_t getTime()
        {
            return _time;
        }
        
        void setTime(time_t time)
        {
            _time = time;
            
            Resource::setValue(prov::atTime, &time);
        }
        
        string getContent()
        {
            return _content;
        }
        
        void setContent(string content)
        {
            _content = string(content);
            
            setValue(prov::value, _content.c_str());
        }
        
        string getDescription()
        {
            return _description;
        }
        
        void setDescription(string description)
        {
            _description = string(description);
            
            setValue(dces::description, _description.c_str());
        }
    };
}

#endif // ACTIVITYINFLUENCE_H
