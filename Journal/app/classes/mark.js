function Mark() {
    var t = this;

    t.startTime = new Date();
    t.p1 = {
        x: 0,
        y: 0
    };
    t.p2 = {
        x: 0,
        y: 0
    };
}

Mark.prototype.x = function () {
    var t = this;

    return t.p1.x;
};

Mark.prototype.y = function () {
    var t = this;

    return t.p1.y;
};

Mark.prototype.width = function () {
    var t = this;

    return t.p2.x - t.p1.x;
};

Mark.prototype.height = function () {
    var t = this;

    return t.p2.y - t.p1.y;
};