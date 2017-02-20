(function () {
    angular.module('app').directive('artFilePreview', artFilePreviewDirective);

    function artFilePreviewDirective() {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: 'app/directives/art-file-preview/art-file-preview.html',
            controller: FilePreviewDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attr, ctrl) {
                ctrl.setFile(attr.file);
            }
        }
    }

    angular.module('app').controller('FilePreviewDirectiveController', FilePreviewDirectiveController);

    function FilePreviewDirectiveController(api, $scope, entityService, derivationService, commentService) {
        var t = this;

        t.setFile = setFile;
        t.entity = null;
        t.derivations = [];

        t.stage = new createjs.Stage("canvas");
        t.layers = {
            image: new createjs.Container(),
            canvas: new createjs.Container(),
            comments: new createjs.Container()
        };

        initialize();

        function initialize() {
            var canvas = document.getElementById("canvas");
            canvas.width = window.innerWidth;
            canvas.height = window.innerHeight;

            initializeCanvas(canvas);

            t.stage.autoClear = true;
            t.stage.autoFit = true;
            t.stage.addChild(t.layers.image);
            t.stage.addChild(t.layers.canvas);
            t.stage.addChild(t.layers.comments);
        }

        function initializeCanvas(canvas) {
            canvas.addEventListener("mousewheel", MouseWheelHandler, false);
            canvas.addEventListener("DOMMouseScroll", MouseWheelHandler, false);

            var zoom;

            function MouseWheelHandler(e) {
                if (Math.max(-1, Math.min(1, (e.wheelDelta || -e.detail))) > 0) {
                    zoom = 1.1;
                } else {
                    zoom = 1 / 1.1;
                }

                var local = t.stage.globalToLocal(t.stage.mouseX, t.stage.mouseY);
                t.stage.regX = local.x;
                t.stage.regY = local.y;
                t.stage.x = t.stage.mouseX;
                t.stage.y = t.stage.mouseY;
                t.stage.scaleX = t.stage.scaleY *= zoom;
                t.stage.update();
            }

            setPanMode();
        }

        function setPanMode() {
            t.stage.removeAllEventListeners("stagemousedown");
            t.stage.addEventListener("stagemousedown", function (e) {
                var offset = {
                    x: t.stage.x - e.stageX,
                    y: t.stage.y - e.stageY
                };

                t.stage.addEventListener("stagemousemove", function (ev) {
                    t.stage.x = ev.stageX + offset.x;
                    t.stage.y = ev.stageY + offset.y;
                    t.stage.update();
                });

                t.stage.addEventListener("stagemouseup", function () {
                    t.stage.removeAllEventListeners("stagemousemove");
                });
            });
        }

        function setRectangleMode() {
            t.stage.removeAllEventListeners("stagemousedown");
            t.stage.addEventListener("stagemousedown", function (e) {
                var offset = t.stage.globalToLocal(e.stageX, e.stageY)
                var width = 1;
                var height = 1;
                var color = "#0000FF";
                var square = null;
                var circle = null;
                var added = false;
                square = new createjs.Shape();
                square.graphics.setStrokeStyle(1).beginStroke(color).drawRect(offset.x, offset.y, width, height);

                square.name = "marker" + new Date().valueOf();
                t.stage.addEventListener("stagemousemove", function (ev) {
                    if (added == false) {
                        t.layers.canvas.addChild(square);
                        added = true;
                    }
                    var loc = t.stage.globalToLocal(ev.stageX, ev.stageY);
                    width = loc.x - offset.x;
                    height = loc.y - offset.y;

                    square.graphics.clear().setStrokeStyle(1).beginStroke(color).drawRect(offset.x, offset.y, width, height);
                    t.stage.update();
                });
                t.stage.addEventListener("stagemouseup", function () {
                    t.stage.removeAllEventListeners("stagemousemove");
                    if (circle == null && added == true) {
                        circle = new createjs.Shape();
                        circle.graphics.setStrokeStyle(2).beginStroke("#000000").beginFill("#FF0000").drawCircle(offset.x, offset.y, 10);
                        circle.on("click", function () {
                            t.layers.canvas.removeChild(square, circle);
                            t.stage.update();
                        });
                        addMarker({
                            x: offset.x,
                            y: offset.y,
                            width: width,
                            height: height,
                            color: color
                        });
                        t.layers.canvas.addChild(circle);
                        t.stage.update();
                    }

                });
            });
        }

        function setFile(entityUri) {
            api.getCanvasRenderingsFromEntity(entityUri).then(function(data) {
                for(i = 0; i < data.length; i++) {
                    console.log(data[i]);
                }
            });

            entityService.getById(entityUri).then(function (response) {
                t.entity = response;

                if (t.entity.Revisions != null) {
                    loadDerivations();
                }
            });
        }

        function loadDerivations() {
            var promises = [];

            t.entity.Revisions.forEach(function (item) {
                var p = derivationService.getById(item);

                promises.push(p);

                p.then(function (derivation) {
                    derivation.images = [];
                    derivation.RenderedAs.forEach(function (rendering) {
                        rendering.url = derivationService.getImageUrl(rendering.Uri);

                        derivation.images.push(rendering);
                    });

                    t.derivations.push(derivation);
                });
            });

            Promise.all(promises).then(function () {
                if (t.derivations.length > 0) {
                    render(t.derivations[0]);
                }
            });
        }

        function render(derivation) {
            t.layers.image.removeAllChildren();

            t.stage.update();

            derivation.images.forEach(function (img) {
                var image = new Image();
                image.src = img.url;
                image.onload = function handleImageLoad(event) {
                    var image = event.target;
                    var bitmap = new createjs.Bitmap(image);
                    bitmap.x = img.Region.x;
                    bitmap.y = -img.Region.y;
                    t.layers.image.addChild(bitmap);
                    t.stage.update();
                };
            });
        }
    }
})();