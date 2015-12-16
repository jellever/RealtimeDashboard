(function () {
    'use strict';

    angular
        .module('app')
        .controller('ChatMessageController', ChatMessageController);


    function ChatMessageController($scope, WebApi, EntityTracker, Utils, params) {
        $scope.title = 'Home';
        $scope.chatMessages = [];
        var entityTracker = new EntityTracker();

        activate();

        function getChatMessages() {
            if (Utils.isNotNull(params.chatRoomId)) {
                return WebApi.GetChatRoomChatMessages(params.chatRoomId);
            } else {
                return WebApi.GetChatMessages();
            }
        }

        function subscribe() {
            if (Utils.isNotNull(params.chatRoomId)) {
                return entityTracker.subscribe("ChatRoom", params.chatRoomId, "ChatRoom_ChatMessages");
            } else {
                return entityTracker.subscribe("ChatMessage");
            }
        }

        function activate() {
            getChatMessages.success(function (data) {
                $.each(data, function () {
                    $scope.chatMessages.push(this);
                });
            });

            entityTracker.onNewEntity(function (data) {
                $scope.$apply(function () {
                    $scope.chatMessages.push(data.Entity);
                });
            });

            entityTracker.onUpdatedEntity(function (data) {
                $scope.$apply(function () {
                    Utils.replaceEntityById($scope.chatMessages, data.Entity);
                });
            });

            entityTracker.onDeletedEntity(function (data) {
                $scope.$apply(function () {
                    Utils.removeEntityById($scope.chatMessages, data);
                });
            });

            subscribe();
        }
    }
})();
