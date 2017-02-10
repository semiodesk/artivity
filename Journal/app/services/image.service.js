(function () {
    'use strict';

    angular.module('explorerApp').factory('imageService', imageService);

    imageService.$inject = ['$http'];

    function imageService($http) {
        var endpoint = apid.endpointUrl + "images";

        var service = {};
        service.getAll = getAll;
        service.getById = getById;
        service.create = create;
        service.update = update;
        service.remove = remove;
        service.getRenderingUrl = getRenderingUrl;
        
        service.currentImage = null;

        return service;

        function getAll() {
            return new Promise(function (resolve, reject) {
                $http.get(endpoint).then(function (result) {
                    var data = result.data;
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

        function update(fileUri) {
            var uri = encodeURIComponent(fileUri.Uri);

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

        function getRenderingUrl(fileUri) {
            var uri = encodeURIComponent(fileUri);

            return endpoint + '/rendering?uri=' + uri;
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