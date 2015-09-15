(function (angular) {
    angular
        .module('testility.solution')
        .factory('SolutionService', SolutionService);

    function Error (){
        this.data = '';
        this.status = undefined;
        this.statusText = '';
    };

    SolutionService.$inject = ['$location', 'Restangular', 'qSpiner', 'uiBrowserDialog', 'ui.config'];
    function SolutionService($location, Restangular, qSpiner, uiBrowserDialog, uiConfig) {
        var serivce = {
            Solution: {},
            Loaded: false,
            empty: _notInitialized,
            newItem: _notInitialized,
            removeItem: _notInitialized,
            init: _init,
            compile: _notInitialized,
            submit: _notInitialized,
            changeSolution: _notInitialized,
            runTest: _notInitialized,
            get: _notInitialized
        };

        function _notInitialized(){
            throw 'Service should be initialized or action is not supported';
        }

        function _init() {
            var id;
            angular.extend(this, CommonSerivce(Restangular, qSpiner));
            if ($location.absUrl().match(/Solution\/Create/) ||
                $location.absUrl().match(/Solution\/Edit/)) {
                var array = /Solution\/Edit\/(\d+)/.exec($location.absUrl());
                if (array && array.length > 1) {
                    id = array[1];
                }
                angular.extend(this, SetupSolutionService(Restangular, qSpiner));
            } else if ($location.absUrl().match(/UnitTest\/Create/) ||
                       $location.absUrl().match(/UnitTest\/Edit/)) {
                var array = /UnitTest\/Edit\/(\d+)/.exec($location.absUrl());
                if (array && array.length > 1) {
                    id = array[1];
                }
                angular.extend(this, UnitTestService(Restangular, qSpiner, uiBrowserDialog, uiConfig));
            }
            this.Solution = this.empty();
            this.Solution.Id = id;
        };

        return serivce;
    };

    function CommonSerivce(Restangular, qSpiner) {
        var service = {
            Solution: {},
            Loaded: false,
            empty: _empty,
            newItem: _newItem,
            removeItem: _removeItem,
            compile: _compile,
            submit: _submit
        };

        function _empty() {
            var e = Restangular.one(this.WebApi);
            var element = {
                Id: 0,
                Name: '',
                Language: 0,
                References: [],
                Items: [],
                SetupId: 0
            };
            element = _initializer(angular.extend({}, e, element));
            return element;
        };

        function _initializer(solution) {
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

        function _newItem(name) {
            this.Solution.ItemsList.push({ Id: 0, Name: name, active: true, SolutionId: this.Solution.Id });
        };

        function _removeItem(index) {
            if (index <= this.Solution.ItemsList.length) {
                this.Solution.ItemsList.splice(index, 1);
            } else {
                throw 'Index Out of Bounds';
            }
        };

        function _compile() {
            var d = qSpiner.defer('Compiling');
            Restangular.copy(this.Solution)
                .post('Compile').then(
                function (response) {
                    d.resolve(response);
                }, function (data, status) {
                    d.reject(CreateError(data));
                });
            return d.promise;
        };

        function _submit() {
            var d = qSpiner.defer('Saving');
            var instance = this;
            if (this.Solution.Id) {
                Restangular.copy(this.Solution)
                    .put().then(function (response) {
                        if (response.hasOwnProperty('solution')) {
                            angular.extend(instance.Solution, response.solution);
                        }
                        angular.extend(instance.Solution, response);
                        d.resolve(response);
                    },
                    function (data, status) {
                        d.reject(CreateError(data));
                    });
            } else {
                Restangular.all(this.WebApi).post(this.Solution)
                    .then(function (response) {
                        if (response.hasOwnProperty('solution')) {
                            angular.extend(instance.Solution, response.solution);
                        }
                        angular.extend(instance.Solution, response);
                        d.resolve(response);
                    },
                    function (data, status) {
                        d.reject(CreateError(data));
                    });
            }
            return d.promise;
        };

        return service;
    };

    function SetupSolutionService(Restangular, qSpiner) {
        var service = {
            Entry: 'Setup',
            WebApi: 'Solution',
            get: function () {
                var d = qSpiner.defer('Loading');
                var instance = this;
                if (this.Solution.Id) {
                    Restangular.one('Solution', this.Solution.Id).get()
                    .then(function (solution) {
                        instance.Loaded = true;
                        angular.extend(instance.Solution, solution);
                        d.resolve(instance.Solution);
                    }, function (data, status) {
                        instance.Loaded = false;
                        d.reject(CreateError(data));
                    });
                } else {
                    this.Loaded = true;
                    d.resolve(this.Solution);
                }
                return d.promise;
            }
        };
        return service;
    };

    function UnitTestService(Restangular, qSpiner, uiBrowserDialog, uiConfig) {
        var service = {
            Entry: 'UnitTest',
            WebApi: 'UnitTest',
            get: function () {
                var instance = this;
                if (this.Solution.Id) {
                    var d = qSpiner.defer('Loading');
                    Restangular.one('UnitTest', this.Solution.Id).get()
                        .then(function (solution) {
                            instance.Loaded = true;
                            angular.extend(instance.Solution, solution);
                            d.resolve(instance.Solution);
                        }, function (data, status) {
                            instance.Loaded = false;
                            d.reject(CreateError(data));
                        });
                    return d.promise;
                } else {
                    if (!this.Solution.SetupId) {
                        return _changeSolution.bind(this)();
                    } else {
                        var d = qSpiner.defer('Loading');
                        Loaded = true;
                        d.resolve(this.Solution);
                        return d.promise;
                    }
                }
            },
            changeSolution: function (resolve, reject) {
                return _changeSolution.bind(this)().then(function (response) {
                    resolve(response);
                }, function (error) {
                    reject(error);
                });
            },
            runTest: function () {
                var d = qSpiner.defer('Testing');
                Restangular.copy(this.Solution)
                    .post('RunTest').then(
                    function (response) {
                        d.resolve(response);
                    }, function (data, status) {
                        d.reject(CreateError(data));
                    });
                return d.promise;
            }
        };

        function _changeSolution() {
            var instance = this;
            var promise = new Promise(function (resolve, reject) {
                var options = {};
                options = uiConfig.browsersConfig['Solutions'] || {};
                
                var setSelected = function (solutionId) {
                    if (solutionId) {
                        var d = qSpiner.defer('Initializing');
                        if (instance.Solution.SetupId !== solutionId.items) {
                            Restangular.one('UnitTest/Init', solutionId.items).get()
                                .then(function (solution) {
                                    instance.Loaded = true;
                                    angular.extend(instance.Solution, solution.plain());
                                    d.resolve();
                                    resolve();
                                }, function (data, status) {
                                    instance.Solution = empty();
                                    instance.Loaded = false;
                                    d.reject();
                                    reject(CreateError(data));
                                });
                        } else {
                            d.resolve();
                            resolve("Unchanged");
                        }
                    } else {
                        d.reject();
                        reject(CreateError("Please select setup solution"));
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
                    if (instance.Solution.SetupId) {
                        reject();
                    } else {
                        reject(CreateError("Please select setup solution"));
                    }
                };
                uiBrowserDialog.show(options, 'md', getSelected, setSelected, cancelled);
            });
            return promise;
        }

        return service;
    }

    function CreateError(e) {
        var error = new Error();
        if (typeof e === 'object') {
            angular.copy(e, error);
        } else {
            error.data = e;
        }
        return error;
    }
})(window.angular);