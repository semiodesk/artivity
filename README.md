# Artivity - Readme #

Understanding the techniques of artists is an essential part of studying art and art history. The process of creating an artwork is often more valueable than the artwork itself.

In traditional art historical discourses, art forms such as painting, sculpture and printmaking, can be studied by technically examining the artwork for evidence of technique. In digital art, this evidence are often lost as soon as the editing session on a piece of software ends.

Artivity can document the creation process of your digital artwork. This is critical for attributing art which is increasingly shared online, but also for interpreting individual artworks and their context within a given social and technical environment. It is a long term self archiving tool which does not intefere with your practice.

[Artivity](http://artivity.io) is a project which aims to produce a toolkit for capturing contextual data produced during the creative process of artists and designers while working on a computer. 
The Artivity open source software is developed by [Semiodesk GmbH](http://www.semiodesk.com) in partnership with the [University of the Arts, London](http://www.arts.ac.uk) . The project was initiated by [Dr. Athanasios Velios](http://www.arts.ac.uk/research/ual-staff-researchers/a-z/dr-athanasios-velios/) at the [Ligatus Research Centre](http://www.ligatus.org.uk/). It is funded by [JISC](http://www.jisc.ac.uk) since March 2015.

## Supported Platforms ##
The Artivity core software is built to be platform independent and runs on any major desktop operating system. There are extensions for the most popular professional and open source creative tools and web browsers.

### Operating Systems ###
* [Mac OS X](https://www.apple.com/osx/)
* [Windows](https://www.microsoft.com/en-us/windows) 
* [Ubuntu](http://www.ubuntu.com/)

### Creative Tools ###
* [Adobe Photoshop CC](http://www.adobe.com/products/photoshop.html)
* [Adobe Illustrator CC](http://www.adobe.com/products/illustrator.html)
* [Inkscape](https://inkscape.org/en/) 
* [Krita](http://www.krita.org/)

### Web Browsers ###
* [Mozilla Firefox](https://www.mozilla.org/en-GB/firefox)
* [Google Chrome](https://www.google.com/chrome/)
* [Opera](http://www.opera.com/)

## Installing ##
As the development is currently ongoing, we only have an installer for Mac OS X available.
You can download the latest version from here: [DOWNLOAD](https://static.semiodesk.com/artivity/osx/).

## Architecture ##
The Artivity software consits of multiple parts that each serve a specific purpose.

* **Server** 
  This core part of the software is written in C# and compatible with .NET and Mono. It collects the data and provides means to query and visualize it.
  When started the server instantiates a OpenLink Virutoso database. This database is configured to have a small footprint.
  On Mac OS X this is started as a user agent on login. On Windows this runs a service.

* **Journal**
  This piece is responsible for displaying the files, data and settings to the user. It is basically a webbrowser with specific changes to communicate with the platform.
  It is reimplemented for every platform, because the window frame as well as the browser needs to be fitted specifically for the operating system of the host.
  
* **Plugins**
  There are a number of plugins, at least one for each supported software.
  

## Compiling ##
### On Windows ###
**Needed Tools**

* [Visual Studio](https://www.visualstudio.com/downloads/download-visual-studio-vs.aspx) - The community edition should be fine.
* Boost - use the provided script to install it
### On Mac OS X ###
**Needed Tools**

* [XCode](https://itunes.apple.com/de/app/xcode/id497799835)
* [Xamarin Studio](https://www.xamarin.com/download-it) - The community edition should be fine.
* [Homebrew](http://brew.sh/)
* Boost - use homebrew to install it