(function (angular) {

    angular.module('ui.browser')
        .controller('uiBrowserController', UiBrowserController)
        .controller('BrowserInstanceController', BrowserInstanceController);


    UiBrowserController.$inject = ['$scope', '$attrs', 'ui.config', 'uiBrowserDialog'];

    function UiBrowserController($scope, $attrs, uiConfig, uiBrowserDialog) {
        var options = {};
        if ($attrs.config) options = uiConfig.browsersConfig[$attrs.config] || {};
        options = angular.extend({}, options, $attrs.uiBrowser);
        var getSelectedItem = this.getSelected;
        var setSelectedItem = this.setSelected;
        var modelSize = this.modelSize;
        $scope.open = function () {
            uiBrowserDialog.show(options, modelSize, getSelectedItem, setSelectedItem);
        }        
    };


    BrowserInstanceController.$inject = ['$scope', '$modalInstance', 'items', 'config', 'selected'];

    function BrowserInstanceController($scope, $modalInstance, items, config, selected) {
        $scope.allitems = items || [];
        $scope.title = config.title;
        $scope.selectedItem = selected || [];

        $scope.itemsPerPage = 3;
        $scope.currentPage = 1;
        $scope.pageCount = function () {
            return Math.ceil($scope.allitems.length / $scope.itemsPerPage);
        };
        $scope.totalItems = $scope.allitems.length;
        $scope.$watch('currentPage + itemsPerPage', function () {
            var begin = (($scope.currentPage - 1) * $scope.itemsPerPage),
              end = begin + $scope.itemsPerPage;

            $scope.items = $scope.allitems.slice(begin, end);
        });


        $scope.ok = function () {
            if (config.MultiSelection) {
                $modalInstance.close($scope.selectedItem);
            } else {
                if ($scope.selectedItem.length > 0) {
                    $modalInstance.close($scope.selectedItem[0]);
                } else {
                    $modalInstance.dismiss('cancel');
                }
            }
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        var indexOf = function(item) {
            return $scope.selectedItem.findIndex(function (e) { return config.Equal(e, item); });
        }

        $scope.selected = function (item) {
            return indexOf(item) >= 0;
        };

        $scope.mark = function (item) {
            if (config.MultiSelection) {
                if ($scope.selected(item)) {
                    var index = indexOf(item);
                    $scope.selectedItem.splice(index, 1);
                } else {
                    $scope.selectedItem.push(config.GetResultValue(item));
                }
            } else {
                if ($scope.selected(item)) {
                    $scope.selectedItem = [];
                } else {
                    $scope.selectedItem = [config.GetResultValue(item)];

                }
            }
        };    

        $scope.printItem = function (e) { return e; };
        if (typeof config.PrintElement === "function") {
            $scope.printItem = config.PrintElement;
        }

        $scope.printSelected = function () {
            if ($scope.selectedItem.length == 0) {
                return 'None';
            }
            var s = undefined;
            for (var i = 0; i < $scope.allitems.length; i++) {
                var ele = $scope.allitems[i];
                if ($scope.selected(ele)) {
                    if (s == undefined) {
                        s = $scope.printItem(ele);
                    } else {
                        s = s + ',' + $scope.printItem(ele);
                    }
                }
            }
            return s;
        };
    }

})(window.angular);