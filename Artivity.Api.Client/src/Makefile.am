nobase_pkginclude_HEADERS = Agent.h SoftwareAgent.h Role.h Person.h Viewbox.h Association.h SoftwareAssociation.h Entities/Entity.h Entities/FileDataObject.h Entities/WebDataObject.h Activity.h  \
	ActivityLog.h  Property.h  PropertyMap.h  PropertyValue.h  Resource.h  Serializer.h UriGenerator.h  XsdTypeMap.h UriGenerator.h \
	Activities/Add.h  Activities/Create.h  Activities/Delete.h  Activities/Remove.h  Activities/Undo.h  Activities/Update.h Activities/View.h Activities/Open.h Activities/Close.h Activities/Save.h \
	Ontologies/prov.h Ontologies/rdf.h Ontologies/xsd.h Ontologies/art.h Ontologies/svg.h Ontologies/nfo.h
	
lib_LTLIBRARIES = libartivity.la

libartivity_la_SOURCES = Activity.cpp  ActivityLog.cpp  main.cpp  Resource.cpp  Serializer.cpp 
libartivity_la_LDFLAGS = $(CURL_LIBS)
libartivity_la_CFLAGS = $(CURL_CFLAGS) 
