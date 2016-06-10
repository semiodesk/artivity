#!/bin/bash

WC_DIR="$1"
TARGET="$2"



WC_DEV=$(hdiutil info | grep -B1 "${WC_DIR}$" | awk 'NR==1{print $1}' )

umount $WC_DIR
hdiutil create -srcdevice $WC_DEV -format UDZO -imagekey zlib-level=9 $TARGET -scrub
hdiutil detach $WC_DEV -quiet -force
