(function (angular) {
    angular
        .module('testility.solution')
        .controller('SolutionController', SolutionController)
        .controller('CodeMirrorController', CodeMirrorController);
           
    SolutionController.$inject = ['$scope', 'SolutionService', 'dialogbox', 'messaging'];
    function SolutionController($scope, service, dialogbox, messaging) {
        
        var vm = this;

        service.init();
        messaging.init(SolutionInputForm, $scope);

        //Members
        vm.Entry = service.Entry;
        vm.Solution = service.Solution;
        //Functions
        vm.IsLoaded = isLoaded;
        vm.AddTab = addTab;
        vm.RemoveTab = removeTab;
        vm.Refresh = refresh;
        vm.Compile = compile;
        vm.Submit = submit;        
        vm.GetReferences = getReferences;
        vm.SetReferences = setReferences;
        vm.ChangeSolution = changeSolution;
        vm.SelectedTab = selectedTab;

        refresh();

        function isLoaded() {
            return service.Loaded;
        }

        function addTab(solutionId) {
            if (service.Loaded) {
                var result = dialogbox.show({
                    caption: 'Specify Name for Item',
                    type: 'DialogBox',
                    buttons: 'OkCancel', 
                    value: 'Class.cs',
                    modal: true
                });
                result.then(function (result) {
                    service.newItem(result);
                }
                , function (result) {
                });
            } else {
                throw 'Solution not loaded';
            }
        };

        function removeTab(index) {
            service.removeItem(index);
        };

        function refresh() {
            messaging.clear();

            service.get()
                .then(function (solution) { },
                      function (error) {
                          dialogbox.show({
                              caption: 'Solution', message: error, icon: 'Error'
                          });
                      });
        };

        function compile() {
            if (service.Loaded) {
                messaging.clear();
                service.compile().then(function (response) {
                    if (Array.isArray(response)) {
                        messaging.add(response);
                    }
                }, function (error) {
                    if (Array.isArray(error)) {
                        messaging.add(error);
                    } else {
                        dialogbox.show({ caption: 'Solution', message: error, icon: 'Error' });
                    }
                });
            } else {
                throw 'Solution not loaded';
            }
        };

        function submit() {
            messaging.clear();
            if (!SolutionInputForm.$invalid && !SolutionInputForm.$pending) {
                service.submit().then(function (response) {
                    if (response) {
                        if (response.hasOwnProperty('compileErrors'))
                            messaging.add(response.compileErrors);
                    }
                }, function (error) {
                    if (Array.isArray(error)) {
                        messaging.add(error);
                    } else {
                        dialogbox.show({ caption: 'Solution', message: error, icon: 'Error' });
                    }
                });
            }
        };
        function getReferences() {
            return service.Solution.RefList
        };

        function setReferences(ref) {
            service.Solution.RefList = ref;
        };

        function changeSolution() {
            service.changeSolution(function (response) {                
            }, function (error) {
                if(error) {
                    dialogbox.show({ caption: 'Solution', message: error, icon: 'Error' });
                }
            });
        };

        function selectedTab(item) {
            $scope.$broadcast('SelectedTab_Changed', item);
        }
    };

    CodeMirrorController.$inject = ['$scope', '$timeout'];
    function CodeMirrorController($scope, $timeout) {
        var editorInstance = null;
        var vm = this;        
        vm.editorOptions = {
            lineNumbers: true,
            matchBrackets: true,
            mode: 'text/x-csharp'
        };
        vm.OnLoad = onLoad;
        vm.Code = getSourceCode;

        function getSourceCode(newCode) {
            return arguments.length ? ($scope.$parent.item.Code = newCode) : $scope.$parent.item.Code;
        }

        function onLoad(instance) {
            $timeout(_onLoad.bind(null, instance), 0, false)
        }

        function _onLoad(instance) {
            instance.refresh();
            instance.clearHistory();
            instance.focus();
            editorInstance = instance;
        }

        $scope.$on('SelectedTab_Changed', function (e, item) {            
            if ($scope.$parent.item === item) {
                if (!editorInstance) return;
                var refresh = editorInstance.refresh.bind(editorInstance);
                $timeout(refresh, 0, false);                
            }
        });
    };
})(window.angular);
    