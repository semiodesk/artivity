PACKAGE_NAME = artivity-apid
PACKAGE_VERSION = $(shell dpkg-parsechangelog | grep Version: | cut -d' ' -f2- | cut -d'-' -f1)
PROJECT_FILE = Artivity.Apid.csproj

SHARED_LIBS = Artivity.DataModel.dll dotNetRDF.Data.Virtuoso.dll dotNetRDF.dll OpenLink.Data.Virtuoso.dll Semiodesk.Trinity.dll VDS.Common.dll HtmlAgilityPack.dll Newtonsoft.Json.dll

all:

clean:
	if test -f $(PROJECT_FILE); then \
		xbuild $(PROJECT_FILE) /p:Configuration=Release /t:Clean; \
		rm -Rf bin/Release/*.mdb; \
		rm -Rf bin/Release/*.dll; \
		rm -Rf bin/Release/*.config; \
	fi;
	rm -Rf tmp
	rm -f $(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz

dist:
	xbuild $(PROJECT_FILE) /p:Configuration=Release
	mkdir tmp
	cp bin/Release/*.exe tmp/
	cp bin/Release/*.config tmp/
	cp bin/Release/*.dll tmp/
	cp artivity.png artivity-apid artivity-apid.desktop Makefile COPYING tmp/
	cd tmp && rm $(SHARED_LIBS)
	chmod 644 tmp/*
	chmod 755 tmp/*.exe
	cp -R debian tmp/
	cd tmp && tar cvzf ../$(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz *
	rm -R tmp

install:
	mkdir -p $(DESTDIR)/usr/bin/
	mkdir -p $(DESTDIR)/usr/lib/artivity/
	install -D -m 644 *.dll $(DESTDIR)/usr/lib/artivity/
	install -D -m 755 artivity-apid.exe $(DESTDIR)/usr/lib/artivity/artivity-apid.exe
	install -D -m 644 artivity-apid.exe.config $(DESTDIR)/usr/lib/artivity/artivity-apid.exe.config
	install -D -m 644 artivity-apid.desktop $(DESTDIR)/usr/share/applications/artivity-apid.desktop
	install -D -m 644 artivity.png $(DESTDIR)/usr/share/icons/Artivity/32x32/artivity.png
	install -D -m 755 artivity-apid $(DESTDIR)/usr/bin/

uninstall:
	rm -f $(DESTDIR)/usr/lib/artivity/artivity-apid.exe
	rm -f $(DESTDIR)/usr/lib/artivity/artivity-apid.exe.config
	rm -f $(DESTDIR)/usr/bin/artivity-apid
	rm $(DESTDIR)/usr/share/applications/artivity-apid.desktop
	rm $(DESTDIR)/usr/share/icons/Artivity/32x32/artivity.png
