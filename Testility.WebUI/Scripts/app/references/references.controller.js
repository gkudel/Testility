(function (angular) {
    angular
        .module('testility.references')
        .controller('ReferencesController', ReferencesController);

    ReferencesController.$inject = ['$scope'];
    function ReferencesController($scope) {

        $scope.fileUploadSuccess = function (message) {
        };

        $scope.fileUploadProgress = function (progress) {
            $scope.fileProgress = Math.round(progress * 100);
        };

        $scope.uploadError = function (message) {
            //var jsonResponse = JSON.parse(message);
            //var modelState = jsonResponse.modelState;
            //$scope.modelState = modelState;
        };

        $scope.validateFile = function ($file) {
            $scope.modelState = undefined;
            $scope.formUpload.$serverErrors = undefined;
            var allowedExtensions = ['dll'];
            var isValidType = allowedExtensions.indexOf($file.getExtension()) >= 0;
            if (!isValidType) $scope.modelState = { file: ['type'] };
            return isValidType;
        };
    }
})(window.angular);