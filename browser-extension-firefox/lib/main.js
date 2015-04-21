// Copyright (c) 2015 Semiodesk GmbH
//
// AUTHORS
// - Sebastian Faubel <sebastian@semiodesk.com>
// - Moritz Eberl <moritz@semiodesk.com>
// 
// Distributed under the GNU GPL version 3.

"use strict";

var data = require('sdk/self').data;

var panel = require('sdk/panel').Panel({
  contentURL: data.url('popup.html'),
  contentScriptFile: data.url('popup.js'),
  height: 160,
  onHide: onPanelHide
});

var button = require("sdk/ui/button/toggle").ToggleButton({
  id: 'artivity-tab',
  label: 'Artivity Settings',
  icon: './icon-disabled.png',
  onChange: onButtonStateChanged
});

// Show the panel when the toggle button is pressed.
function onButtonStateChanged(state) {
  if (state.checked) panel.show({ position: button });
}

// Set the property toggle button state when the panel is hidden for _any_ reason.
function onPanelHide() {
  button.state('window', {checked: false});
}

// Listen to messages from the panel and set the button state accordingly.
panel.on("message", function(message) {
  if(message) {
    button.icon = './icon-enabled.png';
  }
  else {
    button.icon = './icon-disabled.png';
  }
});

// Attach the content script any time a new tab becomes ready.
require("sdk/tabs").on('ready', logArtivity);
 
function logArtivity(tab) {
  tab.attach({
    contentScriptFile: data.url('content.js')
  });
}
