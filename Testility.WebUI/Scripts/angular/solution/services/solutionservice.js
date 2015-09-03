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
    .factory('solutionservice', ['$location', 'setupservice', 'unitTestService', 'ctr', function ($location, setupservice, unitTestService, ctr) {
        var serivce = {
            Solution: {},
            empty: function () {
                return ctr({
                    Id: 0,
                    Name: '',
                    Language: 0,
                    References: [],
                    Items: []
                });
            },
            newItem: function (name) {
                this.Solution.ItemsList.push({ Id: 0, Name: name, active: true, SolutionId: Solution.Id });
            },
            removeItem: function (index) {
                if (index <= this.Solution.ItemsList.length) {
                    this.Solution.ItemsList.splice(index, 1);
                }
            },
            getInstance: function () {
                this.Solution = this.empty();
                if($location.absUrl().match(/Solution\/Create/)  ||
                   $location.absUrl().match(/Solution\/Edit/)) {
                    var array = /Solution\/Edit\/(\d+)/.exec($location.absUrl());
                    if (array && array.length > 1) {
                        this.Solution.Id = array[1];
                    }
                    Object.setPrototypeOf(this, setupservice.getInstance());                    
                } else if ($location.absUrl().match(/UnitTest\/Create/) ||
                           $location.absUrl().match(/UnitTest\/Edit/)) {
                    var array = /UnitTest\/Edit\/(\d+)/.exec($location.absUrl());
                    if (array && array.length > 1) {
                        this.Solution.Id = array[1];
                    }
                    Object.setPrototypeOf(this, unitTestService.getInstance());
                }
                this.init();
            },
            init: function () { }
        };
        return serivce;
    }]);