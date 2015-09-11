(function (angular) {
    angular
        .module('testility', [
            'restangular',
            'ui.codemirror',
            'ui.browser',
            'ui.messaging',
            'ui.dialogbox',
            'ui.spiner',
            'data.validation',
            'testility.util',
            'testility.solution']);
})(window.angular);