(function () {
    'use strict';

    angular
        .module('app')
        .controller('ChatRoomDetailsController', ChatRoomDetailsController);


    function ChatRoomDetailsController($scope, WebApi, EntityTracker, Utils, params) {
        $scope.chatroom = null;
        $scope.id = params.id;
        $scope.submitChatRoomMessage = submitChatRoomMessage;
        var entityTracker = new EntityTracker();

        activate();

        function submitChatRoomMessage() {
            var msg = $scope.chatMsgForm;
            msg.ChatRoomId = $scope.id;
            WebApi.CreateNewChatMessage(msg);
        }

        function activate() {
            WebApi.GetChatRoom(params.id).success(function (data) {
                $scope.chatroom = data;
            });

            entityTracker.onUpdatedEntity(function (data) {
                $scope.$apply(function () {
                    $scope.chatroom = data.Entity;
                });
            });
            entityTracker.subscribe("ChatRoom", params.id);
        }
    }
})();
