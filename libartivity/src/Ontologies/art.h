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

#define ART(label) "http://semiodesk.com/artivity/1.0/" label;

namespace artivity
{
    namespace art
    {
        #ifdef CreateFile
        #undef CreateFile
        #endif
        #ifdef DeleteFile
        #undef DeleteFile
        #endif

		static const char* USER = ART("USER");
		static const char* SOFTWARE = ART("SOFTWARE");

        // http://www.w3.org/ns/activitystreams#View
        static const char* WebBrowsing = ART("WebBrowsing");
        
        // No match in ActivityStreams
		static const char* EditFile = ART("EditFile");
        
        // No match in ActivityStreams
		static const char* CreateFile = ART("CreateFile");
        
        // No match in ActivityStreams
		static const char* DeleteFile = ART("DeleteFile");
        
        // --- ACTIVITIES ---

        // Indicates that a entity influence invalidated another.
        static const char* reverted = ART("reverted");

        // Indicates that a entity influence re-generated another.
        static const char* restored = ART("restored");
   
        // The invalidation of a prior entity influence.
		static const char* Undo = ART("Undo");
        
        // The re-generation of a formerly invalidated entity influence.
		static const char* Redo = ART("Redo");

        static const char* count = ART("count");
        
		static const char* selectedLayer = ART("selectedLayer");

        static const char* layerZPosition = ART("layerZPosition")
        
        // --- AGENT ---
        static const char* loggingEnabled = ART("loggingEnabled");

        // --- THUMBNAIL ---
        // A rasterized portion of an image that provides bitmap data for an entity influence.
        static const char* renderedAs = ART("renderedAs");

        // Specifies the shape and location of something in Euclidean space.
        static const char* region = ART("region");

        // Rasterized bitmap data of the entire extents of an image.
        static const char* RenderingDataObject = ART("RenderingDataObject");

        // Rasterized bitmap data of a specific region of an image.
        static const char* PartialRenderingDataObject = ART("PartialRenderingDataObject");

        
        // --- GEOMETRY ---
        // NOTE: We try to be compatible with OGC geometry vocabularies since
        //       many databases support spatial querying using this standard.
        
		static const char* canvas = ART("canvas");
                
        // Subclass of :hadGeometry
		static const char* hadCanvas = ART("hadCanvas");
        
        // Subclass of :Rectangle
		static const char* Canvas = ART("Canvas");
        
        // No equivalent in GeoSPARQL
		static const char* coordinateSystem = ART("coordinateSystem");
        
        // No equivalent in GeoSPARQL
		static const char* CoordinateSystem = ART("CoordinateSystem");
        
        // WKT CS, Cartesian
        // https://en.wikipedia.org/wiki/Well-known_text
		static const char* CartesianCoordinateSystem = ART("CartesianCoordinateSystem");
        
        // No equivalent in GeoSPARQL
		static const char* lengthUnit = ART("lengthUnit");
        
        // No equivalent in GeoSPARL, MATLAB notation [1 0 0; 0 1 0; 0 0 1]
		static const char* transformationMatrix = ART("transformationMatrix");
        
        // http://www.opengis.net/ont/geosparql#hasGeometry
		static const char* hadGeometry = ART("hadGeometry");
        
        // WKT Geometry
        // http://www.opengis.net/ont/geosparql#Geometry
		static const char* Geometry = ART("Geometry");
        
        // http://www.opengis.net/ont/geosparql#coordinateDimension
		static const char* coordinateDimension = ART("coordinateDimension");
        
        // No equivalent in GeoSPARQL
        static const char* Rectangle = ART("Rectangle");
        
        // No equivalent in GeoSPARQL
        static const char* width = ART("width");
        
        // No equivalent in GeoSPARQL
        static const char* height = ART("height");
        
        // No equivalent in GeoSPARQL
        static const char* Cube = ART("Cube");
        
        // No equivalent in GeoSPARQL
        static const char* depth = ART("depth");
        
        // No equivalent in GeoSPARQL
        static const char* hadBoundaries = ART("hadBoundaries");
        
        // Subclass of :Rectangle
        static const char* BoundingRectangle = ART("BoundingRectangle");
        
        // Subclass of :Cube
        static const char* BoundingCube = ART("BoundingCube");
        
        // No equivalent in GeoSPARQL
        static const char* hadViewport = ART("hadViewport");
        
        // Subclass of :Rectangle
        static const char* Viewport = ART("Viewport");
        
        // No equivalent in GeoSPARQL
        static const char* zoomFactor = ART("zoomFactor");
        
        // http://www.opengis.net/ont/geosparql#coordinateDimension
        static const char* position = ART("position");
        
        // WKT Point
        // No equivalent in GeoSPARQL
        static const char* Point = ART("Point");
        
        // No equivalent in GeoSPARQL
        static const char* x = ART("x");
        
        // No equivalent in GeoSPARQL
        static const char* y = ART("y");
        
        // No equivalent in GeoSPARQL
        static const char* z = ART("z");
        
        // --- UNITS ---
        // NOTE: Except for px, all units are owl:sameAs their counterparts in the QUDT ontology.
        
        // http://qudt.org/vocab/unit#Millimeter
        static const char* mm = ART("mm");
        
        // http://qudt.org/vocab/unit#Centimeter
        static const char* cm = ART("cm");
        
        // http://qudt.org/vocab/unit#Meter
        static const char* m = ART("m");
        
        // http://qudt.org/vocab/unit#Point
        static const char* pt = ART("pt");
        
        // http://qudt.org/vocab/unit#Inch
        static const char* in = ART("in");
        
        // http://qudt.org/vocab/unit#Foot
        static const char* ft = ART("ft");
        
        // No equivalent in QUDT
        static const char* px = ART("px");

        // --- CHANGES ---
        static const char* Change = ART("Change");

        static const char* qualifiedChange = ART("qualifiedChange");

        static const char* property = ART("Property");

        static const char* order = ART("order");

        static const char* location = ART("location");

        static const char* bounds = ART("bounds");

        static const char* textValue = ART("textValue");

        static const char* fontSize = ART("fontSize");

        static const char* font = ART("font");

        static const char* strokeColour = ART("strokeColour");

        static const char* fillColour = ART("fillColour");

        static const char* strokePattern = ART("strokePattern");
        
        static const char* fillPattern = ART("fillPattern");

        static const char* strokeWidth = ART("strokeWidth");
        
    }
}

#endif // _ART_ART_H
