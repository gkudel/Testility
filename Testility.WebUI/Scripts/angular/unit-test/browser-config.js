angular.module('browser.config', [])
    .factory('config', function ($http, $q) {
        return {
            getDataSource: function () {
                var d = $q.defer();
                $http.get('/api/Solutions').success(function (response) {
                    d.resolve(response);
                });
                return d.promise;
            },
            MultiSelection: false,
            PrintElement: function (element) { return element.Name; }
        };
    });