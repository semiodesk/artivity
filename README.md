# Readme #

Artivity is a project which aims to produce a toolkit for capturing contextual data produced during the creative process of artists and designers while working on a computer. The project is funded by [JISC](http://www.jisc.ac.uk) and based on a cooparation of the [University of the Arts London](http://www.arts.ac.uk) and [Semiodesk GmbH, Germany](http://www.semiodesk.com). The captured data may include browsing history, email exchange and file editing statistics. Such information is already provided by [GNOME's Activity Journal](http://en.wikipedia.org/wiki/GNOME_Activity_Journal) which is build using the [Zeitgeist Framework](http://zeitgeist-project.com). This project will extend Activity Journal to capture data about the way designers and artists use popular software applications such as [Inkscape](https://inkscape.org/en/) and [GIMP](http://www.gimp.org).

### Summary ###

This repository currently contains the following modules:

* **inkscape**: Modified version of Inkscape which pushes undo/redo events to Zeitgeist
* **artivity-explorer**: Simple GUI for viewing the undo/redo events for Inkscape drawings
* **artivity-extenion-firefox**: Browser extension for web browsers which are based on Mozilla Firefox
* **artivity-extenion-chromium**: Browser extension for web browsers which are based on Google Chrome
* **artivity-apid**: OS daemon which provides a REST API for pushing events from a browser extension into Zeitgeist

### Installing ###

There are prebuild binaries for [Ubuntu 14.04 LTS](http://www.ubuntu.com/), which are provided via a so called ['Personal Package Archive'](http://wiki.ubuntuusers.de/Launchpad/PPA). If you want to try the current release, please follow these steps:

1. Download [Ubuntu 14.04 LTS](http://releases.ubuntu.com/14.04/) and install it on your computer or in a virtual machine, such as [Virtualbox](https://www.virtualbox.org).
2. Enable the Artivity PPA by pasting the following commands in a terminal:
	sudo add-apt-repository ppa:faubulous/Artivity
   and
	sudo apt-get update
3. Install the Artivity desktop packages with this command:
	sudo apt-get install artivity-desktop
4. Enable the Zeitgeist browser event logging feature with the following command:
	artivity-setup
   A dialog box will appear. Simply check the 'Enable tacking your web browsing history in Activity Journal' to enable the browser support daemon for your user.
5. Click on the Artivity icon in Mozilla Firefox and check the 'Capture browsing history' box.

Now you're set up to use the Artivity event logging features. You will also receive updates and new features via the system's updating mechanism as soon as they're published. Enjoy!
