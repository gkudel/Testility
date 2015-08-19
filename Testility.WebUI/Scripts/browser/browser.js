angular.module('browser', ['ngAnimate', 'ui.bootstrap', 'browser.config']);

angular.module('browser').controller('BrowserController', function ($scope, $modal, $log, config) {

    $scope.items = config.getDataSource();
    
    $scope.open = function (size) {
        var modalInstance = $modal.open({
            animation: true,
            templateUrl: 'Browser.html',
            controller: 'BrowserInstnace',
            size: size,
            resolve: {
                items: function () {
                    return $scope.items;
                },
                multiselection: function () {
                    return config.MultiSelection;
                }
            }
        });

        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
});


angular.module('browser').controller('BrowserInstnace', function ($scope, $modalInstance, items, multiselection) {

    $scope.items = items;
    $scope.selectedItem = [];

    $scope.ok = function () {
        $modalInstance.close($scope.selected.item);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.selected = function (item) {
        return $scope.selectedItem.indexOf(item) >= 0;
    };

    $scope.mark = function (item) {
        if (multiselection) {
            if ($scope.selected(item)) {
                var index = $scope.selectedItem.indexOf(item);
                $scope.selectedItem.splice(index, 1);
            } else {
                $scope.selectedItem.push(item);
            }
        } else {
            if ($scope.selected(item)) {
                $scope.selectedItem = [];
            } else {
                $scope.selectedItem = [item];

            }
        }
    };

    $scope.printSelected = function () {
        return $scope.selectedItem.join();
    };
});