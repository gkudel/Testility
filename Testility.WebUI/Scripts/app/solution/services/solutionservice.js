angular.module('testility')
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
    .factory('solutionservice', ['$location', 'Restangular', 'setupservice', 'unitTestService', 'ctr', 'qSpiner', function ($location, Restangular, setupservice, unitTestService, ctr, qSpiner) {
        var serivce = {
            Solution: {},
            Loaded: false,
            empty: function () {
                var e = Restangular.one(this.WebApi);
                var element = {
                    Id: 0,
                    Name: '',
                    Language: 0,
                    References: [],
                    Items: []
                };
                element = ctr(angular.extend({}, e, element));
                return element;
            },
            newItem: function (name) {
                this.Solution.ItemsList.push({ Id: 0, Name: name, active: true, SolutionId: this.Solution.Id });
            },
            removeItem: function (index) {
                if (index <= this.Solution.ItemsList.length) {
                    this.Solution.ItemsList.splice(index, 1);
                } else {
                    throw 'Index Out of Bounds';
                }
            },
            getInstance: function () {
                var id;
                if($location.absUrl().match(/Solution\/Create/)  ||
                   $location.absUrl().match(/Solution\/Edit/)) {
                    var array = /Solution\/Edit\/(\d+)/.exec($location.absUrl());
                    if (array && array.length > 1) {
                        id = array[1];
                    }
                    Object.setPrototypeOf(this, setupservice.getInstance());                    
                } else if ($location.absUrl().match(/UnitTest\/Create/) ||
                           $location.absUrl().match(/UnitTest\/Edit/)) {
                    var array = /UnitTest\/Edit\/(\d+)/.exec($location.absUrl());
                    if (array && array.length > 1) {
                        id = array[1];
                    }
                    Object.setPrototypeOf(this, unitTestService.getInstance());
                }
                this.Solution = this.empty();
                this.Solution.Id = id;
                this.init();
            },
            init: function () { },
            compile: function () {
                var d = qSpiner.defer('Compiling');
                Restangular.copy(this.Solution)
                    .post('Compile').then(
                    function (response) {
                        d.resolve(response);
                    }, function (data, status) {
                        if (data.hasOwnProperty('data'))
                            data = data.data;
                        d.reject(data);
                    });
                return d.promise;
            },
            submit: function () {
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
                            if (data.hasOwnProperty('data'))
                                data = data.data;
                            d.reject(data);
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
                            if (data.hasOwnProperty('data'))
                                data = data.data;
                            d.reject(data);
                        });
                }
                return d.promise;
            }

        };
        return serivce;
    }]);