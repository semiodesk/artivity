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
