angular.module('explorerApp').controller('FileListController', FileListController);

function FileListController(api, $scope, $uibModal, fileService, hotkeys) {
    var t = this;
    var s = $scope;

    // USER INFO
    s.user = {};
    s.userPhotoUrl = api.getUserPhotoUrl()+"?q="+Date.now();

    api.getUser().then(function (data) {
        s.user = data;
    });

    s.activities = [];

    // RECENTLY USED FILES
    s.files = [];
    s.hasFiles = false;

    s.loadRecentFiles = function () {
        api.getRecentFiles().then(function (data) {
            for (var i = 0; i < data.length; i++) {
                var file = data[i];

                if (i < s.files.length && s.files[i].uri == file.uri) {
                    break;
                }

                s.files.splice(i, 0, file);
            }

            s.hasFiles = data.length > 0;
        });
    };

    s.getFileName = fileService.getFileName;
    s.getFileNameWithoutExtension = fileService.getFileNameWithoutExtension;
    s.getFileExtension = fileService.getFileExtension;
    s.hasFileThumbnail = api.hasThumbnail;
    s.getFileThumbnailUrl = api.getThumbnailUrl;

    s.loadRecentFiles();

    // CALENDAR
    s.calendar = null;

    s.toggleCalendar = function () {
        if (s.calendar) {
            s.calendar.close();

            s.calendar = null;
        } else {
            s.calendar = $uibModal.open({
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
            s.toggleCalendar();
        }
    });

    // REFRESH
    window.addEventListener("focus", function (e) {
        // Redraw the scene to prevent blank scenes when switching windows.		
        if (!document.hidden) {
            console.log("Reloading..");

            s.loadRecentFiles();
        }
    });
}