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
	var fileUrl = $location.search().fileUrl;
	
	$scope.fileName = getFileName(fileUrl);
	$scope.agent = {};

	$scope.activities = [];
	$scope.selectedActivity;

	$scope.influences = [];
	$scope.previousInfluence = undefined;
	$scope.selectedInfluence = {
		name: "",
		time: new Date()
	};

	$scope.playloop = undefined;

	var buffer = document.getElementsByClassName('buffer')[0];

	api.getAgent(fileUrl).then(function (data) {
		$scope.agent = data;
	});

	api.getActivities(fileUrl).then(function (data) {
		$scope.activities = data;

		if (data.length > 0) {
			$scope.selectedActivity = data[0];
		}
	});

	api.getInfluences(fileUrl).then(function (data) {
		$scope.influences = data;

		if (data.length > 0) {
			$scope.selectedInfluence = data[0];
			
			$scope.renderInfluence($scope.selectedInfluence);
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

	$scope.selectInfluence = function (influence) {
		$scope.selectedInfluence = influence;
		$scope.previousInfluence = undefined;
	};

	$scope.previewInfluence = function (influence) {
		$scope.previousInfluence = $scope.selectedInfluence;
		$scope.selectedInfluence = influence;

		$scope.renderInfluence(influence);
	};
	
	$scope.renderInfluence = function (influence) {		
		if (influence === undefined) {
			var context = buffer.getContext('2d');

			context.clearRect(0, 0, buffer.width, buffer.height);
		} else if(influence.time !== undefined) {			
			// A list with all loaded bitmaps.
			var T = [];

			// Loads a single bitmap and pushes it into the list when loading is complete.
			var load = function (thumbnails, i, complete) {
				var t = new Image();

				t.onload = function () {
					T.push({
						image: t,
						x: thumbnails[i].x,
						y: thumbnails[i].y,
						w: t.width,
						h: t.height
					});

					complete(thumbnails, i);
				};

				t.src = api.getThumbnailUrl(thumbnails[i].thumbnailUrl);
			};

			// Renders all loaded images onto the canvas.
			var render = function (thumbnails, i) {
				var context = buffer.getContext('2d');

				context.clearRect(0, 0, buffer.width, buffer.height);

				T.forEach(function (t) {
					console.log(t.x, -t.y, t.w, t.h);
					
					context.drawImage(t.image, t.x, -t.y, t.w, t.h);
				});
			};

			// Trigger loading the bitmaps of the influence.
			api.getThumbnails(fileUrl, influence.time).then(function (thumbnails) {
				loadItems(thumbnails, load, render);
			});
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
			api.setUserPhoto($scope.userPhoto);
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
				console.log(agent.ColourCode);
				api.setAgent(agent);
			});
		}
	};

	this.reset = function () {
		$scope.agentForm.reset();
	};
});