
var pkg = require('./package.json');

var version = pkg["version"].split(".");

var _major = false;
if(process.argv.indexOf("-m") != -1){
   _major = true;
} 

var _minor = false;
if(process.argv.indexOf("-i") != -1){
   _minor = true;
} 

var _build = false;
if(process.argv.indexOf("-b") != -1){
   _build = true;
} 

var _rev
if(process.argv.indexOf("-r") != -1){
   _rev = true;
} 

if(version.length > 0 && _major )
console.log("MAJOR_VERSION=" + version[0]);
if (version.length > 1 && _minor)
    console.log("MINOR_VERSION=" + version[1]);
if (version.length > 2 && _build )
    console.log("BUILD_VERSION=" + version[2]);
if (version.length > 3 && _rev )
    console.log("REVISION=" + version[3]);