(function () {
    angular.module('app').directive('artKeyboardInputHandler', KeyboardInputHandlerDirective);

    function KeyboardInputHandlerDirective() {
        return {
            scope: true,
            template: '',
            controller: KeyboardInputHandlerDirectiveController
        }
    };

    KeyboardInputHandlerDirectiveController.$inject = ['$scope', 'api', 'hotkeys', 'windowService', 'syncService'];

    function KeyboardInputHandlerDirectiveController($scope, api, hotkeys, windowService, syncService) {
        $scope.getUrlWithFile = function (file) {
            var url = window.location.href.split('#');

            if (url.length < 2) {
                console.log('Unable to parse fragment from url:' + window.location.href);

                return '';
            } else {
                return url[0].replace(/index.html/i, '') + file;
            }
        }

        $scope.getUrlWithFragment = function (fragment) {
            var url = window.location.href.split('#');

            if (url.length < 2) {
                console.log('Unable to parse fragment from url:' + window.location.href);

                return '';
            } else {
                return url[0] + '#' + fragment;
            }
        }

        $scope.navigateToFile = function (file) {
            var url = $scope.getUrlWithFile(file);

            if (url !== '') {
                window.location.href = url;
            }
        };

        $scope.navigateToFragment = function (fragment) {
            var url = $scope.getUrlWithFragment(fragment);

            if (url !== '') {
                window.location.href = url;
            }
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
            combo: 'ctrl+h',
            description: 'Go to the dashboard view.',
            callback: function () {
                // This will be replaced with the correct home page by the route provider.
                $scope.navigateToFragment('/');
            }
        });

        hotkeys.add({
            combo: 'ctrl+shift+i',
            description: 'Open development tools.',
            callback: function () {
                windowService.currentWindow().openDevTools();
            }
        });

        hotkeys.add({
            combo: 'f5',
            description: 'Reload the current view.',
            callback: function () {
                windowService.reload();
            }
        });

        hotkeys.add({
            combo: 'f6',
            description: 'Synchronize online accounts.',
            callback: function () {
                syncService.synchronize();
            }
        });

        hotkeys.add({
            combo: 'f12',
            description: 'Open SPARQL query editor.',
            callback: function () {
                var url = $scope.getUrlWithFragment('/query');

                windowService.openWindow(url);
            }
        });
    }
})();