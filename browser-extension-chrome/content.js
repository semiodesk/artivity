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
  console.log(">>> [", url, ",", title, "]");

  chrome.runtime.sendMessage({url: url, title: title});
}
