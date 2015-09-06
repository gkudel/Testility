angular.module('Testility')
    .factory('unitTestService', ['Restangular', '$location', 'qSpiner', 'uiBrowserDialog', 'ui.config', 'ctr', function (Restangular, $location, qSpiner, uiBrowserDialog, uiConfig, ctr) {
        var service = function () {
            var _changeSolution = function (instance) {
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
                                        instance.Solution = instance.empty();
                                        instance.Loaded = false;
                                        d.reject();
                                        if (data.hasOwnProperty('data'))
                                            data = data.data;
                                        d.reject(data);
                                    });                                
                            } else {
                                d.resolve();
                                resolve("Unchanged");
                            }
                        } else {
                            d.reject();
                            reject("Please select setup solution");
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
                        if(instance.Solution.SetupId) {
                            reject();
                        } else {
                            reject("Please select setup solution");
                        }
                    };
                    uiBrowserDialog.show(options, 'md', getSelected, setSelected, cancelled);
                });
                return promise;
            };
            var solutuions = Restangular.all("UnitTest");
            this.getInstance = function () {
                return {
                    Entry: 'UnitTest',                    
                    WebApi: 'UnitTest',
                    init: function() {
                        Object.defineProperty(this.Solution, 'SetupId', { value: 0, writable: true });
                    },
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
                                if (data.hasOwnProperty('data'))
                                    data = data.data;
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
                    }
                };
            };
        };
        return new service();
    }]);
