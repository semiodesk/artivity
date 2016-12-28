var pkg = require('./package.json');

var ver = process.argv[2];

if (ver != undefined) {

    pkg['version'] = ver;

    var fs = require('fs');
    fs.writeFile("./package.json", JSON.stringify(pkg, null, 4), function (err) {
        if (err) {
            return console.log(err);
        }
    });
    console.log("Version was set to "+ver);
}