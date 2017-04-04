(function () {
    angular.module('app').factory('agentService', agentService);

    agentService.$inject = ['api'];

    function agentService(api) {
        var endpoint = apid.endpointUrl + "agents";

        var t = {
            initialized: null,
            currentUser: null,
            newPerson: newPerson,
            getPerson: getPerson,
            putPerson: putPerson,
            putPhoto: putPhoto,
            getPhotoUrl: getPhotoUrl,
            getAccountOwner: getAccountOwner,
            findPersons: findPersons
        };

        t.initialized = init();

        return t;

        function init() {
            return new Promise(function (resolve, reject) {
                if (t.currentUser) {
                    resolve();
                } else {
                    getAccountOwner().then(function (user) {
                        if (user && user.Uri) {
                            t.currentUser = user;
                            t.currentUser.PhotoUrl = api.getUserPhotoUrl(user.Uri);

                            resolve();
                        } else {
                            reject();
                        }
                    });
                }
            });
        }

        function getAccountOwner() {
            return api.get(endpoint + '/users?role=AccountOwner').then(
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