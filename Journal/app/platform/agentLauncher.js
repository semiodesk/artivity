

/**
 *  This class can be used to control the LaunchAgent behaviour on macOS.
 *  @constructor
 *  @param {string} agentName - The name of the agent. Results in the following plist file <agentName>.plist;
 */
function AgentLauncher(agentName, agentDirectory, appPath) {
    this.agentName = agentName;

    const path = require('path');
    const fs = require('fs');

    this.plistName = this.agentName + ".plist";
    this.plistPath = path.resolve(path.join(agentDirectory, this.plistName));
    this.plistExists = fs.existsSync(this.plistPath);

    this.appDirectory = appPath;


    this.plistContent = '<?xml version="1.0" encoding="UTF-8"?>\n' +
        '<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">\n' +
        '<plist version="1.0">\n' +
        '<dict>\n' +
        '<key>KeepAlive</key>\n' +
        '<true/>\n' +
        '<key>Label</key>\n' +
        '<string>com.semiodesk.artivity</string>\n' +
        '<key>ProgramArguments</key>\n' +
        '<array>\n' +
        '<string>{0}/Applications/artivity-apid.app/Contents/MacOS/artivity-apid</string>\n' +
        '</array>\n' +
        '<key>WorkingDirectory</key>\n' +
        '<string>{0}/Applications/artivity-apid.app/Contents/Resources</string>\n' +
        '<key>ExitTimeOut</key>\n' +
        '<integer>60</integer>\n' +
        '</dict>\n' +
        '</plist>\n';
}


/**
 * 
 */
AgentLauncher.prototype.startAgent = function () {
    var self = this;

    var exec = require('child_process').execSync;
    exec('launchctl start ' + self.agentName, { encoding: 'utf8' });
}

AgentLauncher.prototype.stopAgent = function () {
    var self = this;

    var exec = require('child_process').execSync;
    exec('launchctl stop ' + self.agentName, { encoding: 'utf8' });
}

AgentLauncher.prototype.installAgent = function() {
    var self = this;

    var template = self.plistContent.replace(new RegExp(/\{0\}/g), self.appDirectory);
    var fs = require('fs');
    fs.writeFileSync(self.plistPath, template, {encoding: "utf8", flag: 'w'});

    if( self.getStatus() != -1 ){
        self.stopAgent();
    }

    var exec = require('child_process').execSync;
    exec('launchctl load ' + self.plistPath, { encoding: 'utf8' });
}

AgentLauncher.prototype.uninstallAgent = function() {
    var self = this;
 
    if( self.getStatus()!= -1) {
        var exec = require('child_process').execSync;
        exec('launchctl unload ' + self.plistPath, { encoding: 'utf8' });
    }
    var fs = require('fs');
    fs.unlinkSync(self.plistPath);
    
}

/**
 * This method returns the code of the agents status:
 * -1: The LaunchAgent plist file does not exist
 * 0: The LaunchAgent is running
 * 78: The LaunchAgent plist file does exist but the target is missing
 */
AgentLauncher.prototype.getStatus = function () {
    var self = this;
    var exec = require('child_process').execSync;
    var res = exec('launchctl list | grep \'' + self.agentName + '$\' | awk \'{print $2}\'', { encoding: 'utf8' });
    if (res == "")
        return -1;
    res = res.replace(/\n$/, '')

    return res;
}


/**
 * Tests if the agent is already running.
 */
AgentLauncher.prototype.isRunning = function () {
    var self = this;

    var res = self.getStatus();
    if (res == 0) {
        return true;
    }
    else { 
        return false; 
    }
}


AgentLauncher.prototype.checkLocalAgent = function () {
    var self = this;

    var fs = require(fs);
    if(!fs.existsSync(plistPath)){
        installAndStart();
    }

}

module.exports = AgentLauncher;