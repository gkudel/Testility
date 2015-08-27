angular.module('Testility')
    .controller('SolutionController', ['$scope', 'solutionservice', 'messagebox', function ($scope, service, messagebox) {

        $scope.Loaded = false;
        $scope.ReferencesModelSize = 'md';
        $scope.Messages = [];
        $scope.References = function (items) {
            if (items !== undefined) $scope.Solution.References = items;
            return $scope.Solution.References || [];
        };

        service.get(Solution.Json.value).then(function (solution) {
            $scope.Solution = solution;
            $scope.Loaded = true;
        }, function (error) {
            $scope.Solution = service.empty();
            if (error.hasOwnProperty('Message')) messagebox.show('Solution', error.Message, 'Error');
        });
        
        $scope.addTab = function (solutionId) {
            if (!$scope.Solution.Items) $scope.Solution.Items = [];
            $scope.Solution.Items.push({ Id: 0, Name: 'Any Name', active: true, SolutionId: $scope.Solution.Id });
        };

        $scope.removeTab = function (index) {
            if (!$scope.Solution.Items) $scope.Solution.Items = [];
            if (index <= $scope.Solution.Items.length) {
                $scope.Solution.Items.splice(index, 1);
            }
        };

        $scope.compile = function () {
            service.compile($scope.Solution).then(function (status) {
                $scope.Messages = $scope.Messages.concat(status);
            }, function (error) {

            });
        };

        $scope.removeMessage = function (index) {
            if (!$scope.Messages) $scope.Messages = [];
            if (index <= $scope.Messages.length) {
                $scope.Messages.splice(index, 1);
            }
        };
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
    