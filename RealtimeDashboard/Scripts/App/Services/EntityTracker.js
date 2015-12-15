(function () {
    'use strict';

    angular
        .module('app')
        .factory('EntityTracker', function (EntityFeedService, Utils) {
            function EntityTracker() {
                var groups = [];

                this.subscribe = function (typeName, entityId, relationName) {
                    EntityFeedService.subscribe(typeName, entityId, relationName).then(function (groupName) {
                        groups.push(groupName);
                    });
                }

                this.onNewEntity = function (func) {
                    EntityFeedService.onNewEntity(function (data) {
                        if (Utils.arrayContains(groups, data.Group)) {
                            func(data);
                        }
                    });
                }

                this.onUpdatedEntity = function (func) {
                    EntityFeedService.onUpdatedEntity(function (data) {
                        if (Utils.arrayContains(groups, data.Group)) {
                            func(data);
                        }
                    });
                }

                this.onDeletedEntity = function (func) {
                    EntityFeedService.onDeletedEntity(function (data) {
                        if (Utils.arrayContains(groups, data.Group)) {
                            func(data);
                        }
                    });
                }
            }

            return (EntityTracker);
        });



})();