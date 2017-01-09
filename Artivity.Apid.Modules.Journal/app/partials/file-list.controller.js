angular.module('explorerApp').controller('FileListController', FileListController);

function FileListController(api, $scope, $uibModal, fileService, windowService, hotkeys) {
    var t = this;

    windowService.setMinimizable();
    windowService.setMaximizable();

    // USER INFO
    t.user = {};
    t.userPhotoUrl = api.getUserPhotoUrl() + "?q=" + Date.now();

    api.getUser().then(function (data) {
        t.user = data;
    });

    t.activities = [];

    // RECENTLY USED FILES
    t.files = [];
    t.hasFiles = false;

    t.loadRecentFiles = function () {
        api.getRecentFiles().then(function (data) {
            for (var i = 0; i < data.length; i++) {
                var file = data[i];

                if (i < t.files.length && t.files[i].uri == file.uri) {
                    break;
                }

                t.files.splice(i, 0, file);
            }

            t.hasFiles = data.length > 0;
        });
    };

    t.getFileName = fileService.getFileName;
    t.getFileNameWithoutExtension = fileService.getFileNameWithoutExtension;
    t.getFileExtension = fileService.getFileExtension;
    t.hasFileThumbnail = api.hasThumbnail;
    t.getFileThumbnailUrl = api.getThumbnailUrl;

    t.loadRecentFiles();

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
        combo: 'alt+c',
        description: 'Open the calendar view.',
        callback: function () {
            t.toggleCalendar();
        }
    });

    // REFRESH
    window.addEventListener("focus", function (e) {
        // Redraw the scene to prevent blank scenes when switching windows.		
        if (!document.hidden) {
            t.loadRecentFiles();
        }
    });
}