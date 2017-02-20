(function () {
    angular.module('app').factory('formattingService', formattingService);

    function formattingService() {
        return {
            getFormattedTime: getFormattedTime,
            getFormattedDate: getFormattedDate,
            getFormattedTimeFromNow: getFormattedTimeFromNow
        }

        function getFormattedTime(time, format) {
            if(format)
            {
                return moment(time).format(format);
            }
            else
            {
                return moment(time).format('hh:mm:ss');
            }
        };

        function getFormattedDate(time) {
            return moment(time).format('dddd, Do MMMM YYYY');
        };

        function getFormattedTimeFromNow(time) {
            return moment(time).fromNow();
        };
    }
})();