(function () {
    angular.module('app').factory('projectService', projectService);

    projectService.$inject = ['api'];

    function projectService(api) {
        var endpoint = apid.endpointUrl + "projects";

        var t = {
            projects: null,
            currentProject: null,
            create: create,
            update: update,
            remove: remove,
            get: get,
            getAll: getAll,
            getFiles: getFiles,
            getFolders: getFolders,
            getMembers: getMembers,
            findMembers: findMembers,
            addFile: addFile,
            addFolder: addFolder,
            addMember: addMember,
            removeFile: removeFile,
            removeFolder: removeFolder,
            removeMember: removeMember,
        };

        return t;

        // PROJECTS
        function get(projectUri) {
            return api.get(endpoint + '?uri=' + encodeURIComponent(uri)).then(handleSuccess, handleError('Error when getting projects by id.'));
        }

        function getAll() {
            if (t.projects != null) {
                return new Promise(function (resolve, reject) {
                    resolve(t.projects);
                });
            }

            return new Promise(function (resolve, reject) {
                api.get(endpoint).then(function (result) {
                        var response = result.data;

                        if (response && response.success) {
                            t.projects = response.data;

                            resolve(t.projects);
                        }
                    },
                    handleError(getAll));
            });
        }

        function create() {
            return new Promise(function (resolve, reject) {
                api.get(endpoint + '/new').then(function (result) {
                    resolve(result.data);
                }, handleError(create));
            });
        }

        function update(project) {
            return api.put(endpoint + '?uri=' + project.Uri, project).then(function (response) {
                t.projects = null;
                return response.data;
            }, handleError(update));
        }

        function remove(projectUri) {
            return api.delete(endpoint + '?uri=' + projectUri).then(function (response) {
                t.projects = null;
                return response.data;
            }, handleError(remove));
        }

        function getFiles(projectUri) {
            return api.get(endpoint + '/files/?projectUri=' + projectUri).then(handleSuccess, handleError(getFiles));
        }

        // FILES
        function addFile(projectUri, fileUri) {
            var uri = encodeURIComponent(projectUri);

            return api.post(endpoint + '/files/?projectUri=' + uri + "&fileUri=" + encodeURIComponent(fileUri)).then(handleSuccess, handleError(addFile));
        }

        function removeFile(projectUri, fileUri) {
            var uri = encodeURIComponent(projectUri);

            return api.delete(endpoint + '/files/?projectUri=' + uri + "&fileUri=" + encodeURIComponent(fileUri)).then(handleSuccess, handleError(removeFile));
        }

        // FOLDERS
        function getFolders(projectUri) {
            var uri = encodeURIComponent(projectUri);

            return api.get(endpoint + '/folders/?projectUri=' + uri).then(handleSuccess, handleError(getFolders));
        }

        function addFolder(projectUri, folderUrl) {
            var uri = encodeURIComponent(projectUri);

            return api.post(endpoint + '/folders/?projectUri=' + uri + "&folderUrl=" + encodeURIComponent(folderUrl)).then(handleSuccess, handleError(addFolder));
        }

        function removeFolder(projectUri, folderUrl) {
            var uri = encodeURIComponent(projectUri);

            return api.delete(endpoint + '/folders/?projectUri=' + uri + "&folderUrl=" + encodeURIComponent(folderUrl)).then(handleSuccess, handleError(removeFolder));
        }

        // MEMBERS
        function getMembers(projectUri) {
            return api.get(endpoint + '/agents?projectUri=' + projectUri).then(handleSuccess, handleError(getMembers));
        }

        function addMember(projectUri, agentUri) {
            return api.post(endpoint + '/agents?projectUri=' + projectUri + '&agentUri=' + agentUri + '&role=ProjectMemberRole').then(handleSuccess, handleError(addMember));
        }

        function removeMember(projectUri, agentUri) {
            return api.delete(endpoint + '/agents?projectUri=' + projectUri + '&agentUri=' + agentUri).then(handleSuccess, handleError(removeMember));
        }

        function findMembers(projectUri, query) {
            return api.get(endpoint + '/agents?projectUri=' + projectUri + '&q=' + query).then(handleSuccess, handleError(findMembers));
        }

        // RESULT HANDLERS
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