var explorerControllers = angular.module('explorerControllers', ['ngInputModified', 'ui.bootstrap']);

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

explorerControllers.controller('FileViewController', function (api, $scope, $location, $routeParams, $translate) {
	var fileUri = $location.search().uri;

	$scope.entity = {
		uri: fileUri
	};

	// File metadata
	$scope.file = {};

	api.getFile(fileUri).then(function (data) {
		$scope.file = data;

		console.log("Entity: ", $scope.file);
	});

	// Agent metadata
	$scope.agent = {
		iconUrl: ''
	};

	api.getAgent(fileUri).then(function (data) {
		$scope.agent = data;
		$scope.agent.iconUrl = api.getAgentIconUrl(data.agent);

		console.log("Agent: ", $scope.agent);
	});

	// Load the user data.
	$scope.user = {};

	api.getUser().then(function (data) {
		$scope.user = data;
		$scope.user.photoUrl = api.getUserPhotoUrl();

		console.log("User: ", $scope.user);
	});

	// RENDERING
	var canvas = document.getElementById('canvas');

	var renderer = new DocumentRenderer(canvas, api.getRenderingUrl(fileUri));

	// Layers in the file
	$scope.layers = [];

	var getLayers = function (influence) {
		var time = new Date(influence.time);
		var layers = [];

		renderer.layerCache.getAll(time, function (layer) {
			layer.label = layer.getLabel(time);
			layers.push(layer);
		});

		$scope.layers = layers;
	};

	// INFLUENCES
	$scope.influences = [];
	$scope.previousInfluence;
	$scope.selectedInfluence;

	// ACTIVITIES
	$scope.activities = [];

	$scope.loadActivities = function () {
		api.getActivities(fileUri).then(function (data) {
			console.log("Loaded activities: ", data);

			$scope.activities = data;

			if (data.length > 0) {
				api.getInfluences(fileUri).then(function (data) {
					console.log("Loaded influences:", data.length, data);

					$scope.influences = data;

					if (data.length > 0) {
						$scope.previewInfluence(data[0]);

						// Canvases in the file.
						api.getCanvases(fileUri).then(function (data) {
							renderer.canvasCache.load(data, function () {
								console.log("Loaded canvases: ", renderer.canvasCache);

								$scope.renderInfluence($scope.selectedInfluence);

								api.getLayers(fileUri).then(function (data) {
									renderer.layerCache.load(data, function (layers) {
										console.log("Loaded layers: ", layers);

										// Trigger loading the bitmaps.
										api.getRenderings(fileUri).then(function (data) {
											renderer.renderCache.load(data, function () {
												console.log("Loaded renderings: ", renderer.renderCache);

												$scope.renderInfluence($scope.selectedInfluence);
											});
										});
									});
								});
							});
						});

						// Add the loaded influences to the activities for easier acccess in the frontend.
						var i = 0;

						var activity = $scope.activities[i];
						activity.showDate = true;
						activity.influences = [];
						activity.startTime = new Date(activity.startTime);
						activity.endTime = new Date(activity.endTime);
						activity.totalTime = moment(activity.endTime) - moment(activity.startTime);

						// NOTE: We assume that the influences and activities are ordered by descending time.
						for (var j = 0; j < data.length; j++) {
							var influence = data[j];

							while (activity.uri !== influence.activity && i < $scope.activities.length - 1) {
								var a = $scope.activities[++i];

								var t1 = new Date(a.startTime);
								var t2 = activity.startTime;

								a.showDate = t1.getDay() != t2.getDay() || t1.getMonth() != t2.getMonth() || t1.getYear() != t2.getYear();

								activity = a;
								activity.influences = [];
							}

							activity.isComment = influence.comment != null;

							if (influence.activity === activity.uri) {
								activity.influences.push(influence);
							}

							if (activity.endTime < activity.maxTime) {
								activity.endTime = activity.maxTime;
							}

							activity.startTime = new Date(activity.startTime);
							activity.endTime = new Date(activity.endTime);
							activity.totalTime = moment(activity.endTime) - moment(activity.startTime);
						}
					}
				});
			}
		})
	};

	$scope.loadActivities();

	$scope.selectInfluence = function (influence) {
		$scope.selectedInfluence = influence;
		$scope.previousInfluence = undefined;
	};

	$scope.previewInfluence = function (influence) {
		$scope.previousInfluence = $scope.selectedInfluence;
		$scope.selectedInfluence = influence;

		if (influence.time !== undefined) {
			$scope.renderInfluence(influence);

			// Note: this is experimental.
			//var heatmap = new HeatmapRenderer(canvas);
			//heatmap.render($scope.influences);
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
	$scope.playing = false;

	var playloop = undefined;

	$scope.togglePlay = function () {
		if (playloop) {
			$scope.pause();
		} else {
			$scope.play();
		}
	};

	$scope.play = function () {
		var end = $scope.influences.indexOf($scope.selectedInfluence) === 0;

		if (!playloop && !end) {
			playloop = setInterval($scope.skipNext, 500);

			$scope.playing = playloop !== undefined;
		}
	};

	$scope.pause = function () {
		console.log(playloop);

		if (playloop) {
			clearInterval(playloop);

			playloop = undefined;

			$scope.playing = playloop !== undefined;

			$scope.$digest();
		}
	};

	$scope.skipPrev = function () {
		if ($scope.influences === undefined) {
			return;
		}

		var i = $scope.influences.indexOf($scope.selectedInfluence) + 1;

		if (0 < i && i < $scope.influences.length) {
			$scope.selectedInfluence = $scope.influences[i];

			console.log($scope.selectedInfluence.offsetTop);

			$scope.renderInfluence($scope.selectedInfluence);
		}

		if (playloop) {
			$scope.$digest();

			if (i === $scope.influences.length) {
				$scope.pause();
			}
		}
	};

	$scope.skipNext = function () {
		var i = $scope.influences.indexOf($scope.selectedInfluence);

		if (0 < i && i < $scope.influences.length) {
			$scope.selectedInfluence = $scope.influences[i - 1];

			$scope.renderInfluence($scope.selectedInfluence);
		}

		if (playloop) {
			$scope.$digest();

			if (i === 0) {
				$scope.pause();
			}
		}
	};

	$scope.historyKeyDown = function (e) {
		if (e.which == 40) { // Arrow key down
			$scope.skipPrev();

			e.preventDefault();
		} else if (e.which === 38) { // Arrow up
			$scope.skipNext();

			e.preventDefault();
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

	// EXPORT
	$scope.exportFile = function () {
		api.exportFile(fileUri, $scope.file.label);
	};

	// PRINT LABEL

	var getChangedProperty = function (influence) {
		for (var i = 0; i < influence.changes.length; i++) {
			var change = influence.changes[i];

			if (change.entityType !== 'http://w3id.org/art/terms/1.0/Layer' && change.property) {
				return change.property;
			}
		}

		return '';
	};

	$scope.getLabel = function (influence) {
		var key;

		switch (influence.type) {
		case 'http://www.w3.org/ns/prov#Generation':
			{
				key = 'FILEVIEW.http://www.w3.org/ns/prov#Generation';
				break;
			}
		case 'http://www.w3.org/ns/prov#Invalidation':
			{
				key = 'FILEVIEW.http://www.w3.org/ns/prov#Invalidation';
				break;
			}
		default:
			{
				// TODO: pluralize
				key = 'FILEVIEW.' + getChangedProperty(influence);
				break;
			}
		}

		var result;

		// Only translate if we actually found a property in the previous loop.
		if (key && key !== 'FILEVIEW.') {
			result = $translate.instant(key)
		} else if (influence.description) {
			result = influence.description;
		} else {
			result = $translate.instant('FILEVIEW.' + influence.type);
		}

		return result;
	};

	$scope.getIcon = function (influence) {
		switch (influence.type) {
			/*
			case 'http://www.w3.org/ns/prov#Generation':
				return 'zmdi-plus';
			case 'http://www.w3.org/ns/prov#Invalidation':
				return 'zmdi-delete';
			*/
		case 'http://www.w3.org/ns/prov#Derivation':
			return 'zmdi-floppy';
		case 'http://www.w3.org/ns/prov#Undo':
			return 'zmdi-undo';
		case 'http://www.w3.org/ns/prov#Redo':
			return 'zmdi-redo';
		case 'http://w3id.org/art/terms/1.0/Save':
			return 'zmdi-floppy';
		case 'http://w3id.org/art/terms/1.0/SaveAs':
			return 'zmdi-floppy';
		}

		/*
		var property = getChangedProperty(influence);

		if (property !== '') {
			switch (property) {
			case 'http://w3id.org/art/terms/1.0/position':
				return 'zmdi-arrows';
			case 'http://w3id.org/art/terms/1.0/hadBoundaries':
				return 'zmdi-border-style';
			case 'http://www.w3.org/2000/01/rdf-schema#label':
				return 'zmdi-format-color-text';
			case 'http://w3id.org/art/terms/1.0/textValue':
				return 'zmdi-format-color-text';
			case 'http://w3id.org/art/terms/1.0/strokeWidth':
				return 'zmdi-border-color';
			}
		}
		*/

		return 'zmdi-brush';
	};

	$scope.comment = {
		text: ''
	};

	$scope.updateComment = function () {
		if (!$scope.comment.startTime) {
			$scope.comment.activity = $scope.activities[0].uri;
			$scope.comment.agent = $scope.user.Uri;
			$scope.comment.entity = $scope.entity.uri;
			$scope.comment.startTime = new Date();

			console.log("Start comment: ", $scope.comment);
		}
	};

	$scope.resetComment = function (clearText) {
		if (clearText) {
			$scope.comment.text = '';
		}

		if ($scope.comment.text === '') {
			$scope.comment.startTime = undefined;
			$scope.comment.endTime = undefined;
		}

		console.log("Reset comment: ", $scope.comment);
	};

	$scope.postComment = function () {
		$scope.comment.endTime = new Date();

		if ($scope.comment.agent && $scope.comment.text !== '') {
			console.log("Post comment: ", $scope.comment);

			api.postComment($scope.comment).then(function (data) {
				$scope.comment = "";

				$scope.loadActivities();
			});
		}
	};

}).directive('ngEnter', function () {
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
}).directive('ngEsc', function () {
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
}).directive('artTimeline', function () {
	return {
		template: '\
		<div class="timeline">\
			<div class="timeline-control"> \
				<div class="position"><label></label></div> \
				<div class="duration"><label></label></div> \
				<div class="track-col"> \
					<div class="track-container"> \
						<div class="track"></div> \
						<div class="track-preview"></div> \
						<div class="track-indicator"></div> \
						<div class="thumb draggable"></div> \
					</div> \
					<div class="comments"></div> \
					<div class="activities"></div> \
				</div> \
			</div> \
		</div>',
		link: function (scope, element, attributes) {
			var timeline = new TimelineControl(element);

			timeline.setActivities(getValue(scope, attributes.artActivities));
			timeline.setInfluences(getValue(scope, attributes.artInfluences));

			timeline.selectedInfluenceChanged = function (influence) {
				scope.previewInfluence(influence);
			};

			scope.$watchCollection(attributes.artActivities, function () {
				timeline.setActivities(getValue(scope, attributes.artActivities));
			});

			scope.$watchCollection(attributes.artInfluences, function () {
				timeline.setInfluences(getValue(scope, attributes.artInfluences));
			});

			scope.$watch('selectedInfluence', function () {
				if (scope.selectedInfluence !== undefined) {
					timeline.setPosition(scope.selectedInfluence);
				}
			});
		}
	}
}).directive('artChartDonut', function () {
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
}).directive('artStyleBinder', function () {
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

explorerControllers.controller('SettingsController', function (api, $scope, $location, $rootScope, $routeParams) {
	$scope.children = [];

	$scope.submit = function () {
		$scope.children.forEach(function (child) {
			if (child.submit) {
				child.submit();
			}
		});
	};

	$scope.$watch('agent.iconUrl', function () {
		if (scope.agent.iconUrl !== "") {
			timeline.setUserPhotoUrl(scope.user.photoUrl);
		}
	});

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
						uri: agent.Manifest.AgentUri,
						name: agent.Manifest.DisplayName,
						color: agent.Manifest.DefaultColor,
						associationUri: agent.AssociationUri,
						iconSrc: api.getAgentIconUrl(agent.Manifest.AgentUri),
						softwareInstalled: agent.IsSoftwareInstalled,
						softwareVersion: agent.DetectedVersion,
						pluginInstalled: agent.IsPluginInstalled,
						pluginVersion: agent.Manifest.PluginVersion,
						pluginEnabled: agent.IsPluginEnabled
					});
				}
			}

			$scope.agentForm.$setPristine();
		});
	}

	$scope.reload();

	this.submit = function () {
		if ($scope.agents.length > 0) {
			api.setAgents($scope.agents);
		}
	};

	this.reset = function () {
		$scope.agentForm.reset();
	};
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
PREFIX dces: <http://purl.org/dc/elements/1.1/>\n\n\
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