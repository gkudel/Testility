angular.module('solutionApp').controller('SolutionController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
    var array = /Solution\/Edit\/(\d+)/.exec($location.absUrl());
    var id = undefined;
    if (array && array.length > 1) {
        id = array[1]
    }

    $scope.Name = '';
    $scope.Language = 0;
    $scope.References = [];
    $scope.counter = 2;
    $scope.Items = [
        { Id: 1, Name: 'Account.cs', Active: true },
        { Id: 2, Name: 'IRepositorium.cs', Active: false }
    ];

    if (id !== undefined) {
        $http.get('/api/Solution/' + id).then(function (response) {
        });
    }

    $scope.selectedTab = 0; 

    $scope.selectTab = function (index) {
        $scope.selectedTab = index;
    }

    $scope.addTab = function () {
        $scope.counter++;
        $scope.Items.push({ Id: $scope.counter, Name: 'Any Name' });
        $scope.selectedTab = $scope.Items.length - 1; 
    }

    $scope.deleteTab = function (index) {
        $scope.Items.splice(index, 1);
    }
}]);
