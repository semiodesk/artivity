(function () {
    angular.module('explorerApp').directive('artWindowControls', WindowControlsDirective);

    function WindowControlsDirective() {
        return {
            templateUrl: 'app/partials/directives/art-window-controls.html',
            controller: WindowControlsDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attributes, ctrl) {
                ctrl.init();
            }
        }
    }

    angular.module('explorerApp').controller('WindowControlsDirectiveController', WindowControlsDirectiveController);

    function WindowControlsDirectiveController($scope, windowService) {
        var t = this;

        t.init = function () {
            t.remote = require('electron').remote;

            var controls = $('.window-control-btns');
            var btns = controls.find('.btn-title');

            if (process.platform === 'win32') {
                controls.addClass('win');

                if (btns.length === 3) {
                    $(btns[0]).addClass('btn-min');
                    $(btns[1]).addClass('btn-max');
                    $(btns[2]).addClass('btn-close');
                }
            } else if (process.platform === 'darwin') {
                controls.addClass('mac');

                if (btns.length === 3) {
                    $(btns[0]).addClass('btn-close');
                    $(btns[1]).addClass('btn-min');
                    $(btns[2]).addClass('btn-max');
                }
            }

            controls.find('.btn-min').click(t.minimizeWindow);
            controls.find('.btn-max').click(t.maximizeWindow);
            controls.find('.btn-close').click(t.closeWindow);
        }

        t.minimizeWindow = function (e) {
            const window = t.remote.getCurrentWindow();

            window.minimize();
        }

        t.maximizeWindow = function (e) {
            const window = t.remote.getCurrentWindow();

            // TODO: Change the button state via CSS class to OS independent.
            // Requires refactoring the window controls in index.html.
            if (!window.isMaximized()) {
                window.maximize();

                $('.window-title-btns .btn-max').addClass("maximized");
            } else {
                window.unmaximize();

                $('.window-title-btns .btn-max').removeClass("maximized");
            }
        }

        t.closeWindow = function (e) {
            windowService.close();
        }
    }
})();