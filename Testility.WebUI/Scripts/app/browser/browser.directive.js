(function (angular) {
    angular.module('ui.browser')
        .directive('uiBrowser', UiBrowser);

    UiBrowser.$inject = ['ui.config'];

    function UiBrowser(uiConfig) {
        return {
            restrict: 'A',
            replace: false,
            transclude: true,
            scope: {},
            bindToController: {
                getSelected: '&',
                setSelected: '&',
                modelSize: '='
            },
            link: function ($scope, $element) {
                $element.bind('click', $scope.open);
                $scope.$on('$destroy', function () {
                    $element.unbind('click');
                });
            },
            template: '<span ng-transclude></span>',
            controller: 'UiBrowserController',
            controllerAs: 'ctrl'
        };
    }
})(window.angular);