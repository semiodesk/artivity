// Copyright (c) 2015 Semiodesk GmbH                                                                                   
//      
// AUTHORS
// - Sebastian Faubel <sebastian@semiodesk.com>                                                                        
// - Moritz Eberl <moritz@semiodesk.com>                                                                               
//                                                                                                                     
// Distributed under the GNU GPL version 3.  

function onRequestError() {
  console.log("<<<", this.status);
}

function onRequestResponse() {
  console.log("<<<", this.response);

  var enabledBox = document.getElementById('enabledBox');

  if(this.response == true || this.response == false) {
     enabledBox.checked = this.response;
  }

  self.postMessage(this.response);
}

function sendRequest(callback, errorCallback, params) {
  var url = 'http://localhost:8272/artivity/1.0/status';

  params['actor'] = 'application://firefox.desktop';

  console.log(">>>", url);

  var request = new XMLHttpRequest();
  request.open('POST', url, true);
  request.setRequestHeader("Content-type", "text/plain");
  request.responseType = 'json';
  request.onload = callback;
  request.onerror = errorCallback;
  request.send(JSON.stringify(params));
}

// Initialize the value of the checkbox.
sendRequest(onRequestResponse, onRequestError, {});

document.getElementById('enabledBox').onchange = function() {
  // NOTE: Setting the new value from the response ensures that the value 
  // of the checkbox reflects the value in the Artivity deamon in case of errors.
  sendRequest(onRequestResponse, onRequestError, {'enabled' : this.checked});
};
