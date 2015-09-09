(function (angular) {
    angular.module("testility")
        .config(Config);

    Config.$inject = ['$httpProvider', 'RestangularProvider'];

    function Config ($httpProvider, RestangularProvider) {
        $httpProvider.interceptors.push(function ($q) {
            return {
                'request': function (config) {
                    return config;
                },
                'requestError': function (rejection) {
                    return $q.reject(rejection);
                },
                'response': function (response) {
                    return response;
                },
                'responseError': function (rejection) {
                    return $q.reject(rejection);
                }
            };
        });
        RestangularProvider.setBaseUrl('/api')
    };
})(window.angular);