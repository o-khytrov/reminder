(function () {
    'use strict';

    angular
        .module('app')
        .factory('AuthenticationService', AuthenticationService);

    AuthenticationService.$inject = ['$http', '$rootScope', '$timeout', '$location'];
    function AuthenticationService($http, $rootScope, $timeout, $location) {
        var service = {};

        service.Login = Login;
        service.SetCredentials = SetCredentials;
        service.ClearCredentials = ClearCredentials;
        service.isAuthenticated = isAuthenticated;
        service.logout = logout;
     
        return service;

        function Login(username, password, callback) {


            return $http.post('/api/auth/login', { userName: username, password: password }).then(handleSuccess, handleError('Error creating user'));


        }
        function handleSuccess(res) {
            setSession(res.data);
            // set default auth header for http requests
            $http.defaults.headers.common['Authorization'] = 'Basic ' + res.data.auth_token;
            $rootScope.loggedIn = true;

            $location.path('/dashboard');
            console.log($rootScope.loggedIn);
        }
        function handleError(res) {
            console.log(res);
        }
        function SetCredentials(username, password) {



        }

        function setSession(authResult) {
            // Set the time that the access token will expire at
            var t = new Date();
            t.setSeconds(t.getSeconds() + authResult.expires_in);
            const expiresAt = JSON.stringify(t);
            localStorage.setItem('auth_token', authResult.auth_token);
            localStorage.setItem('id', authResult.id);
            localStorage.setItem('expires_in', expiresAt);
            localStorage.setItem('user_photo', authResult.photo);
            localStorage.setItem('user_firtsName', authResult.firstname);
            localStorage.setItem('user_lastName', authResult.lastName);

        }

        function logout() {
            // Remove tokens and expiry time from localStorage
            localStorage.removeItem('auth_token');
            localStorage.removeItem('id');
            localStorage.removeItem('expires_in');
            localStorage.removeItem('user_photo');
            localStorage.removeItem('user_firtsName');
            localStorage.removeItem('user_lastName');
             $location.path('/login');
          
        }
    
        function isAuthenticated() {
            // Check whether the current time is past the
            // access token's expiry time
            const expiresAt = JSON.parse(localStorage.getItem('expires_in') || '{}');

            return (new Date() < new Date(expiresAt));

        }

        function ClearCredentials() {
            $rootScope.globals = {};

            $http.defaults.headers.common.Authorization = 'Basic';
        }
    }



})();