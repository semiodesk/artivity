// Copyright (c) 2015 Semiodesk GmbH
//
// AUTHORS
// - Sebastian Faubel <sebastian@semiodesk.com>
// - Moritz Eberl <moritz@semiodesk.com>
//
// Distributed under the GNU GPL version 3.

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
