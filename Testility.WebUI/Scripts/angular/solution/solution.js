angular.module('solutionApp', ['browser'])
    .factory('config', function ($http, $q) {
        return {
            getDataSource: function () {
                var d = $q.defer();
                $http.get('/api/Reference').success(function (response) {
                    d.resolve(response);
                });
                return d.promise;
            },
            MultiSelection: true,
            PrintElement: function (element) { return element.Name; },
            title: 'References',
            isResult: true,
            resultName: 'References',
            getResultValue: function (item) {
                return item.Id;
            }
        };
    });