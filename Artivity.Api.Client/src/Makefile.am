lib_LTLIBRARIES = libartivity.la

libartivity_la_SOURCES = main.cpp  Resource.cpp  Serializer.cpp ActivityLog.cpp ObjectModel/Activity.cpp
libartivity_la_LDFLAGS = $(CURL_LIBS)
libartivity_la_CFLAGS = $(CURL_CFLAGS) 

nobase_pkginclude_HEADERS = artivity.h \
    Resource.h \
    Property.h \
    PropertyMap.h \
    PropertyValue.h \
    Serializer.h \
    UriGenerator.h \
    XsdTypeMap.h \
    ActivityLog.h \
    ObjectModel/Activity.h  \
    ObjectModel/Agent.h \
    ObjectModel/Association.h \
    ObjectModel/Role.h \
    ObjectModel/Person.h \
    ObjectModel/Viewbox.h \
    ObjectModel/SoftwareAssociation.h \
    ObjectModel/SoftwareAgent.h \
    ObjectModel/Entity.h \
    ObjectModel/Entities/FileDataObject.h \
    ObjectModel/Entities/WebDataObject.h \
    ObjectModel/Activities/Add.h \
    ObjectModel/Activities/Create.h \
    ObjectModel/Activities/Delete.h \
    ObjectModel/Activities/Remove.h \
    ObjectModel/Activities/Undo.h \
    ObjectModel/Activities/Update.h \
    ObjectModel/Activities/View.h \
    ObjectModel/Activities/Open.h \
    ObjectModel/Activities/Close.h \
    ObjectModel/Activities/Save.h \
    Ontologies/prov.h \
    Ontologies/rdf.h \
    Ontologies/xsd.h \
    Ontologies/art.h \
    Ontologies/svg.h \
    Ontologies/nfo.h
