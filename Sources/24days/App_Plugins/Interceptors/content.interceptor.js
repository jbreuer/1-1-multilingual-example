(function (ng) {
    var umbraco = ng.module("umbraco");

    function ContentEventsService() {
        var listeners = [];

        this.register = function (listener) {
            listeners.push(listener);
        }

        this.unregister = function (listener) {
            var index = listeners.indexOf(listener);
            listeners.splice(index, 1);
        }

        this.raise = function (type) {
            var i;
            for (i = 0; i < listeners.length; i++) {
                listeners[i](type);
            }
        }
    }

    function createInterceptor(httpEvents, q) {
        return {
            'response': function (response) {
                if (response.config.url === "/umbraco/backoffice/UmbracoApi/Content/PostSave") {
                    if (response.config.data.value.action === "publish") {
                        httpEvents.raise("published");
                    } else {
                        httpEvents.raise("saved");
                    }
                }

                return response || q.when(response);
            }
        };
    }

    umbraco.service("content-events-service", [
        ContentEventsService
    ]);

    umbraco.factory("content-http-interceptor", [
        "content-events-service",
        "$q",
        createInterceptor
    ]);

    umbraco.config([
        "$httpProvider", function (httpProvider) {
            httpProvider.interceptors.push("content-http-interceptor");
        }
    ]);


}(angular));