angular.module('ui.messaging', [])
    .directive('uiMessaging', ['ui.config', function (uiConfig) {
        return {
            restrict: 'A',
            templateUrl: function (element, attrs) {
                var options = uiConfig.messagingConfig || {
                    templateUrl: '/Views/Shared/_Messaging.html'
                };
                options = angular.extend({}, options, attrs.uiMessaging);
                return options.templateUrl;
            },
        };
    }])
    .factory('messaging', function () {
        var service = function () {
            var _Messages = [];
            var _remove = function (index) {
                if (index <= _Messages.length) {
                    _Messages.splice(index, 1);
                }
            };
            var _clear = function() {
                _Messages.length = 0;
            };
            var _add = function(message) {
                if(Array.isArray(message)) {
                    _Messages = _Messages.concat(message);
                } else {
                    _Messages.push(message);
                }
            };
            return {
                init: function (scope, form) {
                    if (scope) {
                        Object.defineProperty(scope, 'Messages', {
                            get: function () { return _Messages; }
                        });
                        Object.getPrototypeOf(scope).removeMessage = function (index) {
                            _remove(index);
                        };
                        Object.getPrototypeOf(scope).clearMessages = function () {
                            _clear();
                        };
                        Object.getPrototypeOf(scope).addMessage = function (message) {
                            _add(message);
                        };
                        if (form) {
                            angular.forEach(form, function (value, key) {
                                if (value.attributes.getNamedItem('client-validation-enabled')) {
                                    scope.$watchGroup(
                                        [form.name + ".$submitted",
                                         form.name + "." + value.name + '.$touched',
                                         form.name + "." + value.name + '.$invalid',
                                         form.name + "." + value.name + '.$error'],
                                        function (n, o, scope) {
                                            if ((n[0] || n[1]) && n[2]) {
                                                _Messages.push({ Alert: 'danger', Message: 'ValidationError' });
                                            } else {
                                                _clear();
                                            }
                                        });
                                }
                            });
                        }
                    }
                },
                get Messages() {
                    return _Messages;
                },
                add: function (message) {
                    _add(message);
                },
                remove: function (index) {
                    _remove(index);
                },
                clear: function(){
                    _clear
                }
            };
        };
        return new service();
    });
