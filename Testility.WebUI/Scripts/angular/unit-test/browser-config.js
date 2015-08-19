angular.module('browser.config', [])
    .factory('config', function ($http) {
        return {
            getDataSource: function () {
                return ['Item1', 'Item2', 'Item3'];
            },
            MultiSelection: false
        };
    });