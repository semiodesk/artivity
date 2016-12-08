

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
    this.isUpdate = isUpdate;
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
                var x = lastItem['rss:enclosure']['@'];
                resolve(x);
            });

        });
    }

    function isUpdate(package) {
        var updateVer = package["sparkle:version"];
        var semver = require('semver');
        return semver.gte(updateVer, this.currentVersion);
    }

    function executeUpdate(package) {
        return new Promise(function (resolve, reject) {

            var crypto = require("crypto");

            var fs = require("fs");


            var signature = package['sparkle:dsasignature'];

            var verifier = crypto.createVerify('sha1');

            var https = require('https');
            var fs = require('fs');
            var remote = require('electron').remote;
            var app = remote.app;

            var path = app.getPath('temp') + "\\artivity-update-" + package["sparkle:version"] + ".exe";

            var pubkeyPath = app.getAppPath() + "/dsa_pub.pem";
            var pubkey = fs.readFileSync(pubkeyPath).toString();

            if (fs.existsSync(path)) {
                var s = fs.ReadStream(path);
                s.on('data', function(d) {
                    verifier.update(d);
                });
                
                s.on('end', function() { 
                    
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


}

var checker = new UpdateChecker("https://static.semiodesk.com/artivity/osx/appcast.xml");
checker.mostRecentUpdate().then(function (res) {
    if (checker.isUpdate(res))
        checker.executeUpdate(res);
});