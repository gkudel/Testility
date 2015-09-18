(function (angular) {
    angular
        .module('testility', [
            'restangular',
            'ui.codemirror',
            'flow', 
            'ui.browser',
            'ui.messaging',
            'ui.dialogbox',
            'ui.spiner',
            'data.validation',
            'testility.util',
            'testility.solution',
            'testility.references']);
})(window.angular);