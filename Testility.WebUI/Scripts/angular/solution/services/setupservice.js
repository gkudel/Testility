angular.module('Testility')
    .factory('setupservice', ['$http', 'qSpiner', 'ctr', function ($http, qSpiner, ctr) {
        var service = function () {
            this.getInstance = function () {
                return {
                    Entry: 'Setup',
                    Loaded: false,
                    get: function () {
                        var d = qSpiner.defer('Loading');
                        var instance = this;
                        if (this.Solution.Id) {
                            $http.get('/api/Solution/' + this.Solution.Id)
                                .success(function (response) {
                                    instance.Loaded = true;                                    
                                    angular.copy(response, instance.Solution);
                                    d.resolve(instance.Solution);
                                })
                                .error(function (data, status) {
                                    instance.Loaded = false;
                                    d.reject(data);
                                });
                        } else {
                            this.Loaded = true;
                            d.resolve(this.Solution);
                        }
                        return d.promise;
                    },
                    submit: function () {
                        var d = qSpiner.defer('Saving');
                        var instance = this;
                        $http.post('/api/Solution/', JSON.stringify(this.Solution))
                            .success(function (response) {
                                if (response.hasOwnProperty('solution')) {
                                    angular.copy(response.solution, instance.Solution);
                                }
                                d.resolve(instance.Solution);
                            })
                            .error(function (data, status) {
                                d.reject(data);
                            });
                        return d.promise;
                    },
                    compile: function () {
                        var d = qSpiner.defer('Compiling');
                        $http.post('/api/Solution/Compile/', JSON.stringify(this.Solution))
                            .success(function (response) {
                                d.resolve(response);
                            })
                            .error(function (data, status) {
                                d.reject(data);
                            });
                        return d.promise;
                    }
                };
            };
        };
        return new service();
    }]);
