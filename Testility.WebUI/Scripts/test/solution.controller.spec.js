define(['angular', 'angularMocks', 'solutionServiceMock','uiMessagingMock', 'uiDialogboxMock',
        'solutionModule', 'solutionController'], function () {

    var solutionController, scope, solutionService, dialog, msg;

    beforeEach(angular.mock.module('ui.dialogbox'));
    beforeEach(angular.mock.module('ui.messaging'));
    beforeEach(angular.mock.module('testility.solution'));
    beforeEach(inject(function ($controller, $rootScope, SolutionService, dialogbox, messaging) {
        scope = $rootScope.$new();
        scope.SolutionForm = {};
        solutionService = SolutionService;
        dialog = dialogbox;
        msg = messaging;
        solutionController = $controller('SolutionController', {
            $scope: scope
        });        
    }));

    describe('testility.SolutionController', function () {
        it('SolutionService get was invoked during opening', function () {            
            expect(solutionService.get).toHaveBeenCalled();
        });

        it('Messaging init was invoked', function () {
            expect(msg.init).toHaveBeenCalled();
        });

        it('RemoveTab was invoked', function () {
            solutionController.RemoveTab(1);
            expect(solutionService.removeItem).toHaveBeenCalledWith(1);
        });

        it('AddTab for not Loaded Controller', function () {
            solutionService.Loaded = false
            expect(function () {
                solutionController.AddTab();
            }).toThrow();
        });

        it('AddTab for Loaded Controller', function () {
            solutionController.AddTab();
            expect(dialog.show).toHaveBeenCalled();
            expect(solutionService.newItem).toHaveBeenCalledWith('Ok');
        });

        it('AddTab press Cancelled', function () {
            dialog.show = jasmine.createSpy('show')
                        .and.callFake(function () {
                            return {
                                then: function (f, r) {
                                    if (r) {
                                        r('Cancelled');
                                    }
                                }
                            }
                        });
            solutionController.AddTab();
            expect(dialog.show).toHaveBeenCalled();
            expect(solutionService.newItem).not.toHaveBeenCalled();
        });

        it('Referesh was faild Message was showed', function () {
            solutionService.get = function () {
                return {
                    then: function (f, r) {
                        if (r) {
                            r('Error');
                        };
                    }
                }
            };
            solutionController.Refresh();
            expect(dialog.show).toHaveBeenCalledWith({ caption: 'Solution', message: 'Error', icon: 'Error' });
        });

        it('Compile for not Loaded Controller', function () {
            solutionService.Loaded = false
            expect(function () {
                solutionController.Compile();
            }).toThrow();
        });

        it('Compile for without Errors', function () {
            solutionService.Loaded = true
            solutionService.compile = jasmine.createSpy('compile')
                        .and.callFake(function () {
                            return {
                                then: function (f, r) {
                                    if (f) {
                                        f('Ok');
                                    }
                                }
                            }
                        });
            solutionController.Compile();
            expect(msg.clearMessages).toHaveBeenCalled();
            expect(msg.addMessage).not.toHaveBeenCalled();
        });

        it('Compile for with Errors', function () {
            solutionService.Loaded = true
            solutionService.compile = jasmine.createSpy('compile')
                        .and.callFake(function () {
                            return {
                                then: function (f, r) {
                                    if (f) {
                                        f([{ Message: 'Error', Alert: 'danger' }]);
                                    }
                                }
                            }
                        });
            solutionController.Compile();
            expect(msg.clearMessages).toHaveBeenCalled();
            expect(msg.addMessage).toHaveBeenCalled();
        });

        it('Compile faild for with Errors', function () {
            solutionService.Loaded = true
            solutionService.compile = jasmine.createSpy('compile')
                        .and.callFake(function () {
                            return {
                                then: function (f, r) {
                                    if (r) {
                                        r([{ Message: 'Error', Alert: 'danger' }]);
                                    }
                                }
                            }
                        });
            solutionController.Compile();
            expect(msg.clearMessages).toHaveBeenCalled();
            expect(msg.addMessage).toHaveBeenCalled();
        });

        it('Compile faild', function () {
            solutionService.Loaded = true
            solutionService.compile = jasmine.createSpy('compile')
                        .and.callFake(function () {
                            return {
                                then: function (f, r) {
                                    if (r) {
                                        r('Error');
                                    }
                                }
                            }
                        });
            solutionController.Compile();
            expect(msg.clearMessages).toHaveBeenCalled();
            expect(msg.addMessage).not.toHaveBeenCalled();
            expect(dialog.show).toHaveBeenCalled();
        });

        it('Submit $invalid not saved', function () {
            solutionService.submit = jasmine.createSpy('submit');
            scope.SolutionForm.$invalid = true;
            scope.SolutionForm.$pending = false;
            solutionController.Submit();
            expect(solutionService.submit).not.toHaveBeenCalled();
        });

        it('Submit $pending not saved', function () {
            solutionService.submit = jasmine.createSpy('submit');
            scope.SolutionForm.$invalid = false;
            scope.SolutionForm.$pending = true;
            solutionController.Submit();
            expect(solutionService.submit).not.toHaveBeenCalled();
        });

        it('Submit saved with compilation Errors', function () {
            solutionService.submit = jasmine.createSpy('submit')
                            .and.callFake(function () {
                                return {
                                    then: function (f, r) {
                                        if (f) {
                                            f({
                                                compileErrors: [{ Message: 'Error', Alert: 'danger' }]
                                            });
                                        }
                                    }
                                }
                            });
            scope.SolutionForm.$invalid = false;
            scope.SolutionForm.$pending = false;
            solutionController.Submit();
            expect(solutionService.submit).toHaveBeenCalled();
            expect(msg.addMessage).toHaveBeenCalled();
        });

        it('Submit saved with Errors', function () {
            solutionService.submit = jasmine.createSpy('submit')
                            .and.callFake(function () {
                                return {
                                    then: function (f, r) {
                                        if (r) {
                                            r([{ Message: 'Error', Alert: 'danger' }]);
                                        }
                                    }
                                }
                            });
            scope.SolutionForm.$invalid = false;
            scope.SolutionForm.$pending = false;
            solutionController.Submit();
            expect(solutionService.submit).toHaveBeenCalled();
            expect(msg.addMessage).toHaveBeenCalled();
        });

        it('Submit failed', function () {
            solutionService.submit = jasmine.createSpy('submit')
                            .and.callFake(function () {
                                return {
                                    then: function (f, r) {
                                        if (r) {
                                            r('Error');
                                        }
                                    }
                                }
                            });
            scope.SolutionForm.$invalid = false;
            scope.SolutionForm.$pending = false;
            solutionController.Submit();
            expect(solutionService.submit).toHaveBeenCalled();
            expect(dialog.show).toHaveBeenCalled();
        });
    });
});