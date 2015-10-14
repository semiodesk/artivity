PACKAGE_NAME = artivity-extension-chromium
PACKAGE_VERSION = 1.5.0

all:

dist:
	tar cvzf $(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz package.crx Makefile COPYING debian

install:
	install -D -m 644 package.crx $(DESTDIR)/usr/share/chromium-browser/extensions/artivity-$(PACKAGE_VERSION).crx
