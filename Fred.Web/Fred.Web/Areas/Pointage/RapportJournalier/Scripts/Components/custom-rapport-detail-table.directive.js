(function () {
  'use strict';

  /*Convertion RGB*/
  function hexToRgb(hex) {
    // Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")
    var shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
    hex = hex.replace(shorthandRegex, function (m, r, g, b) {
      return r + r + g + g + b + b;
    });

    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
      r: parseInt(result[1], 16),
      g: parseInt(result[2], 16),
      b: parseInt(result[3], 16)
    } : null;
  }

  //////////////////////////////////////////////////////////////////
  //     DIRECTIVE POUR GERER LA PERSONNALISATION                 //
  //////////////////////////////////////////////////////////////////

  angular.module('Fred').directive('fredUxtable', function () {
    var linkFunction = function (scope, element, attributes) {

      /*Fonction de convertion et consolidation de la couleur*/
      var getColor = function (hexColor) {

        /*Définition du transparence en fonction du type*/
        var a = "1.0";
        // Récupération du type de controle père pour savoir si Entete (THEAD) ou Tableau (TBODY)
        var parent = element.context.parentElement.parentElement.tagName;
        var classname = element.context.parentElement.className;
        // Récupération Alternance de la ligne 
        var even = scope.$parent.$odd;

        if (parent === "THEAD") {
          if (classname === "table-header-level2") {
            a = "0.8";
          } else {
            a = "1.0";
          }

        }
        if (parent === "TBODY" && even === true) {
          a = "0.5";
        }
        if (parent === "TBODY" && even === false) {
          a = "0.3";
        }
        if (parent === "TFOOT") {
          a = "1.0";
        }
        /*Décodage de couleur hex en RGB*/
        var rgb = hexToRgb(hexColor);
        var color = "rgba(" + rgb.r + "," + rgb.g + "," + rgb.b + "," + a + ")";
        return color;
      };

      ///*Récupération du tableau personnalisé*/
      //var _userproperties = scope.$parent.UserProperties;
      ///*Récupération de la configuration pour la colonne cible*/
      //var _userproperty = _userproperties[attributes.fredUxtable]

      /*Surveillance si changement de mode de vue pour afficher ou masquer les éléments*/
      scope.$parent.$watch('UserProperties.viewMode', function (newValue, oldValue) {
        if (newValue) {
          if (!scope.$parent.UserProperties[attributes.fredUxtable].visibility[newValue]) {
            element.hide();
          } else {
            element.show();
          }
        }
      });

      /*Surveillance dans le changement de couleur en Background*/
      scope.$parent.$watch('UserProperties["' + attributes.fredUxtable + '"].backgroundColor', function (newValue, oldValue) {
        if (newValue)
          var colorRgba = getColor(newValue);
        element.css({ "background-color": colorRgba });
      });

      /*Surveillance dans le changement de couleur de police*/
      scope.$parent.$watch('UserProperties["' + attributes.fredUxtable + '"].color', function (newValue, oldValue) {
        if (newValue)

          element.css({ "color": newValue });
      });

      /*Surveillance dans le changement de visibilité (uniquement sur le mode Custo) */
      scope.$parent.$watch('UserProperties["' + attributes.fredUxtable + '"].visibility.custo', function (newValue, oldValue) {
        if (scope.$parent.UserProperties.viewMode === "custo") {
          if (newValue) {
            element.show();
          } else {
            element.hide();
          }
        }
      });

      element.css({ "background-color": scope.$parent.UserProperties[attributes.fredUxtable].backgroundColor });
      /*element.css({ "width": scope.$parent.UserProperties[attributes.fredUxtable].width });*/
      element.css({ "color": scope.$parent.UserProperties[attributes.fredUxtable].color });

    };
    return {
      replace: true,
      scope: {
        type: '@myType',
        userProperties: '='
      },
      link: linkFunction
    };
  });
}());