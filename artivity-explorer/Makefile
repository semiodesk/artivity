version=1.1.3

all:

dist:
	zip -r artivity-explorer.zip __main__.py lib/ 
	echo "#! /usr/bin/env python" | cat - artivity-explorer.zip > artivity-explorer

	chmod 755 artivity-explorer

	tar cvzf artivity-explorer-$(version).tar.gz artivity-explorer artivity-explorer.desktop res/*.png Makefile COPYING

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
	install -D -m 644 res/edit.png $(DESTDIR)/usr/share/icons/Artivity/16x16/edit.png
	install -D -m 644 res/editing_begin.png $(DESTDIR)/usr/share/icons/Artivity/16x16/editing_begin.png
	install -D -m 644 res/editing_end.png $(DESTDIR)/usr/share/icons/Artivity/16x16/editing_end.png
	install -D -m 644 res/eye.png $(DESTDIR)/usr/share/icons/Artivity/16x16/eye.png
	install -D -m 644 res/globe.png $(DESTDIR)/usr/share/icons/Artivity/16x16/globe.png
	install -D -m 644 res/move.png $(DESTDIR)/usr/share/icons/Artivity/16x16/move.png
	install -D -m 644 res/save.png $(DESTDIR)/usr/share/icons/Artivity/16x16/save.png
	install -D -m 644 res/undo.png $(DESTDIR)/usr/share/icons/Artivity/16x16/undo.png

uninstall:
	rm $(DESTDIR)/usr/bin/artivity-explorer
	rm $(DESTDIR)/usr/share/applications/artivity-explorer.desktop
	rm $(DESTDIR)/usr/share/icons/Artivity/32x32/artivity-explorer.png
	rm $(DESTDIR)/usr/share/icons/Artivity/64x64/artivity-explorer.png
	rm $(DESTDIR)/usr/share/icons/Artivity/128x128/artivity-explorer.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/composition.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/editing.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/colour.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/edit.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/editing_begin.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/editing_end.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/eye.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/globe.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/move.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/save.png
	rm $(DESTDIR)/usr/share/icons/Artivity/16x16/undo.png
