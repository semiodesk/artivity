all:

dist:
	tar cvzf artivity-extension-chromium-1.0.0.tar.gz package.crx Makefile COPYING

install:
	install -D -m 644 package.crx $(DESTDIR)/usr/share/chromium-browser/extensions/artivity-1.0.0.crx
