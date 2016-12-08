const electron = require('electron')

const {app, BrowserWindow} = electron

app.on('ready', () => {
    // http://stackoverflow.com/questions/35876939/frameless-window-with-controls-in-electron-windows
    var win = new BrowserWindow({width:1100, height: 600,  frame: true, icon: __dirname + '/img/icon.ico', show: false})
    win.setMenuBarVisibility(false);
    win.loadURL(`file://${__dirname}/index.html`)
    win.webContents.on('did-finish-load', function() {
        win.show();
    });
})
