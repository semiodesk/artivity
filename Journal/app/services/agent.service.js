(function () {
    angular.module('app').factory('agentService', agentService);

    agentService.$inject = ['api'];

    function agentService(api) {
        var service = {

        };

        return service;

        function initialize(){
            t.agent = {
            iconUrl: ''
            };

            api.getAgent(fileUri).then(function (data) {
                t.agent = data;
                t.agent.iconUrl = api.getAgentIconUrl(data.agent);

                console.log("Agent: ", t.agent);
            });
        }
    }
})();