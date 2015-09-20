(function (angular) {
    angular.module("testility")
        .filter('bytes', function () {
                return function (bytes, precision) {
                    if (isNaN(parseFloat(bytes)) || !isFinite(bytes)) return '-';
                    if (typeof precision === 'undefined') precision = 2;
                    var units = ['bytes', 'kB', 'MB', 'GB', 'TB', 'PB'];
                    var number = Math.floor(Math.log(bytes) / Math.log(1024));
                    return (bytes / Math.pow(1024, Math.floor(number))).toFixed(precision) + ' ' + units[number];
            };
        });
})(window.angular);