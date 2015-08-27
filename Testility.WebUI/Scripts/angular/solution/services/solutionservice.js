angular.module('Testility')
    .factory('solutionservice', ['$http', '$location', '$q', function ($http, $location, $q) {

        var service = {
            get: function () {
                var d = $q.defer();
                var array = /Solution\/Edit\/(\d+)/.exec($location.absUrl());
                var id = undefined;
                if (array && array.length > 1) {
                    id = array[1];
                }

                if (id) {
                    $http.get('/api/Solution/' + id)
                        .success(function (response) {
                            d.resolve(response);
                        })
                        .error(function (data, status) {                                
                            d.reject(data);
                        });
                } else {
                    d.resolve(this.empty());
                }
                return d.promise;
            },
            submit: function(Solution) {
                var d = $q.defer();
                $http.post('/api/Solution/', JSON.stringify(Solution))
                    .success(function (response) {
                        d.resolve(response);
                    })
                    .error(function (data, status) {
                        d.reject(data);
                    });
                return d.promise;
            },
            empty: function () {
                return {
                    Id: 0,
                    Name: '',
                    Language: 0,
                    References: [],
                    Items: []
                };
            },
            compile: function (Solution) {
                var d = $q.defer();
                $http.post('/api/Compiler/', JSON.stringify(Solution))
                    .success(function (response) {
                        d.resolve(response);
                    })
                    .error(function (data, status) {
                        d.reject(data);
                    });
                return d.promise;
            }
        };
        return service;
    }]);