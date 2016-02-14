//adds the resource to umbraco.resources module:
angular.module('umbraco.resources').factory('urlPreviewResource',
    function ($q, $http) {
        //the factory object returned
        return {
            getUrls: function (id) {
                return $http.get("backoffice/UrlPreview/UrlPreviewApi/GetUrls?id=" + id);
            }
        };
    }
);