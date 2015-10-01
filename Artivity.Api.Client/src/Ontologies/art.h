#ifndef ART_H
#define ART_H

#include "../Property.h"

#define ART(label) "http://semiodesk.com/artivity/1.0/"label;

namespace artivity
{
    namespace art
    {
        // --- ACTIVITIES ---
        // NOTE: There are very little predefined activity types in PROV.
        //       The following are inspired by the Activity Streams vocabulary.
        
        // http://www.w3.org/ns/activitystreams#Add
        static const Resource Add = ART("Add");
        
        // http://www.w3.org/ns/activitystreams#Remove
        static const Resource Remove = ART("Remove");
        
        // http://www.w3.org/ns/activitystreams#Create
        static const Resource Create = ART("Create");
        
        // http://www.w3.org/ns/activitystreams#Delete
        static const Resource Delete = ART("Delete");
        
        // http://www.w3.org/ns/activitystreams#Undo
        static const Resource Undo = ART("Undo");
        
        // http://www.w3.org/ns/activitystreams#Update
        static const Resource Update = ART("Update");
        
        // http://www.w3.org/ns/activitystreams#View
        static const Resource View = ART("View");
        
        // No match in ActivityStreams
        static const Resource Redo = ART("Redo");
        
        // No match in ActivityStreams
        static const Resource Open = ART("Open");
        
        // No match in ActivityStreams
        static const Resource Close = ART("Close");
        
        // No match in ActivityStreams
        static const Resource Save = ART("Save");
        
        static const Property selectedLayer = ART("selectedLayer");
        
        // --- AGENT ---
        static const Property isCaptureEnabled = ART("isCaptureEnabled");
        
        // --- GEOMETRY ---
        // NOTE: We try to be compatible with OGC geometry vocabularies since
        //       many databases support spatial querying using this standard.
        
        // Subclass of :hadGeometry
        static const Property hadCanvas = ART("hadCanvas");
        
        // Subclass of :Rectangle
        static const Resource Canvas = ART("Canvas");
        
        // No equivalent in GeoSPARQL
        static const Property coordinateSystem = ART("coordinateSystem");
        
        // No equivalent in GeoSPARQL
        static const Resource CoordinateSystem = ART("CoordinateSystem");
        
        // WKT CS, Cartesian
        // https://en.wikipedia.org/wiki/Well-known_text
        static const Resource CartesianCoordinateSystem = ART("CartesianCoordinateSystem");
        
        // No equivalent in GeoSPARQL
        static const Property lengthUnit = ART("lengthUnit");
        
        // No equivalent in GeoSPARL, MATLAB notation [1 0 0; 0 1 0; 0 0 1]
        static const Property transformationMatrix = ART("transformationMatrix");
        
        // http://www.opengis.net/ont/geosparql#hasGeometry
        static const Property hadGeometry= ART("hadGeometry");
        
        // WKT Geometry
        // http://www.opengis.net/ont/geosparql#Geometry
        static const Resource Geometry = ART("Geometry");
        
        // http://www.opengis.net/ont/geosparql#coordinateDimension
        static const Property coordinateDimension = ART("coordinateDimension");
        
        // No equivalent in GeoSPARQL
        static const Resource Rectangle = ART("Rectangle");
        
        // No equivalent in GeoSPARQL
        static const Property width = ART("width");
        
        // No equivalent in GeoSPARQL
        static const Property height = ART("height");
        
        // No equivalent in GeoSPARQL
        static const Resource Cube = ART("Cube");
        
        // No equivalent in GeoSPARQL
        static const Property depth = ART("depth");
        
        // No equivalent in GeoSPARQL
        static const Property hadBoundaries = ART("hadBoundaries");
        
        // Subclass of :Rectangle
        static const Resource BoundingRectangle = ART("BoundingRectangle");
        
        // Subclass of :Cube
        static const Resource BoundingCube = ART("BoundingCube");
        
        // No equivalent in GeoSPARQL
        static const Property hadViewport = ART("hadViewport");
        
        // Subclass of :Rectangle
        static const Resource Viewport = ART("Viewport");
        
        // No equivalent in GeoSPARQL
        static const Property zoomFactor = ART("zoomFactor");
        
        // http://www.opengis.net/ont/geosparql#coordinateDimension
        static const Property position = ART("position");
        
        // WKT Point
        // No equivalent in GeoSPARQL
        static const Resource Point = ART("Point");
        
        // No equivalent in GeoSPARQL
        static const Property x = ART("x");
        
        // No equivalent in GeoSPARQL
        static const Property y = ART("y");
        
        // No equivalent in GeoSPARQL
        static const Property z = ART("z");
        
        // --- UNITS ---
        // NOTE: Except for px, all units are owl:sameAs their counterparts in the QUDT ontology.
        
        // http://qudt.org/vocab/unit#Millimeter
        static const Resource mm = ART("mm");
        
        // http://qudt.org/vocab/unit#Centimeter
        static const Resource cm = ART("cm");
        
        // http://qudt.org/vocab/unit#Meter
        static const Resource m = ART("m");
        
        // http://qudt.org/vocab/unit#Point
        static const Resource pt = ART("pt");
        
        // http://qudt.org/vocab/unit#Inch
        static const Resource in = ART("in");
        
        // http://qudt.org/vocab/unit#Foot
        static const Resource ft = ART("ft");
        
        // No equivalent in QUDT
        static const Resource px = ART("px");
    }
}

#endif // ART_H
