//adds the resource to umbraco.resources module:
angular.module('umbraco.resources').factory('urlPreviewResource',
    function ($q, $http) {
        //the factory object returned
        return {
            getUrls: function (id) {
                // Get the URLs from the frontend so we can also get URLs that are in preview.
                return $http.get("/umbraco/surface/urlpreview/geturls?id=" + id);
            },
            updatePreview: function () {
                // Updating the preview happens in UmbracoAuthorizedApiController so it can only be triggered if a user is in the backoffice.
                return $http.get("/umbraco/backoffice/api/urlpreviewapi/updatepreview");
            }
        };
    }
);