#ifndef VIEWBOX_H
#define VIEWBOX_H

#include "../Ontologies/rdf.h"
#include "../Ontologies/art.h"
#include "../UriGenerator.h"

namespace artivity
{
    class Viewbox : public Resource
    {
    private:
        double _left;
        double _right;
        double _top;
        double _bottom;
        double _zoomFactor;
        
    public:
        Viewbox() : Resource(UriGenerator::getUri())
        {
            setValue(rdf::_type, art::Viewbox);
        }
        
        Viewbox(const char* uriref) : Resource(uriref)
        {
            setValue(rdf::_type, art::Viewbox);
        }
        
        double getLeft()
        {
            return _left;
        }
        
        void setLeft(double left)
        {
            _left = left;
            
            setValue(art::left, left);
        }
        
        double getRight()
        {
            return _right;
        }
        
        void setRight(double right)
        {
            _right = right;
            
            setValue(art::right, right);
        }
        
        double getTop()
        {
            return _top;
        }
        
        void setTop(double top)
        {
            _top = top;
            
            setValue(art::top, top);
        }
        
        double getBottom()
        {
            return _bottom;
        }
        
        void setBottom(double bottom)
        {
            _bottom = bottom;
            
            setValue(art::bottom, bottom);
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
