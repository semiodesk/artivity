(function () {
    angular.module('app').factory('agentService', agentService);

    agentService.$inject = ['api', 'authenticationService'];

    function agentService(api, authenticationService) {
        var endpoint = apid.endpointUrl + "agents";

        var t = {
            initialize: initialize,
            currentUser: null,
            newPerson: newPerson,
            getPerson: getPerson,
            putPerson: putPerson,
            putPhoto: putPhoto,
            getPhotoUrl: getPhotoUrl,
            getAccountOwner: getAccountOwner,
            findPersons: findPersons
        };

        var dispatcher = new EventDispatcher(t);

        authenticationService.getAuthenticatedUser(function (user) {
            t.currentUser = user;

            dispatcher.raise('currentUserChanged', user);
        });

        return t;

        function initialize(success, error) {
            authenticationService.getAuthenticatedUser(function (user) {
                t.currentUser = user;

                dispatcher.raise('currentUserChanged', user);

                if (user) {
                    if (typeof (success) === 'function') {
                        success(user);
                    }
                } else {
                    if (typeof (error) === 'function') {
                        error();
                    }
                }
            });
        }

        function getAccountOwner() {
            return api.get(endpoint + '/users?role=AccountOwnerRole').then(
                function (response) {
                    if (response && response.data.length === 1) {
                        return response.data[0];
                    }
                });
        }

        function getPerson(agentUri) {
            var uri = encodeURIComponent(agentUri);

            return api.get(endpoint + '/users?agentUri=' + uri).then(
                function (response) {
                    console.log(response);

                    if (response && response.data) {
                        return response.data;
                    }
                });
        }

        function newPerson() {
            return api.get(endpoint + '/users/new').then(
                function (response) {
                    if (response && response.data) {
                        return response.data;
                    }
                });
        }

        function putPerson(person) {
            return api.put(endpoint + '/users/', person).then(
                function (response) {
                    if (response && response.data.length === 1) {
                        return response.data[0];
                    }
                });
        }

        function putPhoto(agentUri, data) {
            var uri = encodeURIComponent(agentUri);

            return api.put(endpoint + '/users/photo?agentUri=' + uri, data).then(
                function (response) {
                    return response;
                });
        }

        function getPhotoUrl(agentUri) {
            var uri = encodeURIComponent(agentUri);

            return endpoint + '/users/photo?agentUri=' + uri;
        }

        function findPersons(query) {
            return api.get(endpoint + '/users?q=' + query).then(
                function (response) {
                    return response.data;
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