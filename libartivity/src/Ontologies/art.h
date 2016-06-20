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

#ifndef ART_H
#define ART_H

namespace artivity
{
	#define art(label) "art:"label;
	#define ART(label) "http://w3id.org/art/terms/1.0/"label;

    namespace art
    {
		static const char* NS_PREFIX = art();
		static const char* NS_URI = ART();
       
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
        // NOTE: There are very little predefined activity types in PROV.
        //       The following are inspired by the Activity Streams vocabulary.
        
		static const char* Edit = art("Edit");
        
		static const char* Add = art("Add");
        
        static const char* Remove = art("Remove");
        
		static const char* Undo = art("Undo");
        
		static const char* Redo = art("Redo");
        
		static const char* Save = art("Save");
        
		static const char* selectedLayer = art("selectedLayer");

        static const char* layerZPosition = art("layerZPosition")
        
		static const char* isCaptureEnabled = art("isCaptureEnabled");

        static const char* thumbnailUrl = art("thumbnailUrl");

        static const char* thumbnailPos = art("thumbnailPosition"); 
        
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
    }
}

#endif // ART_H
