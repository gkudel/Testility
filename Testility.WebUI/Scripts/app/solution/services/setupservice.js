angular.module('testility')
    .factory('setupservice', ['Restangular', 'qSpiner', 'ctr', function (Restangular, qSpiner, ctr) {
        var service = function () {
            this.getInstance = function () {
                var solutuions = Restangular.all("Solution");
                return {
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
            };
        };
        return new service();
    }]);
