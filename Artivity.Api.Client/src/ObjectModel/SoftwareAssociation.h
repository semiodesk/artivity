#ifndef SOFTWAREASSOCIATION_H
#define SOFTWAREASSOCIATION_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/prov.h"
#include "../UriGenerator.h"

#include "Viewbox.h"

namespace artivity
{
    class SoftwareAssociation : public Association
    {
    private:
        const Viewbox* _viewbox;
        
    public:
        SoftwareAssociation() : Association() {}
        
        SoftwareAssociation(const char* uriref) : Association(uriref) {}
        
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

#endif // SOFTWAREASSOCIATION_H
