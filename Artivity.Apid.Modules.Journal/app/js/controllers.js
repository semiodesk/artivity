var explorerControllers = angular.module('explorerControllers', ['ngInputModified']);

explorerControllers.controller('FileListController', function (api, $scope) {
	api.getUser().then(function (data) {
		$scope.user = data;
	});

	api.getRecentFiles().then(function (data) {
		$scope.files = data;
	});

	$scope.encodeBase64 = function(url) {
		var result = Base64.encode(url, true);

		return result;
	};

	$scope.getFileName = getFileName;
});

explorerControllers.controller('FileDetailController', function (api, $scope, $rootScope, $routeParams) {
	var fileUrl = Base64.decode($routeParams.fileUrl);

	$scope.fileName = getFileName(fileUrl);
	$scope.agent = {};

	$scope.activities = [];
	$scope.selectedActivity;

	$scope.influences = [];
	$scope.selectedInfluence = { name: "", time: new Date() };
	$scope.previousInfluence = undefined;

	$scope.playloop = undefined;

	api.getAgent(fileUrl).then(function (data) {
		$scope.agent = data;
	});

	api.getActivities(fileUrl).then(function (data) {
		$scope.activities = data;

		if(data.length > 0) {
			$scope.selectedActivity = data[0];
		}
	});

	api.getInfluences(fileUrl).then(function (data) {
		$scope.influences = data;

		if(data.length > 0) {
			$scope.selectedInfluence = data[0];
		}
	});

	$scope.getFormattedTime = function(time) {
		return moment(time).format('hh:mm:ss');
	};

	$scope.getFormattedDate = function(time) {
		return moment(time).format('dddd, Do MMMM YYYY');
	};

	$scope.getFormattedTimeFromNow = function(time) {
		var result = moment(time).fromNow();

		return result;
	};

	$scope.selectInfluence = function(influence) {
		$scope.selectedInfluence = influence;
		$scope.previousInfluence = undefined;
	};

	$scope.previewInfluence = function(influence) {
		$scope.previousInfluence = $scope.selectedInfluence;
		$scope.selectedInfluence = influence;
	}

	$scope.resetInfluence = function() {
		if($scope.previousInfluence) {
			$scope.selectedInfluence = $scope.previousInfluence;
			$scope.previousInfluence = undefined;
		}
	}

	$scope.skipPrev = function () {
		var i = $scope.influences.indexOf($scope.selectedInfluence);

		if(0 < i) {
			$scope.selectedInfluence = $scope.influences[i - 1];
		}
	};

	$scope.togglePlay = function () {
		if($scope.playloop) {
			$scope.pause();
		}
		else {
			$scope.play();
		}
	};

	$scope.play = function() {
		if(!$scope.playloop) {
			$scope.playloop = setInterval($scope.skipNext, 500);
		}
	};

	$scope.pause = function() {
		if($scope.playloop) {
			clearInterval($scope.playloop);

			$scope.playloop = undefined;
		}
	};

	$scope.skipNext = function () {
		var i = $scope.influences.indexOf($scope.selectedInfluence) + 1;

		if(0 < i && i < $scope.influences.length) {
			$scope.selectedInfluence = $scope.influences[i];
		}

		if($scope.playloop) {
			$scope.$digest();

			if(i == $scope.influences.length) {
				$scope.pause();
			}
		}
	};
})
.directive('ganttChart', function () {
	return {
		template: '<div class="chart"><svg class="canvas"></svg></div>',
		link: function (scope, element, attributes) {
			var gantt = d3.gantt();
			gantt.init(element, getValue(scope, attributes.chartData));

			scope.$watchCollection(attributes.chartData, function () {
				gantt.update(getValue(scope, attributes.chartData));
			});

			scope.$watch('selectedInfluence', function() {
				gantt.position(scope.selectedInfluence.time);
			});

			$(window).resize(function(){
				var container = $('.col-playback-chart');
				gantt.width(container.innerWidth());
				gantt.update();
			});

			$(document).ready(function() {
				var container = $('.col-playback-chart');
				gantt.width(container.innerWidth());
				gantt.update();
			});
		}
	}
});

explorerControllers.controller('SettingsController', function (api, $scope, $rootScope, $routeParams) {
	$scope.children = [];

	$scope.submit = function() {
		$scope.children.forEach(function (child) {
			if(child.submit) { child.submit(); }
		});
	};

	$scope.reset = function() {
		$scope.children.forEach(function (child) {
			if(child.reset) { child.reset(); }
		});
	};
}).directive("ngPhotoPicker", function () {
	return {
		link: function (scope, element, attributes) {
			element.bind("change", function (changeEvent) {
				scope.$apply(function () {
					// TODO: This does not work. Browsers do not allow to generate local file system URLs.
					scope.user.Photo = changeEvent.target.files[0];
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

			button.on('changeColor', function(e){
				if(e.color==null) {
					//when select transparent color
					//$('.color-fill-icon', btn).addClass('colorpicker-color');
				} else {
					//$('.color-fill-icon', btn).removeClass('colorpicker-color');
					indicator.css('background-color', e.color);
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

explorerControllers.controller('UserSettingsController', function(api, $scope) {
	$scope.$parent.children.push(this);

	api.getUser().then(function (data) {
		$scope.model = data;
		$scope.userForm.$setPristine();
	});
	
	this.submit = function() {
		api.setUser($scope.model);
	};

	this.reset = function() {
		$scope.userForm.reset();
	};
});

explorerControllers.controller('AgentSettingsController', function(api, $scope) {
	$scope.$parent.children.push(this);

	api.getAgents().then(function (data) {
		$scope.model = data;
		$scope.agentForm.$setPristine();
	});

	this.submit = function() {
	};

	this.reset = function() {
		$scope.agentForm.reset();
	};
});