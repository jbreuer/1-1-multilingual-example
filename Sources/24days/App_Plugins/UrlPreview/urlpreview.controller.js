(function (ng) {
    var umbraco = ng.module("umbraco");

    function urlPreviewController(scope, httpEvents, urlPreviewResource, editorState) {

        function posted(type) {
            if (type === "saved") {
                // After the save button has been pressed we want the frontend website to go into preview mode with the lastest changes.
                // This means for the content editor the website is in preview, but it will not affect other users.
                scope.updatePreview();
            }

            if (type === "published") {
                // After a node has been published we just need to get the new URLs.
                scope.getUrls();
            }
        }

        httpEvents.register(posted);

        scope.$on("$destroy", function () {
            httpEvents.unregister(posted);
        });

        scope.getUrls = function () {
            urlPreviewResource.getUrls(editorState.current.id).then(function (response) {
                console.log(response.data);
                scope.isPreview = response.data.IsPreview;
                scope.urls = response.data.Urls;
            });
        };

        scope.updatePreview = function () {
            scope.updateStatus = "- updating preview...";
            urlPreviewResource.updatePreview().then(function (response) {
                if (response.status === 200) {
                    scope.updateStatus = "- done";

                    // Update the URLs after the preview has been updated.
                    scope.getUrls();
                }
                else {
                    scope.updateStatus = "- failed";
                }
            });
        }

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
