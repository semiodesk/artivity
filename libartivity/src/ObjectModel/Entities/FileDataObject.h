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

#ifndef _ART_FILEDATAOBJECT_H
#define _ART_FILEDATAOBJECT_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/nfo.h"
#include "../Entity.h"
#include "../Geometry/Canvas.h"

namespace artivity
{
    class FileDataObject;

    typedef boost::shared_ptr<FileDataObject> FileDataObjectRef;

    class FileDataObject : public Entity
    {
    private:
        string _url;
        
        time_t _creationTime;
        
        time_t _lastAccessTime;
        
        time_t _lastModificationTime;
        
        long _size;
        
        CanvasRef _canvas;
        
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
        
        time_t getCreationTime()
        {
            return _creationTime;
        }
        
        void setCreationTime(time_t time)
        {
            _creationTime = time;
            
            setValue(nfo::fileCreated, &_creationTime);
        }
        
        time_t getLastAccessTime()
        {
            return _lastAccessTime;
        }
        
        void setLastAccessTime(time_t time)
        {
            _lastAccessTime = time;
            
            setValue(nfo::fileLastAccessed, &_lastAccessTime);
        }
        
        time_t getLastModificationTime()
        {
            return _lastModificationTime;
        }
        
        void setLastModificationTime(time_t time)
        {
            _lastModificationTime = time;
            
            setValue(nfo::fileLastModified, &_lastModificationTime);
        }
        
        long getSize()
        {
            return _size;
        }
        
        void setSize(long size)
        {
            _size = size;
            
            setValue(nfo::fileSize, size);
        }
        
        CanvasRef getCanvas()
        {
            return _canvas;
        }
        
        void setCanvas(CanvasRef canvas)
        {
            _canvas = canvas;
            
            Resource::setValue(art::canvas, canvas);
        }
    };
}

#endif // _ART_FILEDATAOBJECT_H
