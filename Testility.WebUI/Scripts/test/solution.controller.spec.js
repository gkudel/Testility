define(['angular', 'angularMocks', 'solutionServiceMock','uiMessagingMock', 'uiDialogboxMock',
        'solutionModule', 'solutionController'], function () {
    var solutionController, scope;

    beforeEach(angular.mock.module('ui.dialogbox'));
    beforeEach(angular.mock.module('ui.messaging'));
    beforeEach(angular.mock.module('testility.solution'));
    beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();
        scope.SolutionForm = {};
        solutionController = $controller('SolutionController', {
            $scope: scope
        });
    }));

    describe('Testility', function () {
        it('is defined', function () {            
        });
    });
});