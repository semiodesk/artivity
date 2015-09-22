#ifndef ACTIVITYINFLUENCE_H
#define ACTIVITYINFLUENCE_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"
#include "Viewbox.h"

namespace artivity
{
    class ActivityInfluence : public Resource
    {
    private:
        Viewbox* _viewbox;
        
        Resource* _location;
        
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
        
        virtual ~ActivityInfluence() {}
        
        Viewbox* getViewbox()
        {
            return _viewbox;
        }
        
        void setViewbox(Viewbox* viewbox)
        {
            _viewbox = viewbox;
            
            Resource::setValue(art::hadViewbox, viewbox);
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
