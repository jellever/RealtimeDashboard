(function () {
    'use strict';

    angular
        .module('app')
        .factory('WebApi', WebApi);

    WebApi.$inject = ['$http'];

    function WebApi($http) {
        var service = {
        };

        return service;
    }
})();