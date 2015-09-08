// Copyright (c) 2015 Semiodesk GmbH                                                                                   
//      
// AUTHORS
// - Sebastian Faubel <sebastian@semiodesk.com>                                                                        
// - Moritz Eberl <moritz@semiodesk.com>                                                                               
//                                                                                                                     
// Distributed under the GNU GPL version 3.

var url = document.URL;
var title = document.title;

// Do not track secure connections.
if(url.indexOf('http') === 0)
{
  var tabs = require('sdk/tabs');
  var tabId = tabs.activeTab.id;
    
  console.log(">>> [", url, ",", title, "]");

  var params = {title: document.title, url: document.URL, actor: "application://firefox.desktop", tab: tabId}

  var request = new XMLHttpRequest();
  request.open("POST", 'http://localhost:8272/artivity/1.0/activities/web/', true);
  request.setRequestHeader("Content-type", "application/json");
  request.send(JSON.stringify(params));
}
