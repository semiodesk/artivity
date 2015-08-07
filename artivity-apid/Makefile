all:

dist:
	zip -r artivity-apid.zip __main__.py lib/
	echo "#! /usr/bin/env python" | cat - artivity-apid.zip > artivity-apid

	chmod 755 artivity-apid
	chmod 755 artivity-setup

	tar cvzf artivity-apid-1.0.3.tar.gz artivity-apid artivity-apid.desktop artivity-setup artivity.png Makefile COPYING

	rm artivity-apid.zip
	rm artivity-apid

install:
	install -D -m 755 artivity-apid $(DESTDIR)/usr/bin/artivity-apid
	install -D -m 644 artivity-apid.desktop $(DESTDIR)/usr/share/applications/artivity-apid.desktop
	install -D -m 755 artivity-setup $(DESTDIR)/usr/bin/artivity-setup
	install -D -m 644 artivity.png $(DESTDIR)/usr/share/icons/Artivity/32x32/artivity.png

uninstall:
	rm $(DESTDIR)/usr/bin/artivity-apid
	rm $(DESTDIR)/usr/share/applications/artivity-apid.desktop
	rm $(DESTDIR)/usr/bin/artivity-setup
