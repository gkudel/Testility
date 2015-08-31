angular.module('Testility')
    .factory('ctr', function () {
        return function (solution) {
            Object.defineProperty(solution, 'RefList', {
                get: function () {
                    if (!this.References) this.References = [];
                    return this.References;
                },
                set: function (ref) {
                    if (!this.References) this.References = [];
                    this.References.length = 0;
                    this.References = this.References.concat(ref);
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
    })        
    .factory('setupservice', ['$http', '$location', 'qSpiner', 'ctr', function ($http, $location, qSpiner, ctr) {
        var service = function () {
            this.init = function (id) {
                return {
                    Id: id,
                    Entry: 'Setup',
                    Loaded: false,
                    get: function (solution) {
                        var d = qSpiner.defer('Loading');
                        var instance = this;
                        if (this.Id) {
                            $http.get('/api/Solution/' + this.Id)
                                .success(function (response) {
                                    instance.Loaded = true;
                                    d.resolve(ctr(response));
                                })
                                .error(function (data, status) {
                                    instance.Loaded = false;
                                    d.reject(data);
                                });
                        } else {
                            this.Loaded = true;
                            d.resolve(solution);
                        }
                        return d.promise;
                    },
                    submit: function (solution) {
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
            };
        };
        return new service();
    }])
    .factory('unitTestService', ['$http', '$location', 'qSpiner', 'uiBrowserDialog', 'ui.config', 'ctr', function ($http, $location, qSpiner, uiBrowserDialog, uiConfig, ctr) {
        var service = function () {
            this.init = function (id) {
                var _changeSolution = function () {
                    var promise = new Promise(function (resolve, reject) {
                        var options = {};
                        options = uiConfig.browsersConfig['Solutions'] || {};
                        var selected = function (solution) {
                            if (solution) {
                                $http.get('/api/UnitTest/' + solution.items)
                                    .success(function (response) {
                                        resolve(ctr(response));
                                    })
                                    .error(function (data, status) {
                                        reject(data);
                                    });
                            }
                            return [];
                        }
                        var cancelled = function () {
                            reject("Please select setup solution");
                        };
                        uiBrowserDialog.show(options, 'md', selected, cancelled);
                    });
                    return promise;
                };
                return {
                    Entry: 'UnitTest',
                    Id: id,
                    SolutionId: undefined,
                    Loaded: false,
                    get: function (solution) {
                        if (this.Id) {
                            var d = qSpiner.defer('Loading');
                            var instance = this;
                            $http.get('/api/UnitTest/' + this.Id)
                                .success(function (response) {
                                    instance.Loaded = true;
                                    d.resolve(ctr(response));
                                })
                                .error(function (data, status) {
                                    instance.Loaded = false;
                                    d.reject(data);
                                });
                            return d.promise;
                        } else {
                            if (!this.SolutionId) {
                                return _changeSolution();
                            } else {
                                var d = qSpiner.defer();
                                Loaded = true;
                                d.resolve(solution);
                                return d.promise;
                            }
                        }
                    },
                    changeSolution: function () {
                        return _changeSolution().then(function (response) {
                                }, function (reject) { });
                    }
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
                    }*/
                };
            };
        };
        return new service();
    }])
    .factory('solutionservice', ['$location', 'setupservice', 'unitTestService', 'ctr', function ($location, setupservice, unitTestService, ctr) {

        var serivce = {
            empty: function () {
                return ctr({
                    Id: 0,
                    Name: '',
                    Language: 0,
                    References: [],
                    Items: []
                });
            },
            init: function() {
                if($location.absUrl().match(/Solution\/Create/)  ||
                   $location.absUrl().match(/Solution\/Edit/)) {
                    var array = /Solution\/Edit\/(\d+)/.exec($location.absUrl());
                    var id = undefined;
                    if (array && array.length > 1) {
                        id = array[1];
                    }
                    Object.setPrototypeOf(this, setupservice.init(id));                    
                } else if ($location.absUrl().match(/UnitTest\/Create/) ||
                           $location.absUrl().match(/UnitTest\/Edit/)) {
                    var array = /UnitTest\/Edit\/(\d+)/.exec($location.absUrl());
                    var id = undefined;
                    if (array && array.length > 1) {
                        id = array[1];
                    }
                    Object.setPrototypeOf(this, unitTestService.init(id));
                }
            }
        };
        return serivce;
    }]);