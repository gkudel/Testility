(function (angular) {
    angular.module('ui.messaging')
        .directive('uiMessaging', uiConfigDirective);

    uiConfigDirective.$inject = ['ui.config'];
    function uiConfigDirective(uiConfig) {
        return {
            restrict: 'A',
            templateUrl: function (element, attrs) {
                var options = uiConfig.messagingConfig || {
                    templateUrl: '/Views/Shared/_Messaging.html'
                };
                options = angular.extend({}, options, attrs.uiMessaging);
                return options.templateUrl;
            },
        };
    };
})(window.angular);