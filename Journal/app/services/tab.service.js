(function () {
    angular.module('app').factory('tabService', tabService);

    tabService.$inject = ['projectService'];

    function tabService(projectService) {
        var defaultContext = new TabContext({
            name: 'main.view.project-dashboard'
        }, {
            index: 0
        });

        var tabs = [
            new Tab(null, new TabContext({
                name: 'main.view.recently-used'
            }, {
                index: 0
            }), false, {
                class: "zmdi zmdi-menu"
            }),
            new Tab(null, new TabContext({
                name: 'main.view.recently-used'
            }), false, {
                class: "zmdi zmdi-plus zmdi-lg"
            })
        ];

        var t = {
            initialized: projectService.getAll().then(function (projects) {
                var result = [];

                for (var i = 0; i < projects.length; i++) {
                    var project = projects[i];

                    var context = new TabContext(defaultContext.state, {
                        index: i + 1,
                        project: project
                    });

                    result.push(new Tab(project.Title, context, true))
                }

                tabs.splice.apply(tabs, [1, tabs.length - 2].concat(result));
            }),
            addTab: function (project) {
                if (project) {
                    var i = tabs.length - 1;

                    var context = new TabContext(defaultContext.state, {
                        index: i,
                        project: project
                    });

                    var tab = new Tab(project.Title, context, true);

                    tabs.splice(i, 0, tab);

                    return tab;
                }
            },
            removeTab: function (index) {
                if (0 < index && index < tabs.length - 1) {
                    tabs.splice(index, 1);
                }
            },
            /**
             * Get all tabs.
             * @return {array} An array of tabs.
             */
            getTabs: function () {
                return tabs;
            },
            /**
             * Get a tab from its index.
             * @return {object} A tab object.
             */
            getTab: function (index) {
                return tabs[index];
            },
            /**
             * Set the currently selected tab.
             * @param {number} Tab index.
             */
            setSelectedTab: function (index) {
                if (index) {
                    var n = parseInt(index);

                    for (var i = 0; i < tabs.length; i++) {
                        tabs[i].selected = (i === n);
                    }
                }
            },
            /**
             * 
             */
            getSelectedTab: function (index) {
                var n = t.getSelectedTabIndex(index);

                if (n > -1) {
                    return tabs[n];
                }
            },
            /**
             * 
             */
            getSelectedTabIndex: function () {
                for (var i = 0; i < tabs.length; i++) {
                    if (tabs[i].selected) {
                        return i;
                    }
                }

                return 0;
            },
            /**
             * Saves the tab context such as its state and state parameters.
             * @param {number} Tab index.
             * @param {object} Tab state.
             * @param {object} Tab state parameters.
             */
            setTabContext: function (index, context) {
                var tab = tabs[index];

                if (tab) {
                    tab.context = context;
                }
            },
            /**
             * Saves the tab state params.
             * @param {object} UI-Router state parameters.
             */
            saveTabState: function (index, state, stateParams) {
                var context = t.getTabContext(index);

                if (context) {
                    context.state = state;
                    context.stateParams = stateParams;
                }
            },
            /**
             * Get the tab context such as its state and state parameters.
             * @param {number} Tab index.
             */
            getTabContext: function (index) {
                var tab = tabs[index];

                if (tab && tab.context) {
                    return tab.context;
                }
            },
            /**
             * Get the tab context of the currently selected tab, if any.
             * @return {TabContext} on success, undefined otherwise.
             */
            getSelectedTabContext: function () {
                for (var i = 0; i < tabs.length; i++) {
                    if (tabs[i].selected) {
                        return tabs[i].context;
                    }
                }
            }
        }

        return t;
    }

    function Tab(title, context, closable = false, icon = null, selected = false) {
        var t = this;

        t.icon = icon ? icon : null;
        t.title = title;
        t.closable = closable;
        t.selected = selected;
        t.context = context;
    }

    function TabContext(state, stateParams) {
        var t = this;

        t.state = state;
        t.stateParams = stateParams;
    }
})();