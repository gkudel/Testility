(function (angular) {
    angular.module('ui.dialogbox')
        .directive('dialogBody', function() {
            return {
                restrict: 'A',
                templateUrl: function(elem, attr) {
                    return attr.template;
                } 
            };
        }).
        directive('inputBox', function(){
            return {
                restrict: 'A',
                controller: 'InputBoxController'
            };
        });
})(window.angular);
