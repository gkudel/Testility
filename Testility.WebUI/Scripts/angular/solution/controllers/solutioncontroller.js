angular.module('Testility')
    .directive("codeArea", function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            scope: true,
            link: function ($scope, $element, $attr) {
                CodeMirror.fromTextArea($element[0], {
                    matchBrackets: true,
                    mode: "text/x-csharp",
                    lineNumbers: true
                });
            }
        };
    })
    .controller('SolutionController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        var array = /Solution\/Edit\/(\d+)/.exec($location.absUrl());
        var id = undefined;
        if (array && array.length > 1) {
            id = array[1]
        }

        $scope.Solution = {
            Id: 0,
            Name: '',
            Language: 0,
            References: [],
            Items: []
        };

        if (id !== undefined) {
            $http.get('/api/Solution/' + id).then(function (response) {
                $scope.Solution = response.data;
            });
        }

        $scope.addTab = function (solutionId) {
            $scope.Solution.Items.push({ Id: 0, Name: 'Any Name', active: true, SolutionId: $scope.Solution.Id });
        }
    }])