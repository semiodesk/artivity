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

        //initialize();

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
            entityService.getById(entityUri).then(function (response) {
                t.entity = response;

                if (t.entity.Revisions.length > 0) {
                    // Initialize the viewer and load the renderings when the document is ready.
                    $(document).ready(function () {
                        t.canvas = document.getElementById('canvas');

                        if (t.canvas) {
                            t.viewer = new DocumentViewer(t.canvas, api.getRenderingUrl(entityUri));

                            api.getCanvasRenderingsFromEntity(t.entity.Uri).then(function (data) {
                                t.viewer.pageCache.load(data, function () {
                                    console.log('Loaded pages:', data);

                                    t.viewer.render(t.entity.Revisions[0]);
                                });
                            });
                        }
                    });
                }
            });
        }
    }
})();