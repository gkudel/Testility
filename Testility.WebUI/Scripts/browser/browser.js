angular.module('browser', ['ngAnimate', 'ui.bootstrap']);

angular.module('browser').controller('BrowserController', ['$scope', '$modal', '$log', 'config', function ($scope, $modal, $log, config) {

    $scope.isResult = false;
    if (config.isResult !== undefined) {
        $scope.isResult = config.isResult;
    }
    $scope.resultName = config.resultName;
    $scope.getResultValue = config.getResultValue;

    $scope.open = function (size) {
        var modalInstance = $modal.open({
            animation: true,
            templateUrl: 'Browser.html',
            controller: 'BrowserInstnace',
            size: size,
            resolve: {
                items: function () {
                    return config.getDataSource();
                },
                config: function () {
                    return config;
                }
            }
        });       
        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
}]);


angular.module('browser').controller('BrowserInstnace', ['$scope', '$modalInstance', 'items', 'config', function ($scope, $modalInstance, items, config) {
    $scope.allitems = items;
    $scope.title = config.title;
    $scope.selectedItem = [];

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
        $modalInstance.close($scope.selectedItem);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.selected = function (item) {
        return $scope.selectedItem.indexOf(item) >= 0;
    };

    $scope.mark = function (item) {
        if (config.MultiSelection) {
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

    $scope.printItem = function (e) { return e; };
    if (config.PrintElement !== undefined) {
        $scope.printItem = config.PrintElement;
    }

    $scope.printSelected = function () {
        if ($scope.selectedItem.length == 0) {
            return 'None';
        }
        return $scope.selectedItem.map($scope.printItem).join();
    };
}]);