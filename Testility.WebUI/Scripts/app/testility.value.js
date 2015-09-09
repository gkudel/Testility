(function (angular) {
    angular.module("testility")
        .value('ui.config', {
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
                    Equal: function (id, element) { return id === element.Id; },
                    GetResultValue: function(element) { return element.Id; }, 
                    title: 'References'                
                },
                'Solutions': {
                    DataSource: '/api/Solution',
                    MultiSelection: false,
                    PrintElement: function (element) { return element.Name; },
                    Equal: function (id, element) { return id === element.Id; },
                    GetResultValue: function(element) { return element.Id; }, 
                    title: 'Solutions'
                }
            }
        });
})(window.angular);