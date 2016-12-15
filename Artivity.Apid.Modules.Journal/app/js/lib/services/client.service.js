angular.module('explorerApp').factory('clientService', function() {
	return {
		pollServiceState : function (api, sessionId, handleClientStateChange) {
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
		}
	}
});