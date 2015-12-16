(function () {
    'use strict';

    var myApp = angular.module('app', [
        // Angular modules 
        'ngRoute',
        'ngAnimate',
        'SignalR',
        'angularUtils.directives.dirPagination'


        // Custom modules 

        // 3rd Party Modules
    ]);


    myApp.directive("ngInject", function ($parse, $interpolate, $controller, $compile) {
        return {
            terminal: true,
            transclude: true,
            priority: 510,
            link: function (scope, element, attrs, ctrls, transclude) {

                if (!attrs.ngController) {
                    element.removeAttr("ng-inject");
                    $compile(element)(scope);
                    return;
                }

                var controllerName = attrs.ngController;

                var newScope = scope.$new(false);

                var locals = $parse(attrs.ngInject)(scope);
                locals.$scope = newScope;

                var controller = $controller(controllerName, locals);

                element.data("ngControllerController", controller);

                element.removeAttr("ng-inject").removeAttr("ng-controller");
                $compile(element)(newScope);
                transclude(newScope, function (clone) {
                    element.append(clone);
                });
                // restore to hide tracks
                element.attr("ng-controller", controllerName);
            }
        };
    });

    myApp.config(['$routeProvider', function ($routeProvider) {
        $routeProvider.
            when('/', {
                title: 'Home',
                templateUrl: 'Views/home.html'
            }).
            when('/chatrooms/', {
                title: 'Blas',
                templateUrl: 'Views/chatroom-overview.html',
                controller: 'ChatRoomController',
                resolve: {

                }
            }).
            when('/chatrooms/:chatRoomId', {
                title: 'Blas',
                templateUrl: 'Views/chatroom-overview.html',
                controller: 'ChatRoomController',
                resolve: {
                    params: [
                        '$route', function($route) {
                            var id = $route.current.params.chatRoomId; 
                            return { id: id };
                        }
                    ]
                }
            });
    }]);

    myApp.run(['$rootScope', 'EntityFeedService', function ($rootScope, EntityFeedService) {
        $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
            EntityFeedService.resetSubscribtions();
        });
    }]);

})();

