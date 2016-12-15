(function () {
  const remote = require('electron').remote;

  function init() {
    document.getElementById("btn-min").addEventListener("click", function (e) {
      const window = remote.getCurrentWindow();
      
      window.minimize();
    });

    document.getElementById("btn-max").addEventListener("click", function (e) {
      const window = remote.getCurrentWindow();

      if (!window.isMaximized()) {
        window.maximize();
      } else {
        window.unmaximize();
      }
    });

    document.getElementById("btn-close").addEventListener("click", function (e) {
      remote.app.exit();
    });
  };

  document.onreadystatechange = function () {
    if (document.readyState == "complete") {
      init();
    }
  };
})();