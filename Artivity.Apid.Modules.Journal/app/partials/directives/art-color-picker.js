angular.module('explorerApp').directive('artColorPicker', ColorPickerDirective);

function ColorPickerDirective() {
    return {
        template: '<button type="button" class="btn btn-colorpicker"><span class="color-fill-icon"><i></i></span></button>',
        link: function (scope, element, attributes) {
            var indicator = $('.color-fill-icon', element);
            indicator.css('background-color', getValue(scope, attributes.selectedColor));

            var button = $(element);

            button.on('changeColor', function (e) {
                if (e.color == null) {
                    //when select transparent color
                    //$('.color-fill-icon', btn).addClass('colorpicker-color');
                } else {
                    //$('.color-fill-icon', btn).removeClass('colorpicker-color');
                    indicator.css('background-color', e.color);

                    // Update the bound value.
                    setValue(scope, attributes.selectedColor, e.color.toHex());
                }
            });

            button.colorpicker({
                customClass: 'colorpicker-lg',
                align: 'right',
                format: 'rgb',
                color: getValue(scope, attributes.selectedColor),
                colorSelectors: {
                    'default': '#777777',
                    'primary': '#337ab7',
                    'success': '#5cb85c',
                    'info': '#5bc0de',
                    'warning': '#f0ad4e',
                    'danger': '#d9534f'
                },
                sliders: {
                    saturation: {
                        maxLeft: 200,
                        maxTop: 200
                    },
                    hue: {
                        maxTop: 200
                    },
                    alpha: {
                        maxTop: 200
                    }
                }
            });
        }
    }
}