angular.module('Testility')
    .controller('SolutionController', ['$scope', 'solutionservice', 'dialogbox', 'messaging', function ($scope, service, dialogbox, messaging) {

        $scope.Loaded = false;
        $scope.ReferencesModelSize = 'md';
        $scope.Solution = service.empty();
        messaging.init($scope, SolutionForm);

        $scope.addTab = function (solutionId) {
            if ($scope.Loaded) {                
                var result = dialogbox.show({
                    caption: 'Specify Name for Item',
                    type: 'DialogBox',
                    buttons: 'OkCancel', 
                    value: 'Class.cs',
                    modal: true
                });
                result.then(function (result) {
                    $scope.Solution.ItemsList.push({ Id: 0, Name: result, active: true, SolutionId: $scope.Solution.Id });
                }
                , function (result) {
                });
            }
        };

        $scope.removeTab = function (index) {
            if (index <= $scope.Solution.ItemsList.length) {
                $scope.Solution.ItemsList.splice(index, 1);
            }
        };

        $scope.refresh = function () {
            $scope.clearMessages();
            service.get($scope.Solution).then(function (solution) {
                if (solution) {
                    $scope.Solution = solution;                    
                    $scope.Loaded = true;
                } else {
                    $scope.Solution = service.empty();
                    $scope.Loaded = false;
                }                
            }, function (error) {
                $scope.Solution = service.empty();
                $scope.Loaded = false;
                dialogbox.show({ caption: 'Solution', message: error, icon: 'Error' });
            });
        };

        $scope.compile = function () {
            if ($scope.Loaded) {
                $scope.clearMessages();
                service.compile($scope.Solution).then(function (response) {
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
                service.submit($scope.Solution).then(function (response) {
                    if (response) {
                        if (response.hasOwnProperty('compileErrors'))
                            $scope.addMessage(response.compileErrors);
                        if (response.hasOwnProperty('solution'))
                            $scope.Solution = response.solution;
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

        $scope.References = function (ref) {
            if ($scope.Loaded) {
                if (ref) $scope.Solution.RefList = ref;
                return $scope.Solution.RefList;
            }
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
    