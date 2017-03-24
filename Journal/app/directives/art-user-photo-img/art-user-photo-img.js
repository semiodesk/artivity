(function () {
    angular.module('app').directive('artUserPhotoImg', AvatarImageDirective);

    function AvatarImageDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-user-photo-img/art-user-photo-img.html',
            controller: AvatarImageDirectiveController,
            controllerAs: 't',
            bindToController: true,
            scope: {
                src: "=src",
                width: "=width",
                height: "=height",
                changed: "&?"
            }
        }
    }

    AvatarImageDirectiveController.$inject = ['$rootScope', '$scope', '$sce'];

    function AvatarImageDirectiveController($rootScope, $scope, $sce) {
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
                        var reader = new FileReader();

                        reader.onload = function () {
                            if (reader.result) {
                                $scope.$apply(function () {
                                    t.src = reader.result;
                                });

                                if (t.changed) {
                                    t.changed()(args.target.files[0]);
                                }
                            }
                        }

                        reader.readAsDataURL(args.target.files[0]);

                        input.unbind('change', handler);
                    };

                    input.bind('change', handler);
                    input[0].click();
                });
            }
        }
    };
})();