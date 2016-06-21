// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2016

#ifndef _ART_ART_H
#define _ART_ART_H

namespace artivity
{
	#define art(label) "art:"label;
	#define ART(label) "http://w3id.org/art/terms/1.0/"label;

    namespace art
    {
		static const char* NS_PREFIX = art("");
		static const char* NS_URI = ART("");
       
		static const char* EditFile = art("EditFile");
        
		#ifdef CreateFile
		#undef CreateFile
		#endif

		static const char* CreateFile = art("CreateFile");
        
		#ifdef DeleteFile
		#undef DeleteFile
		#endif

		static const char* DeleteFile = art("DeleteFile");
        
		static const char* WebBrowsing = art("WebBrowsing");

        // --- ACTIVITIES ---

        // Indicates that a entity influence invalidated another.
        static const char* reverted = art("reverted");

        // Indicates that a entity influence re-generated another.
        static const char* restored = art("restored");
   
        // The invalidation of a prior entity influence.
        static const char* Undo = art("Undo");
        
        // The re-generation of a formerly invalidated entity influence.
        static const char* Redo = art("Redo");

        static const char* count = art("count");
        
        static const char* selectedLayer = art("selectedLayer");

        static const char* layerZPosition = art("layerZPosition")
        
        static const char* isCaptureEnabled = art("isCaptureEnabled");
        static const char* loggingEnabled = art("loggingEnabled");

        static const char* thumbnailUrl = art("thumbnailUrl");
        
        // A rasterized portion of an image that provides bitmap data for an entity influence.
        static const char* renderedAs =art("renderedAs");

        // Specifies the shape and location of something in Euclidean space.
        static const char* region = art("region");

        // Rasterized bitmap data of the entire extents of an image.
        static const char* RenderingDataObject = art("RenderingDataObject");

        // Rasterized bitmap data of a specific region of an image.
        static const char* PartialRenderingDataObject = art("PartialRenderingDataObject");

        
        // --- GEOMETRY ---
        // NOTE: We try to be compatible with OGC geometry vocabularies since
        //       many databases support spatial querying using this standard.
        
        static const char* canvas = art("canvas");
                
        // Subclass of :hadGeometry
		static const char* hadCanvas = art("hadCanvas");
        
        // Subclass of :Rectangle
		static const char* Canvas = art("Canvas");
        
        // No equivalent in GeoSPARQL
		static const char* coordinateSystem = art("coordinateSystem");
        
        // No equivalent in GeoSPARQL
		static const char* CoordinateSystem = art("CoordinateSystem");
        
        // WKT CS, Cartesian
        // https://en.wikipedia.org/wiki/Well-known_text
		static const char* CartesianCoordinateSystem = art("CartesianCoordinateSystem");
        
        // No equivalent in GeoSPARQL
		static const char* lengthUnit = art("lengthUnit");
        
        // No equivalent in GeoSPARL, MATLAB notation [1 0 0; 0 1 0; 0 0 1]
		static const char* transformationMatrix = art("transformationMatrix");
        
        // http://www.opengis.net/ont/geosparql#hasGeometry
		static const char* hadGeometry = art("hadGeometry");
        
        // WKT Geometry
        // http://www.opengis.net/ont/geosparql#Geometry
		static const char* Geometry = art("Geometry");
        
		static const char* SoftwareAssociation = art("SoftwareAssociation");

        // http://www.opengis.net/ont/geosparql#coordinateDimension
		static const char* coordinateDimension = art("coordinateDimension");
        
        // No equivalent in GeoSPARQL
        static const char* Rectangle = art("Rectangle");
        
        // No equivalent in GeoSPARQL
        static const char* width = art("width");
        
        // No equivalent in GeoSPARQL
        static const char* height = art("height");
        
        // No equivalent in GeoSPARQL
        static const char* Cube = art("Cube");
        
        // No equivalent in GeoSPARQL
        static const char* depth = art("depth");
        
        // No equivalent in GeoSPARQL
        static const char* hadBoundaries = art("hadBoundaries");
        
        // Subclass of :Rectangle
        static const char* BoundingRectangle = art("BoundingRectangle");
        
        // Subclass of :Cube
        static const char* BoundingCube = art("BoundingCube");
        
        // No equivalent in GeoSPARQL
        static const char* hadViewport = art("hadViewport");
        
        // Subclass of :Rectangle
        static const char* Viewport = art("Viewport");
        
		static const char* version = art("version");

        // No equivalent in GeoSPARQL
        static const char* zoomFactor = art("zoomFactor");
        
        // http://www.opengis.net/ont/geosparql#coordinateDimension
        static const char* position = art("position");
        
        // WKT Point
        // No equivalent in GeoSPARQL
        static const char* Point = art("Point");
        
        // No equivalent in GeoSPARQL
        static const char* x = art("x");
        
        // No equivalent in GeoSPARQL
        static const char* y = art("y");
        
        // No equivalent in GeoSPARQL
        static const char* z = art("z");
        
        // --- UNITS ---
        // NOTE: Except for px, all units are owl:sameAs their counterparts in the QUDT ontology.
        
        // http://qudt.org/vocab/unit#Millimeter
        static const char* mm = art("mm");
        
        // http://qudt.org/vocab/unit#Centimeter
        static const char* cm = art("cm");
        
        // http://qudt.org/vocab/unit#Meter
        static const char* m = art("m");
        
        // http://qudt.org/vocab/unit#Point
        static const char* pt = art("pt");
        
        // http://qudt.org/vocab/unit#Inch
        static const char* in = art("in");
        
        // http://qudt.org/vocab/unit#Foot
        static const char* ft = art("ft");
        
        // No equivalent in QUDT
        static const char* px = art("px");

        // NOTE: The following instances are absolute URLs!
        static const char* USER = ART("USER");
        static const char* SOFTWARE = ART("SOFTWARE");

        
        // --- CHANGES ---
        static const char* Change = art("Change");		
        static const char* qualifiedChange = art("qualifiedChange");

        static const char* property = art("property");

        static const char* order = art("order");

        static const char* location = art("location");

        static const char* bounds = art("bounds");

        static const char* textValue = art("textValue");

        static const char* fontSize = art("fontSize");

        static const char* font = art("font");

        static const char* strokeColour = art("strokeColour");

        static const char* fillColour = art("fillColour");

        static const char* strokePattern = art("strokePattern");
        
        static const char* fillPattern = art("fillPattern");

        static const char* strokeWidth = art("strokeWidth");

        static const char* unknown = art("unkown");
        
    }
}

#endif // _ART_ART_H
