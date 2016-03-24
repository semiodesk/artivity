var Journal = angular.module('Journal', ['artivity']);

Journal.controller('ItemListController', function (activityService, $scope, $rootScope)
{
    activityService.listFiles()
        .then(function (data)
        {
            $scope.items = data;
        });

    $scope.select = function (value) { $rootScope.$broadcast('show', value); }
});

Journal.controller('ActivityController', function (activityService, $scope) {
    this.isHidden = true;
    $scope.close = function () { $scope.isHidden = true; };

    $scope.$on('show', function (event, args) {
        alert(args);
    });

    
});