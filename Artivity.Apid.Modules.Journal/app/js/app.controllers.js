var explorerControllers = angular.module('explorerControllers', [
	'ngAnimate',
	'ngInputModified',
	'ui.bootstrap',
	'ui.bootstrap.modal',
	'ui.bootstrap.progressbar'
]);

explorerControllers.filter('reverse', function () {
    return function (items) {
        if (items !== undefined && items.length > 1) {
            return items.slice().reverse();
        } else {
            return items;
        }
    };
});

explorerControllers.handleServiceClientState = function (api, sessionId, handleClientStateChange) {
    // The polling interval hanler.
    var interval = undefined;

    // The polling interval in milliseconds.
    var intervalMs = 500;

    // The maximum number of queries.
    var maxQueries = 500;

    // The number of status queries.
    var n = 0;

    // Stop polling if errors occur.
    var onError = function (data) {
        if (interval) {
            clearInterval(interval);
        }
    };

    // Query the current client status in a regular interval.
    var interval = setInterval(function () {
        api.getAccountClientStatus(sessionId, onError).then(function (data) {
            var client = data;

            if (!client) {
                // The sessionId was not found, no need to query again.
                clearInterval(interval);
            }

            n++;

            if (handleClientStateChange) {
                handleClientStateChange(interval, client);
            }

            if (n === maxQueries) {
                clearInterval(interval);
            }
        });
    }, intervalMs);
};

explorerControllers.controller('KeyboardController', function (api, $scope, hotkeys) {
    $scope.navigateTo = function (path) {
        var url = window.location.href.split('#');

        if (url.length < 2) {
            console.log('Navigation failed; unable to parse fragment from url:' + window.location.href);

            return;
        }

        window.location.href = url[0].replace(/index.html/i, '') + path;
    };

    $scope.navigateToFragment = function (pathFragment) {
        var url = window.location.href.split('#');

        if (url.length < 2) {
            console.log('Navigation failed; unable to parse fragment from url:' + window.location.href);

            return;
        }

        window.location.href = url[0] + '#' + pathFragment;
    };

    hotkeys.add({
        combo: 'backspace',
        description: 'Go back to the previous view.',
        callback: function () {
            window.history.back();
        }
    });

    hotkeys.add({
        combo: 'shift+backspace',
        description: 'Go forward to the next view.',
        callback: function () {
            window.history.forward();
        }
    });

    hotkeys.add({
        combo: 'alt+h',
        description: 'Go to the dashboard view.',
        callback: function () {
            // This will be replaced with the correct home page by the route provider.
            $scope.navigateToFragment('/');
        }
    });

    hotkeys.add({
        combo: 'alt+q',
        description: 'Open the SPARQL query editor view.',
        callback: function () {
            $scope.navigateTo('query.html');
        }
    });
});

explorerControllers.controller('CalendarController', function (api, $scope, $filter, $uibModalInstance, $sce) {
    $scope.isLoading = true;

    $scope.dialog = $uibModalInstance;

    $scope.getActivities = api.getActivities;

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});

