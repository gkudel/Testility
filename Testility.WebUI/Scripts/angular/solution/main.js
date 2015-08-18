var solutionApp = angular.module("solutionApp", []);

solutionApp.controller("ReferenceController", ['$scope', '$http', function ($scope, $http) {

    $scope.Selected = function (id) {
        var returnValue = false;
        angular.forEach($scope.SolutionReferences, function (item, key) {
            if (item.Id === id) {
                returnValue = true;
                return;
            }
        });
        return returnValue;
    };
   
    $scope.ReferencesClick = function (id) {
        if ($scope.References === undefined) {
            $http.get('/References/GetListOfReferences/' + id, 'content.json').then(function (response) {
                $scope.References = response.data.AvailableReferences;
                $scope.SolutionReferences = response.data.SelectedReferences;
            });
        }
    }

    $scope.showData = function () {
        $scope.curPage = 0;
        $scope.pageSize = 5;
        $scope.numberOfPages = function () {
            return Math.ceil($scope.References.length / $scope.pageSize);
        };

    }

}]);


angular.module('solutionApp').filter('pagination', function () {
    return function (input, start) {
        start = +start;
        return input.slice(start);
    };
});

