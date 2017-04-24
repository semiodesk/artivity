(function () {
    angular.module('app').directive('artLocalizationHandler', LocalizationHandlerDirective);

    function LocalizationHandlerDirective() {
        return {
            restrict: 'A',
            scope: true,
            template: '',
            controller: LocalizationHandlerDirectiveController
        }
    };

    LocalizationHandlerDirectiveController.$inject = ['$scope', 'gettextCatalog', 'cookieService'];

    function LocalizationHandlerDirectiveController($scope, gettextCatalog, cookieService) {
        // Apply the saved locale when (re)loading the application.
        var locale = cookieService.get('settings.locale');

        if (locale) {
            var l = locale.split('-')[0];

            gettextCatalog.setCurrentLanguage(l);
        }
    }
})();