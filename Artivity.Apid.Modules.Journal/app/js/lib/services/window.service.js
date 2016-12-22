(function () {
    angular.module('explorerApp').factory('windowService', windowService);

    function windowService() {
        const window = require('electron').remote.getCurrentWindow();

        return {
            setMinimizable: setMinimizable,
            setMaximizable: setMaximizable,
            setWidth: setWidth
        }

        function setMinimizable(value) {
            if (value || value === undefined) {
                window.setMinimizable(true);

                $('#btn-min').show();
            } else {
                $('#btn-min').hide();
            }
        }

        function setMaximizable(value) {
            if (value || value === undefined) {
                window.setMaximizable(true);

                $('#btn-max').show();
            } else {
                $('#btn-max').hide();
            }
        }

        function setWidth(width) {
            var bounds = window.getBounds();

            window.setSize(width, bounds.height, true);
        }
    }
})();