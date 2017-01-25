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

// Note: fs.existsSync is deprecated and throws IO exceptions if the file is not accessible.
// See: https://github.com/nodejs/node/issues/1592
function existsSync(filename) {
	try {
		var fs = require('fs');
		fs.accessSync(filename);
		return true;
	} catch(err) {
		console.error(err);
		return false;
	}
}