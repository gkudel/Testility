(function (angular) {
    angular
        .module('ui.messaging', [])
        .factory('messaging', function () {
            return {
                init: jasmine.createSpy('init'),
                clearMessages: jasmine.createSpy('clearMessages'),
                addMessage: jasmine.createSpy('addMessage')
            };
        });
})(window.angular);