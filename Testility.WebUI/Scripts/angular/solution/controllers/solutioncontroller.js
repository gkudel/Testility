﻿angular.module('Testility')
    .controller('SolutionController', ['$scope', '$http', '$location', '$q', function ($scope, $http, $location, $q) {

        $scope.ReferencesModelSize = 'md';
        $scope.References = function (items) {
            if (items !== undefined) $scope.Solution.References = items;
            return $scope.Solution.References || [];
        };

        $scope.Solution = {
            Id: 0,
            Name: '',
            Language: 0,
            References: [],
            Items: []
        };


        if (Solution.Json.value) {
            $scope.Solution = JSON.parse(Solution.Json.value);
        } else {

            var array = /Solution\/Edit\/(\d+)/.exec($location.absUrl());
            var id = undefined;
            if (array && array.length > 1) {
                id = array[1];
            }

            if (id !== undefined) {
                $http.get('/api/Solution/' + id).then(function (response) {
                    $scope.Solution = response.data;
                });
            }
        }

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
    