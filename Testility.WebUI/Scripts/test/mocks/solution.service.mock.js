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
                }
            };
        });
})(window.angular);
