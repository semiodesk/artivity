
nobase_pkginclude_HEADERS = Activity.h  ActivityLog.h  Property.h  PropertyMap.h  PropertyValue.h  Resource.h  Serializer.h  XsdTypeMap.h \
	Activities/Add.h  Activities/Create.h  Activities/Delete.h  Activities/Remove.h  Activities/Undo.h  Activities/Update.h Activities/View.h \
	Ontologies/as.h	Ontologies/rdf.h  Ontologies/xsd.h
	
lib_LTLIBRARIES = libartivity.la
libartivity_la_SOURCES = Activity.cpp  ActivityLog.cpp  main.cpp  Resource.cpp  Serializer.cpp 
libartivity_la_LDFLAGS = $(CURL_LIBS)
libartivity_la_CFLAGS = $(CURL_CFLAGS) 

	



