(function () {
    angular.module('app').controller('FileListController', FileListController);

    FileListController.$inject = ['$scope', '$uibModal', 'api', 'agentService', 'filesystemService', 'windowService', 'hotkeys'];

    function FileListController($scope, $uibModal, api, agentService, filesystemService, windowService, hotkeys) {
        var t = this;

        windowService.setMinimizable();
        windowService.setMaximizable();

        // USER INFO
        t.user = {};

        agentService.getAccountOwner().then(function (data) {
            t.user = data;

            // Note: For some reason, binding does not work reliably with the user data and image source.
            $('#userName').text(data.Name);
            $('#userPhoto').attr('src', api.getUserPhotoUrl(data.Uri));
        });

        t.activities = [];

        t.getFileName = filesystemService.getFileName;
        t.getFileNameWithoutExtension = filesystemService.getFileNameWithoutExtension;
        t.getFileExtension = filesystemService.getFileExtension;
        t.hasFileThumbnail = api.hasThumbnail;
        t.getFileThumbnailUrl = api.getThumbnailUrl;

        // CALENDAR
        t.calendar = null;

        t.toggleCalendar = function () {
            if (t.calendar) {
                t.calendar.close();

                t.calendar = null;
            } else {
                t.calendar = $uibModal.open({
                    animation: true,
                    templateUrl: 'app/dialogs/calendar-dialog/calendar-dialog.html',
                    controller: 'CalendarDialogController',
                    windowClass: 'modal-window-lg',
                    scope: $scope
                });
            }
        };

        hotkeys.add({
            combo: 'f9',
            description: 'Open the calendar view.',
            callback: function () {
                t.toggleCalendar();
            }
        });

        // REFRESH
        window.addEventListener("focus", function (e) {
            // Sorry i broke this ~ Mo
            // TODO: Is it enough to refresh data in service? Check how to refresh a directive
            // Redraw the scene to prevent blank scenes when switching windows.		
            //if (!document.hidden) {
            //    t.loadRecentFiles();
            //}
        });
    }
})();