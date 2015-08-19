var solutionApp = angular.module("solutionApp", []);

solutionApp.controller("ReferenceController", ['$scope', '$http', function ($scope, $http) {

    $scope.Selected = function (id) {
        var returnValue = false;
        angular.forEach($scope.SolutionReferences, function (item, key) {
            if (item === id) {
                returnValue = true;
                return;
            }
        });
        return returnValue;
    };
   
    $scope.ReferencesClick = function (id) {
        if ($scope.References === undefined) {
            $http.get('/References/GetListOfReferences/' + id).then(function (response)
                 {
                    $scope.References = response.data.AvailableReferences;
                    $scope.SolutionReferences = response.data.SelectedReferencesIds;
            }).catch(function(response) {
                console.error('Gists error', response.status, response.data);
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

    $scope.showData();
}]);

angular.module('solutionApp').filter('pagination', function () {
    return function (input, start) {
        start = +start;
        return input.slice(start);
    };
});

