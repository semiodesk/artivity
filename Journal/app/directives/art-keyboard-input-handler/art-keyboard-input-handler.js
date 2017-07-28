(function () {
    angular.module('app').directive('artKeyboardInputHandler', KeyboardInputHandlerDirective);

    function KeyboardInputHandlerDirective() {
        return {
            restrict: 'A',
            scope: true,
            template: '',
            controller: KeyboardInputHandlerDirectiveController
        }
    };

    KeyboardInputHandlerDirectiveController.$inject = ['$scope', 'hotkeys', 'windowService', 'navigationService', 'syncService'];

    function KeyboardInputHandlerDirectiveController($scope, hotkeys, windowService, navigationService, syncService) {

        var helpToggled = false;

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
            var location = window.location.href;
            var url = location.split('#');

            if (url.length < 2) {
                console.log('Unable to parse fragment from url:' + window.location.href);

                return '';
            } else {
                return url[0] + '#!' + fragment;
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

        hotkeys.del('?');

        hotkeys.add({
            combo: 'f1',
            description: 'Show / hide this help menu.',
            callback: function() {
                if( !helpToggled ){
                    showOverlay('msg-help');
                    helpToggled = true;
                    
                    }
                else{
                    hideOverlays('msg-help');
                    helpToggled = false;
                    }
            }
        });

        

        hotkeys.add({
            combo: 'backspace',
            description: 'Go back to the previous view.',
            callback: function (e) {
                navigationService.navigateBack(e);
            }
        });

        hotkeys.add({
            combo: 'shift+backspace',
            description: 'Go forward to the next view.',
            callback: function () {
                navigationService.navigateForward();
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
            combo: 'shift+f5',
            description: 'Reload the entire window.',
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

        hotkeys.add({
            combo: 'ctrl+shift+i',
            description: 'Open development tools.',
            callback: function () {
                windowService.currentWindow().openDevTools();
            }
        });
    }
})();