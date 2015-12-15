(function () {
    'use strict';

    angular
        .module('app')
        .factory('LogService', LogService);


    function LogService() {
        var service = {
            writeLine: writeLine,
            DEBUG: { str: 'DEBUG', style: 'color:blue;' },
            INFO: { str: 'INFO', style: 'color:darkblue;' },
            ERROR: { str: 'ERROR', style: 'color:red;' },
            WARNING: { str: 'WARNING', style: 'color:orange;' }
        };

        return service;

        function writeLine(severity, origin, evtData, objectsToLog) {
            var now = new Date();
            var str = "%c[" + formatDate(now) + "] ";
            str += severity.str + " - " + origin + " -> " + evtData;
            if (objectsToLog != null) {
                console.log(str, severity.style, objectsToLog);
            } else {
                console.log(str, severity.style);
            }
        }

        function pad(n) {
            return ("0" + n).slice(-2);
        }

        function formatDate(date) {
            var dateStr = pad(date.getHours()) + ":" + pad(date.getMinutes()) + ":" + pad(date.getMinutes());
            return dateStr;
        }
    }
})();