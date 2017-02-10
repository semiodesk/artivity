(function () {
    angular.module('explorerApp').controller('FileViewSimpleController', FileViewSimpleController);

    function FileViewSimpleController(api, $rootScope, $scope, $location, $routeParams, $uibModal, selectionService, hotkeys) {
        var t = this;
        var fileUri = $location.search().uri;

        t.publishFile = publishFile;
        t.exportFIle = exportFile;

        t.entity = {
            uri: fileUri
        };

        t.file = {};
        t.agent = {
            iconUrl: ''
        };
        t.user = {};

        initialize();

        function initialize() {

            // File metadata
            api.getFile(fileUri).then(function (data) {
                t.file = data;

                console.log("Entity: ", t.file);
            });

            // Agent metadata
            api.getAgent(fileUri).then(function (data) {
                t.agent = data;
                t.agent.iconUrl = api.getAgentIconUrl(data.agent);

                console.log("Agent: ", t.agent);
            });

            // Load the user data.


            api.getUser().then(function (data) {
                t.user = data;
                t.user.photoUrl = api.getUserPhotoUrl();

                console.log("User: ", t.user);
            });
        }

        // EXPORT
        function exportFile() {
            api.exportFile(fileUri, t.file.label);
        };

        // SHARING
        function publishFile() {
            var influence = selectionService.selectedItem();

            selectionService.mute();
            selectionService.selectedItem({
                uri: t.entity.uri,
                label: t.file.label,
                agentColor: t.agent.color
            });

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'app/partials/dialogs/publish-file-dialog.html',
                controller: 'PublishFileDialogController',
                controllerAs: 't',
                scope: $scope
            }).closed.then(function () {
                selectionService.selectedItem(influence);
                selectionService.unmute();
            });
        }
    }
})();