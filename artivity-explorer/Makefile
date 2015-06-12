all:

dist:
	zip -r artivity-explorer.zip __main__.py lib/
	echo "#! /usr/bin/env python" | cat - artivity-explorer.zip > artivity-explorer

	chmod 755 artivity-explorer

	tar cvzf artivity-explorer-1.0.6.tar.gz artivity-explorer artivity-explorer.desktop icon32.png icon64.png icon128.png Makefile COPYING

	rm artivity-explorer.zip
	rm artivity-explorer

install:
	install -D -m 755 artivity-explorer $(DESTDIR)/usr/bin/artivity-explorer
	install -D -m 644 artivity-explorer.desktop $(DESTDIR)/usr/share/applications/artivity-explorer.desktop
	install -D -m 644 icon32.png $(DESTDIR)/usr/share/icons/Artivity/32x32/artivity-explorer.png
	install -D -m 644 icon64.png $(DESTDIR)/usr/share/icons/Artivity/64x64/artivity-explorer.png
	install -D -m 644 icon128.png $(DESTDIR)/usr/share/icons/Artivity/128x128/artivity-explorer.png

uninstall:
	rm $(DESTDIR)/usr/bin/artivity-explorer
	rm $(DESTDIR)/usr/share/applications/artivity-explorer.desktop
	rm $(DESTDIR)/usr/share/icons/Artivity/32x32/artivity-explorer.png
	rm $(DESTDIR)/usr/share/icons/Artivity/64x64/artivity-explorer.png
	rm $(DESTDIR)/usr/share/icons/Artivity/128x128/artivity-explorer.png
