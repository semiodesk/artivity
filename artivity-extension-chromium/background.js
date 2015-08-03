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
		var tabId;
		if( sender && sender.tab && sender.tab.id)
			tabId = sender.tab.id;
		
        var params = {title: title, url: url, actor: "application://chromium-browser.desktop", tab: tabId}

        var request = new XMLHttpRequest();
        request.open("POST", 'http://localhost:8272/artivity/1.0/activities', true);
        request.setRequestHeader("Content-type", "application/json");
        request.send(JSON.stringify(params));
     }
});
