angular.module('umbraco.resources')
    .factory('Extract.Resource', function ($http) {
        return {
            getLanguages: function () {
                return $http.get("/umbraco/backoffice/api/extractapi/getlanguages");
            },
            extractContent: function (extract) {
                return $http.post('/umbraco/backoffice/api/extractapi/extractcontent?nodeid=' + extract.nodeId +
                    '&sourceLanguage=' + extract.sourceLanguage +
                    '&targetLanguage=' + extract.targetLanguage +
                    '&includeSubNodes=' + extract.includeSubNodes);
            }
        };
    });