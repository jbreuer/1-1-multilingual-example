angular.module('umbraco')
    .controller('Extract.MenuActionController',
        ['$scope', 'navigationService', 'notificationsService', 'Extract.Resource',
        function ($scope, navigationService, notificationsService, extractResource) {

            $scope.extract = {
                nodeId: $scope.currentNode.id,
                includeSubNodes: false
            };

            $scope.sending = false;

            $scope.send = function () {
                if ($scope.extract.sourceLanguage === $scope.extract.targetLanguage) {
                    notificationsService.warning('Warning', 'Source and target language can not be equal');
                    return;
                }

                $scope.sending = true;
                extractResource.extractContent($scope.extract)
                    .then(function (response) {
                        if (response.data.Data.Error === '') {
                            notificationsService.success('Extract succeeded');
                            console.log(response.data.Data.Xml);
                            navigationService.hideNavigation();
                        } else {
                            notificationsService.error('Error extracting : ' + response.data.Data.Error);
                            $scope.sending = false;
                        }
                    });

            };
        }]);