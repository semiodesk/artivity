
function UpdateChecker(appcastUrl, currentVersion) {

    if (currentVersion == undefined) {
        var pjson = require('./package.json');
        this.currentVersion = pjson.version;
        console.log(this.currentVersion);
    } else {
        this.currentVersion = currentVersion;
    }

    this.appcastUrl = appcastUrl;
    this.package;
    this.localPath;

    this.mostRecentUpdate = mostRecentUpdate;
    this.canUpdate = canUpdate;
    this.downloadUpdate = downloadUpdate;
    this.executeUpdate = executeUpdate;
    //    this.validateUpdate = validateUpdate;

    function mostRecentUpdate() {
        return new Promise(function(resolve, reject) {

            var FeedParser = require('feedparser');
            var request = require('request');

            var req = request(appcastUrl);
            var feedparser = new FeedParser();
            var lastItem;
            req.on('error', function(error) {
                reject();
            });

            req.on('response', function(res) {
                var stream = this;

                if (res.statusCode != 200) return this.emit('error', new Error('Bad status code'));

                stream.pipe(feedparser);
            });


            feedparser.on('error', function(error) {
                reject();
            });

            feedparser.on('readable', function() {
                var meta = this.meta; // **NOTE** the "meta" is always available in the context of the feedparser instance
                var item;

                while (item = this.read()) {
                    lastItem = item;
                }


            });

            feedparser.on('end', function() {
                var package = lastItem['rss:enclosure']['@'];
                resolve(package);
            });

        });
    }

    function canUpdate() {
        var currentVersion = this.currentVersion;
        return new Promise(function(resolve, reject) {
            mostRecentUpdate().then(function(package) {
                var updateVer = package["sparkle:version"];
                var semver = require('semver');
                if (semver.gte(updateVer, currentVersion)) {
                    this.package = package;
                    resolve(updateVer);
                }
                else {
                    reject();
                }
            });
        });
    }

    function downloadUpdate() {
        return new Promise(function(resolve, reject) {

            var fs = require("fs");

            var https = require('https');
            var fs = require('fs');
            var remote = require('electron').remote;
            var app = remote.app;

            var p = app.getPath('temp') + "\\artivity-update-" + package["sparkle:version"] + ".exe";
            this.localPath = p;

            if (fs.existsSync(p)) {
                verifySignature(this.localPath, function(result) {
                    resolve();
                });
            } else {
                var file = fs.createWriteStream(p);
                var request = https.get(package.url, function(response) {
                    response.pipe(file);
                });

                request.on('close', function() {
                    verifySignature(this.localPath, function(result) {
                        resolve();
                    });
                });
            }
        });
    }

    function verifySignature(p) {
        return new Promise(function(resolve, reject) {
            var exec = require('child_process').exec;

            var child = exec("UpdateChecker.exe \"" + p + "\"");
            child.stdout.on('data', function(chunk) {
                list.push(chunk);
            });

            child.stdout.on('end', function() {
                var result = JSON.parse(list.join());
                if ('error' in result)
                    reject();

                resolve();
            });

        });
    }

    function executeUpdate() {

    }
}

