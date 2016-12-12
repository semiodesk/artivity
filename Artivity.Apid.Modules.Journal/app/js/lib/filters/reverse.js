angular.module('explorerApp').filter('reverse', ReverseFilter);

function ReverseFilter() {
    return function (items) {
        if (items !== undefined && items.length > 1) {
            return items.slice().reverse();
        } else {
            return items;
        }
    }
}