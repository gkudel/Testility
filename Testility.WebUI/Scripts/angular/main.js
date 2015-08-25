(function(angular){
    if (!Array.prototype.findIndex) {
      Array.prototype.findIndex = function(predicate) {
        if (this === null) {
          throw new TypeError('Array.prototype.findIndex called on null or undefined');
        }
        if (typeof predicate !== 'function') {
          throw new TypeError('predicate must be a function');
        }
        var list = Object(this);
        var length = list.length >>> 0;
        var thisArg = arguments[1];
        var value;

        for (var i = 0; i < length; i++) {
          value = list[i];
          if (predicate.call(thisArg, value, i, list)) {
            return i;
          }
        }
        return -1;
      };
    }
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
                Equal: function (id, element) { return id === element.Id; },
                GetResultValue: function(element) { return element.Id; }, 
                title: 'References'                
            }
        }
    });
})(window.angular);