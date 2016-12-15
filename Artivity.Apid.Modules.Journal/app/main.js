const electron = require('electron');

const {app, BrowserWindow} = electron;

app.on('ready', () => {
    // NOTE: Setting the background color prevents flickering on resize:
    // http://www.christianengvall.se/electron-white-screen-app-startup/
    var options = {
        frame: false,
        show: false,
        width: 992,
        minWidth: 800, 
        height: 800,
        minHeight: 600,
        icon: __dirname + '/img/icon.ico',
        backgroundColor: '#1D1D1D'
    };

    // http://stackoverflow.com/questions/35876939/frameless-window-with-controls-in-electron-windows
    var win = new BrowserWindow(options);
    win.setMenuBarVisibility(false);
    win.setMaximizable(false);
    win.setMinimizable(false);
    win.loadURL(`file://${__dirname}/index.html`);
    win.webContents.on('did-finish-load', function() {
        win.show();
    });
})

app.on('window-all-closed', function() {
  app.quit();
});
