const electron = require('electron');

const {app, BrowserWindow} = electron;

app.on('ready', () => {
    // http://stackoverflow.com/questions/35876939/frameless-window-with-controls-in-electron-windows
    // NOTE: Setting the background color prevents flickering on resize:
    // http://www.christianengvall.se/electron-white-screen-app-startup/
    var win = new BrowserWindow({width:1100, height: 600,  frame: false, icon: __dirname + '/img/icon.ico', show: false, backgroundColor: '#1D1D1D'});
    win.setMenuBarVisibility(false);
    win.loadURL(`file://${__dirname}/index.html`);
    win.webContents.on('did-finish-load', function() {
        win.show();
    });
})

app.on('window-all-closed', function() {
  app.quit();
});
