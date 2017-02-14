(function () {
    'use strict';

    angular.module('app').factory('syncService', syncService);

    syncService.$inject = ['api', '$http'];

    function syncService(api, $http) {
        var t = {
            // Private event dispatcher instance.
            dispatcher: new EventDispatcher()
        };

        var service = {
            endpoint: api.endpointUrl + 'sync',
            mute: mute,
            unmute: unmute,
            on: on,
            off: off,
            synchronize: synchronize
        };

        return service;

        function synchronize() {
            t.dispatcher.raise('syncBegin');

            api.synchronize().then(function() {
                t.dispatcher.raise('syncEnd');
            }, function() {
                t.dispatcher.raise('syncError');
                t.dispatcher.raise('syncEnd');
            });
        }

        function mute() {
            t.dispatcher.mute();
        }

        function unmute() {
            t.dispatcher.unmute();
        }

        function on(event, callback) {
            t.dispatcher.on(event, callback);
        }

        function off(event, callback) {
            t.dispatcher.off(event, callback);
        }
    }
})();