(function () {
    'use strict';

    angular
        .module('app')
        .factory('Utils', Utils);


    function Utils() {
        var service = {
            findEntityIndexById: findEntityIndexById,
            replaceEntityById: replaceEntityById,
            arrayContains: arrayContains,
            removeEntityById: removeEntityById,
            isNotNull: isNotNull
        };

        return service;

        function findEntityIndexById(arr, id) {
            var idx = -1;
            $.each(arr, function (i) {
                if (this.Id === id) {
                    idx = i;
                    return false;
                }
            });
            return idx;
        }

        function replaceEntityById(arr, entity) {
            var idx = findEntityIndexById(arr, entity.Id);
            if (idx > -1) {
                arr[idx] = entity;
                return true;
            }
            return false;
        }

        function removeEntityById(arr, entity) {
            var idx = findEntityIndexById(arr, entity.Id);
            if (idx > -1) {
                arr.splice(idx, 1);
                return true;
            }
            return false;
        }

        function arrayContains(arr, obj) {
            return arr.indexOf(obj) > -1;
        }

        function isNotNull(obj) {
            return (obj !== undefined) && (obj !== null);
        }
    }
})();