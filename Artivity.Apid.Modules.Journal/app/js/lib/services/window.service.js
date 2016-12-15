angular.module('explorerApp').factory('windowService', function () {
    const window = require('electron').remote.getCurrentWindow();
    
    return {
        setMinimizable: function(value) {
            if(value || value === undefined) {
                $('#btn-min').show();
            } else {
                $('#btn-min').hide();
            }     
        },
        setMaximizable: function(value) {
            if(value || value === undefined) {
                $('#btn-max').show();
            } else {
                $('#btn-max').hide();
            }
        },
        setWidth: function(width) {
            var bounds = window.getBounds();

            window.setSize(width, bounds.height, true);
        }
    }
});