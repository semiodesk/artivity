(function () {
    'use strict';

    angular.module('app').factory('authenticationService', authenticationService);

    authenticationService.$inject = ['$rootScope', 'api'];

    function authenticationService($rootScope, api) {
        var endpoint = apid.endpointUrl + 'agents';

        return {
            getAuthenticatedUser: getAuthenticatedUser
        };

        function getAuthenticatedUser(result) {
            if (typeof (result) !== 'function') {
                return;
            }

            api.get(endpoint + '/users?role=AccountOwnerRole').then(function (response) {
                if (response && response.data.length === 1) {
                    var user = response.data[0];

                    if (user && user.Uri) {
                        user.PhotoUrl = api.getUserPhotoUrl(user.Uri);

                        result(user);
                    } else {
                        result(null);
                    }
                }
            }, function() {
                result(null);
            });
        }
    }
})();