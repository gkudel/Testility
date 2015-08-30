angular.module('validation', [])
    .value('getPrefix', function (ngModel) {
        var prefix = '';
        if (ngModel) {
            var array = ngModel.split('.');
            for (var i = 0; i < array.length - 1; i++) {
                if (prefix) prefix = prefix + '.' + array[i];
                else prefix = array[i];
            }
            if (prefix) {
                prefix = prefix + '.';
            }
        }
        return prefix;
    })
    .directive('val', ['$compile', 'getPrefix', function ($compile, getPrefix) {
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

        var remoteAdditionalFields = function (element) {
            var fields = element.attr('data-val-remote-additionalfields');
            if (!fields) fields = element.attr('val-remote-additionalfields');            
            if (fields) {                
                var array = fields.split(',');
                var out;
                if (array) {
                    var prefix = getPrefix(element.attr('ng-model'));
                    for (var i = 0; i < array.length; i++) {
                        var field = array[i];
                        if (field.indexOf('*.') === 0) field = field.substring(2);                        
                        if (out) out = out + ', ' + "\"" + field + "\": \"{{" + prefix + field + "}}\"";
                        else out = "\"" + field + "\": \"{{" + prefix + field + "}}\"";
                    }
                }
                if (out) {
                    out = "{" + out + "}";
                    element.attr('remote-fields', out);
                }
            }
        };

        var handlers = {
            'valRequired': required,
            'valLength': length,
            'valRemote': onBlur,
            'valRemoteAdditionalfields': remoteAdditionalFields
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
                element.attr("client-validation-enabled", "true");
                return {
                    pre: function preLink(scope, iElement, iAttrs, controller) { },
                    post: function postLink(scope, iElement, iAttrs, controller) {
                        $compile(iElement)(scope);
                    }
                };
            }
        };        
    }])
    .directive('valRemote', ['$http', '$q', 'getPrefix', function ($http, $q, getPrefix) {
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
                angular.extend(options, iAttrs);
                if (options.valRemoteUrl) {
                    ngModel.$asyncValidators.remote = function (newValue, oldValue) {                        
                        if (shouldProcess()) {
                            var obj = newValue;
                            var remote = iAttrs.remoteFields;
                            if (remote) {
                                var name = iAttrs.ngModel.substring(getPrefix(iAttrs.ngModel).length);
                                obj = angular.fromJson(remote);
                                obj[name] = newValue;
                            }
                            return $http.post(options.valRemoteUrl, obj)
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