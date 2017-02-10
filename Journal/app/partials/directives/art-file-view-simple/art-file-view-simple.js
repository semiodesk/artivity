(function () {
    angular.module('explorerApp').directive('artFileViewSimple', FileViewSimpleDirective);

    function FileViewSimpleDirective() {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: 'app/partials/directives/art-file-view-simple/art-file-view-simple.html',
            controller: FileViewSimpleDirectiveController,
            controllerAs: 'vm',
            link: function (scope, element, attr, ctrl) {
                ctrl.setFile(attr.file);
            }
        }
    }

    angular.module('explorerApp').controller('FileViewSimpleDirectiveController', FileViewSimpleDirectiveController);

    function FileViewSimpleDirectiveController(api, $scope, entityService, derivationService, commentService) {
        var vm = this;

        vm.imageFileId = null;
        vm.entity = null;
        vm.derivations = [];
        vm.selectedDerivation = null;
        vm.setFile = setFile;
        vm.selectDerivation = selectDerivation;
        vm.loadDerivations = loadDerivations;
        vm.loadImages = loadImages;
        vm.createNewCollection = createNewCollection;
        vm.createNewComment = createNewComment;
        vm.saveCurrentComment = saveCurrentComment;
        vm.selectComment = selectComment;
        vm.removeComment = removeComment;
        vm.setRectangleMode = setRectangleMode;
        vm.setPanMode = setPanMode;
        vm.stage = null;
        vm.currentCollection = null;
        vm.currentComment = null;
        vm.canvasContainer = null;
        vm.imageContainer = null;

        initialize();

        function initialize() {
            var canvas = document.getElementById("canvas");
            canvas.width = window.innerWidth;
            canvas.height = window.innerHeight;
            vm.stage = new createjs.Stage("canvas");

            initializeCanvas(canvas);
            initializeCommenting();
            vm.imageContainer = new createjs.Container();
            vm.canvasContainer = new createjs.Container();
            vm.stage.addChild(vm.imageContainer, vm.canvasContainer);
        }

        function setFile(file) {
            imageFileId = file;
            entityService.getById(imageFileId).then(function (res) {
                vm.entity = res;
                if (vm.entity.Revisions != null)
                    loadDerivations();
            });
        }

        function loadDerivations() {
            var promises = [];
            vm.entity.Revisions.forEach(function (item) {
                var p = derivationService.getById(item);

                p.then(function (res) {
                    res.images = [];
                    res.RenderedAs.forEach(function (img) {
                        img.url = derivationService.getImageUrl(img.Uri);
                        res.images.push(img);

                    });
                    vm.derivations.push(res);

                });
                promises.push(p);
            })
            Promise.all(promises).then(function () {
                if (vm.selectDerivationed == null) {
                    vm.selectDerivation(vm.derivations[0]);
                }
                $scope.$apply();
            });
        }

        function selectDerivation(derivation) {
            vm.selectedDerivation = derivation;
            vm.loadImages(derivation);
        }

        function loadImages(deriv) {
            vm.imageContainer.removeAllChildren();
            vm.stage.update();
            deriv.images.forEach(function (img) {
                var image = new Image();
                image.src = img.url;
                image.onload = function handleImageLoad(event) {
                    var image = event.target;
                    var bitmap = new createjs.Bitmap(image);
                    bitmap.x = img.Region.x;
                    bitmap.y = -img.Region.y;
                    vm.imageContainer.addChild(bitmap);
                    vm.stage.update();
                };
            });

        }

        function initializeCanvas(canvas) {
            canvas.addEventListener("mousewheel", MouseWheelHandler, false);
            canvas.addEventListener("DOMMouseScroll", MouseWheelHandler, false);

            var zoom;
            function MouseWheelHandler(e) {
                if (Math.max(-1, Math.min(1, (e.wheelDelta || -e.detail))) > 0)
                    zoom = 1.1;
                else
                    zoom = 1 / 1.1;
                var local = vm.stage.globalToLocal(vm.stage.mouseX, vm.stage.mouseY);
                vm.stage.regX = local.x;
                vm.stage.regY = local.y;
                vm.stage.x = vm.stage.mouseX;
                vm.stage.y = vm.stage.mouseY;
                vm.stage.scaleX = vm.stage.scaleY *= zoom;

                vm.stage.update();

            }
            setPanMode();
        }

        function setPanMode() {
            vm.stage.removeAllEventListeners("stagemousedown");
            vm.stage.addEventListener("stagemousedown", function (e) {
                var offset = { x: vm.stage.x - e.stageX, y: vm.stage.y - e.stageY };
                vm.stage.addEventListener("stagemousemove", function (ev) {
                    vm.stage.x = ev.stageX + offset.x;
                    vm.stage.y = ev.stageY + offset.y;
                    vm.stage.update();
                });
                vm.stage.addEventListener("stagemouseup", function () {
                    vm.stage.removeAllEventListeners("stagemousemove");
                });

            });
        }


        function setRectangleMode() {

            vm.stage.removeAllEventListeners("stagemousedown");
            vm.stage.addEventListener("stagemousedown", function (e) {
                var offset = vm.stage.globalToLocal(e.stageX, e.stageY)
                var width = 1;
                var height = 1;
                var color = "#0000FF";
                var square = null;
                var circle = null;
                var added = false;
                square = new createjs.Shape();
                square.graphics.setStrokeStyle(1).beginStroke(color).drawRect(offset.x, offset.y, width, height);

                square.name = "marker" + new Date().valueOf();
                vm.stage.addEventListener("stagemousemove", function (ev) {
                    if (added == false) {
                        vm.canvasContainer.addChild(square);
                        added = true;
                    }
                    var loc = vm.stage.globalToLocal(ev.stageX, ev.stageY);
                    width = loc.x - offset.x;
                    height = loc.y - offset.y;

                    square.graphics.clear().setStrokeStyle(1).beginStroke(color).drawRect(offset.x, offset.y, width, height);
                    vm.stage.update();
                });
                vm.stage.addEventListener("stagemouseup", function () {
                    vm.stage.removeAllEventListeners("stagemousemove");
                    if (circle == null && added == true) {
                        circle = new createjs.Shape();
                        circle.graphics.setStrokeStyle(2).beginStroke("#000000").beginFill("#FF0000").drawCircle(offset.x, offset.y, 10);
                        circle.on("click", function () {
                            vm.canvasContainer.removeChild(square, circle);
                            vm.stage.update();
                        });
                        addMarker({
                            x: offset.x,
                            y: offset.y,
                            width: width,
                            height: height,
                            color: color
                        });
                        vm.canvasContainer.addChild(circle);
                        vm.stage.update();
                    }

                });


            });
        }

        function setPathMode() {

        }

        /**
         * Creates a new comment collection. Resets the current comment.
         * @param {String} entityUri 
         * @param {String} influenceUri
         */
        function createNewCollection(entityUri, influenceUri) {
            vm.currentCollection = {
                influence: influenceUri,
                entity: entityUri,
                startTime: new Date(),
                comments: []
            }
            vm.currentComment = null;
        }

        /**
         * Creates a new comment. Removes any previous unsafed comment.
         */
        function createNewComment() {
            vm.currentComment = { text: '', marker: [] };
            vm.currentComment.id = vm.currentCollection.comments.length;
            vm.currentCollection.comments.push(vm.currentComment);
        }

        /**
         * Saves current comment tothe collection.
         */
        function saveCurrentComment() {
            if (vm.currentCollection != null) {

                vm.currentCollection.comments[vm.currentComment.id] = vm.currentComment;
                createNewComment();
                vm.canvasContainer.removeAllChildren();
                vm.stage.update();
            }
        }

        function loadMarker(m) {
            var square = new createjs.Shape();
            square.graphics.setStrokeStyle(1).beginStroke(m.color).drawRect(m.x, m.y, m.width, m.height);
            var circle = new createjs.Shape();
            circle.graphics.setStrokeStyle(2).beginStroke("#000000").beginFill("#FF0000").drawCircle(m.x, m.y, 10);
            circle.on("click", function () {
                vm.canvasContainer.removeChild(square, circle);
                vm.stage.update();
            });
            vm.canvasContainer.addChild(square, circle);
        }

        /**
         * Selects a comment from the collection and sets it as current.
         * @param {Number} index
         */
        function selectComment(index) {
            if (vm.currentCollection != null) {
                vm.currentComment = vm.currentCollection.comments[index];

                vm.canvasContainer.removeAllChildren();
                for (i = vm.currentComment.marker.length - 1; i >= 0; i--) {

                    var m = vm.currentComment.marker[i];
                    loadMarker(m);

                }

                vm.stage.update();
            }
        }

        /**
         * Removes a comment from the collection.
         * @param {Number} index
         */
        function removeComment(index) {
            vm.currentCollection.comments.splice(index, 1);
        }

        function initializeCommenting() {
            vm.createNewCollection();
            vm.createNewComment();
        }

        function addMarker(marker) {
            vm.currentComment.marker.push(marker);
        }

        function removeMarker(markerName) {
            for (i = vm.currentComment.marker.length - 1; i >= 0; i--) {
                if (vm.currentComment.marker[i].name == markerName)
                    vm.currentComment.marker.splice(i, 1);
            }
        }

    }
})();