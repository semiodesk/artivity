// Copyright (c) 2015 Semiodesk GmbH                                                                                   
//      
// AUTHORS
// - Sebastian Faubel <sebastian@semiodesk.com>                                                                        
// - Moritz Eberl <moritz@semiodesk.com>                                                                               
//                                                                                                                     
// Distributed under the GNU GPL version 3.

chrome.runtime.onMessage.addListener(
  function(request, sender, sendResponse) {
     var url = request.url;
     var title = request.title;

     if(url.indexOf('http') === 0) {
        var params = {title: title, url: url, actor: "application://chromium-browser.desktop"}

        var request = new XMLHttpRequest();
        request.open("POST", 'http://localhost:8272/artivity/1.0/activities', true);
        request.setRequestHeader("Content-type", "text/plain");
        request.send(JSON.stringify(params));
     }
});
