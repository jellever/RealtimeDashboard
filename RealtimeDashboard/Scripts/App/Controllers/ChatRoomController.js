(function () {
    'use strict';

    angular
        .module('app')
        .controller('ChatRoomController', ChatRoomController);


    function ChatRoomController($scope, WebApi, EntityTracker, Utils) {
        $scope.chatRooms = [];
        var entityTracker = new EntityTracker();

        activate();

        function activate() {
            WebApi.GetChatRooms().success(function (data) {
                $.each(data, function () {
                    $scope.chatRooms.push(this);
                });
            });

            entityTracker.onNewEntity(function (data) {
                $scope.$apply(function () {
                    $scope.chatRooms.push(data.Entity);
                });
            });

            entityTracker.onUpdatedEntity(function (data) {
                $scope.$apply(function () {
                    Utils.replaceEntityById($scope.chatRooms, data.Entity);
                });
            });

            entityTracker.onDeletedEntity(function (data) {
                $scope.$apply(function () {
                    Utils.removeEntityById($scope.chatRooms, data);
                });
            });

            entityTracker.subscribe("ChatRoom");
        }
    }
})();
