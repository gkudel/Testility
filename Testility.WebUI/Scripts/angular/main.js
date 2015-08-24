(function(angular){
    var myApp = angular.module("Testility", ['ui', 'ui.bootstrap']);
    myApp.value('ui.config', {
        codemirror: {
            lineNumbers: true,
            matchBrackets: true,
            mode: 'text/x-csharp'
        }
    });
})(window.angular);