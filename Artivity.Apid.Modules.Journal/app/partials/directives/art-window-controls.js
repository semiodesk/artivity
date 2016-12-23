var app = angular.module('explorerApp');

app.directive('artWindowControls', WindowControlsDirective);

function WindowControlsDirective() {
    return {
        templateUrl: 'partials/directives/art-window-controls.html',
        controller: WindowControlsDirectiveController,
        controllerAs: 't',
        link: function (scope, element, attributes, ctrl) {
            var controls = $('.window-title-btns');

            if(process.platform === 'win32') {
                controls.addClass('win');
            } else if(process.platform === 'darwin') {
                controls.addClass('mac');
            }
        }
    }
}

app.controller('WindowControlsDirectiveController', WindowControlsDirectiveController);

function WindowControlsDirectiveController($scope, windowService) {
    var t = this;
}