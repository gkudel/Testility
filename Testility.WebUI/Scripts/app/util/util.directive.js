(function (angular) {
    angular
        .module('testility.util')
        .directive('convertNumber', convertNumber);
    
    function convertNumber() {
        return {
            require: 'ngModel',
            link: function (scope, el, attr, ctrl) {
                ctrl.$parsers.push(function (value) {
                    return parseInt(value, 10);
                });

                ctrl.$formatters.push(function (value) {
                    return value.toString();
                });
            }
        }
    };
})(window.angular);