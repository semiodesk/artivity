all:

dist:
	zip -r artivity-explorer.zip __main__.py lib/
	echo "#! /usr/bin/env python" | cat - artivity-explorer.zip > artivity-explorer

	chmod 755 artivity-explorer

	tar cvzf artivity-explorer-1.0.3.tar.gz artivity-explorer artivity-explorer.desktop Makefile COPYING

	rm artivity-explorer.zip
	rm artivity-explorer

install:
	install -D -m 755 artivity-explorer $(DESTDIR)/usr/bin/artivity-explorer
	install -D -m 644 artivity-explorer.desktop $(DESTDIR)/usr/share/applications/artivity-explorer.desktop

uninstall:
	rm $(DESTDIR)/usr/bin/artivity-explorer
	rm $(DESTDIR)/usr/share/applications/artivity-explorer.desktop
