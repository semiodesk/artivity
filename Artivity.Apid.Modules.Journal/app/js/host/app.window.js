(function () {
  const remote = require('electron').remote;

  function init() {
    document.getElementById("btn-min").addEventListener("click", function (e) { 
      const window = remote.getCurrentWindow();

      window.minimize();
    });

    document.getElementById("btn-max").addEventListener("click", function (e) {
      const window = remote.getCurrentWindow();

      // TODO: Change the button state via CSS class to OS independent.
      // Requires refactoring the window controls in index.html.
      if (!window.isMaximized()) {
        window.maximize();

        $('#btn-max img').attr('src', 'img/windows/btn-restore.png');
      } else {
        window.unmaximize();

        $('#btn-max img').attr('src', 'img/windows/btn-maximize.png');
      }
    });

    document.getElementById("btn-close").addEventListener("click", function (e) {
      remote.app.exit();
    });
  };

  document.onreadystatechange = function () {
    if (document.readyState === "complete") {
      init();
    }
  };
})();