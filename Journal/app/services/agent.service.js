(function () {
    angular.module('app').factory('agentService', agentService);

    agentService.$inject = ['api', 'authenticationService'];

    function agentService(api, authenticationService) {
        var endpoint = apid.endpointUrl + "agents";
        var currentUser = null;

        var t = {
            newPerson: newPerson,
            getPerson: getPerson,
            putPerson: putPerson,
            putPhoto: putPhoto,
            getPhotoUrl: getPhotoUrl,
            getCurrentUser: getCurrentUser,
            getAccountOwner: getAccountOwner,
            findPersons: findPersons,
            software: [],
            getSoftwareAgents: getSoftwareAgents,
            getActiveSoftwareAgents: getActiveSoftwareAgents,
            getAccounts: getAccounts,
            getArtivityAccount: getArtivityAccount
        };

        var dispatcher = new EventDispatcher(t);

        return t;

        function getCurrentUser(reload) {
            return new Promise(function (resolve, reject) {
                if (currentUser && !reload) {
                    resolve(currentUser);
                } else {
                    authenticationService.getAuthenticatedUser(function (user) {
                        currentUser = user;

                        dispatcher.raise('currentUserChanged', user);

                        if (user) {
                            resolve(currentUser);
                        } else {
                            reject(null);
                        }
                    });
                }
            });
        }

        function getAccounts() {
            return new Promise(function (resolve, reject) {
                api.getAccounts().then(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var account = data[i];

                        if (account) {
                            resolve(account);
                        }
                    }
                });
            });
        }

        function getArtivityAccount() {
            return new Promise(function (resolve, reject) {
                api.getAccounts().then(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var account = data[i];

                        if (account && account.ServiceClient.Uri.startsWith('http://artivity.online')) {
                            resolve(account);
                        }

                        return;
                    }

                    reject();
                });
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
            var url = endpoint + '/users/photo?agentUri=' + uri;

            if (t.currentUser && t.currentUser.UserName) {
                url += '&user=' + t.currentUser.UserName;
            }

            return url;
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

        function getSoftwareAgents() {
            api.getAgents().then(function (data) {
                var software = [];

                for (var i = 0; i < data.length; i++) {
                    var plugin = data[i];

                    var agent = {
                        uri: plugin.Manifest.AgentUri,
                        name: plugin.Manifest.DisplayName,
                        iconSrc: api.getAgentIconUrl(plugin.Manifest.AgentUri),
                        softwareInstalled: plugin.IsSoftwareInstalled,
                        pluginInstalled: plugin.IsPluginInstalled,
                        pluginEnabled: plugin.IsPluginEnabled,
                        autoInstall: plugin.Manifest.AutoInstall,
                        hasError: false
                    };

                    software.push(agent);
                }

                software.sort(function (a, b) {
                    return a.name.localeCompare(b.name);
                });

                t.software = software;
            });
        }

        function getActiveSoftwareAgents() {
            var result = [];

            for (var i = 0; i < t.software.length; i++) {
                var software = t.software[i];

                if (software.softwareInstalled && software.autoInstall) {
                    result.push(software);
                }
            }

            return result;
        }
    }
})();