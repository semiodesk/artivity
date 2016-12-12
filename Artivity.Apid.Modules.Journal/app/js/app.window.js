(function () {
  const remote = require('electron').remote;

  function init() {
    document.getElementById("min-btn").addEventListener("click", function (e) {
      const window = remote.getCurrentWindow();
      
      window.minimize();
    });

    document.getElementById("max-btn").addEventListener("click", function (e) {
      const window = remote.getCurrentWindow();

      if (!window.isMaximized()) {
        window.maximize();
      } else {
        window.unmaximize();
      }
    });

    document.getElementById("close-btn").addEventListener("click", function (e) {
      remote.app.exit();
    });
  };

  document.onreadystatechange = function () {
    if (document.readyState == "complete") {
      init();
    }
  };
})();

var dragging = 0;

window.ondragenter = (ev) => {
  if (ev.dataTransfer.files != null && ev.dataTransfer.files.length > 0 && dragging === 0) {
    var filePath = ev.dataTransfer.files[0].path;

    if (filePath.endsWith(".arta") || filePath.endsWith(".arty"))
      showOverlay("#msg-import");
    else
      showOverlay("#msg-add-app");
  }

  dragging++;

  ev.stopPropagation();
  ev.preventDefault();
}

document.ondrop = (ev) => {
  if (ev.dataTransfer.files != null && ev.dataTransfer.files.length > 0) {
    var fileUrl = require('file-url');
    var filePath = ev.dataTransfer.files[0].path;

    if (filePath.endsWith(".arta") || filePath.endsWith(".arty")) {
      var endpoint = apid.endpointUrl + '/import?fileUrl=' + fileUrl(filePath);
    } else {
      var endpoint = apid.endpointUrl + '/agents/software/paths/add?url=' + fileUrl(filePath);
    }

    $.get(endpoint)
  }

  ev.preventDefault();
  ev.stopPropagation();

  hideOverlays();

  dragging = 0;

  return false;
}

document.ondragover = (ev) => {
  ev.preventDefault();
}

window.ondragend = (ev) => {
  ev.preventDefault();

  return false;
}

window.ondragleave = (ev) => {
  dragging--;

  ev.stopPropagation();
  ev.preventDefault();

  if (dragging === 0) {
    hideOverlays();
  }

  return false;
}