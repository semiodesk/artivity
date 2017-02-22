function Marker() {
    var t = this;

    t.isNew = true;
    t.p1 = {x: 0, y: 0};
    t.p2 = {x: 0, y: 0};
};

Marker.prototype.x = function() {
    var t = this;
    
    return t.p1.x;
}

Marker.prototype.y = function() {
    var t = this;
    
    return t.p1.y;
}

Marker.prototype.width = function() {
    var t = this;
    
    return t.p2.x - t.p1.x;
}

Marker.prototype.height = function() {
    var t = this;
    
    return t.p2.y - t.p1.y;
}