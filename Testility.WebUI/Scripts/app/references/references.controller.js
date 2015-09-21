(function (angular) {
    angular
        .module('testility.references')
        .controller('ReferencesController', ReferencesController);

    ReferencesController.$inject = ['$scope'];
    function ReferencesController($scope) {

        var vm = this;
        vm.obj = {};
        vm.UniqId = '';

        vm.FileName = 
        vm.FilePath = '';
        vm.fileUploadSuccess = fileUploadSuccess;
        vm.fileUploadProgress = fileUploadProgress;
        vm.uploadError = uploadError;
        vm.validateFile = validateFile;

        function fileUploadSuccess(message) {
            var jsonResponse = JSON.parse(message);
            vm.FilePath = jsonResponse.FilePath;
            vm.FileName = jsonResponse.FileName;
        }

        function fileUploadProgress(progress) {
            vm.fileProgress = Math.round(progress * 100);
        }

        function uploadError(message) {
            //var jsonResponse = JSON.parse(message);
            //var modelState = jsonResponse.modelState;
            //$scope.modelState = modelState;
        }

        function validateFile($file) {
            vm.modelState = undefined;
            var allowedExtensions = ['dll'];
            var isValidType = allowedExtensions.indexOf($file.getExtension()) >= 0;
            if (!isValidType) vm.modelState = { file: ['type'] };
            return isValidType;
        }
    }
})(window.angular);