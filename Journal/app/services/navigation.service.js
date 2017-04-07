(function () {
    angular.module('app').factory('navigationService', navigationService);

    function navigationService() {
        var scopes = {};

        return {
            registerScope: registerScope,
            navigateBack: navigateBack,
            navigateForward: navigateForward
        }

        function registerScope(scope) {
            if (scope.$id) {
                scopes[scope.$id] = scope;
            }
        }

        function navigateBack(scope) {
            // Iterate the parents of the given scope and look for a navigation handler.
            if (scope) {
                var s = scope;

                while (s && !scopes.hasOwnProperty(s.$id)) {
                    s = s.$parent;
                }

                if (s && typeof (s.onNavigateBack) === 'function') {
                    var e = {
                        handled: false,
                        scope: scope
                    };

                    s.onNavigateBack(e);

                    if (e.handled) {
                        return;
                    }
                }
            }

            window.history.back();
        }

        function navigateForward(scope) {
            // Iterate the parents of the given scope and look for a navigation handler.
            if (scope) {
                var s = scope;

                while (s && !scopes.hasOwnProperty(s.$id)) {
                    s = s.$parent;
                }

                if (s && typeof (s.onNavigateForward) === 'function') {
                    var e = {
                        handled: false,
                        scope: scope
                    };

                    s.onNavigateForward(e);

                    if (e.handled) {
                        return;
                    }
                }
            }

            window.history.forward();
        }
    }
})();