PACKAGE_NAME = artivity-extension-firefox
PACKAGE_VERSION = 1.5.0
PACKAGE_ID = jid1-1BXmXMTUuyzP8g@jetpack

all:

dist:
	tar cvzf $(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz $(PACKAGE_ID)-$(PACKAGE_VERSION).xpi Makefile COPYING debian

install:
	install -D -m 644 $(PACKAGE_ID)-$(PACKAGE_VERSION).xpi $(DESTDIR)/usr/lib/firefox-addons/extensions/$(PACKAGE_ID).xpi

uninstall:
	rm $(DESTDIR)/usr/lib/firefox-addons/extensions/$(PACKAGE_ID).xpi
