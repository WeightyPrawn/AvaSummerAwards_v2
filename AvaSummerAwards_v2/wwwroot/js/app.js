$(document).on('click', function (e) {
    if ($(e.target).closest('nav').length < 1) {
        $('#main-menu').collapse('hide');
    }
});

angular.module('AvaSummerAwards', ['ngRoute'])
    .config(['$routeProvider', '$httpProvider',
        function ($routeProvider, $httpProvider) {
            $routeProvider
                .when('/',
                {
                    controller: 'AppController',
                    templateUrl: '/App'
                })
                .when('/admin',
                {
                    controller: 'AdminController',
                    templateUrl: '/App/Admin',
                })
                .when('/nominate',
                {
                    controller: 'NominationController',
                    templateUrl: '/App/Nomination',
                })
                .otherwise({ redirectTo: '/' });
            var endpoints = {
                // Map the location of a request to an API to a the identifier of the associated resource
                //url:apiResourceId
            };
        }])
    .controller('AppController', ['VoteService', '$interval', '$scope', '$rootScope', function (VoteService, $interval, $scope, $rootScope) {
        console.log($rootScope.userInfo)
        $scope.data = VoteService.data;

        $scope.vote = VoteService.vote;
        $scope.removeVote = VoteService.removeVote;

    }]).controller('NominationController', ['NominationService', '$interval', '$scope', '$rootScope', function (NominationService, $interval, $scope, $rootScope) {
        console.log($rootScope.userInfo)
        $scope.data = NominationService.data;
        $scope.error = "";
        $scope.add = function (nomination) {
            $scope.error = "";
            NominationService.add(nomination).then(function successCallback(response) {
                // this callback will be called asynchronously
                // when the response is available
                NominationService.refreshData();
                $('#myModal').modal('hide');
            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                $scope.error = response.data;
            });
        }
        $scope.delete = NominationService.delete;

        $scope.editNomination = {};
        $scope.openModal = function (data) {
            if (data) {
                $scope.editNomination.categoryID = data.categoryID;
                $scope.editNomination.email = data.email;
                $scope.editNomination.reason = data.nominations[0].reason;
                $scope.editNomination.nominationId = data.nominations[0].id;
            }
            $('#myModal').modal('show')
        };
        $('#myModal').on('hide.bs.modal', function (e) {
            $scope.editNomination = {};
            $scope.error = "";
        })

    }]).controller('AdminController', ['VoteService', '$interval', '$scope', '$rootScope', function (VoteService, $interval, $scope, $rootScope) {
        console.log($rootScope.userInfo)
        $scope.data = VoteService.data;

    }]).controller('MainController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
        $scope.login = function () {
            $location.path('/');
            console.log($rootScope.userInfo)
        };
    }])
    .factory('VoteService', ['$http', function ($http) {
        var apiBaseUrl = "";
        var data = {};

        var URLS = {
            CATEGORIES: '/api/Categories',
            VOTE: '/api/Votes'
        };

        function init() {
            getData();
        };
        function getData() {
            $http.get(apiBaseUrl + URLS.CATEGORIES).success(function (resp) {
                data.categories = resp;
                console.log(data);
            }).error(function (error) {
                console.log(error);
                alert("Error: " + error);
            });
        };
        init();

        return {
            vote: function (nominee) {
                console.log(nominee);

                $http.post(apiBaseUrl + URLS.VOTE + "/" + nominee.id)
                    .then(function successCallback(response) {
                        // this callback will be called asynchronously
                        // when the response is available
                        getData();
                        return response;
                    }, function errorCallback(response) {
                        // called asynchronously if an error occurs
                        // or server returns response with an error status.
                    });
            },

            removeVote: function (nominee) {
                console.log(nominee);

                return $http.delete(apiBaseUrl + URLS.VOTE + "/" + nominee.vote.id)
                    .then(function successCallback(response) {
                        // this callback will be called asynchronously
                        // when the response is available
                        getData();
                        return response;
                    }, function errorCallback(response) {
                        // called asynchronously if an error occurs
                        // or server returns response with an error status.
                    });
            },

            data: data
        }
    }])
    .factory('NominationService', ['$http', function ($http) {
        var apiBaseUrl = "";
        var data = {};
        var error = "";

        var URLS = {
            NOMINATIONS: '/api/Nominations'
        };

        function init() {
            getData();
        };
        function getData() {
            $http.get(apiBaseUrl + URLS.NOMINATIONS).success(function (resp) {
                data.categories = resp;
                console.log(data);
            }).error(function (error) {
                console.log(error);
                alert("Error: " + error);
            });
        };
        init();

        return {
            add: function (nomination) {
                console.log(nomination);

                return $http.post(apiBaseUrl + URLS.NOMINATIONS, nomination);
                    
            },
            delete: function (nominationId) {
                console.log(nominationId);

                return $http.delete(apiBaseUrl + URLS.NOMINATIONS + "/" + nominationId)
                    .then(function successCallback(response) {
                        // this callback will be called asynchronously
                        // when the response is available
                        getData();
                        return response;
                    }, function errorCallback(response) {
                        // called asynchronously if an error occurs
                        // or server returns response with an error status.
                    });
            },
            refreshData: function () { getData() },
            data: data
        }
    }])