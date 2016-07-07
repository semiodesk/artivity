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

chrome.tabs.onCreated.addListener(
    function(tab) {
        var now = new Date();

        console.log("[", now, ", CREATED] ");

        logActivity(tab.id, now, null);
    }
);

chrome.tabs.onUpdated.addListener(
    function(tabId, changeInfo, tab) {
        if(tab.url !== undefined) {
            // Do not track secure connections and only track on load to avoid duplicate events.
            if(tab.url.indexOf('https') === -1 && changeInfo.status == "loading") {
                var now = new Date();

                console.log("[", now, ", UPDATED] ", tab.url, tab.title);

                logUsage(tab.id, now, tab.url, tab.title);
            }
        }
    }
);

chrome.tabs.onRemoved.addListener(
    function(tabId, removeInfo) {
        var now = new Date();

        console.log("[", now, ", REMOVED] ");

        logActivity(tabId, null, now);
    }
);

function logUsage(tabId, time, url, title)
{
    var params = {agent: manifest.agent, tab: tabId, url: url, title: title, time: time, startTime: null, endTime: null};

    sendRequest(params);
}

function logActivity(tabId, startTime, endTime)
{
    var params = {agent: manifest.agent, tab: tabId, url: null, title: null, time: null, startTime: startTime, endTime: endTime};

    sendRequest(params);
}

function sendRequest(params)
{
    var endpoint = manifest.endpoint + "/activities/web/";
    var now = new Date();

    console.log("[", now, "] ", endpoint, params);

    var request = new XMLHttpRequest();
    request.open("POST", endpoint, true);
    request.setRequestHeader("Content-type", "application/json");
    request.send(JSON.stringify(params));
}
