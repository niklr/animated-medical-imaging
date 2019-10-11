function AMI(baseUrl) {
    this.baseUrl = baseUrl;
}

angular.module('ami-app', [
    'ami-main'
]).config(['$logProvider', '$httpProvider', function ($logProvider, $httpProvider) {

    $logProvider.debugEnabled(false);

    // initialize httpProvider defaults if undefined
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }

    // disable IE ajax request caching
    $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
    // disable general caching
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';

}]);

angular.module('ami-main', [
]).controller('MainController', ['$scope', '$http', function ($scope, $http) {

    $scope.user = {};

    $scope.error = undefined;

    $scope.login = function () {
        $scope.error = undefined;
        $http.post(ami.baseUrl + 'account/login', $scope.user).then(
            function (response) {
                console.log(response);
            },
            function (error) {
                console.log(error);
                $scope.error = error.data;
            });
    };

    $scope.test = function () {
        console.log(ami.baseUrl);
    };

}]);