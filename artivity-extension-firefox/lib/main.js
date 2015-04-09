"use strict";

var self = require("sdk/self");

var button = require("sdk/ui/button/action").ActionButton({
  id: "artivity-tab",
  label: "Artivity",
  icon: "./icon.png",
  onClick: function() {
    require("sdk/tabs").activeTab.attach({
      contentScriptFile: self.data.url("content.js")
    });
  }
});
