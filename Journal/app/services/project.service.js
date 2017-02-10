(function () {
    'use strict';

    angular.module('app').factory('projectService', projectService);

    projectService.$inject = ['$http'];

    function projectService($http) {
        var endpoint = apid.endpointUrl + "projects";

        var service = {};
        service.getAll = getAll;
        service.getById = getById;
        service.create = create;
        service.update = update;
        service.remove = remove;
        service.addFileToProject = addFileToProject;
        service.selectedProject = null;
        service.projects = null;

        return service;

        function getAll() {
            if (service.projects != null) {
                return new Promise(function (resolve, reject) {
                    resolve(service.projects);
                });
            }

            return new Promise(function (resolve, reject) {
                $http.get(endpoint).then(function (result) {
                    var data = result.data;

                    data.sort(function compare(a, b) {
                        if (a.Title < b.Title) return -1;
                        if (a.Title > b.Title) return 1;
                        return 0;
                    });

                    service.projects = data;
                    resolve(data);
                }, handleError('Error when getting all projects.'));
            });
        }

        function getById(uri) {
            return $http.get(endpoint + '?uri=' + encodeURIComponent(uri)).then(handleSuccess, handleError('Error when getting projects by id.'));
        }

        function create() {
            return new Promise(function (resolve, reject) {
                $http.get(endpoint + '/new').then(function (result) {
                    resolve(result.data);
                }, handleError('Error when creating new project.'));
            });
        }

        function update(project) {
            var uri = encodeURIComponent(project.Uri);

            return $http.put(endpoint + '?uri=' + uri, project).then(function (response) {
                service.projects = null;
                return response.data;
            }, handleError('Error when updating project.'));
        }

        function remove(projectUri) {
            var uri = encodeURIComponent(projectUri);

            return $http.delete(endpoint + '?uri=' + uri).then(function (response) {
                service.projects = null;
                return response.data;
            }, handleError('Error when deleting project.'));
        }

        function addFileToProject(projectUri, fileUri) {
            var uri = encodeURIComponent(projectUri);

            return $http.get(endpoint + '/addFileToProject?projectUri=' + uri + "&fileUri=" + encodeURIComponent(fileUri)).then(handleSuccess, handleError('Error when adding file to project.'));
        }

        // private functions
        function handleSuccess(res) {
            return res.data;
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