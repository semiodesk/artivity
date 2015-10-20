PACKAGE_NAME = artivity-extension-chromium
PACKAGE_VERSION = $(shell dpkg-parsechangelog | grep Version: | cut -d' ' -f2- | cut -d'-' -f1)

all:

package:
	zip -r package.crx COPYING *.js *.json *.png *.html css fonts

dist: package
	tar cvzf $(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz package.crx Makefile COPYING debian

install:
	install -D -m 644 package.crx $(DESTDIR)/usr/share/chromium-browser/extensions/artivity-$(PACKAGE_VERSION).crx
	mkdir $(DESTDIR)/usr/share/chromium-browser/extensions/artivity
	unzip package.crx -d $(DESTDIR)/usr/share/chromium-browser/extensions/artivity/
