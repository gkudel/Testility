(function (angular) {
    angular
        .module('ui.messaging', [])
        .factory('messaging', function () {
            return {
                init: jasmine.createSpy('init'),
                clear: jasmine.createSpy('clear'),
                add: jasmine.createSpy('add')
            };
        });
})(window.angular);