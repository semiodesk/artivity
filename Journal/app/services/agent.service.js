(function () {
    angular.module('app').factory('agentService', agentService);

    agentService.$inject = ['$http'];

    function agentService($http) {
        var endpoint = apid.endpointUrl + "agents";

        return {
            newPerson: newPerson,
            putPerson: putPerson,
            putPhoto: putPhoto,
            getAccountOwner: getAccountOwner
        };

        function getAccountOwner() {
            return $http.get(endpoint + '/users?role=AccountOwner').then(
                function (response) {
                    if (response && response.data.length === 1) {
                        return response.data[0];
                    }
                });
        }

        function newPerson() {
            return $http.get(endpoint + '/users/new').then(
                function (response) {
                    if (response && response.data) {
                        return response.data;
                    }
                });
        }

        function putPerson(person) {
            return $http.put(endpoint + '/users/', person).then(
                function (response) {
                    if (response && response.data.length === 1) {
                        return response.data[0];
                    }
                });
        }

        function putPhoto(agentUri, data) {
            return $http.put(endpoint + '/users/photo?agentUri=' + agentUri, data).then(
                function (response) {
                    return response;
                });
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