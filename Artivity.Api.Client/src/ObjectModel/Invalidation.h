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
        const Viewbox* _viewbox;
        
    public:
        Invalidation() : Resource(UriGenerator::getUri())
        {
            setValue(rdf::_type, prov::Invalidation);
        }
        
        Invalidation(const char* uriref) : Resource(uriref)
        {
            setValue(rdf::_type, prov::Invalidation);
        }
        
        virtual ~Invalidation() {}
        
        const Viewbox* getViewbox()
        {
            return _viewbox;
        }
        
        void setViewbox(const Viewbox* viewbox)
        {
            _viewbox = viewbox;
            
            setValue(art::hadViewbox, viewbox);
        }
    };
}


#endif // INVALIDATION_H
