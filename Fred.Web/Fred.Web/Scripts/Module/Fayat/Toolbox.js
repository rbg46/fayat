/*
 * ToolBox JS2019
 */
var FredToolBox = {

    /* bindScroll :   Permet l'abonnement à l'évènement "Fin de scroll" d'un élément passé en paramètre 1
    *                 puis entraine l'exécution de la méthode déléguée passé en paramètre 2.
    *  @return {void}
    */
    bindScrollEnd: function(element, callback) {
        var eventName = 'scroll';
        var $element = $(element);

        $element.off(eventName).on(eventName, function() {
            var hasReachedEnd = $element.scrollTop() + $element.innerHeight() >= $element[0].scrollHeight - 1;
            if (hasReachedEnd) {
                callback();
            }
        });
    },

    /* hasVerticalScrollbarVisible :   Indique si la scrollbar verticale est visible ou non
    *  @return {bool}
    */
    hasVerticalScrollbarVisible: function (element) {
        if (!element) {
            return false;
        }
        var ele = $(element).get(0);
        if (!ele || !ele.scrollHeight || !ele.clientHeight) {
            return false;
        }
        return ele.scrollHeight > ele.clientHeight;
    },

    /* unBindScroll :   Permet le désabonnement à l'évènement "Fin de scroll" d'un élément passé en paramètre.
    *  @return {void}
    */
    unBindScrollEnd: function (element) {
        $(element).unbind('scroll');
    },

  /* Gestion des tableaux "Fixe"    
   * @function manageTableResize
   * @var el identifant DOM du tableau
   * @description Resize la table
   */
    manageTableResize: function (el, window) {
        var $parent;
        var $widthParent;
        var $table;
        var $head;
        var $body;

        $parent = $(el).parent();
        $widthParent = $parent[0].clientWidth;
        $table = $(el);
        $head = $(el).find('thead');
        $body = $(el).find('tbody');
        
        /* Fixe largeur du tableau en fonction la largeur de la page */
        $($table).css("width", $widthParent);
        $($head).css("width", $widthParent);
        $($head).find('th').css("width", $widthParent);
        $($body).css("width", $widthParent);

        /* Gestion de la hauteur du tableau en fonction de l'espace disponible */

        // Position du tableau en position TOP
        var table_top = $table[0].offsetTop;
        
        // Hauteur de la page
        var windows_height = window.innerHeight;

        // Récupération de la taille maxi acceptable du tableau
        var table_height_ideal = windows_height - table_top - 100;
        
        // Affectation de la taille maxi acceptable
        $($body).css("max-height", table_height_ideal);
        $($body).css("height", table_height_ideal);
    },
         
    /*
     * @function manageTable
     * @var el identifant DOM du tableau
     * @description Gère la fixation de la 1er colonne
     */
    manageTable: function (el, mode) {        
        var $head;
        var $body;
                
        $head = $(el).find('thead');
        $body = $(el).find('tbody');
               
        return $($body).scroll(function () {
            //var t, l;
            $($head).css('left', -$($body).scrollLeft());
            $($head).find(':nth-child(1)').css('left', $($body).scrollLeft());
            $($body).find(':nth-child(1)').css('left', $($body).scrollLeft());
        });
    }
};

String.format = function (format) {
    var args = Array.prototype.slice.call(arguments, 1);
    return format.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] !== 'undefined' ? args[number] : match;
    });
};

Number.prototype.round = function round(decimals) {
    if (isNaN(this.valueOf())) {
        throw "La variable n'est pas un nombre.";
    }
    if (!decimals) {
        throw "Paramètre manquant. Veuillez entrer le paramètre 'decimals'.";
    }
    var tmp = this.valueOf() + 'e' + decimals;
    var result = Math.round(Number(tmp)) + 'e-' + decimals;
    return Number(result);    
}