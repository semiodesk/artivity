(function () {
    angular.module('explorerApp').factory('windowService', windowService);

    function windowService() {
        const window = require('electron').remote.getCurrentWindow();

        return {
            setClosable: setClosable,
            setMinimizable: setMinimizable,
            setMaximizable: setMaximizable,
            setWidth: setWidth
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
    }
})();