(function (ng) {
    var umbraco = ng.module("umbraco");

    function urlPreviewController(scope, httpEvents, urlPreviewResource, editorState) {

        function posted(type) {
            if (type === "published") {
                scope.getUrls();
            }
        }

        httpEvents.register(posted);

        scope.$on("$destroy", function () {
            httpEvents.unregister(posted);
        });

        scope.getUrls = function () {
            urlPreviewResource.getUrls(editorState.current.id).then(function (response) {
                scope.urls = response.data;
            });
        };

        scope.getUrls();
    }

    umbraco.controller("UrlPreviewController", [
        "$scope",
        "content-events-service",
        "urlPreviewResource",
        "editorState",
        urlPreviewController
    ]);

}(angular));
