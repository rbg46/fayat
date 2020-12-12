(function () {
  'use strict';
  /*
    @description Directive pour gerer les inputs de nombres
    Utilisation :      
        - fred-input-number="DECIMAL_LIMIT" par défaut, on a 2 décimales
          ex : <input type="text" fred-input-number="3" />
               <input type="text" fred-input-number />
    Remarques : les min et max décimaux doivent être avec un un point et non une virgule.
  */
  angular.module('Fred').directive('fredInputNumber', function () {
    return {
      restrict: 'A',
      require: 'ngModel',
      link: link
    };

    function link(scope, element, attrs, ngModelController) {

      var LANG = config.DEFAULT_LANG;
      var DECIMAL_LIMIT = !attrs.fredInputNumber ? config.DEFAULT_DECIMAL_LIMIT : attrs.fredInputNumber;
      var DECIMAL_SEP = LANG === config.DEFAULT_LANG ? config.DECIMAL_SEP_COMMA : config.DECIMAL_SEP_DOT;
      var GROUP_SEP = LANG === config.DEFAULT_LANG ? config.GROUP_SEP_SPACE : config.GROUP_SEP_COMMA;
      var MIN = parseFloat(attrs.min);
      var MAX = parseFloat(attrs.max);
      var DEFAULT_VALUE = parseFloat(attrs.fredInputDefaultValue);
      // Gestion des changements de valeurs
      attrs.$observe('min', function (value) {MIN = parseFloat(value);});
      attrs.$observe('max', function (value) {MAX = parseFloat(value);});
      /* -------------------------------------------------------------------------------------------------------------
       *                                            ACTIONS
       * -------------------------------------------------------------------------------------------------------------
       */

      /*
       * @description Formatting de l'input text au format Number
       */
      function formatNumber(value) {
        value = value.toString();

        // Si lang = fr, on remplace le point ('.') par une virgule (',')
        if (LANG === config.DEFAULT_LANG) {
          value = value.replace(config.DECIMAL_SEP_DOT, config.DECIMAL_SEP_COMMA);
        }

        var regex = new RegExp("[^0-9\-" + DECIMAL_SEP + "]", "g");
        value = value.replace(regex, "");

        // Allow only one dot in input text
        if (value.split(DECIMAL_SEP).length > 2) {
          var pos = value.lastIndexOf(DECIMAL_SEP);
          value = value.substring(0, pos) + value.substring(pos + 1);
        }

        // Thousand separator (GROUP SEPARATOR)
        var parts = value.split(DECIMAL_SEP);
        parts[0] = parts[0].replace(/\d{1,3}(?=(\d{3})+(?!\d))/g, "$&" + GROUP_SEP);

        if (parts[1] && parts[1].length > DECIMAL_LIMIT) {
          parts[1] = parts[1].substring(0, DECIMAL_LIMIT);
        }
       
        if (parseInt(DECIMAL_LIMIT) > 0) {
          return parts.join(DECIMAL_SEP);
        }
        else {
          return parts[0];
        }
      }

      /*
       * @description Action de formatting
       */
      function applyFormatting() {
        var value = element.val();
        var original = value;
        if (!value || value.length === 0) {
          return;
        }
        value = formatNumber(value);
        if (value !== original) {
          element.val(value);
          element.triggerHandler('input');
        }
      }

      /*
       * @description Action de validation du min et du max
       * Si l'on dépasse la valeur max, c'est la valeur max qui est retenue par l'input
       * Si l'on est en dessous de la valeur min, c'est la valeur min qui est retenue par l'input
       */
      function actionValidate() {
        var value = parseFloat(ngModelController.$viewValue.replace(/ /g, '').replace(config.DECIMAL_SEP_COMMA, config.DECIMAL_SEP_DOT));

        if (MIN !== undefined) {
          if (value < MIN) {
            ngModelController.$setValidity('min', false);
            ngModelController.$setViewValue(MIN.toString());
            ngModelController.$render();
          }
          else {
            ngModelController.$setValidity('min', true);
          }
        }

        if (MAX !== undefined) {
          if (value > MAX) {
            ngModelController.$setValidity('max', false);
            ngModelController.$setViewValue(MAX.toString());
            ngModelController.$render();
          }
          else {
            ngModelController.$setValidity('max', true);
          }
        }
      }

      function actionAddDecimal(value) {
        if (DECIMAL_LIMIT > 0) {
          if (value) {            
            value = value.toString().split(DECIMAL_SEP);
            // Si la valeur est entière
            if (value.length === 1 && value[0] !== "") {
              value = parseFloat(value[0].replace(/ /g, '')).toFixed(DECIMAL_LIMIT);
            }
            // Si la valeur contient des décimaux
            else if (value.length === 2 && value[1] && value[1].length < DECIMAL_LIMIT) {
              value = parseFloat(value[0].replace(/ /g, '') + "." + value[1].replace(/ /g, '')).toFixed(DECIMAL_LIMIT);
            }
          }
        }
        return value;
      }

      /* -------------------------------------------------------------------------------------------------------------
       *                                            EVENTS
       * -------------------------------------------------------------------------------------------------------------
       */

      /*
       * @description évènement lorsque les touches du clavier sont relevées
       */
      element.on('keyup', function (e) {
        var keycode = e.keyCode;
        var isTextInputKey =
          keycode > 47 && keycode < 58 ||     // number keys
          keycode === 32 || keycode === 8 ||  // spacebar or backspace
          keycode > 64 && keycode < 91 ||     // letter keys
          keycode > 95 && keycode < 112 ||    // numpad keys
          keycode > 185 && keycode < 193 ||   // ;=,-./` (in order)
          keycode > 218 && keycode < 223;     // [\]' (in order)

        if (isTextInputKey) {
          applyFormatting();
          actionValidate();
        }
      });

      /*
       * @description évènement lorsque :
       *  - l'on sort le focus de l'input text
       *  - une propriété de l'élément a changé
       *  - l'on colle le contenu du presse-papier dans le champ
       */
      element.on('propertychange blur', function (evt) {
        var value = ngModelController.$viewValue;

        if (value === null || value === undefined || value === "") {
          value = DEFAULT_VALUE.toString();
        }

        if (value) {
          value = actionAddDecimal(value);

          ngModelController.$setViewValue(value.toString());
          ngModelController.$render();

          applyFormatting();
          actionValidate();
        }
      });

      /*
       * @description évènement de parsing : change how view values will be saved in the model.
       */
      ngModelController.$parsers.push(function (value) {
        if (!value || value.length === 0) {
          return value;
        }

        value = value.toString();

        // Si lang = fr, on remplace la virgule (',') par un point ('.')
        // Rq : le model doit être au avec un point et non une virgule.
        if (LANG === config.DEFAULT_LANG) {
          value = value.replace(config.DECIMAL_SEP_COMMA, config.DECIMAL_SEP_DOT);
        }

        return value.replace(/[^0-9\.-]/g, "");
      });

      /*
       * @description évènement de formatting : change how model values will appear in the view.
       */
      ngModelController.$formatters.push(function (value) {
        if (!value || value.length === 0) {
          return value;
        }
        value = actionAddDecimal(value);
        value = formatNumber(value);                
        return value;
      });
    }
  });
})();