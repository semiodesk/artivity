(function () {
    angular.module('explorerApp').directive('artKeyboardInputHandler', KeyboardInputHandlerDirective);

    function KeyboardInputHandlerDirective() {
        return {
            scope: true,
            template: '',
            controller: KeyboardInputHandlerDirectiveController
        }
    };

    function KeyboardInputHandlerDirectiveController(api, $scope, hotkeys) {
        $scope.navigateTo = function (path) {
            var url = window.location.href.split('#');

            if (url.length < 2) {
                console.log('Navigation failed; unable to parse fragment from url:' + window.location.href);

                return;
            }

            window.location.href = url[0].replace(/index.html/i, '') + path;
        };

        $scope.navigateToFragment = function (pathFragment) {
            var url = window.location.href.split('#');

            if (url.length < 2) {
                console.log('Navigation failed; unable to parse fragment from url:' + window.location.href);

                return;
            }

            window.location.href = url[0] + '#' + pathFragment;
        };

        hotkeys.add({
            combo: 'backspace',
            description: 'Go back to the previous view.',
            callback: function () {
                window.history.back();
            }
        });

        hotkeys.add({
            combo: 'shift+backspace',
            description: 'Go forward to the next view.',
            callback: function () {
                window.history.forward();
            }
        });

        hotkeys.add({
            combo: 'alt+h',
            description: 'Go to the dashboard view.',
            callback: function () {
                // This will be replaced with the correct home page by the route provider.
                $scope.navigateToFragment('/');
            }
        });

        hotkeys.add({
            combo: 'alt+q',
            description: 'Open the SPARQL query editor view.',
            callback: function () {
                $scope.navigateToFragment('/query');
            }
        });

        hotkeys.add({
            combo: 'alt+i',
            description: 'Opens the development tools.',
            callback: function () {
                var remote = require('electron').remote;
                var BrowserWindow = remote.getCurrentWindow();
                BrowserWindow.openDevTools();
            }
        });


    }
})();