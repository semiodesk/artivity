(function () {
    angular.module('app').directive('artAvatar', AvatarDirective);

    function AvatarDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-avatar/art-avatar.html',
            controller: AvatarDirectiveController,
            controllerAs: 't',
            bindToController: true,
            scope: {
                src: "=?src",
                width: "=width",
                height: "=height",
                canChange: "=?",
                changed: "&?"
            }
        }
    }

    AvatarDirectiveController.$inject = ['$scope', '$element', '$timeout', '$sce'];

    function AvatarDirectiveController($scope, $element, $timeout, $sce) {
        var t = this;

        t.img = null;
        t.input = null;

        t.onFileSelected = function (args) {
            var data = args.target.files[0];

            if (t.changed) {
                t.changed()(data);
            }

            var reader = new FileReader();

            reader.onload = function () {
                if (reader.result) {
                    $scope.$apply(function () {
                        t.src = reader.result;
                    });
                }
            }

            reader.readAsDataURL(data);
        }

        t.$onInit = function () {
            if (t.canChange === undefined) {
                t.canChange = false;
            }

            $scope.$watch(['height', 'width'], function () {
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

                t.style.cursor = t.canChange ? 'pointer' : 'inherit';
            });
        }

        t.$postLink = function () {
            t.img = $element.find('img');
            t.input = $element.find('input[type="file"]');

            if (t.img) {
                // We set the placeholder img when a loading error occurs with the current img src.
                t.img.error(function () {
                    t.img.attr('src', 'app/resources/img/placeholder-avatar.png');
                });

                // In order to prevent race conditions when handling errors, we implement the src binding manually.
                $scope.$watch('src', function () {
                    var src = t.src;

                    if(src === undefined || src === null) {
                        src = '';
                    }

                    t.img.attr('src', src);
                });
            }

            if (t.input && t.canChange) {
                t.input.bind('change', t.onFileSelected);

                if (typeof ($element.click) === 'function') {
                    // Triggering the click in the template via ng-click prevents digest exceptions.
                    $element.click(function () {
                        if (t.input.length) {
                            t.input[0].click();
                        }
                    });
                }
            }
        }
    };
})();