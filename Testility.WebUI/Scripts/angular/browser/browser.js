angular.module('ui.browser', ['ngAnimate', 'ui.bootstrap'])
    .directive('uiBrowser', ['ui.config', function (uiConfig) {
        return {
            restrict: 'A',
            replace: false,
            transclude: true,
            scope: {},
            bindToController: {
                getSelected: '&',
                setSelected: '&',
                modelSize: '='
            },
            link: function($scope, $element) {
                $element.bind('click', $scope.open);
                $scope.$on('$destroy', function () {
                    $element.unbind('click');
                });
            },
            template: '<span ng-transclude></span>',
            controller: 'uiBrowserController',
            controllerAs: 'ctrl'
        };
    }])
    .factory('uiBrowserDialog', ['$modal', '$q', '$http', 'dialogbox', function ($modal, $q, $http, dialogbox) {
        var service = {
            show: function (options, modelSize, getSelected, setSelected, cancelled) {
                var opt = {
                    templateUrl: '/Views/Shared/_Browser.html',
                    DataSource: function() { return []; },
                    title: 'Browser',
                    MultiSelection: false,
                    Equal: function (e) { return false; },
                    GetResultValue: function (e) { return undefined; },
                    PrintElement: function(e) { return ''; }
                };
                options = angular.extend({}, opt, options);
                if (options.hasOwnProperty('templateUrl')) {
                    templateUrl = options.templateUrl;
                }
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: function () { return templateUrl; },
                    controller: 'BrowserInstnace',
                    size: modelSize,
                    resolve: {
                        selected: function () {
                            return getSelected().slice();
                        },
                        config: function () {
                            return options;
                        },
                        items: function () {
                            var d = $q.defer();
                            if (typeof options.DataSource === "function") {
                                d.resolve(options.DataSource());
                            } else if ((typeof options.DataSource == 'string' || options.DataSource instanceof String) && options.DataSource.length > 0) {

                                $http.get(options.DataSource)
                                    .success(function (response) {
                                        d.resolve(response);
                                    }).error(function (data, status) {
                                        d.reject(data, status);
                                    });;
                            }
                            return d.promise;
                        }
                    }
                });
                modalInstance.result.then(function (items) {
                    setSelected({ items: items });
                }, function (error, status) {
                    if (cancelled) cancelled();
                });
            }
        };
        return service;
    }])
    .controller('uiBrowserController', ['$scope', '$attrs', 'ui.config', 'uiBrowserDialog',
            function ($scope, $attrs, uiConfig, uiBrowserDialog) {
        var options = {};
        if ($attrs.config) options = uiConfig.browsersConfig[$attrs.config] || {};
        options = angular.extend({}, options, $attrs.uiBrowser);
        var getSelectedItem = this.getSelected;
        var setSelectedItem = this.setSelected;
        var modelSize = this.modelSize;
        $scope.open = function () {
            uiBrowserDialog.show(options, modelSize, getSelectedItem, setSelectedItem);
        }        
    }])
    .controller('BrowserInstnace', ['$scope', '$modalInstance', 'items', 'config', 'selected', function ($scope, $modalInstance, items, config, selected) {
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
}]);