explorerControllers.controller('FilePublishDialogController', function (api, $scope, $filter, $uibModalInstance, $sce, fileService) {
    $scope.dialog = {
        step: 'publishing-options',
        title: 'Publish File',
        subtitle: 'Create a dataset for your file and upload it into a digital repository.'
    };

    $scope.getFileName = fileService.getFileName;
    $scope.getFileNameWithoutExtension = fileService.getFileNameWithoutExtension;
    $scope.getFileExtension = fileService.getFileExtension;
    $scope.hasFileThumbnail = api.hasThumbnail;
    $scope.getFileThumbnailUrl = api.getThumbnailUrl;

    console.log($scope.entity);

    // Accounts
    $scope.accounts = [];
    $scope.selectedAccount = null;

    // At first, we need to determine if there are any accounts which can be used for publishing.
    api.getAccountsWithFeature('http://w3id.org/art/terms/1.0/features/PublishArchive').then(function (data) {
        console.log("Accounts:", data);

        $scope.accounts = data;

        if (0 < $scope.accounts.length) {
            var account = $scope.accounts[0];

            $scope.selectedAccount = account;

            $scope.authentication = {
                protocol: account.AuthenticationProtocol.Uri,
                parameter: {}
            };

            for (var i = 0; i < account.AuthenticationParameters.length; i++) {
                var p = account.AuthenticationParameters[i];

                $scope.authentication.parameter[p.Name] = p.Value;
            }
        } else {
            $scope.dialog.step = 'no-accounts';
            $scope.dialog.title = 'No Accounts';
            $scope.dialog.subtitle = 'You have not yet added any accounts which can be used for publication.';
        }
    });

    $scope.selectAccount = function () {
        $scope.dialog.step = 'upload-select-account';
        $scope.dialog.title = 'Select Account';
        $scope.dialog.subtitle = 'Choose the account used for publication and authorize the upload by logging in.';
    };

    // Publishing
    $scope.archive = {
        title: 'Artivity data for ' + $scope.file.label,
        description: '',
        creators: [],
        license: null,
        licenseOptions: [{
                uri: 'https://creativecommons.org/licenses/by-nc-nd/4.0/',
                label: 'Creative Commons BY-NC-ND',
                description: 'Attribution, Non-Commercial, No Derivatives'
			}, {
                uri: 'https://creativecommons.org/licenses/by-nc-sa/4.0/',
                label: 'Creative Commons BY-NC-SA',
                description: 'Attribution, Non-Commercial, Share Alike'
			}, {
                uri: 'https://creativecommons.org/licenses/by-nc/4.0/',
                label: 'Creative Commons BY-NC',
                description: 'Attribution, Non-Commercial'
			}, {
                uri: 'https://creativecommons.org/licenses/by-nd/4.0/',
                label: 'Creative Commons BY-ND',
                description: 'Attribution, No Derivatives'
			}, {
                uri: 'https://creativecommons.org/licenses/by-sa/4.0/',
                label: 'Creative Commons BY-SA',
                description: 'Attribution, Share Alike'
			}, {
                uri: 'https://creativecommons.org/licenses/by/4.0/',
                label: 'Creative Commons BY',
                description: 'Attribution'
			}
		],
        contentOptions: {
            includeFile: true,
            includeEditingHistory: true,
            includeBrowsingHistory: false,
            includeComments: false
        }
    };

    $scope.archive.license = $scope.archive.licenseOptions[0].uri;

    // Load author information.
    $scope.userPhotoUrl = api.getUserPhotoUrl();

    api.getUser().then(function (data) {
        $scope.user = data;

        $scope.archive.creators = [{
            name: data.Name,
            email: data.EmailAddress
		}];
    });

    // Upload
    var interval = undefined;

    $scope.beginUpload = function () {
        $scope.dialog.step = 'upload-progress';
        $scope.dialog.title = "Publishing File";

        $scope.progress = {
            Tasks: [],
            CurrentTask: '',
            PercentComplete: 0
        };

        api.uploadArchive($scope.selectedAccount.Uri, $scope.entity.uri, $scope.authentication.parameter, $scope.archive).then(function (data) {
            var sessionId = data.Id;

            console.log("Session: ", sessionId);

            explorerControllers.handleServiceClientState(api, sessionId, function (intervalHandle, state) {
                interval = intervalHandle;

                $scope.dialog.subtitle = $filter('translate')(state.Progress.CurrentTask.Id + "#description");

                $scope.progress = state.Progress;

                if (parseInt(state.Progress.PercentComplete) === 100) {
                    clearInterval(interval);

                    // Delay the closing of the window so that the UI can update the progress.
                    setTimeout($scope.endUpload, 2000);
                }
            });
        });
    };

    $scope.endUpload = function () {
        if (interval) {
            clearInterval(interval);
        }

        if ($scope.percentComplete < 100) {
            $scope.percentComplete = 0;
        } else {
            $uibModalInstance.close();
        }
    };

    $scope.cancel = function () {
        $scope.endUpload();

        $uibModalInstance.dismiss('cancel');
    };
});

