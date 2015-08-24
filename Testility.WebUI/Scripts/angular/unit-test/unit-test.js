angular.module('Testility')
    .factory('config', function ($http, $q) {
        return {
            getDataSource: function () {
                var d = $q.defer();
                $http.get('/api/Solution').success(function (response) {
                    d.resolve(response);
                });
                return d.promise;
            },
            MultiSelection: false,
            PrintElement: function (element) { return element.Name; },
            title: 'Solutions'
        };
    });