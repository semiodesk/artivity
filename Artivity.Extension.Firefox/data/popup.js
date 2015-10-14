// Copyright (c) 2015 Semiodesk GmbH                                                                                   
//      
// AUTHORS
// - Sebastian Faubel <sebastian@semiodesk.com>                                                                        
// - Moritz Eberl <moritz@semiodesk.com>                                                                               
//                                                                                                                     
// Distributed under the GNU GPL version 3.  

function formatParams(params) {
  return "?" + Object.keys(params).map(function(key){ return key + "=" + params[key] }).join("&");
}

function onRequestError() {
  console.log("<<<", this.status);

  var warningBox = document.getElementById('warning-box');
  warningBox.style.visibility = "visible";
  warningBox.style.display = 'block';

  var settingsBox = document.getElementById('settings-box');
  settingsBox.style.opacity = '0.5';

  var enabledBox = document.getElementById('settings-option-enabled');
  enabledBox.checked = false;

  self.postMessage(false);
}

function onRequestResponse() {
  console.log("<<<", this.response);

  var warningBox = document.getElementById('warning-box');
  warningBox.style.visibility = "collapse";
  warningBox.style.display = 'none';

  var settingsBox = document.getElementById('settings-box');
  settingsBox.style.opacity = '1.0';

  var enabledBox = document.getElementById('settings-option-enabled');
    
  if(this.response != 'null' && this.response.enabled != 'null') {
     enabledBox.checked = this.response.enabled;
  }

  self.postMessage(enabledBox.checked);
}

function sendRequest(type, callback, errorCallback, params) {
  var url = 'http://localhost:8272/artivity/1.0/agents/status/';

  params['agent'] = 'application://firefox-browser.desktop';
  
  if(type === 'GET') {
      url += formatParams(params);
  }

  console.log(">>>", url);
    
  var request = new XMLHttpRequest();
  request.open(type, url, true);
  request.setRequestHeader("Content-type", "application/json");
  request.responseType = 'json';
  request.onload = callback;
  request.onerror = errorCallback;
    
  if(type == 'POST') {
    request.send(JSON.stringify(params));
  }
  else {
    request.send();
  }
}

// Initialize the value of the checkbox.
sendRequest('GET', onRequestResponse, onRequestError, {});

document.getElementById('settings-option-enabled').onchange = function() {
  // NOTE: Setting the new value from the response ensures that the value 
  // of the checkbox reflects the value in the Artivity deamon in case of errors.
  sendRequest('POST', onRequestResponse, onRequestError, {'enabled' : this.checked});
};
