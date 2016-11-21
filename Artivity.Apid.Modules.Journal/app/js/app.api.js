var explorerApp = angular.module('explorerApp');

var apid = {
	endpointUrl: 'http://127.0.0.1:'.concat(port, '/artivity/api/1.0/')
};

explorerApp.factory('api', function ($http) {
	var endpoint = apid.endpointUrl;

	return {
		getAccounts: function () {
			return $http.get(endpoint + '/accounts').then(
				function (response) {
					return response.data;
				});
		},
		getAccountsWithFeature: function (featureUri) {
			return $http.get(endpoint + '/accounts?featureUri=' + featureUri).then(
				function (response) {
					return response.data;
				});
		},
		getAccountClients: function (featureUri) {
			return $http.get(endpoint + '/accounts/clients').then(
				function (response) {
					return response.data;
				});
		},
		getAccountClientsWithFeature: function (featureUri) {
			return $http.get(endpoint + '/accounts/clients?featureUri=' + featureUri).then(
				function (response) {
					return response.data;
				});
		},
		getAccountClient: function (clientUri) {
			return $http.get(endpoint + '/accounts/clients?clientUri=' + clientUri).then(
				function (response) {
					return response.data;
				});
		},
		getAccountClientStatus: function (sessionId, errorHandler) {
			return $http.get(endpoint + '/accounts/clients/status?sessionId=' + sessionId).then(
				function (response) {
					return response.data;
				},
				function (data) {
					if (errorHandler) {
						errorHandler(data);
					}
				});
		},
		authorizeAccount: function (parameter) {
			var p = serialize(parameter);

			return $http.get(endpoint + '/accounts/authorize?' + p).then(
				function (response) {
					return response.data;
				});
		},
		installAccount: function (sessionId) {
			return $http.get(endpoint + '/accounts/install?sessionId=' + sessionId).then(
				function (response) {
					return response.data;
				});
		},
		uninstallAccount: function (accountUri) {
			return $http.get(endpoint + '/accounts/uninstall?accountUri=' + accountUri).then(
				function (response) {
					return response.data;
				});
		},
		uploadArchive: function (accountUri, entityUri, parameter, data) {
			// Add accountUri and entityUri as query parameters to the URL.
			parameter.accountUri = accountUri;
			parameter.entityUri = entityUri;

			return $http.post(endpoint + '/accounts/upload?' + serialize(parameter), data).then(
				function (response) {
					return response.data;
				});
		},
		getAgents: function () {
			return $http.get(endpoint + '/agents').then(
				function (response) {
					return response.data;
				});
		},
		getAgentAssociations: function () {
			return $http.get(endpoint + '/agents/associations').then(
				function (response) {
					return response.data;
				});
		},
		setAgents: function (agents) {
			return $http.post(endpoint + '/agents', agents);
		},
		getAgent: function (entityUri) {
			return $http.get(endpoint + '/agents?entityUri=' + entityUri).then(
				function (response) {
					return response.data;
				});
		},
		getAgentIconUrl: function (associationUri) {
			return endpoint + '/agents/software/icon?uri=' + associationUri;
		},
		setAgent: function (data) {
			return $http.post(endpoint + '/agents', data);
		},
		installAgent: function (associationUri) {
			return $http.get(endpoint + '/agents/software/install?uri=' + associationUri).then(
				function (response) {
					console.log(response);

					return {
						success: true,
						error: ''
					};
				},
				function (response) {
					return {
						success: false,
						error: response
					};
				}
			);
		},
		uninstallAgent: function (associationUri) {
			return $http.get(endpoint + '/agents/software/uninstall?uri=' + associationUri).then(
				function (response) {
					return {
						success: true,
						error: ''
					};
				},
				function (response) {
					return {
						success: false,
						error: response
					};
				}
			);
		},
		getUser: function () {
			return $http.get(endpoint + '/agents/user').then(
				function (response) {
					return response.data;
				});
		},
		setUser: function (data) {
			return $http.post(endpoint + '/agents/user', data);
		},
		setUserPhoto: function (data) {
			return $http.post(endpoint + '/agents/user/photo', data);
		},
		getUserPhotoUrl: function () {
			return endpoint + '/agents/user/photo';
		},
		backupUserProfile: function (fileName) {
			return $http.get(endpoint + '/export/backup?fileName=' + fileName).then(
				function (response) {
					return response.data;
				});
		},
		getFile: function (entityUri) {
			return $http.get(endpoint + '/files?uri=' + entityUri).then(
				function (response) {
					return response.data;
				});
		},
		getRecentFiles: function () {
			return $http.get(endpoint + '/files/recent').then(
				function (response) {
					return response.data;
				});
		},
		getActivities: function (entityUri) {
			if (entityUri) {
				return $http.get(endpoint + '/activities?uri=' + entityUri).then(
					function (response) {
						return response.data;
					});
			} else {
				return $http.get(endpoint + '/activities').then(
					function (response) {
						return response.data;
					});
			}
		},
		getInfluences: function (entityUri) {
			return $http.get(endpoint + '/influences?uri=' + entityUri).then(
				function (response) {
					return response.data;
				});
		},
		getCanvases: function (entityUri, time) {
			if (time !== undefined) {
				return $http.get(endpoint + '/influences/canvas?uri=' + entityUri + '&timestamp=' + time).then(
					function (response) {
						return response.data;
					});
			} else {
				return $http.get(endpoint + '/influences/canvas?uri=' + entityUri).then(
					function (response) {
						return response.data;
					});
			}
		},
		getLayers: function (entityUri) {
			return $http.get(endpoint + '/influences/layers?uri=' + entityUri).then(
				function (response) {
					return response.data;
				});
		},
		hasThumbnail: function (entityUri) {
			return $http.get(endpoint + '/thumbnails?entityUri=' + entityUri + '&exists').then(
				function (response) {
					return response.data;
				});
		},
		getThumbnailUrl: function (entityUri) {
			return endpoint + '/thumbnails?entityUri=' + entityUri;
		},
		getRenderings: function (entityUri, time) {
			if (time !== undefined) {
				return $http.get(endpoint + '/renderings?uri=' + entityUri + '&timestamp=' + time).then(
					function (response) {
						return response.data;
					});
			} else {
				return $http.get(endpoint + '/renderings?uri=' + entityUri).then(
					function (response) {
						return response.data;
					});
			}
		},
		getRenderingUrl: function (entityUri) {
			return endpoint + '/renderings?uri=' + entityUri + '&file=';
		},
		getStats: function (entityUri, time) {
			if (time !== undefined) {
				return $http.get(endpoint + '/stats/influences?uri=' + entityUri + '&timestamp=' + time).then(
					function (response) {
						return response.data;
					});

			} else {
				return $http.get(endpoint + '/stats/influences?uri=' + entityUri).then(
					function (response) {
						return response.data;
					});
			}
		},
		getQueryResults: function (query) {
			return $http.post(endpoint + '/query', query).then(
				function (response) {
					return response.data;
				});
		},
		exportFile: function (entityUri, fileName) {
			return $http.get(endpoint + '/export?entityUri=' + entityUri + '&fileName=' + fileName).then(
				function (response) {
					return response.data;
				});
		},
		postComment: function (comment) {
			return $http.post(endpoint + '/activities/comments', comment).then(
				function (response) {
					return response.data;
				});
		}
	};
});