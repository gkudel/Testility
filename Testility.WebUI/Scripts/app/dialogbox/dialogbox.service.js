(function (angular) {
    angular.module('ui.dialogbox').factory('dialogbox', DialogBoxService);

    DialogBoxService.$inject = ['$modal', '$q'];
    function DialogBoxService($modal, $q) {
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
                    MessageBox: 'MessageBoxInstanceController',
                    DialogBox: 'DialogBoxInstanceController'
                };
                var buttonsTypes = {
                    Ok: 'Ok.html',
                    OkCancel: 'OkCancel.html'
                };
                Object.defineProperty(config, 'ControlerInstance', {
                    get: function () {
                        if (types.hasOwnProperty(this.type)) {
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
                        caption: function () {
                            return config.caption
                        },
                        icon: function () {
                            var icon = 'Info';
                            if (icons.hasOwnProperty(config.icon)) {
                                return icons[config.icon];
                            }
                            return icons.Info;
                        },
                        buttons: function () {
                            return config.Buttons;
                        },
                        config: function () {
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
    };
})(window.angular);


