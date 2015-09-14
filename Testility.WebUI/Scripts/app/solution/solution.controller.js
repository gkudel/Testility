(function (angular) {
    angular
        .module('testility.solution')
        .controller('SolutionController', SolutionController);        

    SolutionController.$inject = ['$scope', 'SolutionService', 'dialogbox', 'messaging'];
    function SolutionController($scope, service, dialogbox, messaging) {
        var vm = this;

        service.init();
        messaging.init($scope.SolutionForm, $scope);

        //Members
        vm.Entry = service.Entry;
        vm.Solution = service.Solution;
        //Functions
        vm.IsLoaded = service.Loaded;
        vm.AddTab = addTab;
        vm.RemoveTab = removeTab;
        vm.Refresh = refresh;
        vm.Compile = compile;
        vm.Submit = submit;
        vm.GetReferences = service.Solution.RefList;
        vm.SetReferences = setReferences;
        vm.ChangeSolution = changeSolution;

        refresh();

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
            messaging.clearMessages();

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
                messaging.clearMessages();
                service.compile().then(function (response) {
                    if (Array.isArray(response)) {
                        messaging.addMessage(response);
                    }
                }, function (error) {
                    if (Array.isArray(error)) {
                        messaging.addMessage(error);
                    } else {
                        dialogbox.show({ caption: 'Solution', message: error, icon: 'Error' });
                    }
                });
            } else {
                throw 'Solution not loaded';
            }
        };

        function submit() {
            messaging.clearMessages();
            if (!$scope.SolutionForm.$invalid && !$scope.SolutionForm.$pending) {
                service.submit().then(function (response) {
                    if (response) {
                        if (response.hasOwnProperty('compileErrors'))
                            messaging.addMessage(response.compileErrors);
                    }
                }, function (error) {
                    if (Array.isArray(error)) {
                        messaging.addMessage(error);
                    } else {
                        dialogbox.show({ caption: 'Solution', message: error, icon: 'Error' });
                    }
                });
            }
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
    };
})(window.angular);
    