(function () {
    angular.module('explorerApp').factory('windowService', windowService);

    function windowService() {
        const remote = require('electron').remote;
        const window = require('electron').remote.getCurrentWindow();

        return {
            currentWindow: currentWindow,
            openWindow: openWindow,
            close: close,
            setClosable: setClosable,
            setMinimizable: setMinimizable,
            setMaximizable: setMaximizable,
            setWidth: setWidth
        }

        function close() {
            remote.getCurrentWindow().close();
        }

        function setClosable(value) {
            if (value || value === undefined) {
                window.setClosable(true);

                $('.window-title-btns .btn-close').show();
            } else {
                $('.window-title-btns .btn-close').hide();
            }
        }

        function setMinimizable(value) {
            if (value || value === undefined) {
                window.setMinimizable(true);

                $('.window-title-btns .btn-min').show();
            } else {
                $('.window-title-btns .btn-min').hide();
            }
        }

        function setMaximizable(value) {
            if (value || value === undefined) {
                window.setMaximizable(true);

                $('.window-title-btns .btn-max').show();
            } else {
                $('.window-title-btns .btn-max').hide();
            }
        }

        function setWidth(width) {
            var bounds = window.getBounds();

            window.setSize(width, bounds.height, true);
        }

        function openWindow(url, options) {
            if (options === undefined) {
                options = {
                    frame: false,
                    show: false,
                    width: 800,
                    minWidth: 800,
                    height: 600,
                    minHeight: 600,
                    icon: __dirname + '/img/icon.ico',
                    backgroundColor: '#1D1D1D'
                };
            }

            const BrowserWindow = remote.BrowserWindow;

            var w = new BrowserWindow(options);
            w.setMenuBarVisibility(false);
            w.loadURL(url);
            w.webContents.on('did-finish-load', function () {
                w.show();
                w.focus();
            });

            return w;
        }

        function currentWindow() {
            return window;
        }
    }
})();