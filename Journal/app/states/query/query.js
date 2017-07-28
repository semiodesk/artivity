(function () {
	angular.module('app').controller('QueryStateController', QueryStateController);

    QueryStateController.$inject = ['$rootScope'];

	function QueryStateController($rootScope) {
		// YASGUI heavily modifies the HTML and adds fragments to the URL.
		// Therefore we detach the root scope to prevent angular from navigating 
		// and trying to update the scope where it should not.
		$rootScope.$destroy();
	};
})();