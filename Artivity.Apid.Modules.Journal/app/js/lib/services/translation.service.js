(function () {
    angular.module('explorerApp').factory('translationService', ['$translate', translationService]);

    function translationService(translate) {
        return {
            getInfluenceLabel: getInfluenceLabel,
            getInfluenceIcon: getInfluenceIcon
        }

        function getInfluenceLabel(influence) {
            var key = undefined;

            switch (influence.type) {
                case 'http://www.w3.org/ns/prov#Generation':
                    {
                        key = 'FILEVIEW.http://www.w3.org/ns/prov#Generation';
                        break;
                    }
                case 'http://www.w3.org/ns/prov#Invalidation':
                    {
                        key = 'FILEVIEW.http://www.w3.org/ns/prov#Invalidation';
                        break;
                    }
                default:
                    {
                        // TODO: pluralize
                        key = 'FILEVIEW.' + getChangedProperty(influence);
                        break;
                    }
            }

            var result;

            // Only translate if we actually found a property in the previous loop.
            if (key && key !== 'FILEVIEW.') {
                result = translate.instant(key)
            } else if (influence.description) {
                result = influence.description;
            } else {
                result = translate.instant('FILEVIEW.' + influence.type);
            }

            return result;
        }

        function getInfluenceIcon(influence) {
            switch (influence.type) {
                case 'http://www.w3.org/ns/prov#Generation':
                    return 'zmdi-plus';
                case 'http://www.w3.org/ns/prov#Invalidation':
                    return 'zmdi-delete';
                case 'http://www.w3.org/ns/prov#Derivation':
                    return 'zmdi-floppy';
                case 'http://www.w3.org/ns/prov#Undo':
                    return 'zmdi-undo';
                case 'http://www.w3.org/ns/prov#Redo':
                    return 'zmdi-redo';
                case 'http://w3id.org/art/terms/1.0/Save':
                    return 'zmdi-floppy';
                case 'http://w3id.org/art/terms/1.0/SaveAs':
                    return 'zmdi-floppy';
            }

            /*
            var property = getChangedProperty(influence);

            if (property !== '') {
                switch (property) {
                case 'http://w3id.org/art/terms/1.0/position':
                    return 'zmdi-arrows';
                case 'http://w3id.org/art/terms/1.0/hadBoundaries':
                    return 'zmdi-border-style';
                case 'http://www.w3.org/2000/01/rdf-schema#label':
                    return 'zmdi-format-color-text';
                case 'http://w3id.org/art/terms/1.0/textValue':
                    return 'zmdi-format-color-text';
                case 'http://w3id.org/art/terms/1.0/strokeWidth':
                    return 'zmdi-border-color';
                }
            }
            */

            return 'zmdi-brush';
        }

        function getChangedProperty(influence) {
            for (var i = 0; i < influence.changes.length; i++) {
                var change = influence.changes[i];

                if (change.entityType !== 'http://w3id.org/art/terms/1.0/Layer' && change.property) {
                    return change.property;
                }
            }

            return '';
        }
    }
})();