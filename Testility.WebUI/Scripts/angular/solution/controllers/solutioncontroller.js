angular.module('Testility')
    .controller('SolutionController', ['$scope', 'solutionservice', 'dialogbox', 'messaging', function ($scope, service, dialogbox, messaging) {

        service.getInstance();
        $scope.Solution = service.Solution;
        $scope.Loaded = function() {
            return service.Loaded;
        }
        $scope.Entry = service.Entry;
        messaging.init($scope, SolutionForm);

        $scope.addTab = function (solutionId) {
            if (service.Loaded) {
                var result = dialogbox.show({
                    caption: 'Specify Name for Item',
                    type: 'DialogBox',
                    buttons: 'OkCancel', 
                    value: 'Class.cs',
                    modal: true
                });
                result.then(function (result) {
                    service.newItem(result);
                }
                , function (result) {
                });
            }
        };

        $scope.removeTab = function (index) {
            service.removeItem(index);
        };

        $scope.refresh = function () {
            $scope.clearMessages();
            
            service.get()
                .then(function (solution) { },
                      function (error) {
                        dialogbox.show({
                            caption: 'Solution', message: error, icon: 'Error'
                        });
            });
        };

        $scope.compile = function () {
            if (service.Loaded) {
                $scope.clearMessages();
                service.compile().then(function (response) {
                    if (Array.isArray(response)) {
                        $scope.addMessage(response);
                    }
                }, function (error) {
                    if (Array.isArray(error)) {
                        $scope.addMessage(error);
                    } else {
                        dialogbox.show({ caption: 'Solution', message: error, icon: 'Error' });
                    }
                });
            }
        };

        $scope.submit = function () {
            $scope.clearMessages();
            if (!SolutionForm.$invalid && !SolutionForm.$pending) {
                service.submit().then(function (response) {
                    if (response) {
                        if (response.hasOwnProperty('compileErrors'))
                            $scope.addMessage(response.compileErrors);
                    }
                }, function (error) {
                    if (Array.isArray(error)) {
                        $scope.addMessage(error);
                    } else {
                        dialogbox.show({ caption: 'Solution', message: error, icon: 'Error' });
                    }
                });
            }
        };

        $scope.getReferences = function () {
            return service.Solution.RefList;
        };

        $scope.setReferences = function (ref) {
            service.Solution.RefList = ref;
        };


        $scope.changeSolution = function () {
            service.changeSolution(function (response) {                
            }, function (error) {
                if(error) {
                    dialogbox.show({ caption: 'Solution', message: error, icon: 'Error' });
                }
            });
        };

        $scope.refresh();        
    }])
    .controller("CodeController", ['$scope', function ($scope) {
        $scope.code = $scope.$parent.item.Code;
        $scope.$watch("code", function (newValue, oldValue) {
            if (newValue !== oldValue) {
                $scope.$parent.item.Code = newValue;
            }
        });

    }]).directive('convertNumber', function() {
        return {
            require: 'ngModel',
            link: function(scope, el, attr, ctrl) {
                ctrl.$parsers.push(function(value) {
                    return parseInt(value, 10);
                });

                ctrl.$formatters.push(function(value) {
                    return value.toString();
                });      
            }
        }
    });
    