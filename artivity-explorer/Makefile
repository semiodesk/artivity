all:

dist:
	zip -r artivity-explorer.zip __main__.py lib/ 
	echo "#! /usr/bin/env python" | cat - artivity-explorer.zip > artivity-explorer

	chmod 755 artivity-explorer

	tar cvzf artivity-explorer-1.1.1.tar.gz artivity-explorer artivity-explorer.desktop res/icon32.png res/icon64.png res/icon128.png res/colour.png res/composition.png res/editing.png Makefile COPYING

	rm artivity-explorer.zip
	rm artivity-explorer

install:
	install -D -m 755 artivity-explorer $(DESTDIR)/usr/bin/artivity-explorer
	install -D -m 644 artivity-explorer.desktop $(DESTDIR)/usr/share/applications/artivity-explorer.desktop
	install -D -m 644 res/icon32.png $(DESTDIR)/usr/share/icons/Artivity/32x32/artivity-explorer.png
	install -D -m 644 res/icon64.png $(DESTDIR)/usr/share/icons/Artivity/64x64/artivity-explorer.png
	install -D -m 644 res/icon128.png $(DESTDIR)/usr/share/icons/Artivity/128x128/artivity-explorer.png
	install -D -m 644 res/composition.png $(DESTDIR)/usr/share/icons/Artivity/16x16/composition.png
	install -D -m 644 res/editing.png $(DESTDIR)/usr/share/icons/Artivity/16x16/editing.png
	install -D -m 644 res/colour.png $(DESTDIR)/usr/share/icons/Artivity/16x16/colour.png

uninstall:
	rm $(DESTDIR)/usr/bin/artivity-explorer
	rm $(DESTDIR)/usr/share/applications/artivity-explorer.desktop
	rm $(DESTDIR)/usr/share/icons/Artivity/32x32/artivity-explorer.png
	rm $(DESTDIR)/usr/share/icons/Artivity/64x64/artivity-explorer.png
	rm $(DESTDIR)/usr/share/icons/Artivity/128x128/artivity-explorer.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/composition.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/editing.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/colour.png
