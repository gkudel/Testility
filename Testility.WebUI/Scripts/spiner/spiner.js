angular.module('ui.spiner', [])
    .factory('spiner', function () {
        var spinerService = {
            show: function (caption, config) {
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
                    left: 'auto'
                };
                options = angular.extend({}, options, config);

                var target = document.createElement("div");
                document.body.appendChild(target);
                var spinner = new Spinner(options).spin(target);
                var overlay = iosOverlay({
                    text: caption || "Loading",
                    spinner: spinner
                });
                return overlay;
            }
        };
        return spinerService;
    });
