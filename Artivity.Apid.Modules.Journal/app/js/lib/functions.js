function each(array, fn) {
    if (array !== undefined) {
        for (var i = 0; i < array.length; i++) {
            fn(i, array[i]);
        }
    }
};

function values(array, fn) {
    if (array !== undefined) {
        for (var k in array) {
            if (array.hasOwnProperty(k)) {
                fn(k, array[k]);
            }
        }
    }
};

function serialize(obj, prefix) {
	var str = [];

	for (var p in obj) {
		if (obj.hasOwnProperty(p)) {
			var k = prefix ? prefix + "[" + p + "]" : p,
				v = obj[p];
			str.push(typeof v == "object" ?
				serialize(v, k) :
				encodeURIComponent(k) + "=" + encodeURIComponent(v));
		}
	}

	return str.join("&");
}

function getValue(obj, path) {
	for (var i = 0, path = path.split('.'), len = path.length; i < len; i++) {
		if (obj === undefined) {
			break;
		}

		obj = obj[path[i]];
	}
	return obj;
}

function setValue(obj, path, value) {
	var p;

	for (var i = 0, path = path.split('.'), len = path.length - 1; i < len; i++) {
		obj = obj[path[i]];

		p = path[i + 1];
	}

	if (p) {
		obj[p] = value;
	}
}

function loadItems(items, action, done) {
	if (!items) {
		return;
	}

	// convert single item to array.
	if ("undefined" === items.length) {
		items = [items];
	}

	var count = items.length;

	// this callback counts down the things to do.
	var completed = function (items, i) {
		count--;

		if (0 == count) {
			done(items);
		}
	};

	// invoke each action, and await callback.
	for (var i = 0; i < items.length; i++) {
		action(items, i, completed);
	}
}