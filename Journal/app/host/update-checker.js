function UpdateChecker(appcastUrl, currentVersion, appService) {
    var t = this;

    t.init(appcastUrl, currentVersion, appService);
}

UpdateChecker.prototype.init = function (appcastUrl, currentVersion, appService) {
    var t = this;

    t.appService = appService;
    t.appcastUrl = appcastUrl;

    console.log('Appcast URL:', t.appcastUrl);

    if (currentVersion === undefined) {
        var pjson = require('./package.json');

        t.currentVersion = pjson.version;
    } else {
        t.currentVersion = currentVersion;
    }

    console.log('Current version:', t.currentVersion);

    t.eventListeners = {};
}

UpdateChecker.prototype.isUpdateAvailable = function () {
    var t = this;

    return new Promise(function (resolve, reject) {
        var FeedParser = require('feedparser');
        var Request = require('request');

        var feedparser = new FeedParser();
        var lastItem;

        var req = Request(t.appcastUrl);

        req.on('error', function (error) {
            reject();
        });

        req.on('response', function (res) {
            var stream = this;

            if (res.statusCode != 200) {
                return this.emit('error', new Error('Bad status code'));
            }

            stream.pipe(feedparser);
        });

        feedparser.on('readable', function () {
            var meta = this.meta; // **NOTE** the "meta" is always available in the context of the feedparser instance.
            var item;

            while (item = this.read()) {
                lastItem = item;
            }
        });

        feedparser.on('error', function (error) {
            reject();
        });

        feedparser.on('end', function () {
            var pkg = lastItem['rss:enclosure']['@'];
            var version = pkg["sparkle:version"];
            var semver = require('semver');

            if (semver.gte(version, t.currentVersion)) {
                var update = {
                    title: lastItem.title,
                    releaseNotesUrl: lastItem['sparkle:releasenoteslink']['#'],
                    published: lastItem.pubDate,
                    version: version,
                    length: pkg.length,
                    url: pkg.url
                };

                resolve(update);

                t.raiseEvent('updateAvailable');
            } else {
                reject();
            }
        });
    });
}

UpdateChecker.prototype.isUpdateDownloaded = function (update) {
    var t = this;

    // Note: This is platform dependent and should be moved into a factory.
    update.localPath = t.appService.configPath('temp') + "\\artivity-update-" + update.version + ".msi";

    return t.verifyUpdateInstallerSignature(update);
}

UpdateChecker.prototype.downloadUpdate = function (update) {
    var t = this;

    return new Promise(function (resolve, reject) {
        t.isUpdateDownloaded(update).then(function () {
            resolve(update);
        }).catch(function () {
            var https = require('https');

            var request = https.get(update.url, function (response) {
                var totalBytes = parseInt(response.headers['content-length']);
                var transferredBytes = 0;

                var fs = require('fs');
                var file = fs.createWriteStream(update.localPath);

                response.pipe(file);

                if (t.eventListeners['progress'] && totalBytes > 0) {
                    response.on('data', function (chunk) {
                        // Update the received bytes
                        transferredBytes += chunk.length;

                        t.raiseEvent('progress', {
                            totalBytes: totalBytes,
                            transferredBytes: transferredBytes,
                            percentComplete: Math.floor(100 * (transferredBytes / totalBytes))
                        });
                    });
                }
            });

            request.on('close', function () {
                t.verifyUpdateInstallerSignature(update).then(function () {
                    resolve(update);
                }).catch(function () {
                    reject(update);
                });
            });
        });
    });
}

UpdateChecker.prototype.installUpdate = function (update) {
    var t = this;

    return new Promise(function (resolve, reject) {
        try {
            // Execute the installer as seperate process.
            const spawn = require('child_process').spawn;
            const child = spawn("Msiexec", ["/i", update.localPath], {
                detached: true,
                stdio: ['ignore', 'ignore', 'ignore']
            });

            // Do not wait for the child process to return.
            child.unref();

            // Close the Artivity app window and process.
            t.appService.exit();
        } catch (err) {
            console.error(err);

            reject(update);
        }
    });
}

UpdateChecker.prototype.verifyUpdateInstallerSignature = function (update) {
    return new Promise(function (resolve, reject) {
        try {
            fs.accessSync("real_exixs_path", fs.R_OK | fs.W_OK)

            var script = require('path').dirname(__filename) + '\\js\\host\\VerifySignature.exe';
            // Execute the signature verifier in a separate process.
            const execute = require('child_process').execFile;
            const child = execute(script, [update.localPath], (error, stdout, stderr) => {
                if (error) {
                    console.error('Error validating signature:', stderr);

                    reject(update);
                }

                var result = JSON.parse(stdout);

                if ("error" in result) {
                    reject(update);
                } else {
                    update.signature = result;

                    resolve(update);
                }
            });
        } catch (e) {
            reject(update);
        }
    });
}

UpdateChecker.prototype.on = function (event, callback) {
    var t = this;

    if (callback) {
        if (!t.eventListeners[event]) {
            t.eventListeners[event] = [];
        }

        t.eventListeners[event].push(callback);
    }
}

UpdateChecker.prototype.off = function (event, callback) {
    var t = this;

    if (callback && event in t.eventListeners) {
        var i = t.eventListeners[event].indexOf(callback);

        if (i > -1) {
            t.eventListeners[event].splice(i, 1);
        }
    }
}

UpdateChecker.prototype.raiseEvent = function (event, params) {
    var t = this;

    if (event in t.eventListeners) {
        var listeners = t.eventListeners[event];

        for (var i in listeners) {
            listeners[i](params);
        }
    }
}