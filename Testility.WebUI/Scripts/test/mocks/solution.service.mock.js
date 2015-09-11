(function (angular) {
    angular
        .module('testility.solution').factory('SolutionService', function () {
            return {
                Loaded: true,
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
                    return {
                        then: function (f) {
                            if (f) {
                                f('Ok');
                            }
                        }
                    }
                },
                removeItem: jasmine.createSpy('removeItem'),
                newItem: jasmine.createSpy('newItem')
            };
        });
})(window.angular);
