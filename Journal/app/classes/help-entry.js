function HelpEntry(title, path) {
	var t = this;

	t.title = title;
	t.path = path;

}


HelpEntry.prototype.getMarkdown = function (language) {
    var t = this;

    return t.path + "/"+language+".md";
};