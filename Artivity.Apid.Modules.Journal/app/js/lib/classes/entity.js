/**
 * A generic entity which is identified by a URI. The class
 * provides methods for accessing the values of properties
 * at a given point in time.
 */
function Entity(uri) {
    var t = this;

    t.uri = uri;
    t.properties = {};
    t.creationTime = undefined;
    t.deletionTime = new Date();
};

Entity.prototype.pushValue = function (time, property, value) {
    var t = this;
    var P = t.properties[property];

    if (P === undefined) {
        P = [];

        t.properties[property] = P;
    }

    P.push({
        time: time,
        value: value
    });
};

Entity.prototype.getValue = function (time, property) {
    var t = this;
    var P = t.properties[property];

    if (P === undefined) {
        return undefined;
    }

    for (var i = 0; i < P.length; i++) {
        var p = P[i];

        if (p.time <= time) {
            return p.value;
        }
    }

    return undefined;
};

Entity.prototype.getLabel = function (time) {
    return this.getValue(time, 'http://www.w3.org/2000/01/rdf-schema#label');
};