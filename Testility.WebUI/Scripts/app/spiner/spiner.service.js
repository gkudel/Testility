(function (angular) {
    angular.module('ui.spiner')
        .factory('qSpiner', qSpinerService);
    
    qSpinerService.$inject = ['$q', '$timeout'];
    function qSpinerService($q, $timeout) {
        var spinerService = {
            defer: function (caption, config) {
                var s = new qSpiner();
                Object.defineProperty(s, 'promise', {
                    get: function () {
                        return s.getPromise();
                    }
                });
                s.defer(caption, config);
                return s;
            }
        };

        function qSpiner() {
            var overlay = undefined;
            var d = undefined;
            var valid = function () {
                if (!d) throw "Defer was not invoked";
            };
            var options = {
                lines: 13,
                length: 11,
                width: 5,
                radius: 17,
                corners: 1,
                rotate: 0,
                color: '#FFF',
                speed: 1,
                trail: 60,
                shadow: false,
                hwaccel: false,
                className: 'spinner',
                zIndex: 2e9,
                top: 'auto',
                left: 'auto',
                resolve: '/Content/images/check.png',
                resolveText: 'Success',
                reject: '/Content/images/cross.png',
                rejectText: 'Fail'
            };
            this.defer = function (caption, config) {
                if (!(!caption || caption.length === 0)) {
                    var options = angular.extend({}, options, config);

                    var target = document.createElement("div");
                    document.body.appendChild(target);
                    var spinner = new Spinner(options).spin(target);
                    overlay = iosOverlay({
                        text: caption || "Loading",
                        spinner: spinner
                    });
                }
                d = $q.defer();
            };
            this.resolve = function (result) {
                valid();
                d.resolve(result);
                if (overlay) {
                    if (options.resolve) {
                        overlay.update({
                            icon: options.resolve,
                            text: options.resolveText
                        });

                        $timeout(function () {
                            overlay.hide();
                        }, 300);
                    } else {
                        overlay.hide();
                    }
                }
            };
            this.reject = function (result) {
                valid();
                d.reject(result);
                if (overlay) {
                    if (options.reject) {
                        overlay.update({
                            icon: options.reject,
                            text: options.rejectText
                        });

                        $timeout(function () {
                            overlay.hide();
                        }, 300);
                    } else {
                        overlay.hide();
                    }
                }
            };
            this.getPromise = function () {
                valid();
                return d.promise;
            };
        };
        return spinerService;
    };
})(window.angular);