#! /bin/bash

echo "ARTIVITY SETUP, Version 1.0"

echo
echo ">>> Installing Artivity REST API deamon for browser extensions.."

LOCAL_BIN_DIR=$HOME/.local/bin/

[ ! -d $LOCAL_BIN_DIR ] && mkdir -p $LOCAL_BIN_DIR

cp -vR artivity-apid/artivity-apid $LOCAL_BIN_DIR
cp -vR artivity-apid/lib $LOCAL_BIN_DIR

echo
echo ">>> Installing autostart scripts for GNOME shell.."

AUTOSTART_DIR=$HOME/.config/autostart/

[ ! -d $AUTOSTART_DIR ] && mkdir -p $AUTOSTART_DIR

cp -v artivity-apid/artivity-apid.desktop $AUTOSTART_DIR

echo
gnome-session-properties

echo
echo "Done. Please restart your desktop session to complete the installation.. :)"
