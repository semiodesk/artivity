(function () {
	angular.module('app').controller('QueryController', QueryController);

	function QueryController($rootScope, $scope) {
		var t = this;

		// YASGUI heavily modifies the HTML and adds fragments to the URL.
		// Therefore we detach the root scope to prevent angular from navigating 
		// and trying to update the scope where it should not.
		$rootScope.$destroy();
	};
})();