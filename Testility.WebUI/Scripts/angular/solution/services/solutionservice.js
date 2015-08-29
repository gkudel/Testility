angular.module('Testility')
    .factory('solutionservice', ['$http', '$location', 'qSpiner', function ($http, $location, qSpiner) {

        var ctr = function(solution) {
            Object.defineProperty(solution, 'RefList', {
                get: function () {
                    if (!this.References) this.References = [];
                    return this.References;
                },
                set: function (ref) {
                    if (!this.References) this.References = [];
                    this.References.length = 0;
                    this.References.push(ref);
                }
            });
            Object.defineProperty(solution, 'ItemsList', {
                get: function () {
                    if (!this.Items) this.Items = [];
                    return this.Items;
                }
            });
            return solution;
        };

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
                            if (response && response.hasOwnProperty('Id')) {
                                d.resolve(ctr(response));
                            } else {
                                d.resolve(response);
                            }
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
                        if (response.hasOwnProperty('solution'))
                            ctr(response.solution);
                        d.resolve(response);
                    })
                    .error(function (data, status) {
                        d.reject(data);
                    });
                return d.promise;
            },
            empty: function () {
                return ctr({
                    Id: 0,
                    Name: '',
                    Language: 0,
                    References: [],
                    Items: []
                });
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