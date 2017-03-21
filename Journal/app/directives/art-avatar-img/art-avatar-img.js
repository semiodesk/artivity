(function () {
    angular.module('app').directive('artAvatarImg', AvatarImageDirective);

    function AvatarImageDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-avatar-img/art-avatar-img.html',
            controller: AvatarImageDirectiveController,
            controllerAs: 't',
            bindToController: true,
            scope: {
                src: "@src",
                width: "=width",
                height: "=height"
            }
        }
    }

    AvatarImageDirectiveController.$inject = ['$rootScope', '$scope', '$sce', '$timeout'];

    function AvatarImageDirectiveController($rootScope, $scope, $sce, $timeout) {
        var t = this;

        $scope.$watch(function () {
            if (t.height > 0 && t.width > 0) {
                t.style = {
                    height: t.height + 'px',
                    width: t.width + 'px'
                };
            } else {
                t.style = {
                    height: '100%',
                    width: '100%'
                };
            }
        });

        t.selectFile = function (e) {
            e.preventDefault();
            
            var input = $(e.currentTarget).find('input[type="file"]');

            if (input) {
                $scope.$evalAsync(function () {
                    var handler = function (args) {
                        $scope.$apply(function () {
                            // Store the selected picture in the model for saving when the changes are applied.
                            t.src = $sce.trustAsResourceUrl('file://' + args.target.files[0].path);
                        });

                        input.unbind('change', handler);
                    };

                    input.bind('change', handler);
                    input[0].click();
                });
            }
        }
    };
})();