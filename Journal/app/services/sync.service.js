(function () {
    angular.module('explorerApp').factory('syncService', syncService);

    function syncService() {
        var serviceUrls = [ 'http://faubulous@localhost:8080/api/1.0/sync'];

        return {
            sync: sync
        };

        function sync() {
            for(var i = 0; i < serviceUrls.length; i++) {
                var url = serviceUrls[i];

                console.log('Synchronizing with service:', url);

                $http.get(url, function(response) {
                    if(response.data) {
                        console.log(' ', response.data.length, ' item(s)');
                    }
                }, function(response) {
                    console.log('Connection error.');
                });
            }
        }
    }
})();