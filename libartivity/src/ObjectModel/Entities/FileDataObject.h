#ifndef FILEDATAOBJECT_H
#define FILEDATAOBJECT_H

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

#endif // FILEDATAOBJECT_H
