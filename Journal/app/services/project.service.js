﻿(function () {
    angular.module('app').factory('projectService', projectService);

    projectService.$inject = ['$http'];

    function projectService($http) {
        var endpoint = apid.endpointUrl + "projects";

        var service = {};
        service.selectedProject = null;
        service.projects = null;
        service.create = create;
        service.update = update;
        service.remove = remove;
        service.get = get;
        service.getAll = getAll;
        service.getFiles = getFiles;
        service.getFolders = getFolders;
        service.getMembers = getMembers;
        service.addFile = addFile;
        service.addFolder = addFolder;
        service.addMember = addMember;
        service.removeFile = removeFile;
        service.removeFolder = removeFolder;
        service.removeMember = removeMember;

        return service;

        function get(projectUri) {
            return $http.get(endpoint + '?uri=' + encodeURIComponent(uri)).then(handleSuccess, handleError('Error when getting projects by id.'));
        }

        function getAll() {
            if (service.projects != null) {
                return new Promise(function (resolve, reject) {
                    resolve(service.projects);
                });
            }

            return new Promise(function (resolve, reject) {
                $http.get(endpoint).then(function (result) {
                        var response = result.data;

                        if (response && response.success) {
                            service.projects = response.data;

                            resolve(service.projects);
                        }
                    },
                    handleError(getAll));
            });
        }

        function create() {
            return new Promise(function (resolve, reject) {
                $http.get(endpoint + '/new').then(function (result) {
                    resolve(result.data);
                }, handleError(create));
            });
        }

        function update(project) {
            var uri = encodeURIComponent(project.Uri);

            return $http.put(endpoint + '?uri=' + uri, project).then(function (response) {
                service.projects = null;
                return response.data;
            }, handleError(update));
        }

        function remove(projectUri) {
            var uri = encodeURIComponent(projectUri);

            return $http.delete(endpoint + '?uri=' + uri).then(function (response) {
                service.projects = null;
                return response.data;
            }, handleError(remove));
        }

        function getFiles(projectUri) {
            var uri = encodeURIComponent(projectUri);

            return $http.post(endpoint + '/files/?projectUri=' + uri).then(handleSuccess, handleError(getFiles));
        }

        function addFile(projectUri, fileUri) {
            var uri = encodeURIComponent(projectUri);

            return $http.post(endpoint + '/files/?projectUri=' + uri + "&fileUri=" + encodeURIComponent(fileUri)).then(handleSuccess, handleError(addFile));
        }

        function removeFile(projectUri, fileUri) {
            var uri = encodeURIComponent(projectUri);

            return $http.delete(endpoint + '/files/?projectUri=' + uri + "&fileUri=" + encodeURIComponent(fileUri)).then(handleSuccess, handleError(removeFile));
        }

        function getFolders(projectUri) {
            var uri = encodeURIComponent(projectUri);

            return $http.get(endpoint + '/folders/?projectUri=' + uri).then(handleSuccess, handleError(getFolders));
        }

        function addFolder(projectUri, folderUrl) {
            var uri = encodeURIComponent(projectUri);

            return $http.post(endpoint + '/folders/?projectUri=' + uri + "&folderUrl=" + encodeURIComponent(folderUrl)).then(handleSuccess, handleError(addFolder));
        }

        function removeFolder(projectUri, folderUrl) {
            var uri = encodeURIComponent(projectUri);

            return $http.delete(endpoint + '/folders/?projectUri=' + uri + "&folderUrl=" + encodeURIComponent(folderUrl)).then(handleSuccess, handleError(removeFolder));
        }

        function getMembers(projectUri) {
            return $http.get(endpoint + '/agents?projectUri=' + projectUri).then(handleSuccess, handleError(getMembers));
        }

        function addMember(projectUri, agentUri) {
            return $http.post(endpoint + '/agents?projectUri=' + projectUri + '&agentUri=' + agentUri + '&role=ProjectMember').then(handleSuccess, handleError(addMember));
        }

        function removeMember(projectUri, agentUri) {
            return $http.delete(endpoint + '/agents?projectUri=' + projectUri + '&agentUri=' + agentUri).then(handleSuccess, handleError(removeMember));
        }

        // private functions
        function handleSuccess(response) {
            return response.data;
        }

        function handleError(context) {
            return function () {
                return {
                    success: false,
                    message: context.toString()
                };
            };
        }
    }
})();