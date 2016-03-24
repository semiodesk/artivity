var artivity = angular.module('artivity', []);


artivity.factory('activityService', function ($http)
{
    return {
        listFiles: function () {
            return $http.get('/artivity/1.0/api/list')
                           .then(
            function (response) {
                return response.data;
            })
        },
        listActivities: function (fileUrl) {
            return $http.get('/artivity/1.0/api/activity/list?file='+fileUrl)
                                       .then(
                        function (response) {
                            return response.data;
                        })
        }
    };
});

