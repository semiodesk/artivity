(function () {
    angular.module('app').controller('FilePreviewController', FilePreviewController);

    FilePreviewController.$inject = ['$rootScope', '$scope', '$location', '$routeParams', '$uibModal', 'api', 'agentService', 'selectionService', 'hotkeys'];

    function FilePreviewController($rootScope, $scope, $location, $routeParams, $uibModal, api, agentService, selectionService, hotkeys) {
        var t = this;
        var fileUri = $location.search().uri;

        t.user = {};
        t.agent = {
            iconUrl: ''
        };
        t.entity = {};
        t.file = {};
        t.fileUri = fileUri;

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
            agentService.getAccountOwner().then(function (data) {
                t.user = data;
                t.user.photoUrl = api.getUserPhotoUrl();

                console.log("Loaded user agent: ", t.user);
            });

            // Make the left and right panes resizable.
            var content = $('.ui-content');
            var sidebar = $('.ui-sidebar-right');

            sidebar.resizable({
                handles: 'w'
            });

            sidebar.resize(function(event, ui) {
                var width = ui.size.width + 'px';

                sidebar.css('left', 'auto');
                sidebar.css('right', 0);
                sidebar.css('width', width);
                content.css('right', width);

                $rootScope.$broadcast('resize');
            });

            content.css('right', sidebar.outerWidth() + 'px');
        }
    }
})();