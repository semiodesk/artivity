PACKAGE_REPO = ppa:faubulous/artivity
PACKAGE_NAME = $(shell cd .. && dpkg-parsechangelog | grep Source: | cut -d' ' -f2)
PACKAGE_VERSION = $(shell cd .. && dpkg-parsechangelog | grep Version: | cut -d' ' -f2- | cut -d'-' -f1)
PACKAGE_RELEASE = $(shell cd .. && dpkg-parsechangelog | grep Version: | cut -d' ' -f2- | cut -d'-' -f2)

default: binary

clean:
	rm -f *.build
	rm -f *.upload
	rm -f *.deb
	rm -f *.changes
	rm -f *.dsc
	rm -f *.orig.tar.gz
	rm -f *.debian.tar.gz
	rm -f *.diff.gz
	rm -Rf build-area
	rm -Rf $(PACKAGE_NAME)

branch: clean
	cd .. && make dist && mv $(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz debian-dist/
	cp $(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz $(PACKAGE_NAME)_$(PACKAGE_VERSION).orig.tar.gz
	mkdir $(PACKAGE_NAME)
	cp -R ../debian $(PACKAGE_NAME)/
	tar xvzf $(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz -C $(PACKAGE_NAME)
	if test -d $(PACKAGE_NAME)/$(PACKAGE_NAME)-$(PACKAGE_VERSION); then \
		mv -f $(PACKAGE_NAME)/$(PACKAGE_NAME)-$(PACKAGE_VERSION)/* $(PACKAGE_NAME)/; \
		rm -Rf $(PACKAGE_NAME)/$(PACKAGE_NAME)-$(PACKAGE_VERSION); \
	fi;

binary: branch
	cd $(PACKAGE_NAME) && debuild -i -us -uc

source: branch
	cd $(PACKAGE_NAME) && debuild -S

release: branch source
	dput $(PACKAGE_REPO) $(PACKAGE_NAME)_$(PACKAGE_VERSION)-$(PACKAGE_RELEASE)_source.changes
