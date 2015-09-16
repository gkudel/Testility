(function (angular) {
    angular
        .module('ui.spiner', [])
        .factory('qSpiner', function () {
            return {
                defer: jasmine.createSpy('defer')
                        .and.callFake(function (s) {
                            return this;
                        }),
                resolve: jasmine.createSpy('resolve'),
                reject: jasmine.createSpy('reject'),
                promise: {}
            };
        });
})(window.angular);