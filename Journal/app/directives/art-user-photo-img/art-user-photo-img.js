(function () {
    angular.module('app').directive('artUserPhotoImg', UserPhotoImageDirective);

    function UserPhotoImageDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-user-photo-img/art-user-photo-img.html',
            controller: UserPhotoImageDirectiveController,
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

    UserPhotoImageDirectiveController.$inject = ['$scope', '$element', '$timeout', '$sce'];

    function UserPhotoImageDirectiveController($scope, $element, $timeout, $sce) {
        var t = this;

        t.img = null;
        t.input = null;

        if(!t.src) {
            t.src = 'app/resources/img/placeholder-avatar.png';
        }

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
            t.img = $element.find('img');
            t.input = $element.find('input[type="file"]');

            if (t.canChange && t.input) {
                t.input.bind('change', t.onFileSelected);

                if (typeof ($element.click) === 'function') {
                    // Triggering the click in the template via ng-click prevents digest exceptions.
                    $element.click(function () {
                        t.input[0].click();
                    });
                }
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
    };
})();