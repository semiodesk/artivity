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
  width: 350,
  height: 200,
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
  if (state.checked) {
      panel.show({ position: button });
  }
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

// Handle tab events.
const {XMLHttpRequest} = require("sdk/net/xhr");

var endpoint = "http://localhost:8272/artivity/1.0/activities/web/";

var agent = "application://firefox-browser.desktop";

var tabs = require("sdk/tabs");

tabs.on('open', logOpen);
tabs.on('close', logClose);
tabs.on('ready', logUsage);
 
function logOpen(tab) {
    var now = new Date();
    
    console.log("[", now, ", CREATE] ");
    
    var params = {agent: agent, tab: tab.id, url: null, title: null, time: null, startTime: now, endTime: null};
    
    sendRequest(params);
}

function logClose(tab) {
    var now = new Date();
    
    console.log("[", now, ", REMOVE] ");
    
    var params = {agent: agent, tab: tab.id, url: null, title: null, time: null, startTime: null, endTime: now};
    
    sendRequest(params);
}

function logUsage(tab) {
    var now = new Date();
    
    console.log("[", now, ", UPDATED] ", tab.url, tab.title);
    
    var params = {agent: agent, tab: tab.id, url: tab.url, title: tab.title, time: now, startTime: null, endTime: null};
    
    sendRequest(params);
}

function sendRequest(params)
{
    var now = new Date();
    
    console.log("[", now, "] ", endpoint, params);
    
    var request = new XMLHttpRequest();
    request.open("POST", endpoint, true);
    request.setRequestHeader("Content-type", "application/json");
    request.send(JSON.stringify(params));
}
