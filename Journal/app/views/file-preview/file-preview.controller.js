(function () {
    angular.module('app').controller('FilePreviewController', FilePreviewController);

    FilePreviewController.$inject = ['$rootScope', '$scope', '$location', '$routeParams', '$uibModal', 'api', 'selectionService', 'hotkeys'];
    
    function FilePreviewController($rootScope, $scope, $location, $routeParams, $uibModal, api, selectionService, hotkeys) {
        var t = this;
        var fileUri = $location.search().uri;

        t.user = {};
        t.agent = {
            iconUrl: ''
        };
        t.entity = {
            uri: fileUri
        };
        t.file = {};

        initialize();

        function initialize() {
            // File metadata
            api.getFile(fileUri).then(function (data) {
                t.file = data;

                console.log("Loaded entity: ", t.file);
            });

            // Agent metadata
            api.getAgent(fileUri).then(function (data) {
                t.agent = data;
                t.agent.iconUrl = api.getAgentIconUrl(data.agent);

                console.log("Loaded software agent: ", t.agent);
            });

            // Load the user data.
            api.getUser().then(function (data) {
                t.user = data;
                t.user.photoUrl = api.getUserPhotoUrl();

                console.log("Loaded user agent: ", t.user);
            });
        }
    }
})();