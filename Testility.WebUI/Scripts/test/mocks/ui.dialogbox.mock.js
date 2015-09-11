(function (angular) {
    angular
        .module('ui.dialogbox', [])
        .factory('dialogbox', function () {
            return {
                show: jasmine.createSpy('show')
                        .and.callFake(function () {
                            return {
                                then: function (f) {
                                    if (f) {
                                        f('Ok');
                                    }
                                }
                            }
                        })
            }
        });
})(window.angular);