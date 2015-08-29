angular.module('validation', [])
    .directive('val', ['$compile', function ($compile) {
        var required = function (element) {
            mapper([{ source: 'val-required', destination: 'required', value: 'true' }], element);
        };
        var length = function (element) {
            mapper([{ source: 'val-length' },
                    { source: 'val-length-max', destination: 'ng-maxlength' },
                    { source: 'val-length-min', destination: 'ng-minlength' }],
                    element);
        };
        var onBlur = function(element) {
            mapper([{destination: 'ng-model-options', value: '{ updateOn: \'blur\' }'}], element);
        };

        var mapper = function (map, element) {
            for (var i = 0; i < map.length; i++) {
                var match = map[i];
                var source = undefined;
                if(match.hasOwnProperty('source')) {
                    source = match.source;
                }
                if (match.hasOwnProperty('destination')) {
                    if (match.hasOwnProperty('value')) {
                        element.attr(match.destination, match.value);
                    } else {
                        if(source) {
                            var val = element.attr(source);
                            if (!val) {
                                val = element.attr('data-' +source);
                            }
                            if (val) {
                                element.attr(match.destination, val);
                            }
                        }
                    }
                }
                if(source) {
                    element.removeAttr(match.source);
                    element.removeAttr('data-' + match.source);
                }
            }
        };
        var handlers = {
            'valRequired': required,
            'valLength': length,
            'valRemote': onBlur
            //'valRemoteAdditionalfields': additionalFields
        };
        return {
            restrict: 'A',
            terminal: true,
            priority: 1001,
            compile: function compile(element, attrs) {
                for (var attr in attrs) {
                    if (handlers.hasOwnProperty(attr)) {
                        var h = handlers[attr];
                        if (h && typeof h === 'function') {
                            h(element);
                        } 
                    }
                }
                element.removeAttr("val");
                element.removeAttr("data-val");
                return {
                    pre: function preLink(scope, iElement, iAttrs, controller) { },
                    post: function postLink(scope, iElement, iAttrs, controller) {
                        $compile(iElement)(scope);
                    }
                };
            }
        };        
    }])
    .directive('valRemote', ['$http', '$q', function ($http, $q) {
        return {
            restrict: 'A',
            require: [ '^form','ngModel' ],
            link: function (scope, iElement, iAttrs, controller) {
                var ngForm = controller[0];
                var ngModel = controller[1];
                var shouldProcess = function() {
                    var otherRulesInValid = false;
                    for ( var p in ngModel.$error ) {
                        if ( ngModel.$error[ p ] && p != 'remote' ) {
                            otherRulesInValid = true;
                            break;
                        }
                    }
                    return !( ngModel.$pristine || otherRulesInValid );
                };
                var options = {
                    valRemoteAdditionalfields: [],
                    valRemoteUrl: undefined
                };
                var prepareData = function (newValue) {
                    var data = {};
                    for (var i = 0; i < options.valRemoteAdditionalfields.length; i++) {
                        var field = options.valRemoteAdditionalfields[i];
                    }
                };
                angular.extend(options, iAttrs);
                if (options.valRemoteUrl) {
                    ngModel.$asyncValidators.remote = function (newValue, oldValue) {                        
                        if (shouldProcess()) {
                            return $http.post(options.valRemoteUrl, { Id: iAttrs.remoteFields, Name: newValue })
                                .then(function (response) {
                                    if (!response || !response.hasOwnProperty('data') || !response.data)
                                        return $q.reject(response.data);
                                    return true;
                                });
                        } else {
                            var d = $q.defer();
                            d.resolve(true);
                            return d.promise;
                        }                        
                    };
                }
            }
        };
    }]);