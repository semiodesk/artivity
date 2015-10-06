#ifndef ACTIVITYINFLUENCE_H
#define ACTIVITYINFLUENCE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"
#include "Geometry/Viewport.h"

namespace artivity
{
    class ActivityInfluence : public Resource
    {
    private:
        Viewport* _viewport;
        
        Geometry* _boundaries;
        
        Resource* _location;
        
        time_t _time;
                
        string _value;
        
    public:
        ActivityInfluence() : Resource(UriGenerator::getUri())
        {
            Resource::setValue(rdf::_type, prov::ActivityInfluence);
        }
        
        ActivityInfluence(const char* uriref) : Resource(uriref)
        {
            Resource::setValue(rdf::_type, prov::ActivityInfluence);
        }
        
        Viewport* getViewport()
        {
            return _viewport;
        }
        
        void setViewport(Viewport* viewport)
        {
            _viewport = viewport;
            
            Resource::setValue(art::hadViewport, viewport);
        }
        
        Geometry* getBoundaries()
        {
            return _boundaries;
        }
        
        void setBoundaries(Geometry* boundaries)
        {
            _boundaries = boundaries;
            
            Resource::setValue(art::hadBoundaries, boundaries);
        }
        
        Resource* getLocation()
        {
            return _location;
        }
        
        void setLocation(Resource* location)
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
        
        string getValue()
        {
            return _value;
        }
        
        void setValue(string value)
        {
            _value = string(value);
            
            Resource::setValue(prov::value, _value.c_str());
        }
    };
}

#endif // ACTIVITYINFLUENCE_H
