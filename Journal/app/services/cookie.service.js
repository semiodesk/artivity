(function () {
    angular.module('app').factory('cookieService', cookieService);

    cookieService.$inject = ['$http', '$cookies'];

    function cookieService($http, $cookies) {
        const settings = require('electron-settings');

        return {
            get: get,
            set: set
        }

        function set(key, value) {
            if (key && value !== undefined) {
                settings.set(key, value);
            }
        }

        function get(key, defaultValue) {
            if (key && settings.has(key)) {
                return settings.get(key);
            } else if(key) {
                return defaultValue;
            }
        }
    }
})();