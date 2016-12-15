(function () {
  const remote = require('electron').remote;

  function init() {
    const window = remote.getCurrentWindow();

    $("btn-min").click(function (e) {      
      window.minimize();
    });

    $("btn-max").click(function (e) {
      if (!window.isMaximized()) {
        window.maximize();
      } else {
        window.unmaximize();
      }
    });

    $("btn-close").click(function (e) {
      remote.app.exit();
    });

    $("#window-title").dblclick(function(){
      alert("The paragraph was double-clicked");
    });
  };

  document.onreadystatechange = function () {
    if (document.readyState === "complete") {
      init();
    }
  };
})();