define(['angular', 'angularMocks', 'testility',
        'solutionController', 'setupserviceMock',
        'restangularMock', 'uiCodemirrorMock',
        'uiBrowserMock', 'uiMessagingMock', 'uiMessagingMock',
        'uiDialogboxMock', 'uiSpinerMock', 'validationMock'], function () {
    var solutionController, scope;

    beforeEach(angular.mock.module('testility'));
    beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();
        solutionController = $controller('SolutionController', {
            $scope: scope
        });
    }));

    describe('Testility', function () {
        it('is defined', function () {
            expect(3).toEqual(3);
        });
    });
});