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
        const Viewbox* _viewbox;
        
    public:
        Generation() : Resource(UriGenerator::getUri())
        {
            setValue(rdf::_type, prov::Generation);
        }
        
        Generation(const char* uriref) : Resource(uriref)
        {
            setValue(rdf::_type, prov::Generation);
        }
        
        virtual ~Generation() {}
        
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

#endif // GENERATION_H
