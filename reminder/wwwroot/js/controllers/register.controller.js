(function () {
    'use strict';

    angular
        .module('app')
        .controller('RegisterController', RegisterController);

    RegisterController.$inject = ['UserService', '$location', '$rootScope', 'FlashService'];
    function RegisterController(UserService, $location, $rootScope, FlashService) {
        var vm = this;

        vm.register = register;

        function register() {
            vm.dataLoading = true;
            UserService.Create(vm.user)
                .then(function (response) {
                    if (response.success) {
                        alertify
                            .alert("Registered successfully", function () {
                                alertify.success('Registered');
                            });
                        $location.path('/login');
                    } else {
                        alertify
                            .alert("Error", function () {
                                alertify.error('Error');
                            });
                        vm.dataLoading = false;
                    }
                });
        }
    }

})();
