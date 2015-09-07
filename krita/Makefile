APP_DIR=calligra-2.8.1
APP_SRC=calligra_2.8.1-1.orig.tar.xz
APP_SRC_URL=https://launchpadlibrarian.net/171284885/$(APP_SRC)

default:
	cd $(APP_DIR)/build
	cmake .. -DCMAKE_INSTALL_PREFIX=/usr -DPRODUCTSET=KRITA

setup:
	if ! test -f $(APP_SRC); then wget $(APP_SRC_URL); fi
	if ! test -f $(APP_DIR)/CMakeLists.txt; then tar -xkf $(APP_SRC); fi
	mkdir -p ./$(APP_DIR)/build
