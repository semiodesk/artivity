
function UpdateChecker(appcastUrl, currentVersion) {

    if (currentVersion == undefined) {
        var pjson = require('./package.json');
        this.currentVersion = pjson.version;
        console.log(this.currentVersion);
    } else {
        this.currentVersion = currentVersion;
    }

    this.appcastUrl = appcastUrl;

    this.mostRecentUpdate = mostRecentUpdate;
    this.canUpdate = canUpdate;
    this.downloadUpdate = downloadUpdate;
    this.executeUpdate = executeUpdate;
    //    this.validateUpdate = validateUpdate;

    function mostRecentUpdate() {
        return new Promise(function (resolve, reject) {

            var FeedParser = require('feedparser');
            var request = require('request');

            var req = request(appcastUrl);
            var feedparser = new FeedParser();
            var lastItem;
            req.on('error', function (error) {
                reject();
            });

            req.on('response', function (res) {
                var stream = this;

                if (res.statusCode != 200) return this.emit('error', new Error('Bad status code'));

                stream.pipe(feedparser);
            });


            feedparser.on('error', function (error) {
                reject();
            });

            feedparser.on('readable', function () {
                var meta = this.meta; // **NOTE** the "meta" is always available in the context of the feedparser instance
                var item;

                while (item = this.read()) {
                    lastItem = item;
                }
            });

            feedparser.on('end', function () {
                var item = lastItem;
                resolve(item);
            });

        });
    }

    function canUpdate() {
        var currentVersion = this.currentVersion;
        return new Promise(function (resolve, reject) {
            mostRecentUpdate().then(function (item) {
                var package = item['rss:enclosure']['@'];
                var updateVer = package["sparkle:version"];
                var semver = require('semver');
                if (semver.gte(updateVer, currentVersion)) {
                    this.update = {
                        title: item.title,
                        releaseNotesUrl: item['sparkle:releaseNotesLink'],
                        published: item.pubDate,
                        version: updateVer,
                        length: package.length,
                        url: package.url
                    };

                    resolve(this.update);
                }
                else {
                    reject();
                }
            });
        });
    }

    function downloadUpdate(update) {
        return new Promise(function (resolve, reject) {
            var https = require('https');
            var fs = require('fs');

            var remote = require('electron').remote;
            var app = remote.app;
            var p = app.getPath('temp') + "\\artivity-update-" + update.version + ".msi";
            
            update.localPath = p;

            if (fs.existsSync(p)) {
                verifySignature(update.localPath).then(function (result) {
                    update.signature = result;
                    resolve(update);
                });
            } else {
                var file = fs.createWriteStream(p);
                var request = https.get(update.url, function (response) {
                    response.pipe(file);
                });

                request.on('close', function () {
                    verifySignature(update.localPath).then(function (result) {
                        update.signature = result;
                        resolve(update);
                    });
                });
            }
        });
    }

    function verifySignature(p) {
        return new Promise(function (resolve, reject) {

            const execFile = require('child_process').execFile;
            var scriptPath = require('path').dirname(__filename);
            const child = execFile(scriptPath + '\\js\\host\\VerifySignature.exe', [p], (error, stdout, stderr) => {
                if (error) {
                    console.error('stderr', stderr);
                    throw error;
                }
                var result = JSON.parse(stdout);
                if ("error" in result) {
                    reject(result);
                } else {
                    resolve(result);
                }
            });
        });
    }

    function executeUpdate(update) {
        return new Promise(function (resolve, reject) {
            const execFile = require('child_process').spawn;
            const child = execFile("Msiexec", ["/i", update.localPath], {detached: true, stdio: ['ignore', 'ignore', 'ignore']});
            child.unref();

            const remote = require('electron').remote;
            remote.app.exit();
        });
    }
}

