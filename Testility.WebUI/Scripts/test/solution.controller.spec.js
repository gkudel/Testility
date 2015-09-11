define(['angular', 'angularMocks', 'solutionServiceMock','uiMessagingMock', 'uiDialogboxMock',
        'solutionModule', 'solutionController'], function () {
    var solutionController, scope;

    beforeEach(angular.mock.module('ui.dialogbox'));
    beforeEach(angular.mock.module('ui.messaging'));
    beforeEach(angular.mock.module('testility.solution'));
    beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();       
        solutionController = $controller('SolutionController', {
            $scope: scope
        });
    }));

    describe('Testility', function () {
        it('is defined', function () {            
        });
    });
});