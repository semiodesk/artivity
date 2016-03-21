lib_LTLIBRARIES = libartivity.la

libartivity_la_SOURCES = main.cpp curlresponse.cpp Resource.cpp  Serializer.cpp ActivityLog.cpp ObjectModel/Activity.cpp UriGenerator.cpp
libartivity_la_LDFLAGS = $(CURL_LIBS) -version-info 0:0:0
libartivity_la_CXXFLAGS = $(CURL_CFLAGS) $(ADD_CXXFLAGS)

nobase_pkginclude_HEADERS = curlresponse.h \
    artivity.h \
    Resource.h \
    Property.h \
    PropertyMap.h \
    PropertyValue.h \
    Serializer.h \
    UriGenerator.h \
    XsdTypeMap.h \
    ActivityLog.h \
    ObjectModel/Activity.h  \
    ObjectModel/ActivityInfluence.h  \
    ObjectModel/Agent.h \
    ObjectModel/Association.h \
    ObjectModel/Role.h \
    ObjectModel/Person.h \
    ObjectModel/Geometry/Canvas.h \
    ObjectModel/Geometry/CoordinateSystem.h \
    ObjectModel/Geometry/CartesianCoordinateSystem.h \
    ObjectModel/Geometry/Point.h \
    ObjectModel/Geometry/Geometry.h \
    ObjectModel/Geometry/Rectangle.h \
    ObjectModel/Geometry/Cube.h \
    ObjectModel/Geometry/BoundingRectangle.h \
    ObjectModel/Geometry/BoundingCube.h \
    ObjectModel/Geometry/Viewport.h \
    ObjectModel/Generation.h \
    ObjectModel/Invalidation.h \
    ObjectModel/SoftwareAgent.h \
    ObjectModel/Entity.h \
    ObjectModel/Entities/FileDataObject.h \
    ObjectModel/Entities/WebDataObject.h \
    ObjectModel/Entities/XmlElement.h \
    ObjectModel/Entities/XmlAttribute.h \
    ObjectModel/Activities/Browse.h \
    ObjectModel/Activities/CreateFile.h \
    ObjectModel/Activities/DeleteFile.h \
    ObjectModel/Activities/EditFile.h \
    ObjectModel/Influences/Save.h \
    ObjectModel/Influences/Add.h \
    ObjectModel/Influences/Remove.h \
    ObjectModel/Influences/Edit.h \
    ObjectModel/Influences/Undo.h \
    ObjectModel/Influences/Redo.h \
    Ontologies/prov.h \
    Ontologies/rdf.h \
    Ontologies/xsd.h \
    Ontologies/xml.h \
    Ontologies/art.h \
    Ontologies/svg.h \
    Ontologies/nfo.h \
    Ontologies/dces.h \
    Ontologies/unit.h

