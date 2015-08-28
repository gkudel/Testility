angular.module('Testility')
    .factory('solutionservice', ['$http', '$location', 'qSpiner', function ($http, $location, qSpiner) {

        var service = {
            get: function (solution) {
                var d = qSpiner.defer('Loading');
                var array = /Solution\/Edit\/(\d+)/.exec($location.absUrl());
                var id = undefined;
                if (array && array.length > 1) {
                    id = array[1];
                } else if (solution.Id && solution.Id > 0) {
                    id = solution.Id;
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
                    d.resolve(solution);
                }
                return d.promise;
            },
            submit: function(Solution) {
                var d = qSpiner.defer('Saving');
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
                var d = qSpiner.defer('Compiling');
                $http.post('/api/Solution/Compile/', JSON.stringify(Solution))
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