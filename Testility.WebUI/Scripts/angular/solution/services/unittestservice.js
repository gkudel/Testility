angular.module('Testility')
    .factory('unitTestService', ['$http', '$location', 'qSpiner', 'uiBrowserDialog', 'ui.config', 'ctr', function ($http, $location, qSpiner, uiBrowserDialog, uiConfig, ctr) {
        var service = function () {
            var _changeSolution = function (instance) {
                var promise = new Promise(function (resolve, reject) {
                    var options = {};
                    options = uiConfig.browsersConfig['Solutions'] || {};

                    var setSelected = function (solutionId) {
                        if (solutionId) {
                            var d = qSpiner.defer('Initializing');
                            if (instance.Solution.SetupId !== solutionId.items) {
                                $http.post('/api/UnitTest/Create/' + solutionId.items)
                                    .success(function (response) {
                                        instance.Loaded = true;
                                        angular.copy(response, instance.Solution);
                                        d.resolve();
                                        resolve(instance.Solution);
                                    })
                                    .error(function (data, status) {
                                        instance.Loaded = false;
                                        d.reject();
                                        reject(data);
                                    });
                            } else {
                                d.resolve();
                                resolve("Unchanged");
                            }
                        }
                    }

                    var getSelected = function () {
                        if (instance.Solution.SetupId === undefined) {
                            return [];
                        } else {
                            return [instance.Solution.SetupId];
                        }
                    }
                    var cancelled = function () {
                        reject("Please select setup solution");
                    };
                    uiBrowserDialog.show(options, 'md', getSelected, setSelected, cancelled);
                });
                return promise;
            };
            this.getInstance = function () {
                return {
                    Entry: 'UnitTest',
                    Loaded: false,
                    init: function() {
                        Object.defineProperty(this.Solution, 'SetupId', { value: 0, writable: true });
                    },
                    get: function () {
                        var instance = this;
                        if (this.Solution.Id) {
                            var d = qSpiner.defer('Loading');                            
                            $http.get('/api/UnitTest/' + this.Solution.Id)
                                .success(function (response) {
                                    instance.Loaded = true;
                                    angular.copy(response, instance.Solution);
                                    d.resolve(instance.Solution);
                                })
                                .error(function (data, status) {
                                    instance.Loaded = false;
                                    d.reject(data);
                                });
                            return d.promise;
                        } else {
                            if (!this.Solution.SetupId) {
                                return _changeSolution(instance);
                            } else {
                                var d = qSpiner.defer('Loading');
                                instance.Loaded = true;
                                d.resolve(this.Solution);
                                return d.promise;
                            }
                        }
                    },
                    changeSolution: function (resolve, reject) {
                        var instance = this;
                        return _changeSolution(instance).then(function (response) {
                                    resolve(response);
                                }, function (error) {
                                    reject(error);
                                });
                    },
                    /*submit: function (solution) {
                        var d = qSpiner.defer('Saving');
                        $http.post('/api/Solution/', JSON.stringify(solution))
                            .success(function (response) {
                                if (response.hasOwnProperty('solution')) {
                                    ctr(response.solution);
                                    this.Id = response.solution.Id;
                                }
                                d.resolve(response);
                            })
                            .error(function (data, status) {
                                d.reject(data);
                            });
                        return d.promise;
                    },*/
                    compile: function () {
                        var d = qSpiner.defer('Compiling');
                        $http.post('/api/UnitTest/Compile/', JSON.stringify(this.Solution))
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
