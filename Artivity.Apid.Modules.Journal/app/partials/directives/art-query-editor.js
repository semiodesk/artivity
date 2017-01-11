(function () {
    angular.module('explorerApp').directive('artQueryEditor', QueryEditorDirective);

    function QueryEditorDirective() {
        return {
            restrict: 'E',
            template: '<div id="editor"></div>',
            controller: QueryEditorDirectiveController,
            controllerAs: 't',
            scope: {}
        }
    }

    angular.module('explorerApp').controller('QueryEditorDirectiveController', QueryEditorDirectiveController);

    function QueryEditorDirectiveController($scope) {
        var t = this;

        function init() {
            // Initialize the SPARQL query GUI.
            var editor = document.getElementById("editor");
            var yasgui = YASGUI(editor);

            console.log('Initialized YASGUI:', yasgui);

            for (tab in yasgui.tabs) {
                var ts = yasgui.tabs[tab];

                // Set the Artivity SPARQL endpoint URL as default option.
                ts.persistentOptions.yasqe.sparql.endpoint = apid.endpointUrl + 'sparql';
                ts.yasqe.addPrefixes({
                    art: 'http://w3id.org/art/terms/1.0/',
                    dc: 'http://purl.org/dc/elements/1.1/',
                    prov: 'http://www.w3.org/ns/prov#',
                    nie: 'http://www.semanticdesktop.org/ontologies/2007/01/19/nie#',
                    nfo: 'http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#'
                });

                console.log('Tab:', ts);
            }

            setQueryButton();
            setResultsViewHeight();

            $(window).on('resize', setResultsViewHeight);
            $('.yasqe_fullscreenBtn').click(onEditorFullscreenEnter);
            $('.yasqe_smallscreenBtn').click(onEditorFullscreenLeave);
            $('.yasr .btn_fullscreen').click(onResultsFullscreenEnter);
            $('.yasr .btn_scmallscreen').click(onResultsFullscreenLeave);
        }

        function setQueryButton() {
            var queryButton = $('.yasqe_queryButton.query_valid');
            queryButton.find('svg').remove();
            queryButton.append($.parseHTML('<div class="btn"><i class="zmdi zmdi-play"></i></div>'));
        }

        function setResultsViewHeight() {
            var height = 'auto';

            if ($('.yasr_fullscreen').length > 0) {
                var contentHeight = $('.yasr').innerHeight();
                var resultsTop = $('.yasr_results').position().top;

                height = contentHeight - resultsTop - 25;
            } else {
                var contentHeight = $('#editor').innerHeight();
                var resultsTop = $('.yasr_results').position().top;

                height = contentHeight - resultsTop - 100;
            }

            if (height !== 'auto') {
                $('.yasr_results').css('height', height + 'px');
            } else {
                $('.yasr_results').css('height', height);
            }

            console.log('Results view height:', height);
        }

        function onEditorFullscreenEnter() {
            var height = $('#window-title').outerHeight();

            $('.yasqe .CodeMirror').css('margin-top', height + 'px');
        }

        function onEditorFullscreenLeave() {
            $('.yasqe .CodeMirror').css('margin-top', '0px');
        }

        function onResultsFullscreenEnter() {
            var height = $('#window-title').outerHeight();

            $('.yasr').css('margin-top', height + 'px');

            setResultsViewHeight();
        }

        function onResultsFullscreenLeave() {
            $('.yasr').css('margin-top', '0px');

            setResultsViewHeight();
        }

        init();
    }
})();