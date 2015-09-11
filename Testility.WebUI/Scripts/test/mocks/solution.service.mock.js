(function (angular) {
    angular
        .module('testility.solution').factory('SolutionService', function () {
            return {
                init: function () {
                },
                Solution: {
                    Id: 0,
                    Name: '',
                    Language: 0,
                    References: [],
                    Items: [],
                    RefList: []
                },
                get: function () {
                    var p = new Promise(function (resolve, reject) {
                        resolve('Ok');
                    });
                    return p;
                },
                removeItem: function (index) {
                }
            };
        });
})(window.angular);
