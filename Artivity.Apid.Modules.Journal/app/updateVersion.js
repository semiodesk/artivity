var package = require('./package.json');

var ver = process.argv[2];

if (ver != undefined) {

    package['version'] = ver;

    var fs = require('fs');
    fs.writeFile("./package.json", JSON.stringify(package, null, 4), function (err) {
        if (err) {
            return console.log(err);
        }
    });
    console.log("Version was set to "+ver);
}