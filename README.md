![Icon64.png](https://bitbucket.org/repo/nybqaa/images/4039620649-Icon64.png)

# Readme #

The Artivity Project aims to produce an open desktop logging framework for arts practice. It allows for capturing detailed data about the creation process of digital artwork and its context. This data can be used to study the techniques, workflow and sources of inspiration in the creation process of digital media. The project is executed as a cooperation with the [University of the Arts, London](http://www.arts.ac.uk) and [Semiodesk GmbH](http://www.semiodesk.com). It is currently funded by [JISC](http://www.jisc.ac.uk).

### Summary ###

This repository currently contains the following modules:

* **artivity-apid**: OS daemon which provides a REST API for logging events from clients into Artivity.
* **artivity-explorer**: Simple user interface for viewing the activity events for drawings.
* **artivity-extension-firefox**: Browser extension for web browsers which are based on Mozilla Firefox.
* **artivity-extension-chromium**: Browser extension for web browsers which are based on Google Chrome.
* **artivity-extension-krita**: An extension to Krita for logging undo/redo events in Artivity.
* **artivity-inkscape**: Modified version of Inkscape for logging undo/redo events in Artivity.
* **libartivity**: A C++ client library for communication with the Artivity database.

### Installing ###

There are prebuild binaries for [Elementary OS](http://elementary.io/), [Ubuntu 14.04 LTS](http://www.ubuntu.com/) or [Ubuntu 14.04 LTS GNOME](http://cdimage.ubuntu.com/ubuntu-gnome/releases/14.04/release/). They are provided via a so called [Personal Package Archive](http://wiki.ubuntuusers.de/Launchpad/PPA). With this repository you will receive updates and new features via Ubuntu's updating mechanism as soon as they're published. If you want to try the current release, please follow these steps:

1) Download [Ubuntu 14.04 LTS](http://releases.ubuntu.com/14.04/) and install it on your computer or in a virtual machine such as [Virtualbox](https://www.virtualbox.org).

2) Enable the [Artivity PPA](https://launchpad.net/~faubulous/+archive/ubuntu/artivity) by pasting the following commands in a terminal:
```
#!bash

sudo add-apt-repository ppa:faubulous/artivity
sudo apt-get update
```

3) Install the Artivity desktop packages with this command:
```
#!bash

sudo apt-get install artivity-desktop
```

4) During the setup you will be asked to set a password for the OpenLink Virtuoso administrator. Please fill in the following values:
```
#!bash

User: dba
Password: dba
```
5) Start the Artivity Explorer application.

6) If you want to activate the browser logging feature, click on the Artivity icon in Mozilla Firefox or Goolge Chrome and check the 'Capture browsing history' box.

### Compiling ###
The core Artivity platform is implemented using .NET/Mono. We're using the [Semiodesk Trinity](http://www.semiodesk.com/products/trinity) Linked Data API for C# on top of the [OpenLink Virtuoso](https://github.com/openlink/virtuoso-opensource) graph database. If you want to use [MonoDevelop](http://www.monodevelop.com) or [Xamarin Studio](https://xamarin.com/studio) for development, please make sure to install the latest [Mono platform from Xamarin](http://www.mono-project.com/download/).

The customized build of Inkscape requires compilation. Please refer to the [Inkscape Wiki](http://wiki.inkscape.org/wiki/index.php/Compiling_Inkscape) 
for detailed instructions on how to compile it. Since Inkscape has a quite large code base it is 
highly recommended to use a byte code caching solution such as [ccache](https://ccache.samba.org).

If you want to build the Krtia plugin from source, please refer to [Building Krita on Linux for cats](http://www.davidrevoy.com/article193/guide-building-krita-on-linux-for-cats) 
for detailed instructions on how to setup your build environment.

### License ###
Artivity consists of multiple software components which may have individual licenses associated with them. For source code, licensing information may be contained in the headers of each file. You should find a license file named 'COPYING' in every sub-folder of the repository, which contains the license which is applied to its contents if not stated otherwise. If there is no such file, source code and other files contained in a folder in this directory are licensed under the terms of the GNU General Public License version 3. You can find a copy of the license in the file named 'COPYING' which is contained in the directory root of this repository.