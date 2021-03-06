﻿define(['angular', 'angularMocks', 'uiMessagingMock','uiDialogboxMock', 'solutionModule', 'solutionController'],
    function (angular, mocks) {

        describe('testility.SolutionController', function () {
            var solutionController, scope, solutionService, dialog, msg;

            beforeEach(mocks.module('ui.dialogbox'));
            beforeEach(mocks.module('ui.messaging'));
            beforeEach(mocks.module('testility.solution', function($provide){
                var mock = {
                    Loaded: true,
                    init: function () {
                    },
                    Solution: {
                        Id: 0,
                        Name: '',
                        Language: 0,
                        References: [],
                        Items: [],
                        RefList: []
                    },
                    get: jasmine.createSpy('get')
                        .and.callFake(function () {
                            return {
                                then: function (f) {
                                    if (f) {
                                        f('Ok');
                                    };
                                }
                            }
                        }),
                    removeItem: jasmine.createSpy('removeItem'),
                    newItem: jasmine.createSpy('newItem')
                };
                $provide.value('SolutionService', mock);
            }));
            beforeEach(inject(function ($controller, $rootScope, SolutionService, dialogbox, messaging) {
                scope = $rootScope.$new();
                solutionService = SolutionService;
                dialog = dialogbox;
                msg = messaging;
                SolutionInputForm = {};
                solutionController = $controller('SolutionController', {
                    $scope: scope
                });
            }));

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
                                r({ data: 'Error' });
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
                expect(msg.clear).toHaveBeenCalled();
                expect(msg.add).not.toHaveBeenCalled();
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
                expect(msg.clear).toHaveBeenCalled();
                expect(msg.add).toHaveBeenCalled();
            });

            it('Compile faild for with Errors', function () {
                solutionService.Loaded = true
                solutionService.compile = jasmine.createSpy('compile')
                            .and.callFake(function () {
                                return {
                                    then: function (f, r) {
                                        if (r) {
                                            r({ data: [{ Message: 'Error', Alert: 'danger' }] });
                                        }
                                    }
                                }
                            });
                solutionController.Compile();
                expect(msg.clear).toHaveBeenCalled();
                expect(msg.add).toHaveBeenCalled();
            });

            it('Compile faild', function () {
                solutionService.Loaded = true
                solutionService.compile = jasmine.createSpy('compile')
                            .and.callFake(function () {
                                return {
                                    then: function (f, r) {
                                        if (r) {
                                            r({ data: 'Error' });
                                        }
                                    }
                                }
                            });
                solutionController.Compile();
                expect(msg.clear).toHaveBeenCalled();
                expect(msg.add).not.toHaveBeenCalled();
                expect(dialog.show).toHaveBeenCalled();
            });

            it('Submit $invalid not saved', function () {
                solutionService.submit = jasmine.createSpy('submit');
                SolutionInputForm.$invalid = true;
                SolutionInputForm.$pending = false;
                solutionController.Submit();
                expect(solutionService.submit).not.toHaveBeenCalled();
            });

            it('Submit $pending not saved', function () {
                solutionService.submit = jasmine.createSpy('submit');
                SolutionInputForm.$invalid = false;
                SolutionInputForm.$pending = true;
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
                SolutionInputForm.$invalid = false;
                SolutionInputForm.$pending = false;
                solutionController.Submit();
                expect(solutionService.submit).toHaveBeenCalled();
                expect(msg.add).toHaveBeenCalled();
            });

            it('Submit saved with Errors', function () {
                solutionService.submit = jasmine.createSpy('submit')
                                .and.callFake(function () {
                                    return {
                                        then: function (f, r) {
                                            if (r) {
                                                r({ data: [{ Message: 'Error', Alert: 'danger' }] });
                                            }
                                        }
                                    }
                                });
                SolutionInputForm.$invalid = false;
                SolutionInputForm.$pending = false;
                solutionController.Submit();
                expect(solutionService.submit).toHaveBeenCalled();
                expect(msg.add).toHaveBeenCalled();
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
                SolutionInputForm.$invalid = false;
                SolutionInputForm.$pending = false;
                solutionController.Submit();
                expect(solutionService.submit).toHaveBeenCalled();
                expect(dialog.show).toHaveBeenCalled();
            });
        });
    });