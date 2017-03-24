(function () {
    angular.module('app').factory('agentService', agentService);

    agentService.$inject = ['$http'];

    function agentService($http) {
        var endpoint = apid.endpointUrl + "agents";

        return {
            getUser: getUser
        };

        function getUser() {
            return $http.get(endpoint + '/users').then(function (response) {
                return response.data;
            }, handleError('Error while retrieving user agent.'));
        }

        function handleError(error) {
            return function () {
                return {
                    success: false,
                    message: error
                };
            };
        }
    }
})();