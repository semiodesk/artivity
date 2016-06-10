#!/bin/bash

set -e
CONSOLE_USER=$(ps aux | grep console | grep -v grep | cut -d' ' -f1)
userID=$(sudo -u $CONSOLE_USER id -u $CONSOLE_USER)

#echo $CONSOLE_USER >> /tmp/script.out
#echo $userID >> /tmp/script.out
launchctl bootstrap gui/$userID /Library/LaunchAgents/com.semiodesk.artivity.plist
