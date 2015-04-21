#! /bin/bash

echo "ARTIVITY SETUP, Version 1.0"

if ! echo "import werkzeug1" | python 2> /dev/null ; then
 echo
 echo "Error: Please install python werkzeug library before running this setup."
 exit -1
fi

echo
echo ">>> Installing Artivity REST API deamon.."

LOCAL_BIN_DIR=$HOME/.local/bin/

[ ! -d $LOCAL_BIN_DIR ] && mkdir -p $LOCAL_BIN_DIR

cp -vR artivity-apid/artivity-apid $LOCAL_BIN_DIR
cp -vR artivity-apid/lib $LOCAL_BIN_DIR

nohup $LOCAL_BIN_DIR/artivity-apid > /dev/null &

echo
echo ">>> Installing autostart scripts for GNOME shell.."

AUTOSTART_DIR=$HOME/.config/autostart/

[ ! -d $AUTOSTART_DIR ] && mkdir -p $AUTOSTART_DIR

cp -v artivity-apid/artivity-apid.desktop $AUTOSTART_DIR

echo
echo ">>> Installing browser extensions.."
echo
echo "NOTE: Please drag & drop the browser extension files into your"
echo "favourite browser to enable the browser extensions."

nautilus --no-desktop browser-packages &

echo
echo "Done. Please restart your desktop session to complete the installation.. :)"
