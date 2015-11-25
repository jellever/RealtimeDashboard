(function () {
    'use strict';

    angular
        .module('app')
        .factory('EntityFeedService', EntityFeedService);

    function EntityFeedService($http, Hub, LogService) {
        var service = {
            subscribe: subscribe,
            subscribeByGroupName: subscribeByGroupName,
            resetSubscribtions: resetSubscribtions,
            onNewEntity: onNewEntity,
            onUpdatedEntity: onUpdatedEntity,
            onDeletedEntity: onDeletedEntity
        };
        activate();
        return service;

        var hub;

        function activate() {
            hub = new Hub("liveHub", {
                methods: ['subscribe', 'resetSubscribtions'],
                listeners: {
                    'NewEntity': function (data) {
                        LogService.writeLine(LogService.DEBUG, 'EntityFeedService', 'New Entity event', data);
                    },
                    'UpdatedEntity': function (data) {
                        LogService.writeLine(LogService.DEBUG, 'EntityFeedService', 'Updated Entity event', data);
                    },
                    'DeletedEntity': function (data) {
                        LogService.writeLine(LogService.DEBUG, 'EntityFeedService', 'Deleted Entity event', data);
                    }
                },
                //logging: true
            });
        }

        function onNewEntity(func) {
            hub.on('NewEntity', func);
        }
        function onUpdatedEntity(func) {
            hub.on('UpdatedEntity', func);
        }
        function onDeletedEntity(func) {
            hub.on('DeletedEntity', func);
        }

        function subscribeByGroupName(groupName) {
            var logStr = "Subscribing to '" + groupName + "'";
            LogService.writeLine(LogService.INFO, 'EntityFeedService', logStr);
            hub.subscribe(groupName);
        }

        function subscribe(typeName, entityId, relationName) {
            var groupName = typeName;
            if (entityId !== null) {
                groupName += "." + entityId;
            }
            if (relationName !== null) {
                groupName += "." + relationName;
            }
            subscribeByGroupName(groupName);
        }

        function resetSubscribtions() {
            LogService.writeLine(LogService.INFO, 'EntityFeedService', "Resetting subscriptions");
            hub.resetSubscribtions();
        }
    }
})();