const electron = require('electron')

const {app, BrowserWindow} = electron

app.on('ready', () => {
    // http://stackoverflow.com/questions/35876939/frameless-window-with-controls-in-electron-windows
    var win = new BrowserWindow({width:800, height: 800})


    win.loadURL(`file://${__dirname}/index.html`)
})
