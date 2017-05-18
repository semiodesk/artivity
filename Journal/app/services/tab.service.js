(function () {
    angular.module('app').factory('tabService', tabService);

    tabService.$inject = ['projectService', 'cookieService'];

    function tabService(projectService, cookieService) {
        var defaultContext = new TabContext({
            name: 'main.view.project-dashboard'
        }, {
            index: 0
        });

        var initialized = false;

        var tabs = [
            new Tab(null, new TabContext({
                name: 'main.view.dashboard'
            }, {
                index: 0
            }), false, {
                class: "zmdi zmdi-menu"
            }),
            new Tab(null, new TabContext({
                name: 'main.view.recently-used'
            }), false, {
                class: "zmdi zmdi-plus"
            })
        ];

        var t = {
            openTabs: function () {
                return new Promise(function (resolve, reject) {
                    if (!initialized) {
                        var projects = cookieService.get('tabs.openedProjects', []);

                        for (var i = 0; i < projects.length; i++) {
                            var uri = projects[i];

                            projectService.get(uri).then(function (project) {
                                t.addTab(project);
                            }, function () {
                                console.error('Failed to load project:', project)
                            });
                        }

                        initialized = true;
                    }

                    resolve(tabs);
                })
            },
            saveTabs: function () {
                var projects = [];

                for (var i = 0; i < tabs.length; i++) {
                    var tab = tabs[i];
                    var stateParams = tab.context.stateParams;

                    if (stateParams && stateParams.project) {
                        projects.push(stateParams.project.Uri);
                    }
                }

                cookieService.set('tabs.openedProjects', projects);
            },
            addTab: function (project) {
                if (project) {
                    // Return the already opened tab if there is one.
                    for(var i = 0; i < tabs.length; i++) {
                        var tab = tabs[i];
                        var stateParams = tab.context.stateParams;

                        if(stateParams && stateParams.project === project) {
                            return tab;
                        }
                    }

                    // Otherwise create a new tab for the project.
                    var i = tabs.length - 1;

                    var context = new TabContext(defaultContext.state, {
                        index: i,
                        project: project
                    });

                    var tab = new Tab(project.Title, context, true);

                    tabs.splice(i, 0, tab);

                    t.setSelectedTab(i);

                    return { index: i, tab: tab };
                }
            },
            removeTab: function (index) {
                if (0 < index && index < tabs.length - 1) {
                    tabs.splice(index, 1);
                }
            },
            closeTab: function (index) {
                var tab = tabs[index];

                if (tab && tab.closable) {
                    t.removeTab(index);

                    if (index > 0) {
                        t.setSelectedTab(index - 1);
                    }

                    t.saveTabs();
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