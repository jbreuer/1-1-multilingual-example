angular.module('umbraco.directives').directive('languageDropdown',
    ['Extract.Resource',
    function (extractResource) {
        return {
            restrict: 'E',
            replace: true,
            transclude: 'true',
            templateUrl: '/App_Plugins/Extract/languagedropdown.directive.html',       
            link: function (scope, elem) {
                extractResource.getLanguages().success(function (data) {
                    scope.languages = data;
                });
            }
        };
    }
]);