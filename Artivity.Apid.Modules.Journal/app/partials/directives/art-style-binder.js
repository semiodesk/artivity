angular.module('explorerApp').directive('artStyleBinder', StyleBinderDirective);

function StyleBinderDirective() {
    return {
        link: function (scope, element, attributes) {
            var template = $(element).text();

            scope.$watch(attributes.artAccentColor, function () {
                var accentColor = getValue(scope, attributes.artAccentColor);

                if (accentColor !== undefined && accentColor !== "#FF0000") {
                    var text = template.replace(/\$accentColor/g, accentColor);

                    $(element).text(text);
                }
            });
        }
    }
}