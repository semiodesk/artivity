(function () {
    angular.module('app').factory('selectionService', selectionService);

    function selectionService() {
        var t = {
            // The data context: array of items in which the selected items are contained in.
            dataContext: [],
            // The selected item.
            selectedItem: null,
            // List of selected items.
            selectedItems: [],
            // Private event dispatcher instance.
            dispatcher: new EventDispatcher()
        };

        return {
            dataContext: dataContext,
            selectedIndex: selectedIndex,
            selectedItem: selectedItem,
            selectedItems: selectedItems,
            selectNext: selectNext,
            selectPrev: selectPrev,
            mute: mute,
            unmute: unmute,
            on: on,
            off: off
        };

        function dataContext(values) {
            if (values) {
                t.dataContext = values;
                t.dispatcher.raise('dataChanged', values);
            }

            return t.dataContext;
        }

        function selectedIndex(i) {
            if (i) {
                if (t.dataContext && -1 < i && i < t.dataContext.length) {
                    selectedItem(t.dataContext[i]);

                    return i;
                } else {
                    return -1;
                }
            } else if (t.dataContext && t.selectedItem) {
                return t.dataContext.indexOf(t.selectedItem);
            } else {
                return -1;
            }
        }

        function selectedItem(value) {
            if (value) {
                t.dispatcher.raise('selectedItemChanging', value);
                t.selectedItem = value;
                t.dispatcher.raise('selectedItemChanged', value);
            }

            return t.selectedItem;
        }

        function selectedItems(values) {
            if (values) {
                t.dispatcher.raise('selectedItemsChanging', values);
                t.selectedItems = values;
                t.dispatcher.raise('selectedItemsChanged', values);
            }

            return t.selectedItems;
        }

        function selectNext() {
            var i = t.dataContext.indexOf(t.selectedItem);

            if (-1 < i && i < t.dataContext.length - 1) {
                selectedItem(t.dataContext[i + 1]);
            }
        }

        function selectPrev() {
            var i = t.dataContext.indexOf(t.selectedItem);

            if (0 < i) {
                selectedItem(t.dataContext[i - 1]);
            }
        }

        function mute() {
            t.dispatcher.mute();
        }

        function unmute() {
            t.dispatcher.unmute();
        }

        function on(event, callback) {
            t.dispatcher.on(event, callback);
        }

        function off(event, callback) {
            t.dispatcher.off(event, callback);
        }
    }
})();