explorerControllers.controller('AddAccountDialogController', function (api, $scope, $filter, $uibModalInstance, $sce) {
    var interval = undefined;

    $scope.title = $filter('translate')('SETTINGS.ACCOUNTS.CONNECT_DIALOG.TITLE');

    $scope.clients = [];

    api.getAccountClients().then(function (data) {
        $scope.clients = data;

        console.log("Available clients:", $scope.clients);
    });

    $scope.selectedClient = null;

    $scope.selectClient = function (client) {
        $scope.title = ($filter('translate')('SETTINGS.ACCOUNTS.CONNECT_DIALOG.TITLE_X')).replace('{0}', client.Title);

        $scope.selectedClient = client;

        $scope.parameter = {
            clientUri: client.Uri,
            authType: client.SupportedAuthenticationClients[0].Uri
        };

        // TODO: Remove hard-wiring. Receive presets and target sites from client.
        if (client.Uri === 'http://orcid.org') {
            $scope.parameter.presetId = 'orcid.org';

            $scope.connectAccount($scope.selectedClient);
        } else if (client.Uri === 'http://eprints.org') {
            $scope.parameter.url = 'https://ualresearchonline.arts.ac.uk';
        }

        console.log("Client selected: ", client);
    }

    // Prevent an account from being installed twice.
    $scope.isInstalling = false;

    // The client state '0' refers to 'None'.	
    $scope.clientState = 0;

    $scope.connectAccount = function (client) {
        // The client state '1' refers to 'InProgress'.
        $scope.clientState = 1;

        api.authorizeAccount($scope.parameter).then(function (data) {
            var sessionId = data.Id;

            explorerControllers.handleServiceClientState(api, sessionId, function (intervalHandle, state) {
                interval = intervalHandle;

                console.log(state);

                for (var i = 0; i < state.Client.SupportedAuthenticationClients.length; i++) {
                    var c = state.Client.SupportedAuthenticationClients[i];

                    // Allow iframes to connect to the URL.
                    $scope.clientUrl = $sce.trustAsResourceUrl(c.AuthorizeUrl);

                    if (c.ClientState > 1) {
                        clearInterval(interval);

                        $scope.clientState = c.ClientState;

                        // The client state '2' refers to 'Authorized'.
                        if (!$scope.isInstalling && c.ClientState == 2) {
                            $scope.isInstalling = true;

                            api.installAccount(sessionId).then(function (r) {
                                console.log("Account installed:", sessionId);

                                // Close the dialog after the account was successfully connected.
                                setTimeout(function () {
                                    $uibModalInstance.close();
                                }, 1000);
                            });
                        }

                        break;
                    }
                }
            });
        });
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');

        if (interval) {
            window.clearInterval(interval);
        }
    };
});

explorerControllers.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) { // 13 = enter key
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});

explorerControllers.directive('ngEsc', function () {
    return function (scope, element, attrs) {
        element.bind('keydown keypress', function (event) {
            if (event.which === 27) { // 27 = esc key
                scope.$apply(function () {
                    scope.$eval(attrs.ngEsc);
                });

                event.preventDefault();
            }
        });
    };
});

explorerControllers.directive("ngPhotoPicker", function () {
    return {
        link: function (scope, element, attributes) {
            element.bind("change", function (changeEvent) {
                scope.$apply(function () {
                    // Store the selected picture in the model for saving when the changes are applied.
                    scope.userPhoto = changeEvent.target.files[0];
                });
            });
        }
    }
});

explorerControllers.directive("ngColorPicker", function () {
    return {
        template: '<button type="button" class="btn btn-colorpicker"><span class="color-fill-icon"><i></i></span></button>',
        link: function (scope, element, attributes) {
            var indicator = $('.color-fill-icon', element);
            indicator.css('background-color', getValue(scope, attributes.selectedColor));

            var button = $(element);

            button.on('changeColor', function (e) {
                if (e.color == null) {
                    //when select transparent color
                    //$('.color-fill-icon', btn).addClass('colorpicker-color');
                } else {
                    //$('.color-fill-icon', btn).removeClass('colorpicker-color');
                    indicator.css('background-color', e.color);

                    // Update the bound value.
                    setValue(scope, attributes.selectedColor, e.color.toHex());
                }
            });

            button.colorpicker({
                customClass: 'colorpicker-lg',
                align: 'right',
                format: 'rgb',
                color: getValue(scope, attributes.selectedColor),
                colorSelectors: {
                    'default': '#777777',
                    'primary': '#337ab7',
                    'success': '#5cb85c',
                    'info': '#5bc0de',
                    'warning': '#f0ad4e',
                    'danger': '#d9534f'
                },
                sliders: {
                    saturation: {
                        maxLeft: 200,
                        maxTop: 200
                    },
                    hue: {
                        maxTop: 200
                    },
                    alpha: {
                        maxTop: 200
                    }
                }
            });
        }
    }
});

explorerControllers.directive("ngDropzone", function () {
    return {
        restrict: "A",
        link: function (scope, elem) {
            elem.bind('drop', function (evt) {
                evt.stopPropagation();
                evt.preventDefault();

                var files = evt.dataTransfer.files;

                alert(files);

                for (var i = 0, f; f = files[i]; i++) {
                    alert(f);
                }
            });
        }
    }
});

