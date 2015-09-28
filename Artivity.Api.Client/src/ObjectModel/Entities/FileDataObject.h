#ifndef FILEDATAOBJECT_H
#define FILEDATAOBJECT_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/nfo.h"
#include "../Entity.h"
#include "../Dimensions.h"

namespace artivity
{
    class FileDataObject : public Entity
    {
    private:
        const char* _url;
        const time_t* _creationTime;
        const time_t* _lastAccessTime;
        const time_t* _lastModificationTime;
        long _size;
        Dimensions* _dimensions;
        
    public:
        FileDataObject() : Entity()
        {
            setValue(rdf::_type, nfo::FileDataObject);
        }
        
        FileDataObject(const char* uriref) : Entity(uriref)
        {
            setValue(rdf::_type, nfo::FileDataObject);
        }
        
        const char* getUrl()
        {
            return _url;
        }
        
        void setUrl(const char* url)
        {
            _url = url;
            
            setValue(nfo::fileUrl, url);
        }
        
        const time_t* getCreationTime()
        {
            return _creationTime;
        }
        
        void setCreationTime(const time_t* time)
        {
            _creationTime = time;
            
            setValue(nfo::fileCreated, _creationTime);
        }
        
        const time_t* getLastAccessTime()
        {
            return _lastAccessTime;
        }
        
        void setLastAccessTime(const time_t* time)
        {
            _lastAccessTime = time;
            
            setValue(nfo::fileLastAccessed, _lastAccessTime);
        }
        
        const time_t* getLastModificationTime()
        {
            return _lastModificationTime;
        }
        
        void setLastModificationTime(const time_t* time)
        {
            _lastModificationTime = time;
            
            setValue(nfo::fileLastModified, _lastModificationTime);
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
        
        Dimensions* getDimensions()
        {
            return _dimensions;
        }
        
        void setDimensions(Dimensions* dimensions)
        {
            _dimensions = dimensions;
            
            Resource::setValue(art::dimensions, dimensions);
        }
    };
}

#endif // FILEDATAOBJECT_H
