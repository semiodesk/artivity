(function () {
    angular.module('app').directive('mdThemePreview', MaterialThemePreviewDirective);

    /**
     * A directive for debugging the Material UI theme.
     */
    function MaterialThemePreviewDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/md-theme-preview/md-theme-preview.html',
            scope: {
                primary: '@',
                accent: '@'
            },
            controller: function ($scope, $mdColors, $mdColorUtil) {
                $scope.getColor = function (color) {
                    return $mdColorUtil.rgbaToHex($mdColors.getThemeColor(color));
                };
            }
        }
    }
})();