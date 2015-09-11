(function (angular) {
    angular.module('ui.dialogbox')
        .controller('MessageBoxInstanceController', MessageBoxInstnaceController)
        .controller('DialogBoxInstanceConttroller', DialogBoxInstnaceConttroller)
        .controller('InputBoxController', InputBoxController);

   
    MessageBoxInstnaceController.$inject = ['$scope', '$modalInstance', 'caption', 'icon', 'buttons', 'config'];
    function MessageBoxInstnaceController($scope, $modalInstance, caption, icon, buttons, config) {
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
    };

    DialogBoxInstnaceConttroller.$inject = ['$scope', '$modalInstance', 'caption', 'icon', 'buttons', 'config'];
    function DialogBoxInstnaceConttroller($scope, $modalInstance, caption, icon, buttons, config) {
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
    };

    InputBoxController.$inject = ['$scope'];
    function InputBoxController($scope) {
        $scope.$watch('Value', function (n, o) {
            if (n !== o) {
                $scope.$emit('Value_Changed', n);
            }
        });
    };


})(window.angular);