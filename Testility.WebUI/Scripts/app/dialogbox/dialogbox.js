angular.module('ui.dialogbox', [])
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
            controller: 'inputBoxController'
        };
    })
    .controller('inputBoxController', ['$scope', function ($scope) {
        $scope.$watch('Value', function (n, o) {
            if (n !== o) {
                $scope.$emit('Value_Changed', n);
            }
        });
    }])
    .factory('dialogbox', ['$modal', '$q', function ($modal, $q) {
        var icons = {
            Info: {
                icon: 'glyphicon-info-sign',
                background: 'alert alert-info'
            },
            Error: {
                icon: 'glyphicon-exclamation-sign',
                background: 'alert alert-danger'
            }
        };
        var service = {
            show: function (config) {
                var cfg = {
                    type: 'MessageBox',
                    caption: 'Caption',
                    icon: 'Info',
                    buttons: 'Ok',
                    modal: false
                };
                cfg = angular.extend({}, cfg, config)
                var types = {
                    MessageBox: 'MessageBoxInstnace',
                    DialogBox: 'DialogBoxInstnace'
                };
                var buttonsTypes = {
                    Ok: 'Ok.html',
                    OkCancel: 'OkCancel.html'
                };
                Object.defineProperty(config, 'ControlerInstance', {
                    get: function() {
                        if(types.hasOwnProperty(this.type)) {
                            return types[this.type];
                        }
                        return types.MessageBox;
                    }
                });
                Object.defineProperty(config, 'Buttons', {
                    get: function () {
                        if (buttonsTypes.hasOwnProperty(this.buttons)) {
                            return buttonsTypes[this.buttons];
                        }
                        return buttonsTypes.Ok;
                    }
                });

                var opt = {
                    animation: true,
                    templateUrl: 'DialogBox.html',
                    controller: config.ControlerInstance,
                    windowClass: 'center-modal',
                    size: 'md',
                    resolve: {
                        caption: function() {
                            return config.caption
                        },
                        icon: function () {
                            var icon = 'Info';
                            if (icons.hasOwnProperty(config.icon)) {
                                return icons[config.icon];
                            }
                            return icons.Info;
                        },
                        buttons: function(){
                            return config.Buttons;
                        },
                        config: function() {
                            return cfg;
                        }
                    }
                };
                var ret = undefined;
                var modalInstance = $modal.open(opt);                
                return modalInstance.result;
            }
        };
        return service;
    }]);


angular.module('ui.dialogbox')
    .controller('MessageBoxInstnace', ['$scope', '$modalInstance', 'caption', 'icon', 'buttons', 'config', function ($scope, $modalInstance, caption, icon, buttons, config) {

        var formatMessage = function () {
            var m;
            if (config.hasOwnProperty('message')) {
                if (config.message) {
                    if (config.message.hasOwnProperty('Message')) {
                        m = config.message.Message || 'NotDefined';
                    } else {
                        m = config.message || 'NotDefined';
                    }
                } else {
                    m = 'NotDefined';
                }
            } else {
                m = 'NotDefined';
            }
            return m;
        };

        $scope.Caption = caption;
        $scope.Message = formatMessage();
        $scope.type = 'MessageBox.html';
        $scope.buttons = buttons;
        $scope.Icon = icon.icon;
        $scope.BackGround = icon.background;

        $scope.ok = function () {
            $modalInstance.close('Ok');
        };
    }]);


angular.module('ui.dialogbox')
    .controller('DialogBoxInstnace', ['$scope', '$modalInstance', 'caption', 'icon', 'buttons', 'config', function ($scope, $modalInstance, caption, icon, buttons, config) {
        $scope.Caption = caption;
        $scope.type = 'InputBox.html';
        $scope.Value = config.hasOwnProperty('value') ? config.value : '';
        $scope.buttons = buttons;
        $scope.Icon = icon.icon;
        $scope.BackGround = icon.background;

        $scope.$on('Value_Changed', function (e, v) {
            e.stopPropagation();
            $scope.Value = v;
        });
        $scope.ok = function () {
            $modalInstance.close($scope.Value);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }]);
