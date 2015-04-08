# artivity project #

Artivity is a project funded by JISC which aims to produce a toolkit for capturing contextual data produced during the creative process of artists and designers while working on a computer. This data may include browsing history, email exchange and file editing statistics. Such information is already provided by GNOME's Activity Journal (zeitgeist framework). This project will extend Activity Journal to capture data about the way designers and artists use popular software applications such as Inkscape and GIMP.

### summary ###

The repository currently contains the following modules:

* Modified version of Inkscape which pushes undo/redo events to Zeitgeist (inkscape)
* Simple GUI for viewing the undo/redo events for Inkscape drawings (artivity-explorer)
* Browser extension for Google Chrome (artivity-extension-chrome)
* OS service which provides a simple REST API for pushing events from a browser into Zeitgeist (artivity-deamon)

### installing ###

Currently there are no prebuild binaries or setup tools, sorry. Anyone interested in trying the modified Inkscape version needs to do the following:

1. Check out the source code in the repository
2. Compile and run Inkscape
3. Start artivity-explorer.py

For testing the browser extension, proceed as follows:

1. Check out the source doe in the repository
2. Add the custom chrome extension from the folder 'artivitiy-extension-chrome'
3. Start the REST deamon 'artivity-deamon.py'