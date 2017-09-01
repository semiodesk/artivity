(function () {
	angular.module('app').directive('artProjectTabs', function () {
		return {
			restrict: 'E',
			templateUrl: 'app/directives/art-project-tabs/art-project-tabs.html',
			controller: ProjectTabsDirectiveController,
			controllerAs: 't',
			bindToController: true,
			scope: {
				project: "=?",
			}
		}
	});

	ProjectTabsDirectiveController.$inject = ['$scope', '$state', 'hotkeys', 'cookieService', 'projectService'];

	function ProjectTabsDirectiveController($scope, $state, hotkeys, cookieService, projectService) {
		var t = this;

		t.loadProjects = function () {
			var projects = cookieService.get('tabs.openedProjects', []);

			// Restore the tabs.
			var n = cookieService.get('tabs.selectedTab', 0);

			// The dashboard will not be selected by the project loading procedure.
			if(n === 0) {
				t.selectTab(n);
			}

			for (var i = 0; i < projects.length; i++) {
				var uri = projects[i];

				if (uri) {
					projectService.get(uri).then(function (project) {
						// The index n denotes the selected tab with 0 being the dashboard and not a project.
						var select = (0 < n) ? projects[n - 1] === project.Uri : false;

						t.openProject(project, select);						
					}, function () {
						console.error('Failed to load project:', project)
					});
				}
			}
		}

		t.saveProjects = function () {
			var projects = [];

			for (var i = 0; i < t.tabs.length; i++) {
				var tab = t.tabs[i];
				var stateParams = tab.context.stateParams;

				if (stateParams && stateParams.project) {
					var project = stateParams.project;

					if (project && project.Uri && !project.IsNew) {
						projects.push(stateParams.project.Uri);
					}
				}
			}

			cookieService.set('tabs.openedProjects', projects);
		}

		t.getProject = function (project) {
			if (project && project.Uri) {
				for (var i = 0; i < t.tabs.length; i++) {
					var tab = t.tabs[i];
					var stateParams = tab.context.stateParams;

					if (stateParams && stateParams.project) {
						var p = stateParams.project;

						if (p.Uri && p.Uri === project.Uri) {
							return {
								index: i,
								tab: tab
							};
						}
					}
				}
			}

			return null;
		}

		t.openProject = function (project, select = true) {
			if (project) {
				var p = t.getProject(project);

				if (p && select) {
					t.selectTab(p.index);
				} else if (!p) {
					// Otherwise create a new tab for the project.
					var i = t.tabs.length - 1;

					var context = new TabContext({
						name: 'main.view.project-dashboard'
					}, {
						index: i,
						project: project
					});

					var tab = new Tab(project.Title, context, true);

					t.addTab(i, tab, select);
				}
			}
		}

		t.closeProject = function (project) {
			var p = t.getProject(project);

			if (p) {
				t.removeTab(p.index);
			}
		}

		t.selectTab = function (index) {
			if (index === t.tabs.length - 1) {
				projectService.create().then(function (project) {
					project.IsNew = true;

					var context = new TabContext({
						name: 'main.view.project-dashboard'
					}, {
						index: index,
						project: project
					});

					var tab = new Tab('', context, true);

					t.addTab(index, tab, true, false);
				});
			} else {
				t.selectedTab = index;

				// Save the last selected tab so that we can restore it after an app restart.
				// Note: Do not save the position of the last '+' tab.
				if (t.selectedTab < t.tabs.length - 1) {
					cookieService.set('tabs.selectedTab', t.selectedTab);
				}

				var tab = t.tabs[index];

				if (tab && tab.context) {
					var context = tab.context;

					if (context && context.state) {
						$state.go(context.state.name, context.stateParams);
					}
				}
			}
		}

		t.addTab = function (index, tab, select, save) {
			if (0 < index && index < t.tabs.length) {
				t.tabs.splice(index, 0, tab);

				if (select) {
					t.selectTab(index);
				}

				if (save || save === undefined) {
					t.saveProjects();
				}

				return {
					index: index,
					tab: tab
				};
			}
		}

		t.removeTab = function (index, e) {
			if (e && typeof (e.stopPropagation) === 'function') {
				e.stopPropagation();
			}

			if (0 < index && index < t.tabs.length - 1) {
				var tab = t.tabs[index];

				if (tab && tab.closable) {
					t.tabs.splice(index, 1);

					if (index < t.tabs.length - 1) {
						t.selectTab(index);
					} else if (index > 0) {
						t.selectTab(index - 1);
					}

					t.saveProjects();
				}
			}
		}

		t.$onInit = function () {
			t.tabs = [
				new Tab(null, new TabContext({
					name: 'main.view.dashboard'
				}, {
					index: 0
				}), false, {
					class: "zmdi zmdi-menu"
				}),
				new Tab(null, new TabContext({
					name: 'main.view.recent-files'
				}), false, {
					class: "zmdi zmdi-plus"
				})
			];

			$scope.$on('openProject', function (e, args) {
				if (args && args.data) {
					t.openProject(args.data);
				}
			});

			$scope.$on('closeProject', function (e, args) {
				if (args && args.data) {
					t.closeProject(args.data);
				}
			});

			$scope.$on('commit', function (e, args) {
				var project = args.data;

				if (project) {
					var p = t.getProject(project);

					if (p) {
						p.tab.title = project.Title;

						t.saveTabs();
					}
				}
			});
		}

		t.$postLink = function () {
			t.loadProjects();
		}
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