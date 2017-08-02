(function () {
	angular.module('app').factory('api', api);

	api.$inject = ['$http'];

	function api($http) {
		var endpoint = apid.endpointUrl;

		var serialize = function (obj, prefix) {
			var str = [];

			for (var p in obj) {
				if (obj.hasOwnProperty(p)) {
					var k = prefix ? prefix + "[" + p + "]" : p,
						v = obj[p];
					str.push(typeof v == "object" ?
						serialize(v, k) :
						encodeURIComponent(k) + "=" + encodeURIComponent(v));
				}
			}

			return str.join("&");
		}

		return {
			get: function (url, config) {
				return $http.get(url, config);
			},
			put: function (url, data, config) {
				return $http.put(url, data, config);
			},
			post: function (url, data, config) {
				return $http.post(url, data, config);
			},
			delete: function (url, config) {
				return $http.delete(url, config);
			},
			getAccounts: function () { // +publish-file-controller +art-settings-accounts
				return $http.get(endpoint + '/accounts').then(
					function (response) {
						return response.data;
					});
			},
			getAccountsWithFeature: function (featureUri) { // +publish-file-controller 
				return $http.get(endpoint + '/accounts?featureUri=' + featureUri).then(
					function (response) {
						return response.data;
					});
			},
			getAccountClients: function (featureUri) { // +add-account-dialog +client.service
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
			getAgents: function () { // +art-settings-apps 
				return $http.get(endpoint + '/agents/software').then(
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
				return $http.post(endpoint + '/agents/software', agents);
			},
			getAgent: function (entityUri) { // +file-view-controller +file-preview-controller
				return $http.get(endpoint + '/agents?entityUri=' + entityUri).then(
					function (response) {
						return response.data;
					});
			},
			getAgentIconUrl: function (associationUri) { // +art-settings-apps +file-preview-controller
				return endpoint + '/agents/software/icon?uri=' + associationUri;
			},
			setAgent: function (data) { // +art-settings-apps
				return $http.post(endpoint + '/agents', data);
			},
			installAgent: function (associationUri) { // +art-settings-apps
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
			uninstallAgent: function (associationUri) { // +art-settings-apps
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
			getAccountOwner: function () {
				return $http.get(endpoint + '/agents/users?role=AccountOwnerRole').then(
					function (response) {
						if (response.data.length === 1) {
							return response.data[0];
						}
					});
			},
			putUser: function (data) { // +art-settings-user
				return $http.put(endpoint + '/agents/users', data);
			},
			getUserPhotoUrl: function (uri) { // +art-settings-user +agents.service +file-preview-controller
				return endpoint + '/agents/users/photo?agentUri=' + uri;
			},
			putUserPhoto: function (uri, data) { // +art-settings-user
				return $http.put(endpoint + '/agents/users/photo?agentUri=' + uri, data);
			},
			backupUserProfile: function (fileName) { // +art-settings-user
				return $http.get(endpoint + '/export/backup?fileName=' + fileName).then(
					function (response) {
						return response.data;
					});
			},
			getUserProfileBackupStatus: function (id) { // +art-settings-user
				return $http.get(endpoint + '/export/backup/status?taskId=' + id).then(
					function (response) {
						return response.data;
					});
			},
			editFile: function (fileUri) {
				return $http.get(endpoint + '/files/edit?fileUri=' + fileUri).then(
					function (response) {
						return response.data;
					});
			},
			getFile: function (entityUri) { // file-preview-controller
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
			getLatestFileRevision: function (fileUri) {
				return $http.get(endpoint + '/files/revisions/latest?fileUri=' + fileUri).then(
					function (response) {
						return response.data;
					});
			},
			getProjectFiles: function (projectUri) {
				return $http.get(endpoint + '/projects/files?projectUri=' + projectUri).then(
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
					return $http.get(endpoint + '/influences/canvas?uri=' + encodeURIComponent(entityUri) + '&timestamp=' + time).then(
						function (response) {
							return response.data;
						});
				} else {
					return $http.get(endpoint + '/influences/canvas?uri=' + encodeURIComponent(entityUri)).then(
						function (response) {
							return response.data;
						});
				}
			},
			getCanvasRenderingsFromEntity: function (entityUri) { // +art-document-history-viewer
				return $http.get(endpoint + '/renderings/canvases?entity=' + encodeURIComponent(entityUri)).then(
					function (response) {
						return response.data;
					});
			},
			getLayers: function (entityUri) {
				return $http.get(endpoint + '/influences/layers?uri=' + entityUri).then(
					function (response) {
						return response.data;
					});
			},
			hasThumbnail: function (entityUri) {
				return $http.get(endpoint + '/renderings/thumbnails?entityUri=' + entityUri + '&exists').then(
					function (response) {
						return response.data;
					});
			},
			getThumbnailUrl: function (entityUri) {
				return endpoint + '/renderings/thumbnails?entityUri=' + encodeURIComponent(entityUri);
			},
			getRenderings: function (entityUri, time) {
				if (time !== undefined) {
					return $http.get(endpoint + '/renderings?uri=' + encodeURIComponent(entityUri) + '&timestamp=' + time).then(
						function (response) {
							return response.data;
						});
				} else {
					return $http.get(endpoint + '/renderings?uri=' + encodeURIComponent(entityUri)).then(
						function (response) {
							return response.data;
						});
				}
			},
			getRenderingUrl: function (entityUri) { // +art-document-history-viewer
				return endpoint + '/renderings?uri=' + encodeURIComponent(entityUri) + '&file=';
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
				return $http.get(endpoint + '/export?entityUri=' + encodeURIComponent(entityUri) + '&fileName=' + fileName).then(
					function (response) {
						return response.data;
					});
			},
			setRunSetup: function (enabled) { // +setup.controller
				return $http.post(endpoint + '/setup', {
					runSetup: enabled
				}).then(
					function (response) {
						return response.data;
					});
			},
			synchronize: function () { // +sync.service
				return $http.get(endpoint + '/sync').then(
					function (response) {
						return response.data;
					});
			}
		};
	}
})();