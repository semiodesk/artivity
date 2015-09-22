#ifndef GENERATION_H
#define GENERATION_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"
#include "Viewbox.h"

namespace artivity
{
    class Generation : public Resource
    {
    private:
        Viewbox* _viewbox;
        
        Resource* _location;
        
        string _value;
        
    public:
        Generation() : Resource(UriGenerator::getUri())
        {
            Resource::setValue(rdf::_type, prov::Generation);
        }
        
        Generation(const char* uriref) : Resource(uriref)
        {
            Resource::setValue(rdf::_type, prov::Generation);
        }
        
        virtual ~Generation() {}
        
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

#endif // GENERATION_H
