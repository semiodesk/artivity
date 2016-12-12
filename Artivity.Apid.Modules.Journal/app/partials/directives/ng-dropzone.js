(function () {
    'use strict';

    var app = angular.module('explorerApp');

    app.directive('ngDropzone', DropZoneDirective);

    function DropZoneDirective() {
        return {
            restrict: "A",
            link: function (scope, elem) {
                elem.bind('drop', function (evt) {
                    evt.stopPropagation();
                    evt.preventDefault();

                    var files = evt.dataTransfer.files;

                    alert(files);

                    for (var i = 0, f; f = files[i]; i++) {
                        alert(f);
                    }
                });
            }
        }
    }
})();