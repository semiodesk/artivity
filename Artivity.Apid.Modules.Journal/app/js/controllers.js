var explorerControllers = angular.module('explorerControllers', ['ngInputModified']);

explorerControllers.controller('FileListController', function (api, $scope) {
    api.getUser().then(function (data) {
        $scope.user = data;
    });

    $scope.userPhotoUrl = api.getUserPhotoUrl();

    $scope.getFileName = getFileName;

    api.getRecentFiles().then(function (data) {
        $scope.files = data;
    });
});

explorerControllers.controller('FileViewController', function (api, $scope, $location, $routeParams) {
    var fileUri = $location.search().uri;

    //$scope.fileName = getFileName(fileUrl);
    $scope.canvas = {};
    $scope.agent = {};

    $scope.activities = [];
    $scope.selectedActivity;

    $scope.influences = [];
    $scope.previousInfluence = undefined;
    $scope.selectedInfluence = {
        layer: "",
        name: "",
        time: new Date()
    };

    $scope.stats = {
        confidence: 100,
        steps: 10,
        undos: 0,
        redos: 0,
        layers: []
    };

    $scope.playloop = undefined;

    // Used for extracting colors from images.
    var thief = new ColorThief();

    // The canvas.
    var buffer = document.getElementsByClassName('buffer')[0];

    // The drawing context.
    var context = buffer.getContext('2d');
    
    // A list with all loaded bitmaps.
    var T = [];
    var Tindex = {};

    // Renders all loaded images onto the canvas.
    var renderDocument = function (thumbnails, time) {       
        context.clearRect(0, 0, buffer.width, buffer.height);
        
        // Save the untransformed canvas state.
        context.save();
        
        if ($scope.canvas !== undefined && $scope.canvas.width > 0 && $scope.canvas.height > 0) {
            var canvas = $scope.canvas;
            
            // Fit the rendered document into the available viewport.
            var shadowOffsetX = 3;
            var shadowOffsetY = 3;
            
            var width = canvas.width + shadowOffsetX;
            var height = canvas.height + shadowOffsetY;
            
            var zoom = 1.0;
            
            if(width > buffer.width) {                 
                zoom = Math.min(zoom, buffer.width / width);
            }
            
            if(height > buffer.height) {                
                zoom = Math.min(zoom, buffer.height / height);
            }
                        
            context.scale(zoom, zoom);
            
            // Translate the document to be centered on the canvas.            
            if (buffer.width !== undefined && buffer.width > canvas.width) {                
                context.translate((buffer.width - canvas.width) / 2, 0);
            }
            
            context.save();
            
            context.shadowBlur = 10;
            context.shadowOffsetX = shadowOffsetX;
            context.shadowOffsetY = shadowOffsetY;
            context.shadowColor = "black";

            context.fillStyle = 'white';
            context.fillRect(0, 0, canvas.width, canvas.height);
            
            context.restore();
        };

        // Only render the newest version of every layer.
        var renderedLayers = {};

        for (var j = T.length - 1; j > 0; j--) {
            var t = T[j];

            if (time !== undefined && t.time > time || t.layerZ !== undefined && t.layerZ in renderedLayers) {
                continue;
            }

            context.drawImage(t.image, t.x, -t.y, t.w, t.h);

            if (t.layer == $scope.selectedInfluence.layer) {
                context.font = "1em Roboto";
                context.fillStyle = "blue";
                context.textAlign = "left";
                context.fillText(t.layer, t.x, -t.y - 10);

                // Draw a line around the layer bounds.
                if (t.boundsX === null) {
                    context.lineJoin = 'miter';
                    context.lineWidth = 1;
                    context.strokeStyle = 'red';
                    context.setLineDash([1, 2]);

                    context.strokeRect(t.x, -t.y, t.w, t.h);
                }
            }

            // Draw a line around the current influence bounds.
            if (t.boundsX !== null) {
                context.lineJoin = 'miter';
                context.lineWidth = 1;
                context.strokeStyle = 'blue';
                context.setLineDash([]);

                context.strokeRect(t.boundsX, -t.boundsY, t.boundsWidth, t.boundsHeight);
            }

            renderedLayers[t.layerZ] = t;
        }
        
        // Restore the untransformed canvas state.
        context.restore();
    };

    var loadThumbnailsComplete = function (thumbnails) {
        T.sort(function (a, b) {
            if (a.time > b.time) {
                return 1;
            } else if (a.time == b.time) {
                return 0;
            } else {
                return -1;
            }
        });

        renderDocument(thumbnails);

        api.getStats(fileUri).then(function (data) {
            $scope.updateStats(data);
        });
    }

    // Loads a single bitmap and pushes it into the list when loading is complete.
    var loadThumbnails = function (thumbnails, i, complete) {
        var t = new Image();

        t.onerror = function () {
            complete(thumbnails, i);
        };

        t.onload = function () {
            t.onload = undefined;

            var data = thumbnails[i];

            T.splice(i, 0, {
                layer: data.layer,
                layerZ: data.layerZ,
                time: data.time,
                image: t,
                x: data.x,
                y: data.y,
                w: t.width,
                h: t.height,
                boundsX: data.boundsX,
                boundsY: data.boundsY,
                boundsWidth: data.boundsWidth,
                boundsHeight: data.boundsHeight
            });

            complete(thumbnails, i);
        };

        t.crossOrigin = 'Anonymous'; // Needed for color thief.
        t.src = api.getThumbnailUrl(thumbnails[i].thumbnailUrl);
    };

    // Trigger loading the bitmaps.
    api.getThumbnails(fileUri).then(function (thumbnails) {
        loadItems(thumbnails, loadThumbnails, loadThumbnailsComplete);
    });

    api.getCanvas(fileUri).then(function (data) {
        $scope.canvas = data;
        
        if($scope.selectedInfluence !== undefined) {
            renderDocument(T, $scope.selectedInfluence.time);
        }
    });

    api.getAgent(fileUri).then(function (data) {
        $scope.agent = data;
    });

    api.getActivities(fileUri).then(function (data) {
        // Check if there is a plausable end time.
        for (var i = 0; i < data.length; i++) {
            var activity = data[i];

            if (activity.endTime < activity.maxTime) {
                activity.endTime = activity.maxTime;
            }
        }

        console.log(data);

        $scope.activities = data;

        if (data.length > 0) {
            $scope.selectedActivity = data[0];
        }
    });

    api.getStats(fileUri).then(function (data) {
        $scope.updateStats(data);
    });

    api.getInfluences(fileUri).then(function (data) {
        $scope.influences = data;

        if (data.length > 0) {
            $scope.selectedInfluence = data[0];
        }
    });

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

            if (influence.type == 'http://semiodesk.com/artivity/1.0/Undo') {
                stats.undos += influence.count;
            } else if (influence.type == 'http://semiodesk.com/artivity/1.0/Redo') {
                stats.redos += influence.count;
            }
        }

        for (var i = 0; i < $scope.influences.length; i++) {
            var influence = $scope.influences[i];

            if (time !== undefined && influence.time > time) {
                continue;
            }

            if (influence.layer !== null && stats.layers.indexOf(influence.layer) == -1) {
                stats.layers.push(influence.layer);
                stats.layers.sort();
            }
        }

        stats.confidence = (100 * (stats.steps - stats.undos - stats.redos) / stats.steps).toFixed(0);

        $scope.stats = stats;
    };

    $scope.selectInfluence = function (influence) {
        $scope.selectedInfluence = influence;
        $scope.previousInfluence = undefined;
    };

    $scope.previewInfluence = function (influence) {
        $scope.previousInfluence = $scope.selectedInfluence;
        $scope.selectedInfluence = influence;

        if (influence.time !== undefined) {
            api.getStats(fileUri, influence.time).then(function (data) {
                $scope.updateStats(data, influence.time);
            });
        }

        $scope.renderInfluence(influence);
    };

    $scope.renderInfluence = function (influence) {
        if (influence === undefined) {
            var context = buffer.getContext('2d');

            context.clearRect(0, 0, buffer.width, buffer.height);
        } else if (influence.time !== undefined) {
            renderDocument(T, influence.time);

            // Extract the palette from the current influence render.     
            for (var i = T.length - 1; i > 0; i--) {
                var t = T[i];

                if (t.layer == influence.layer && t.time == influence.time) {
                    // Extract colors from the current image.
                    var palette = thief.getPalette(t.image, 16);

                    // Remove duplicate colors.
                    palette = palette.filter(function (val, j, array) {
                        for (var i = 0; i < palette.length; i++) {
                            var c = palette[i];

                            // Remove all other values with equal color, except the color itself.
                            if (c !== val && c.equals(val)) {
                                return false;
                            }
                        }

                        return true;
                    });

                    // Assign the palette and update the UI.
                    $scope.palette = palette;

                    break;
                }
            }
        }
    };

    $scope.resetInfluence = function () {
        if ($scope.previousInfluence) {
            $scope.selectedInfluence = $scope.previousInfluence;
            $scope.previousInfluence = undefined;

            $scope.renderInfluence($scope.selectedInfluence);
        }
    };

    $scope.skipPrev = function () {
        var i = $scope.influences.indexOf($scope.selectedInfluence);

        if (0 < i) {
            $scope.selectedInfluence = $scope.influences[i - 1];

            renderDocument(T, $scope.selectedInfluence.time);
        }
    };

    $scope.togglePlay = function () {
        if ($scope.playloop) {
            $scope.pause();
        } else {
            $scope.play();
        }
    };

    $scope.play = function () {
        if (!$scope.playloop) {
            $scope.playloop = setInterval($scope.skipNext, 500);
        }
    };

    $scope.pause = function () {
        if ($scope.playloop) {
            clearInterval($scope.playloop);

            $scope.playloop = undefined;
        }
    };

    $scope.skipNext = function () {
        var i = $scope.influences.indexOf($scope.selectedInfluence) + 1;

        if (0 < i && i < $scope.influences.length) {
            $scope.selectedInfluence = $scope.influences[i];

            renderDocument(T, $scope.selectedInfluence.time);
        }

        if ($scope.playloop) {
            $scope.$digest();

            if (i == $scope.influences.length) {
                $scope.pause();
            }
        }
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
                gantt.position(scope.selectedInfluence.time);
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

explorerControllers.controller('SettingsController', function (api, $scope, $rootScope, $routeParams) {
    $scope.children = [];

    $scope.submit = function () {
        $scope.children.forEach(function (child) {
            if (child.submit) {
                child.submit();
            }
        });
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

    api.getAgents().then(function (data) {
        $scope.agents = data;
        $scope.agentForm.$setPristine();
    });

    this.submit = function () {
        if ($scope.agents.length > 0) {
            $scope.agents.forEach(function (agent) {
                api.setAgent(agent);
            });
        }
    };

    this.reset = function () {
        $scope.agentForm.reset();
    };
});

explorerControllers.controller('QueryController', function (api, $scope) {
	var defaultPrefixes = "\
@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .\n\
@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#> .\n\
@prefix art: <http://w3id.org/art/terms/1.0/> .\n\
@prefix foaf: <http://xmlns.com/foaf/0.1/> .\n\
@prefix nie: <http://www.semanticdesktop.org/ontologies/2007/01/19/nie#> .\n\
@prefix nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#> .\n\
@prefix prov: <http://www.w3.org/ns/prov#> .\n\
@prefix xsd: <http://www.w3.org/2001/XMLSchema#> .\n\n\n";
	
    $scope.queryString = defaultPrefixes;
	
	$scope.executeQuery = function()
	{	
		api.executeQuery($scope.queryString).then(function (data) {
			document.getElementById('result').innerHTML = data;
		});
	}
	
	$scope.resetQuery = function()
	{	
		$scope.queryString = defaultPrefixes;
	}
});
