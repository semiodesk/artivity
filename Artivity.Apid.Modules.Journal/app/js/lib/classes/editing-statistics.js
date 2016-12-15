// STATS
function EditingStatistics() {
    var t = this;

    t.stepCount = 0;
    t.undoCount = 0;
    t.redoCount = 0;
};

EditingStatistics.prototype.confidence = function () {
    var t = this;

    if (t.stepCount > 0) {
        return (100 * (t.stepCount - t.undoCount - t.redoCount) / t.stepCount).toFixed(0);
    } else {
        return 100;
    }
}