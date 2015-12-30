(function () {
    'use strict';

    angular
        .module('app')
        .factory('WebApi', WebApi);

    WebApi.$inject = ['$http'];

    function WebApi($http) {
        var service = {
            GetChatRooms: GetChatRooms,
            GetChatRoomChatMessages: GetChatRoomChatMessages,
            GetChatRoom: GetChatRoom,
            GetChatMessages: GetChatMessages,
            GetChatMessage: GetChatMessage,
            CreateNewChatMessage: CreateNewChatMessage
        };

        function CreateNewChatMessage(chatMessage) {
            return $http.post("/api/ChatMessages/",
                chatMessage
            );
        }

        function GetChatRooms() {
            return $http.get("/api/ChatRooms/", {
            });
        }
        function GetChatRoomChatMessages(chatRoomId) {
            return $http.get("/api/ChatRooms/" + chatRoomId + '/ChatMessages', {
            });
        }
        function GetChatRoom(chatRoomId) {
            return $http.get("/api/ChatRooms/" + chatRoomId, {
            });
        }
        function GetChatMessages() {
            return $http.get("/api/ChatMessages/", {
            });
        }
        function GetChatMessage(chatMessageId) {
            return $http.get("/api/ChatMessages/" + chatMessageId, {
            });
        }
        return service;
    }
})();