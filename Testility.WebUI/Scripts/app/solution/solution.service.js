(function (angular) {
    angular
        .module('testility.solution')
        .factory('SolutionService', SolutionService)
        .factory('SetupSolutionService', SetupSolutionService);

    SolutionService.$inject = ['$location', 'Restangular', 'qSpiner', 'SetupSolutionService'];
    function SolutionService($location, Restangular, qSpiner, setupSolutionService) {
        var serivce = {
            Solution: {},
            Loaded: false,
            empty: _empty,
            newItem: _newItem,
            removeItem: _removeItem,
            init: _init,
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
                Items: []
            };
            element = _initializer(angular.extend({}, e, element));
            return element;
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

        function _init() {
            var id;
            if ($location.absUrl().match(/Solution\/Create/) ||
                $location.absUrl().match(/Solution\/Edit/)) {
                var array = /Solution\/Edit\/(\d+)/.exec($location.absUrl());
                if (array && array.length > 1) {
                    id = array[1];
                }
                Object.setPrototypeOf(this, setupSolutionService);
            } else if ($location.absUrl().match(/UnitTest\/Create/) ||
                       $location.absUrl().match(/UnitTest\/Edit/)) {
                var array = /UnitTest\/Edit\/(\d+)/.exec($location.absUrl());
                if (array && array.length > 1) {
                    id = array[1];
                }
                Object.setPrototypeOf(this, new _unitTestSolutionService(Restangular, qSpiner));
            }
            this.Solution = this.empty();
            this.Solution.Id = id;            
        };

        function _compile() {
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
        };
        return serivce;
    };

    SetupSolutionService.$inject = ['Restangular', 'qSpiner'];
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
                        if (data.hasOwnProperty('data'))
                            data = data.data;
                        d.reject(data);
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
})(window.angular);