/*
 * Cette directive permet d'encapsuler un chaine de caractère dans un tag HTML
 * Exemple d'utilisation
 *  $ctrl.searchTexts = ['toto'] // liste de mots à encapsuler dans un tag
 *  {{ref.Libelle}} // Mot dans lequel on va chercher le mot 'toto'
 *   <h2 class="fournisseur-h2" fred-wrap-in-tag="{{ref.Libelle}}" tag="strong" words="$ctrl.searchTexts">{{ref.Libelle}}</h2>
 *   Par défaut le mot trouver est remplacé par un mot en majuscule dans le nouveau tag
 */
(function () {
  'use strict';

  angular.module('Fred').directive('fredWrapInTag', fredWrapInTagDirective);

  function fredWrapInTagDirective() {
    return {
      restrict: 'A',
      scope: {
        words: '=',
        tag: '='
      },
      link: function (scope, element, attr) {
        var tag = scope.tag || 'strong'; // default bold
        var words = scope.words || [];
        var text = attr.fredWrapInTag || "";
        
        for (var i = 0; i < words.length; i++) {
          if (words[i]) {
            var pattern = new RegExp(words[i], 'i');
            text = text.replace(pattern, '<' + tag + '>' + words[i].toUpperCase() + '</' + tag + '>');
          }
          element.html(text);
        }
      }
    }
  }

})();
