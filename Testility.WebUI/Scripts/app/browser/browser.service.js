(function (angular) {
    angular.module('ui.browser')
        .factory('uiBrowserDialog', BrowserService);

    BrowserService.$inject = ['$modal', '$q', '$http', 'dialogbox'];
    function BrowserService($modal, $q, $http, dialogbox) {
        var service = {
            show: function (options, modelSize, getSelected, setSelected, cancelled) {
                var opt = {
                    templateUrl: '/Views/Shared/_Browser.html',
                    DataSource: function () { return []; },
                    title: 'Browser',
                    MultiSelection: false,
                    Equal: function (e) { return false; },
                    GetResultValue: function (e) { return undefined; },
                    PrintElement: function (e) { return ''; }
                };
                options = angular.extend({}, opt, options);
                if (options.hasOwnProperty('templateUrl')) {
                    templateUrl = options.templateUrl;
                }
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: function () { return templateUrl; },
                    controller: 'BrowserInstanceController',
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
    };
})(window.angular);