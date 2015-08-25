angular.module('ui.browser', ['ngAnimate', 'ui.bootstrap'])
    .directive('uiBrowser', ['ui.config', function (uiConfig) {
        return {
            restrict: 'A',
            replace: false,
            transclude: true,
            scope: {},
            bindToController: {
                itemsSelected: '&',
                modelSize: '='
            },
            link: function($scope, $element) {
                $element.bind('click', $scope.open);
                $scope.$on('$destroy', function () {
                    $element.unbind('click');
                });
            },
            templateUrl: function(element, attrs) {
                var options = {};
                if (attrs.config !== undefined) options = uiConfig.browsersConfig[attrs.config] || {};
                options = angular.extend({}, options, attrs.uiBrowser);
                if (options.hasOwnProperty('templateUrl')) {
                    return opts.templateUrl;
                }
                return '/Views/Shared/_Browser.html';
            },
            controller: 'uiBrowserController',
            controllerAs: 'ctrl'
        };
    }]);

angular.module('ui.browser')
    .controller('uiBrowserController', ['$scope', '$element', '$attrs', '$transclude', 'ui.config', '$modal', '$log', '$q', '$http',
            function ($scope, $element, $attrs, $transclude, uiConfig, $modal, $log, $q, $http) {
        var options = {};
        if ($attrs.config !== undefined) options = uiConfig.browsersConfig[$attrs.config] || {};
        options = angular.extend({}, options, $attrs.uiBrowser);
        var selectedItem = this.itemsSelected;
        var modelSize = this.modelSize;

        $scope.open = function () {
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: 'Browser.html',
                controller: 'BrowserInstnace',
                size: modelSize,
                resolve: {
                    selected: function() {
                        return selectedItem().slice();
                    },
                    config: function() {
                        return options;
                    },
                    items: function () {
                        if (typeof options.DataSource === "function") {
                            return options.DataSource();
                        } else if ((typeof options.DataSource == 'string' || options.DataSource instanceof String) && options.DataSource.length > 0) {
                            var d = $q.defer();
                            $http.get(options.DataSource).success(function (response) {
                                d.resolve(response);
                            });
                            return d.promise;
                        }
                        return [];
                    }
                }
            });
            modalInstance.result.then(function (items) {
                selectedItem({ items: items });
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        }
    }]);

angular.module('ui.browser').controller('BrowserInstnace', ['$scope', '$modalInstance', 'items', 'config', 'selected', function ($scope, $modalInstance, items, config, selected) {
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
        $modalInstance.close($scope.selectedItem);
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
}]);