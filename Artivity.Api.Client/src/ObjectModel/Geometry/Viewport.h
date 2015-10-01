#ifndef VIEWBOX_H
#define VIEWBOX_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "Rectangle.h"

namespace artivity
{
    class Viewport : public Rectangle
    {
    private:
        double _zoomFactor;
        
    public:
        Viewport() : Rectangle()
        {
            setValue(rdf::_type, art::Viewport);
        }
        
        Viewport(const char* uriref) : Rectangle(uriref)
        {
            setValue(rdf::_type, art::Viewport);
        }
        
        double getZoomFactor()
        {
            return _zoomFactor;
        }
        
        void setZoomFactor(double zoomFactor)
        {
            _zoomFactor = zoomFactor;
            
            setValue(art::zoomFactor, zoomFactor);
        }
    };
}

#endif // VIEWBOX_H
