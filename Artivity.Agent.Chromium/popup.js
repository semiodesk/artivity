/*
// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015
*/

var manifest = chrome.runtime.getManifest();

var loggingSwitch = document.getElementById('logging-switch');

var formatParams = function (params) {
	return "?" + Object.keys(params).map(function (key) {
		return key + "=" + params[key]
	}).join("&");
};

var onRequestError = function () {
	console.log("<<<", this.status);

	var warningBox = document.getElementById('warning-box');
	warningBox.style.visibility = "visible";
	warningBox.style.display = 'block';

	var settingsBox = document.getElementById('settings-box');
	settingsBox.style.opacity = '0.5';

	loggingSwitch.checked = false;

	chrome.browserAction.setIcon({
		path: 'icon-disabled.png'
	});
};

var onRequestResponse = function () {
	console.log("<<<", this.response);

	var warningBox = document.getElementById('warning-box');
	warningBox.style.visibility = "collapse";
	warningBox.style.display = 'none';

	var settingsBox = document.getElementById('settings-box');
	settingsBox.style.opacity = '1.0';

	if (this.response !== null && this.response.enabled !== null) {
		loggingSwitch.checked = this.response.enabled;
	}

	if (loggingSwitch.checked) {
		chrome.browserAction.setIcon({
			path: 'icon-enabled.png'
		});
	} else {
		chrome.browserAction.setIcon({
			path: 'icon-disabled.png'
		});
	}
};

var sendRequest = function (type, callback, errorCallback, params) {
	var url = manifest.endpoint + "/agents/status";

	params['agent'] = manifest.agent;

	if (type === 'GET') {
		url += formatParams(params);
	}

	console.log(">>>", url);

	var request = new XMLHttpRequest();
	request.open(type, url, true);
	request.setRequestHeader("Content-type", "application/json");
	request.responseType = 'json';
	request.onload = callback;
	request.onerror = errorCallback;

	if (type == 'POST') {
		request.send(JSON.stringify(params));
	} else {
		request.send();
	}
};

// Initialize the value of the checkbox.
sendRequest('GET', onRequestResponse, onRequestError, {});

loggingSwitch.onchange = function () {
	// NOTE: Setting the new value from the response ensures that the value 
	// of the checkbox reflects the value in the Artivity deamon in case of errors.
	sendRequest('POST', onRequestResponse, onRequestError, {
		'enabled': this.checked
	});

	var switchContainer = document.getElementsByClassName('onoffswitch')[0];

	if (switchContainer !== undefined && switchContainer.className.indexOf('animation-enabled') === -1) {
		switchContainer.className += " animation-enabled";
	}
};