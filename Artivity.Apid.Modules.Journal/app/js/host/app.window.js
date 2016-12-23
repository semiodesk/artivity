(function () {
  const remote = require('electron').remote;

  function init() {
    var title = document.getElementById('window-title');

    var btns = title.getElementsByClassName("btn-min");

    for (var i = 0; i < btns.length; i++) {
      btns[i].addEventListener("click", minimizeWindow);
    }

    btns = title.getElementsByClassName("btn-max");

    for (var i = 0; i < btns.length; i++) {
      btns[i].addEventListener("click", maximizeWindow);
    }

    btns = title.getElementsByClassName("btn-close");

    for (var i = 0; i < btns.length; i++) {
      btns[i].addEventListener("click", closeWindow);
    }
  }

  function minimizeWindow(e) {
    const window = remote.getCurrentWindow();

    window.minimize();
  }

  function maximizeWindow(e) {
    const window = remote.getCurrentWindow();

    // TODO: Change the button state via CSS class to OS independent.
    // Requires refactoring the window controls in index.html.
    if (!window.isMaximized()) {
      window.maximize();

      $('.window-title-btns .btn-max').addClass("maximized");
    } else {
      window.unmaximize();

      $('.window-title-btns .btn-max').removeClass("maximized");
    }
  }

  function closeWindow(e) {
    remote.app.exit();
  }

  document.onreadystatechange = function () {
    if (document.readyState === "complete") {
      init();
    }
  };
})();