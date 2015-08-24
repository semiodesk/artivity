#!/bin/bash
#wget https://launchpadlibrarian.net/171284885/calligra_2.8.1-1.orig.tar.xz
tar -xkf calligra_2.8.1-1.orig.tar.xz
mkdir ./calligra-2.8.1/build
cd calligra-2.8.1/build
cmake .. -DCMAKE_INSTALL_PREFIX=/usr -DPRODUCTSET=KRITA

