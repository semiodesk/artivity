

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
                var package = lastItem['rss:enclosure']['@'];
                resolve(package);
            });

        });
    }

    function canUpdate() {
        return new Promise(function (resolve, reject) {
            this.mostRecentUpdate().then(function (package) {
                var updateVer = package["sparkle:version"];
                var semver = require('semver');
                if (semver.gte(updateVer, this.currentVersion)) {
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
        return new Promise(function (resolve, reject) {

            var crypto = require("crypto");

            var fs = require("fs");

            //http://stackoverflow.com/questions/667017/how-to-check-if-a-file-has-a-digital-signature
            var signature = this.package['sparkle:dsasignature'];

            var verifier = crypto.createVerify('sha1');

            var https = require('https');
            var fs = require('fs');
            var remote = require('electron').remote;
            var app = remote.app;

            var path = app.getPath('temp') + "\\artivity-update-" + package["sparkle:version"] + ".exe";
            this.localPath = path;

            var pubkeyPath = app.getAppPath() + "/dsa_pub.pem";
            var pubkey = fs.readFileSync(pubkeyPath).toString();

            if (fs.existsSync(path)) {
                var s = fs.ReadStream(path);
                s.on('data', function (d) {
                    verifier.update(d);
                });

                s.on('end', function () {

                    console.log(verifier.verify(pubkey, signature, 'base64'));
                    resolve();
                });



            } else {
                var file = fs.createWriteStream(path);
                var request = https.get(package.url, function (response) {
                    response.pipe(file);
                    response.pipe(verifier);
                });

                request.on('close', function () {
                    verifier.end();
                    console.log(verifier.verify(pubkey, signature, 'base64'));
                    resolve();
                });
            }
        });



    }

    function executeUpdate()
    {

    }
}



