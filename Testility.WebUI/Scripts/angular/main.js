(function(angular){
    var myApp = angular.module("Testility", ['ui', 'ui.browser']);
    myApp.value('ui.config', {
        codemirror: {
            lineNumbers: true,
            matchBrackets: true,
            mode: 'text/x-csharp'
        },
        browsersConfig: {
            'References': {
                DataSource: '/api/Reference',
                MultiSelection: true,
                PrintElement: function (element) { return element.Name; },
                title: 'References'                
            }
        }
    });
})(window.angular);