/**
 * Caches a set of entities. The class provides methods for 
 * accessing a subset of the stored entities depending on
 * the time they were created.
 */
function EntityCache() {
    var t = this;

    // A lookup table for all entities.
    t.entities = {};

    // The list of entities in the order they were modified, from newest to oldest.
    t.entityHistory = [];
};

EntityCache.prototype.length = function () {
    var t = this;

    return t.entities.length;
};

EntityCache.prototype.load = function (data, complete) {
    var t = this;

    each(data, function (i, influence) {
        if (influence.time !== undefined && influence.uri !== undefined) {
            var time = new Date(influence.time);
            var entity = t.entities[influence.uri];

            if (entity === undefined) {
                entity = new Entity(influence.uri);

                t.entities[influence.uri] = entity;

                t.entityHistory.push(entity);
            };

            if (influence.type === 'http://www.w3.org/ns/prov#Generation') {
                entity.creationTime = time;
            } else if (influence.type === 'http://www.w3.org/ns/prov#Invalidation') {
                entity.deletionTime = time;
            }

            if (influence.property !== undefined) {
                entity.pushValue(time, influence.property, influence.value);
            }
        }
    });

    if (complete !== undefined) {
        complete(t.entities);
    }
};

EntityCache.prototype.get = function (uri) {
    return this.entities[uri];
};

EntityCache.prototype.getAll = function (time, fn) {
    var t = this;

    values(t.entities, function (key, entity) {
        if (time >= entity.creationTime) {
            fn(entity);
        }
    });
};