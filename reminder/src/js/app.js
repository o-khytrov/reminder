// Default colors
var brandPrimary = '#20a8d8';
var brandSuccess = '#4dbd74';
var brandInfo = '#63c2de';
var brandWarning = '#f8cb00';
var brandDanger = '#f86c6b';

var grayDark = '#2a2c36';
var gray = '#55595c';
var grayLight = '#818a91';
var grayLighter = '#d1d4d7';
var grayLightest = '#f8f9fa';

angular
    .module('app', [
        'ui.router',
        'oc.lazyLoad',
        'ncy-angular-breadcrumb',
        'angular-loading-bar'
    ])
    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
        cfpLoadingBarProvider.latencyThreshold = 1;
    }])
    .run(['$rootScope', '$state', '$stateParams', '$location', '$http', 'AuthenticationService', function ($rootScope, $state, $stateParams, $location, $http, AuthenticationService) {
        $rootScope.globals = {};
        $rootScope.$on('$locationChangeStart', function (event, next, current) {
            // redirect to login page if not logged in and trying to access a restricted page
            var restrictedPage = $.inArray($location.path(), ['/login', '/register']) === -1;
            var loggedIn = AuthenticationService.isAuthenticated();
            console.log(loggedIn);
            if (restrictedPage && !loggedIn) {
                $location.path('/login');
            }
           
        });
        $rootScope.$on('$stateChangeSuccess', function () {
            document.body.scrollTop = document.documentElement.scrollTop = 0;
        });
        $rootScope.$state = $state;
        return $rootScope.$stateParams = $stateParams;
    }]);
