PACKAGE_NAME = artivity-apid
PACKAGE_VERSION = $(shell dpkg-parsechangelog | grep Version: | cut -d' ' -f2- | cut -d'-' -f1)

SHARED_LIBS = Artivity.Model.dll dotNetRDF.Data.Virtuoso.dll dotNetRDF.dll OpenLink.Data.Virtuoso.dll Semiodesk.Trinity.dll VDS.Common.dll HtmlAgilityPack.dll Newtonsoft.Json.dll virtado4.dll

all:

clean:
	rm -Rf dist
	rm -f $(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz

dist:
	mkdir dist
	cp bin/Release/*.exe dist/
	cp bin/Release/*.config dist/
	cp bin/Release/*.dll dist/
	cp artivity.png artivity-apid artivity-apid.desktop Makefile COPYING dist/
	cd dist && rm $(SHARED_LIBS)
	chmod 644 dist/*
	chmod 755 dist/*.exe
	cp -R debian dist/
	cd dist && tar cvzf ../$(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz *
	rm -R dist

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
