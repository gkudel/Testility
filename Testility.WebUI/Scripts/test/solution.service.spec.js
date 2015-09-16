define(['angular', 'angularMocks', 'solutionService', 'uiSpinerMock', 'uiBrowserMock'], function (angular, mocks) {
    describe('testility.SolutionService', function () {

        var service;
        var RestangularMock = {};
        var location = {};
        var uiConfig = {};
        var spiner;

        RestangularMock.one = jasmine.createSpy('one')
                              .and.callFake(function () {
                                  return {
                                      Id: 0,
                                      Name: '',
                                      Language: 0,
                                      References: [],
                                      Items: [],
                                      SetupId: 0
                                  };
                              });

        RestangularMock.copy = jasmine.createSpy('copy')
                                .and.callFake(function (s) {
                                    return RestangularMock;
                                });
        RestangularMock.post = jasmine.createSpy('post');

        beforeEach(mocks.module('ui.dialogbox'));
        beforeEach(mocks.module('ui.messaging'));
        beforeEach(mocks.module('ui.browser'));
        beforeEach(mocks.module('ui.spiner'));
        beforeEach(mocks.module('testility.solution', function($provide){
            $provide.value('Restangular', RestangularMock);
            $provide.value('$location', location);
            $provide.value('ui.config', uiConfig);
        }));
        beforeEach(inject(function (SolutionService, qSpiner) {
            service = SolutionService;
            spiner = qSpiner;
        }));

        it('SolutionService call function throw ex if not initialized', function () {
            for (var variable in service) {
                if (typeof service[variable] === 'function') {
                    if (variable !== 'init') {
                        expect(function () {
                            service[variable]();
                        }).toThrow('Service should be initialized or action is not supported');
                    }
                }
            }       
        });

        it('Init Throw exceptionif location not matched', function () {
            location.absUrl = jasmine.createSpy('absUrl')
                                .and.callFake(function () {
                                    return 'NotMatched';
                                });
            expect(function () {
                service.init();
            }).toThrow();
        });
        it('Init not Throw exception if location not matched', function () {
            var url = '';
            location.absUrl = jasmine.createSpy('absUrl')
                                .and.callFake(function () {
                                    return url;
                                });
            url = '/Solution/Create/';
            expect(function () {
                service.init();
            }).not.toThrow();
            expect(function () {
                service.changeSolution();
            }).toThrow('Service should be initialized or action is not supported');
            expect(function () {
                service.runTest('Service should be initialized or action is not supported');
            }).toThrow();

            url = '/Solution/Edit/1';
            expect(function () {
                service.init();
            }).not.toThrow();
            expect(function () {
                service.changeSolution();
            }).toThrow('Service should be initialized or action is not supported');
            expect(function () {
                service.runTest('Service should be initialized or action is not supported');
            }).toThrow();
            expect(service.Solution.Id).toBe(1);

            url = '/UnitTest/Create/';
            expect(function () {
                service.init();
            }).not.toThrow();

            url = '/UnitTest/Edit/1';
            expect(function () {
                service.init();
            }).not.toThrow();
            expect(service.Solution.Id).toBe(1);
        });

        describe('testility.SolutionService Initialized', function () {
            beforeEach(function () {
                location.absUrl = jasmine.createSpy('absUrl')
                    .and.callFake(function () {
                        return '/Solution/Create/';
                    });
                service.init()
            });

            it('Empty return proper object', function () {
                var solution = service.empty();
                expect(solution.Id).toBe(0);
                expect(solution.Name).toBe('');
                expect(solution.References).toBeArray([]);
                expect(solution.Items).toBeArray([]);
                expect(solution.SetupId).toBe(0);
                expect(solution.RefList).toBeDefined();
                expect(solution.ItemsList).toBeDefined();
            });

            it('NewItem added', function () {                
                service.newItem('Class.css');
                expect(service.Solution.Items.length).toBe(1);
                expect(service.Solution.Items[0].Name).toBe('Class.css');
                expect(service.Solution.Items[0].Id).toBe(0);
                expect(service.Solution.Items[0].active).toBe(true);
                expect(service.Solution.Items[0].SolutionId).toBe(service.Solution.Id);
            });

            it('RemoveItem Out of Bounds', function () {
                expect(function () {
                    service.removeItem(10);
                }).toThrow('Index Out of Bounds');
            });

            it('RemoveItem Out of Bounds', function () {
                service.Solution.Items = [
                    { Name: "Class1.css"},
                    { Name: "Class2.css" },
                    { Name: "Class3.css" }
                ];

                expect(function () {
                    service.removeItem(1);
                }).not.toThrow('Index Out of Bounds');
                expect(service.Solution.Items[0].Name).toBe('Class1.css');
                expect(service.Solution.Items[1].Name).toBe('Class3.css');
            });

            it('Compile success', function () {
                RestangularMock.post = jasmine.createSpy('post')
                                        .and.callFake(function (s) {
                                            return {
                                                then: function (r, e) {
                                                    r(s);
                                                }
                                            };
                                        });
                service.compile();
                expect(spiner.defer).toHaveBeenCalled();
                expect(spiner.resolve).toHaveBeenCalledWith('Compile');
                expect(spiner.reject).not.toHaveBeenCalled();
            });

            it('Compile Fail', function () {
                RestangularMock.post = jasmine.createSpy('post')
                                        .and.callFake(function (s) {
                                            return {
                                                then: function (r, e) {
                                                    e(s);
                                                }
                                            };
                                        });
                service.compile();
                expect(spiner.defer).toHaveBeenCalled();
                expect(spiner.reject).toHaveBeenCalled();
                expect(spiner.resolve).not.toHaveBeenCalled();
            });

        });
    });
});