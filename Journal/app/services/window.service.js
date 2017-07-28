(function () {
    angular.module('app').factory('windowService', windowService);

    function windowService() {
        const remote = require('electron').remote;
        const window = require('electron').remote.getCurrentWindow();
        const dialog = require('electron').remote.dialog;
        const shell = require('electron').shell;

        initialize();

        return {
            currentWindow: currentWindow,
            openWindow: openWindow,
            openExternalLink: openExternalLink,
            close: close,
            reload: reload,
            setTitle: setTitle,
            setClosable: setClosable,
            setMinimizable: setMinimizable,
            setMaximizable: setMaximizable,
            setWidth: setWidth,
            selectFolder: selectFolder
        }

        function initialize() {
            // Open external links with the system default browser.
            $(document).on('click', 'a[target="_blank"]', function (event) {
                event.preventDefault();
                openExternalLink(this.href);
            });
        }

        function close() {
            remote.getCurrentWindow().close();
        }

        function setTitle(value) {
            var title = $(document).find('.art-window-titlebar .window-title');

            if (title && title.length) {
                title.text(value);
            }
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
                    icon: __dirname + '/app/resources/img/icon.ico',
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

            /* TODO: The main window is loaded manually in main.js which does not have angular loaded.
            // On Windows, the window content gets clipped with a 5px border when
            // maximized. In order to compensate for this, we add the necessary
            // CSS classes to the window when its state changes.
            if (process.platform === 'win32') {
                $('#window').addClass('win');

                w.on('maximize', function () {
                    $('#window').addClass('maximized');
                });

                w.on('unmaximize', function () {
                    $('#window').removeClass('maximized');
                });
            } else if (process.platform === 'darwin') {
                $('#window').addClass('mac');
            }
            */

            return w;
        }

        function currentWindow() {
            return window;
        }

        function reload() {
            window.reload();
        }

        function selectFolder() {
            var result = dialog.showOpenDialog({
                properties: ['openDirectory']
            });

            if (result && result.length > 0) {
                return result[0];
            }
        }

        function openExternalLink(href) {
            shell.openExternal(href);
        }
    }
})();