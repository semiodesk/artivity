const electron = require('electron');
const path = require('path');

const {
    app,
    BrowserWindow
} = electron;

var userData = app.getPath('userData');
app.setPath('userData', path.join(userData, 'App'));

app.on('ready', () => {
    // NOTE: Setting the background color minimizes flickering on resize:
    // http://www.christianengvall.se/electron-white-screen-app-startup/
    var options = {
        frame: false,
        show: false,
        width: 992,
        minWidth: 800,
        height: 800,
        minHeight: 600,
        icon: __dirname + '/app/resources/images/icon.ico',
        backgroundColor: '#1D1D1D'
    };

    // http://stackoverflow.com/questions/35876939/frameless-window-with-controls-in-electron-windows
    var w = new BrowserWindow(options);
    w.setMenuBarVisibility(false);
    w.setMaximizable(false);
    w.setMinimizable(false);
    w.loadURL(`file://${__dirname}/index.html#/start`);
    w.webContents.on('did-finish-load', function () {
        w.show();
    });
})

app.on('window-all-closed', function () {
    app.quit();
});