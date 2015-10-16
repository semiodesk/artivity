APP_DIR=calligra-2.8.1
APP_SRC=calligra_2.8.1-1.orig.tar.xz
APP_SRC_URL=https://launchpadlibrarian.net/171284885/$(APP_SRC)
BUILD_DIR=$(APP_DIR)/build
PACKAGE_NAME=artivity-extension-krita
PACKAGE_VERSION=1.5.0

setup:
	if test ! -f $(APP_DIR)/CMakeLists.txt; then \
		if test ! -f $(APP_SRC); then \
			wget $(APP_SRC_URL); \
		fi; \
		tar --skip-old-files -xf $(APP_SRC); \
	fi;
	mkdir -p $(BUILD_DIR) && cd $(BUILD_DIR) && cmake .. -DCMAKE_INSTALL_PREFIX=/usr -DPRODUCTSET=KRITA

default: setup
	cd $(BUILD_DIR) && make krita-extension-artivity

install: default
	cd $(BUILD_DIR)/krita/plugins/extensions/artivity/ && make install

clean:
	rm -f $(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz
	rm -Rf $(BUILD_DIR)

dist: clean
	tar cvzf $(PACKAGE_NAME)-$(PACKAGE_VERSION).tar.gz $(APP_DIR) debian Makefile
