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
// Copyright (c) Semiodesk GmbH 2015

#ifndef _ART_ARTIVITY_H
#define _ART_ARTIVITY_H

#include "Resource.h"
#include "Property.h"
#include "UriGenerator.h"
#include "Serializer.h"
#include "ActivityLog.h"

#include "Ontologies/rdf.h"
#include "Ontologies/xsd.h"
#include "Ontologies/xml.h"
#include "Ontologies/prov.h"
#include "Ontologies/nfo.h"
#include "Ontologies/art.h"
#include "Ontologies/dces.h"
#include "Ontologies/unit.h"

#include "ObjectModel/Activity.h"
#include "ObjectModel/ActivityInfluence.h"
#include "ObjectModel/Agent.h"
#include "ObjectModel/Association.h"
#include "ObjectModel/Generation.h"
#include "ObjectModel/Invalidation.h"
#include "ObjectModel/Entity.h"
#include "ObjectModel/EntityInfluence.h"
#include "ObjectModel/Person.h"
#include "ObjectModel/Role.h"
#include "ObjectModel/SoftwareAgent.h"

#include "ObjectModel/Geometry/Point.h"
#include "ObjectModel/Geometry/Canvas.h"
#include "ObjectModel/Geometry/CartesianCoordinateSystem.h"
#include "ObjectModel/Geometry/BoundingRectangle.h"
#include "ObjectModel/Geometry/Viewport.h"

#include "ObjectModel/Activities/WebBrowsing.h"
#include "ObjectModel/Activities/CreateFile.h"
#include "ObjectModel/Activities/DeleteFile.h"
#include "ObjectModel/Activities/EditFile.h"
#include "ObjectModel/Influences/Redo.h"
#include "ObjectModel/Influences/Undo.h"

#include "ObjectModel/Entities/FileDataObject.h"
#include "ObjectModel/Entities/WebDataObject.h"
#include "ObjectModel/Entities/XmlElement.h"
#include "ObjectModel/Entities/XmlAttribute.h"

#endif // _ART_ARTIVITY_H
