angular.module('Testility')
    .factory('solutionservice', ['$http', '$location', '$q', 'spiner', function ($http, $location, $q, spiner) {

        var service = {
            get: function () {
                var d = $q.defer();
                var array = /Solution\/Edit\/(\d+)/.exec($location.absUrl());
                var id = undefined;
                if (array && array.length > 1) {
                    id = array[1];
                }

                if (id) {
                    var overlay = spiner.show('Loading');
                    $http.get('/api/Solution/' + id)
                        .success(function (response) {
                            overlay.hide();
                            d.resolve(response);                           
                        })
                        .error(function (data, status) {
                            overlay.hide();
                            d.reject(data);
                        });
                } else {
                    d.resolve(this.empty());
                }
                return d.promise;
            },
            submit: function(Solution) {
                var d = $q.defer();
                var overlay = spiner.show('Saving');
                $http.post('/api/Solution/', JSON.stringify(Solution))
                    .success(function (response) {
                        overlay.hide();
                        d.resolve(response);
                    })
                    .error(function (data, status) {
                        overlay.hide();
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
                var overlay = spiner.show('Compiling');
                $http.post('/api/Solution/Compile/', JSON.stringify(Solution))
                    .success(function (response) {
                        overlay.hide();
                        d.resolve(response);
                    })
                    .error(function (data, status) {
                        overlay.hide();
                        d.reject(data);
                    });
                return d.promise;
            }
        };
        return service;
    }]);