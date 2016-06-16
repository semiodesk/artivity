#ifndef ARTIVITY_H
#define ARTIVITY_H


#include "defines.h"
#include "Resource.h"
#include "Property.h"
#include "UriGenerator.h"
#include "Serializer.h"
#include "ActivityLog.h"
#include "EditingSession.h"

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
#include "ObjectModel/Person.h"
#include "ObjectModel/Role.h"
#include "ObjectModel/SoftwareAgent.h"

#include "ObjectModel/Geometry/Point.h"
#include "ObjectModel/Geometry/Canvas.h"
#include "ObjectModel/Geometry/CartesianCoordinateSystem.h"
#include "ObjectModel/Geometry/BoundingRectangle.h"
#include "ObjectModel/Geometry/BoundingCube.h"
#include "ObjectModel/Geometry/Viewport.h"

#include "ObjectModel/Activities/Browse.h"
#include "ObjectModel/Activities/CreateFile.h"
#include "ObjectModel/Activities/DeleteFile.h"
#include "ObjectModel/Activities/EditFile.h"
#include "ObjectModel/Influences/Add.h"
#include "ObjectModel/Influences/Edit.h"
#include "ObjectModel/Influences/Save.h"
#include "ObjectModel/Influences/Remove.h"
#include "ObjectModel/Influences/Undo.h"
#include "ObjectModel/Influences/Redo.h"

#include "ObjectModel/Entities/FileDataObject.h"
#include "ObjectModel/Entities/WebDataObject.h"
#include "ObjectModel/Entities/XmlElement.h"
#include "ObjectModel/Entities/XmlAttribute.h"

#endif // ARTIVITY_H
