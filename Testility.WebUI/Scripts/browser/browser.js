angular.module('ui.browser', ['ngAnimate', 'ui.bootstrap'])
    .directive('uiBrowser', ['ui.config', function (uiConfig) {
        return {
            restrict: 'A',
            scope: true,
            replace: false,
            transclude: true,
            templateUrl: function(element, attrs) {
                var options;
                if (attrs.config !== undefined) options = uiConfig.browsersConfig[attrs.config] || {};
                opts = angular.extend({}, options, attrs.uiBrowser);
                if (opts.hasOwnProperty('templateUrl')) {
                    return opts.templateUrl;
                }
                return '/Views/Shared/_Browser.html';
            },
            controller: 'uiBrowserController'
        };
    }]);

angular.module('ui.browser')
    .controller('uiBrowserController', ['$scope', '$element', '$attrs', '$transclude', 'ui.config', '$modal', '$log', '$q', '$http',
            function ($scope, $element, $attrs, $transclude, uiConfig, $modal, $log, $q, $http) {
        var options;
        if ($attrs.config !== undefined) options = uiConfig.browsersConfig[$attrs.config] || {};
        opts = angular.extend({}, options, $attrs.uiBrowser);

        $scope.open = function (size) {
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: 'Browser.html',
                controller: 'BrowserInstnace',
                size: size,
                resolve: {
                    items: function () {
                        if (typeof opts.DataSource === "function") {
                            return opts.DataSource();s
                        } else if ((typeof opts.DataSource == 'string' || opts.DataSource instanceof String) && opts.DataSource.length > 0) {
                            var d = $q.defer()
                            $http.get(opts.DataSource).success(function (response) {
                                d.resolve(response);
                            });
                            return d.promise;
                        }
                        return [];
                    },
                    config: function () {
                        return opts;
                    },
                    selected: $scope.selected
                }
            });
            modalInstance.result.then(function (selectedItem) {
                $scope.selected = selectedItem;
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });

        }
    }]);

angular.module('ui.browser').controller('BrowserInstnace', ['$scope', '$modalInstance', 'items', 'config', 'selected', function ($scope, $modalInstance, items, config, selected) {
    $scope.allitems = items;
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
        $modalInstance.close($scope.selectedItem);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    var indexOf = function(item) {
        return $.grep($scope.selectedItem, function (e) { return config.Equal(e, item); })
    }
    $scope.selected = function (item) {
        return indexOf(item) > 1
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