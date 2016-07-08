var explorerControllers = angular.module('explorerControllers', ['ngInputModified', 'ui.bootstrap', 'ui.grid']);

explorerControllers.filter('reverse', function () {
    return function (items) {
        return items.slice().reverse();
    };
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

explorerControllers.controller('FileListController', function (api, $scope) {
    $scope.hasFiles = false;

    $scope.userPhotoUrl = api.getUserPhotoUrl();

    $scope.getFileName = getFileName;

    api.getUser().then(function (data) {
        $scope.user = data;
    });

    api.getRecentFiles().then(function (data) {
        $scope.files = data;

        $scope.hasFiles = data.length > 0;
    });
});

explorerControllers.controller('FileViewController', function (api, $scope, $location, $routeParams) {
    var fileUri = $location.search().uri;

    console.log(fileUri);

    // File metadata
    $scope.file = {};

    api.getFile(fileUri).then(function (data) {
        $scope.file = data;
    });

    // Agent metadata
    $scope.agent = {
        iconUrl: ''
    };

    api.getAgent(fileUri).then(function (data) {
        data.iconUrl = api.getAgentIconUrl(data.association);

        $scope.agent = data;
    });

    // RENDERING
    var canvas = document.getElementById('canvas');

    var renderer = new DocumentRenderer(canvas, api.getRenderingUrl(fileUri));

    // Canvases in the file
    api.getCanvases(fileUri).then(function (data) {
        renderer.canvasCache.load(data, function () {
            console.log("Loaded canvases: ", renderer.canvasCache);

            if ($scope.selectedInfluence !== undefined) {
                $scope.renderInfluence($scope.selectedInfluence);
            }
        });
    });

    // Trigger loading the bitmaps.
    api.getRenderings(fileUri).then(function (data) {
        renderer.renderCache.load(data, function () {
            console.log("Loaded renderings: ", renderer.renderCache);
        });
    });

    // Layers in the file
    $scope.layers = [];

    api.getLayers(fileUri).then(function (data) {
        renderer.layerCache.load(data, function (layers) {
            console.log("Loaded layers: ", layers);
        });
    });

    var getLayers = function (influence) {
        var time = new Date(influence.time);
        var layers = [];

        renderer.layerCache.getAll(time, function (layer) {
            layer.label = layer.getLabel(time);
            layers.push(layer);
        });

        $scope.layers = layers;
    };

    // ACTIVITIES
    $scope.activities = [];
    $scope.selectedActivity;

    api.getActivities(fileUri).then(function (data) {
        // Check if there is a plausable end time.
        for (var i = 0; i < data.length; i++) {
            var activity = data[i];

            if (activity.endTime < activity.maxTime) {
                activity.endTime = activity.maxTime;
            }
        }

        $scope.activities = data;

        console.log("Loaded activities: ", $scope.activities);

        if (data.length > 0) {
            $scope.selectedActivity = data[0];
        }
    });

    // INFLUENCES
    $scope.influences = [];
    $scope.previousInfluence;
    $scope.selectedInfluence;

    api.getInfluences(fileUri).then(function (data) {
        $scope.influences = data;

        console.log("Loaded influences:", $scope.influences);

        if (data.length > 0) {
            $scope.selectedInfluence = data[0];

            $scope.renderInfluence(data[0]);
        }
    });

    $scope.selectInfluence = function (influence) {
        $scope.selectedInfluence = influence;
        $scope.previousInfluence = undefined;
    };

    $scope.previewInfluence = function (influence) {
        $scope.previousInfluence = $scope.selectedInfluence;
        $scope.selectedInfluence = influence;

        if (influence.time !== undefined) {
            $scope.renderInfluence(influence);
        }
    };

    $scope.renderInfluence = function (influence) {
        if (influence !== undefined) {
            renderer.render(influence);

            getLayers(influence);

            api.getStats(fileUri, influence.time).then(function (data) {
                $scope.updateStats(data, influence.time);
            });

            // Warning: this is slow.
            // $scope.palette = renderer.getPalette();
        }
    };

    $scope.resetInfluence = function () {
        if ($scope.previousInfluence) {
            $scope.selectedInfluence = $scope.previousInfluence;
            $scope.previousInfluence = undefined;

            $scope.renderInfluence($scope.selectedInfluence);
        }
    };

    // STATISTICS
    $scope.stats = {
        confidence: 100,
        steps: 10,
        undos: 0,
        redos: 0,
        layers: []
    };

    $scope.updateStats = function (data, time) {
        var stats = {
            confidence: 100,
            steps: 0,
            undos: 0,
            redos: 0,
            layers: []
        };

        for (var i = 0; i < data.length; i++) {
            var influence = data[i];

            stats.steps += influence.count;

            if (influence.type == 'http://w3id.org/art/terms/1.0/Undo') {
                stats.undos += influence.count;
            } else if (influence.type == 'http://w3id.org/art/terms/1.0/Redo') {
                stats.redos += influence.count;
            }
        }

        stats.confidence = (100 * (stats.steps - stats.undos - stats.redos) / stats.steps).toFixed(0);
        stats.layers = renderer.renderedLayers;

        $scope.stats = stats;
    };

    // PLAYBACK
    var playloop = undefined;

    $scope.togglePlay = function () {
        if (playloop) {
            $scope.pause();
        } else {
            $scope.play();
        }
    };

    $scope.play = function () {
        if (!playloop) {
            playloop = setInterval($scope.skipNext, 500);
        }
    };

    $scope.pause = function () {
        if (playloop) {
            clearInterval(playloop);

            playloop = undefined;
        }
    };

    $scope.skipNext = function () {
        var i = $scope.influences.indexOf($scope.selectedInfluence) + 1;

        if (0 < i && i < $scope.influences.length) {
            $scope.selectedInfluence = $scope.influences[i];

            $scope.renderInfluence($scope.selectedInfluence);
        }

        if (playloop) {
            $scope.$digest();

            if (i == $scope.influences.length) {
                $scope.pause();
            }
        }
    };

    $scope.skipPrev = function () {
        var i = $scope.influences.indexOf($scope.selectedInfluence);

        if (0 < i) {
            $scope.selectedInfluence = $scope.influences[i - 1];

            $scope.renderInfluence($scope.selectedInfluence);
        }
    };

    // FORMATTING
    $scope.getFormattedTime = function (time) {
        return moment(time).format('hh:mm:ss');
    };

    $scope.getFormattedDate = function (time) {
        return moment(time).format('dddd, Do MMMM YYYY');
    };


    $scope.getFormattedTimeFromNow = function (time) {
        var result = moment(time).fromNow();

        return result;
    };
}).directive('ganttChart', function () {
    return {
        template: '<div class="chart"><svg class="canvas"></svg></div>',
        link: function (scope, element, attributes) {
            var gantt = d3.gantt();

            gantt.init(element, getValue(scope, attributes.chartData));

            scope.$watchCollection(attributes.chartData, function () {
                gantt.update(getValue(scope, attributes.chartData));
            });

            scope.$watch('selectedInfluence', function () {
                if (scope.selectedInfluence !== undefined) {
                    gantt.position(scope.selectedInfluence.time);
                }
            });

            $(window).resize(function () {
                var container = $('.col-playback-chart');
                gantt.width(container.innerWidth());
                gantt.update();
            });

            $(document).ready(function () {
                var container = $('.col-playback-chart');
                gantt.width(container.innerWidth());
                gantt.update();
            });
        }
    }
});

explorerControllers.controller('SettingsController', function (api, $scope, $location, $rootScope, $routeParams) {
    $scope.children = [];

    $scope.submit = function () {
        $scope.children.forEach(function (child) {
            if (child.submit) {
                child.submit();
            }
        });
    };

    $scope.submitAndReturn = function () {
        $scope.submit();
        
        // Navigate to dasboard and refresh the page.
        $location.path('/');
    };

    $scope.reset = function () {
        $scope.children.forEach(function (child) {
            if (child.reset) {
                child.reset();
            }
        });
    };
}).directive("ngPhotoPicker", function () {
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
}).directive("ngColorPicker", function () {
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

explorerControllers.controller('UserSettingsController', function (api, $scope, $uibModal, $log) {
    // Register the controller with its parent for global apply/cancel.
    $scope.$parent.children.push(this);

    // Load the user data.
    api.getUser().then(function (data) {
        $scope.user = data;
        $scope.userForm.$setPristine();
    });

    // Load the user accounts.
    api.getAccounts().then(function (data) {
        $scope.accounts = data;
    });

    // Set the user photo URL.
    $scope.userPhotoUrl = api.getUserPhotoUrl();

    $scope.selectAccountProvider = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'addAccountDialog.html',
            controller: 'AccountDialogController'
        });

        modalInstance.result.then(function (account) {
            // Reload the user accounts.
            api.getAccounts().then(function (data) {
                $scope.accounts = data;
            });
        });
    };

    $scope.onPhotoChanged = function (e) {
        // Update the preview image..
        var files = window.event.srcElement.files;

        if (FileReader && files.length) {
            var reader = new FileReader();

            reader.onload = function () {
                document.getElementById('photo-img').src = reader.result;
            }
            
            reader.readAsDataURL(files[0]);
        }
    };

    $scope.uninstallAccount = function (account) {
        api.uninstallAccount(account.Id).then(function () {
            // Reload the accounts.
            api.getAccounts().then(function (data) {
                $scope.accounts = data;
            });
        });
    };

    this.submit = function () {
        console.log("Submitting user..");

        api.setUser($scope.user);

        if ($scope.userPhoto) {
            api.setUserPhoto($scope.userPhoto).then(function () {
                $scope.userPhotoUrl = '';
                $scope.userPhotoUrl = api.getUserPhotoUrl();
            });
        }
    };

    this.reset = function () {
        $scope.userForm.reset();
    };
});

explorerControllers.controller('AccountDialogController', function (api, $scope, $uibModalInstance) {
    var timer = undefined;

    api.getAccountProviders().then(function (data) {
        $scope.providers = data;
    });

    $scope.showStatus = function (provider, $event) {
        var element = $event.currentTarget;

        $(element).find('.account').hide();
        $(element).find('.description').hide();
        $(element).find('.loader').show();
        $(element).find('.status').show();

        // Update the provider status every 500ms.
        timer = window.setInterval(function () {
            api.getAccountProvider(provider.Id).then(function (data) {
                $(element).find('.status').val(data.Status);

                // TODO: Improve server API to provide status code.
                if (data.Status.indexOf("install") > -1) {
                    $(element).find('.loader').hide();
                    $(element).find('.ok').show();

                    window.clearInterval(timer);
                }
            });
        }, 500);
    };

    this.submit = function () {
        console.log("Submitting accounts..");
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');

        if (timer) {
            window.clearInterval(timer);
        }
    };
});

explorerControllers.controller('AgentSettingsController', function (api, $scope, $log) {
    // Register the controller with its parent for global apply/cancel.
    $scope.$parent.children.push(this);

    $scope.agents = [];

    $scope.hasError = false;
    $scope.errorType = '';
    $scope.errorMessage = '';

    $scope.toggleInstall = function (agent) {
        if (agent.pluginInstalled) {
            api.installAgent(agent.uri).then(function (response) {
                agent.pluginInstalled = response.success;

                $scope.hasError = !response.success;
                $scope.errorType = response.error.data.type;
                $scope.errorMessage = response.error.data.message;
            });
        } else {
            api.uninstallAgent(agent.uri).then(function (response) {
                agent.pluginInstalled = !response.success;

                $scope.hasError = !response.success;
                $scope.errorType = response.error.data.type;
                $scope.errorMessage = response.error.data.message;
            });
        }
    };

    $scope.reload = function () {
        $scope.hasError = false;

        api.getAgents().then(function (data) {
            $scope.agents = [];

            for (var i = 0; i < data.length; i++) {
                var agent = data[i];

                if (agent.IsSoftwareInstalled) {
                    $scope.agents.push({
                        uri: agent.AgentUri,
                        name: agent.AgentName,
                        color: agent.AgentColor,
                        associationUri: agent.AssociationUri,
                        iconSrc: api.getAgentIconUrl(agent.AssociationUri),
                        softwareInstalled: agent.IsSoftwareInstalled,
                        softwareVersion: agent.ExecutableVersion,
                        pluginInstalled: agent.IsPluginInstalled,
                        pluginVersion: agent.PluginVersion,
                        pluginEnabled: agent.IsPluginEnabled
                    });
                }
            }

            $scope.agentForm.$setPristine();
        });
    }

    $scope.reload();

    this.submit = function () {};

    this.reset = function () {
        $scope.agentForm.reset();
    };
});

explorerControllers.controller('QueryController', function (api, $scope) {
    var defaultPrefixes = "\
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>\n\
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>\n\
PREFIX art: <http://w3id.org/art/terms/1.0/>\n\
PREFIX foaf: <http://xmlns.com/foaf/0.1/>\n\
PREFIX nie: <http://www.semanticdesktop.org/ontologies/2007/01/19/nie#>\n\
PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>\n\
PREFIX prov: <http://www.w3.org/ns/prov#>\n\
prefix xsd: <http://www.w3.org/2001/XMLSchema#>\n\n\
SELECT ?s ?p ?o WHERE { ?s ?p ?o }";

    $scope.queryString = defaultPrefixes;

    $scope.executeQuery = function () {
        api.getQueryResults($scope.queryString).then(function (data) {
            console.log(data);

            document.getElementById('result').innerHTML = JSON.stringify(data, null, 2);
        });
    }

    $scope.resetQuery = function () {
        $scope.queryString = defaultPrefixes;
    }
});

function dump(arr, level) {
    var dumped_text = "";
    
    if (!level) level = 0;

    //The padding given at the beginning of the line.
    var level_padding = "";
    for (var j = 0; j < level + 1; j++) level_padding += "    ";

    if (typeof (arr) == 'object') { //Array/Hashes/Objects 
        for (var item in arr) {
            var value = arr[item];

            if (typeof (value) == 'object') { //If it is an array,
                dumped_text += level_padding + "'" + item + "' ...\n";
                dumped_text += dump(value, level + 1);
            } else {
                dumped_text += level_padding + "'" + item + "' : \"" + value + "\"\n";
            }
        }
    } else { //Stings/Chars/Numbers etc.
        dumped_text = "===>" + arr + "<===(" + typeof (arr) + ")";
    }
    return dumped_text;
}
