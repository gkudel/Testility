define(['angular', 'angularMocks', 'solutionServiceMock','uiMessagingMock', 'uiDialogboxMock',
        'solutionModule', 'solutionController'], function () {

    var solutionController, scope, solutionService, dialog;

    beforeEach(angular.mock.module('ui.dialogbox'));
    beforeEach(angular.mock.module('ui.messaging'));
    beforeEach(angular.mock.module('testility.solution'));
    beforeEach(inject(function ($controller, $rootScope, SolutionService, dialogbox) {
        scope = $rootScope.$new();
        scope.SolutionForm = {};
        solutionService = SolutionService;
        dialog = dialogbox;
        solutionController = $controller('SolutionController', {
            $scope: scope
        });        
    }));

    describe('testility.solution', function () {
        it('SolutionController RemoveTab was invoked', function () {
            solutionController.RemoveTab(1);
            expect(solutionService.removeItem).toHaveBeenCalledWith(1);
        });
        it('SolutionController AddTab for not Loaded Controller', function () {
            solutionService.Loaded = false
            expect(function () {
                solutionController.AddTab();
            }).toThrow();
        });
        it('SolutionController AddTab for Loaded Controller', function () {
            solutionController.AddTab();
            expect(dialog.show).toHaveBeenCalled();
            expect(solutionService.newItem).toHaveBeenCalledWith('Ok');
        });
    });
});