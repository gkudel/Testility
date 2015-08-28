angular.module('ui.browser')
    .factory('messagebox', ['$modal', function ($modal) {
        var service = {
            show: function (caption, message, icon) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: 'MessageBox.html',
                    controller: 'MessageBoxInstnace',
                    windowClass: 'center-modal',
                    size: 'md',
                    resolve: {
                        caption: function() {
                            return caption
                        },
                        message: function () {
                            var m;
                            if (message) {
                                if (message.hasOwnProperty('Message')) m = message.Message;
                                else m = message || 'NotDefined';
                            }
                            return m;
                        },
                        icon: function(){
                            return icon || 'Info';
                        }
                    }
                });
                modalInstance.result.then(function () {
                }
                , function () {
                    $log.info('Messagebox dismissed at: ' + new Date());
                });
            }
        };
        return service;
    }]);


angular.module('ui.browser')
    .controller('MessageBoxInstnace', ['$scope', '$modalInstance', 'caption', 'message', 'icon', function ($scope, $modalInstance, caption, message, icon) {
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
        $scope.Caption = caption;
        $scope.Message = message;
        $scope.Icon = icons[icon].icon;
        $scope.BackGround = icons[icon].background;

        $scope.ok = function () {
            $modalInstance.close();
        };
    }]);
