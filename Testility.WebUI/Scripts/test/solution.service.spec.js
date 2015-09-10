define(['angular', 'angularMocks', 'testility'], function () {
    beforeEach(angular.mock.module('testility'));
    var solutionController, scope;
    
    /*beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();
        solutionController = $controller('SolutionController', {
            $scope: scope
        });
    }));*/
    describe('Testility', function () {
        it('is defined', function () {
            expect(3).toEqual(3);
        });
    });
});