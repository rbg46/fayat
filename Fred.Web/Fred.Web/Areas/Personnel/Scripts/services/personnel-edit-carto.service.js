/*
 * Ce service sert a la gestion de la carte sur le personnel
 */
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('PersonnelEditCartoService', PersonnelEditCartoService);

  PersonnelEditCartoService.$inject = ['$timeout'];

  function PersonnelEditCartoService($timeout) {

    var _carto = null;
    var _map = null;

    var service = {
      init: init,
      zoomOnFrenchCountry: zoomOnFrenchCountry,
      zoomOn: zoomOn,
      zoomOnPersonnel: zoomOnPersonnel,
      setCoordinateOnPersonnel: setCoordinateOnPersonnel,
      getPersonnelHasCoordinates: getPersonnelHasCoordinates
    };
    return service;

    function init(carto, map) {
      _carto = carto;
      _map = map;
    }

    /*
     * effectue un zoom sur la carte.
     */
    function _setMapZoom(zoom) {
      $timeout(function () {
        if (_map) {
          _map.setZoom(zoom);
        }
      }, 10);
    }

    /*
    * Zoom sur la france
    */
    function zoomOnFrenchCountry() {
      _carto.latitude = '46.1444853';
      _carto.longitude = '-2.4359897';
      _setMapZoom(5);
    }

    /*
     * zoom sur la position du personnel
     */
    function zoomOnPersonnel(personnel) {
      _carto.latitude = personnel.LatitudeDomicile;
      _carto.longitude = personnel.LongitudeDomicile;
      _setMapZoom(10);
    }

    /*
     * transfert les coordonnées  de la carte au personnel
     */
    function setCoordinateOnPersonnel(personnel) {
      personnel.LatitudeDomicile = _carto.latitude;
      personnel.LongitudeDomicile = _carto.longitude;
    }

    /*
    * zoom sur la position trouvé par google api
    */
    function zoomOn(personnel, coordinates) {
      _carto.latitude = coordinates[0].Geometry.Location.Lat;
      _carto.longitude = coordinates[0].Geometry.Location.Lng;
    }


    /*
     * Permet de savoir si un personnel a des coordonnées
     */
    function getPersonnelHasCoordinates(personnel) {
      if (personnel.LatitudeDomicile && personnel.LongitudeDomicile) {
        return true;
      }
      return false;
    }

  }
})();

