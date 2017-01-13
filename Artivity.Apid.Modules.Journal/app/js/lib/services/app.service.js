(function () {
    angular.module('explorerApp').factory('appService', appService);

    function appService() {
        const remote = require('electron').remote;

        // The connection retry interval in ms.
        var interval = 1000;

        // The maximum time in ms the client tries to connect with the APID.
        var timeout = 30000;

        return {
            exit: exit,
            configPath: configPath,
            connectionTimeout: connectionTimeout,
            connectionInterval: connectionInterval
        };

        function exit(exitCode) {
            remote.app.exit(exitCode);
        }

        function configPath(relativePath) {
            return remote.app.getPath(relativePath);
        }

        function connectionInterval(value) {
            if(value) {
                interval = value;
            }

            return interval;
        }

        function connectionTimeout(value) {
            if(value) {
                timeout = value;
            }

            return timeout;
        }
    }
})();