(function () {
    angular.module('app').directive('artHelp', function () {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-help/art-help.html',
            controller: HelpDirectiveController,
            controllerAs: 't',
            scope: {
            }
        }
    });

    HelpDirectiveController.$inject = ['$rootScope', '$scope', 'hotkeys', 'translationService'];

    function HelpDirectiveController($rootScope, $scope, hotkeys, translationService) {
        var t = this;

        t.defaultTopics = {
            'About' : new HelpEntry('About', 'help/about')
        }
        t.topics = {}

        t.currentTopic = null;

        t.getAllHelpTopics = getAllHelpTopics;
        t.selectTopic = selectTopic;
        t.getLanguage = getLanguage;

        initialize();

        function initialize(){
            t.topics = collectHelpTopics($rootScope);
        }

        function selectTopic(topic){
            t.currentTopic = topic;
        }

        function getLanguage(){
            return translationService.getUserInterfaceLocale();
        }


        function getAllHelpTopics(){
             t.topics = collectHelpTopics($rootScope);
            return t.topics;
        }
        
        function collectHelpTopics(root) {
            var topics = t.defaultTopics;

            function visit(scope) {
                var topic = getHelpTopics(scope);
                if( topic !== undefined)
                    topics[topic.title] = topic;
            }
            function traverse(scope) {
                visit(scope);
                if (scope.$$nextSibling)
                    traverse(scope.$$nextSibling);
                if (scope.$$childHead)
                    traverse(scope.$$childHead);
            }

            traverse(root);
            return topics;
        }
        
        function getHelpTopics(scope){
            if( scope.helpEntry !== undefined){
                return scope.helpEntry;
            }

            if( scope.t !== undefined && scope.t.helpEntry !== undefined){
                return scope.t.helpEntry;
            }

        }
    };
})();