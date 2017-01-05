(function () {
    angular.module('explorerApp').factory('formattingService', formattingService);

    function formattingService() {
        return {
            getFormattedTime: getFormattedTime,
            getFormattedDate: getFormattedDate,
            getFormattedTimeFromNow: getFormattedTimeFromNow
        }

        function getFormattedTime(time) {
            return moment(time).format('hh:mm:ss');
        };

        function getFormattedDate(time) {
            return moment(time).format('dddd, Do MMMM YYYY');
        };

        function getFormattedTimeFromNow(time) {
            return moment(time).fromNow();
        };
    }
})();