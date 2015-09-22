#ifndef INVALIDATION_H
#define INVALIDATION_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

namespace artivity
{
    class Invalidation : public Resource
    {
    private:
        Viewbox* _viewbox;
        
        Resource* _location;
        
        string _value;
        
    public:
        Invalidation() : Resource(UriGenerator::getUri())
        {
            Resource::setValue(rdf::_type, prov::Invalidation);
        }
        
        Invalidation(const char* uriref) : Resource(uriref)
        {
            Resource::setValue(rdf::_type, prov::Invalidation);
        }
        
        virtual ~Invalidation() {}
        
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
            _value = value;
            
            Resource::setValue(prov::value, value);
        }
    };
}


#endif // INVALIDATION_H
