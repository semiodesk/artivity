(function () {
    angular.module('explorerApp').controller('FileListController', FileListController);

    function FileListController(api, $scope, $uibModal, fileService, windowService, hotkeys) {
        var t = this;

        windowService.setMinimizable();
        windowService.setMaximizable();

        // USER INFO
        t.user = {};

        api.getUser().then(function (data) {
            t.user = data;

            // Note: For some reason, binding does not work reliably with the user data and image source.
            $('#userName').text(data.Name);
            $('#userPhoto').attr('src', api.getUserPhotoUrl());
        });

        t.activities = [];

        t.getFileName = fileService.getFileName;
        t.getFileNameWithoutExtension = fileService.getFileNameWithoutExtension;
        t.getFileExtension = fileService.getFileExtension;
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
                    templateUrl: 'partials/dialogs/calendar-dialog.html',
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