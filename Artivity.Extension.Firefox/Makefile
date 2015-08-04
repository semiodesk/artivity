all:

dist:
	tar cvzf artivity-extension-firefox-1.0.2.tar.gz package.xpi Makefile COPYING

install:
	install -D -m 644 package.xpi $(DESTDIR)/usr/lib/firefox-addons/extensions/jid1-1BXmXMTUuyzP8g@jetpack.xpi

uninstall:
	rm $(DESTDIR)/usr/lib/firefox-addons/extensions/jid1-1BXmXMTUuyzP8g@jetpack.xpi