explorerControllers.directive('bootstrapSwitch', [
	function () {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, element, attrs, ngModel) {
                element.bootstrapSwitch();

                element.on('switchChange.bootstrapSwitch', function (event, state) {
                    if (ngModel) {
                        scope.$apply(function () {
                            ngModel.$setViewValue(state);
                        });
                    }
                });

                scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                    if (newValue) {
                        element.bootstrapSwitch('state', true, true);
                    } else {
                        element.bootstrapSwitch('state', false, true);
                    }
                });
            }
        };
	}
]);

explorerControllers.directive('artTimeline', function () {
    return {
        template: '\
		<div class="timeline">\
			<div class="timeline-control"> \
				<div class="position"><label></label></div> \
				<div class="duration"><label></label></div> \
				<div class="track-col"> \
					<div class="thumb-container"></div> \
					<div class="track-container"> \
						<div class="track"></div> \
						<div class="track-preview"></div> \
						<div class="track-indicator"></div> \
						<div class="thumb draggable"><div class="thumb-knob"></div></div> \
					</div> \
					<div class="comments"></div> \
					<div class="activities"></div> \
				</div> \
			</div> \
		</div>',
        link: function (scope, element, attributes) {
            var timeline = new TimelineControl(element);

            timeline.setActivities(getValue(scope, attributes.artActivitiesSrc));
            timeline.setInfluences(getValue(scope, attributes.artInfluencesSrc));

            timeline.selectedInfluenceChanged = function (influence) {
                scope.previewInfluence(influence);
            };

            scope.$watchCollection(attributes.artActivitiesSrc, function () {
                timeline.setActivities(getValue(scope, attributes.artActivitiesSrc));
            });

            scope.$watchCollection(attributes.artInfluencesSrc, function () {
                timeline.setInfluences(getValue(scope, attributes.artInfluencesSrc));
            });

            scope.$watch('selectedInfluence', function () {
                if (scope.selectedInfluence !== undefined) {
                    timeline.setPosition(scope.selectedInfluence);

                    console.log(angular.element(scope.selectedInfluence));
                }
            });
        }
    }
});

explorerControllers.directive('artChartDonut', function () {
    return {
        template: '<div class="chart-donut"></div>',
        link: function (scope, element, attributes) {
            var chart = new DonutChart(element);

            console.log("Confidence: ", scope.stats.confidence);
            chart.draw(element, scope.stats.confidence);

            scope.$watch('stats.confidence', function () {
                console.log("Confidence: ", scope.stats.confidence);
                chart.draw(element, scope.stats.confidence);
            });
        }
    }
});

explorerControllers.directive('artStyleBinder', function () {
    return {
        link: function (scope, element, attributes) {
            var template = $(element).text();

            scope.$watch(attributes.artAccentColor, function () {
                var accentColor = getValue(scope, attributes.artAccentColor);

                if (accentColor !== undefined && accentColor !== "#FF0000") {
                    var text = template.replace(/\$accentColor/g, accentColor);

                    $(element).text(text);
                }
            });
        }
    }
});

explorerControllers.directive('artCalendar', function () {
    return {
        template: '',
        link: function (scope, element, attributes) {
            var options = {
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                defaultView: 'agendaWeek',
                businessHours: {
                    dow: [1, 2, 3, 4, 5], // Monday - Friday
                    start: '9:00',
                    end: '17:00',
                },
                height: function () {
                    return $('.modal-body').innerHeight() - 20;
                }
            };

            var element = $(element);
            element.fullCalendar(options);

            scope.getActivities().then(function (data) {
                var activities = [];

                for (var i = 0; i < data.length; i++) {
                    var activity = data[i];
                    activity.title = '';
                    activity.start = new Date(activity.startTime);
                    activity.color = activity.agentColor;

                    if (activity.endTime) {
                        activity.end = new Date(activity.endTime);
                    } else if (activity.maxTime) {
                        activity.end = new Date(activity.maxTime);
                    } else {
                        activity.end = new Date(activity.startTime);
                        activity.end.setSeconds(activity.end.getSeconds() + 30);
                    }

                    activities.push(activity);
                }

                console.log("Activities:", activities);

                if (activities && activities.length > 0) {
                    element.fullCalendar('render');
                    element.fullCalendar('addEventSource', activities);
                    element.fullCalendar('gotoDate', activities[0].start);
                }

                scope.dialog.rendered.then(function () {
                    scope.isLoading = false;

                    element.fullCalendar('render');
                });
            });
        }
    }
});
