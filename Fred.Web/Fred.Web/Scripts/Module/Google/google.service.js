/// <reference path="adresse.model.js" />
(function () {
  'use strict';

  angular
     .module('Fred')
     .service('GoogleService', GoogleService);

  GoogleService.$inject = ['$http'];

  function GoogleService($http) {

    var vm = this;

    vm.geocode = function (adresse1, adresse2, adresse3, codePostal, ville, pays) {
      var address = new Adresse(adresse1, adresse2, adresse3, codePostal, ville, pays);
      return $http.post('/api/GoogleService/Geocode/', address, { cache: false });
    };


    vm.inverserGeocode = function (lat, lng) {
      var location = {
        lat: lat,
        lng: lng
      };
      return $http.post('/api/GoogleService/InverseGeocode/', location, { cache: false });
    };

    vm.addSelectPositionButton = SelectPinnedPositionBtn;

    vm.displaySelectPositionButton = displaySelectPositionBtn;

    /*
     * The SelectPinnedPositionBtn adds a control to the map that select the selected position     
     * @param {any} map Google Map
     * @param {any} mapSelector pinned position
     * @param {any} func Function associated to the control
     * @returns {any} control
     */
    function SelectPinnedPositionBtn(resources, map, mapSelector, func) {

      var centerControlDiv = document.createElement('div');

      // Set CSS for the control border.
      var controlUI = document.createElement('div');
      controlUI.id = "selectPosBtn";
      controlUI.style.marginTop = "10px";
      controlUI.style.borderRadius = '2px';
      controlUI.style.boxShadow = '0 2px 6px rgba(0,0,0,.3)';
      controlUI.style.cursor = 'pointer';
      controlUI.style.textAlign = 'center';
      controlUI.style.display = !map.markers.mapSelectorMap.map || !mapSelector ? 'none' : 'block';
      controlUI.title = 'Cliquer pour sélectionner la position';
      controlUI.style.backgroundColor = 'rgb(0, 82, 160)';

      // Set CSS for the control interior.
      var controlText = document.createElement('div');
      controlText.style.color = '#fff';
      controlText.style.fontFamily = 'Roboto,Arial,sans-serif';
      controlText.style.fontSize = '11px';
      controlText.style.lineHeight = '28px';
      controlText.style.paddingLeft = '5px';
      controlText.style.paddingRight = '5px';
      controlText.innerHTML = resources.Global_Bouton_Selectionner;
      controlUI.appendChild(controlText);

      controlUI.addEventListener('mouseover', function () {
        controlUI.style.backgroundColor = '#F1F2F3';
        controlText.style.color = 'rgb(0, 82, 160)';
      });
      controlUI.addEventListener('mouseout', function () {
        controlText.style.color = '#fff';
        controlUI.style.backgroundColor = 'rgb(0, 82, 160)';
      });

      // Setup the click event listeners
      controlUI.addEventListener('click', func);

      centerControlDiv.appendChild(controlUI);
      centerControlDiv.index = 1;
      map.controls[google.maps.ControlPosition.TOP_LEFT].push(centerControlDiv);
    }

    /*
     * @param {boolean} display Show or hide select position button
     */
    function displaySelectPositionBtn(show) {
      var selectBtn = document.getElementById("selectPosBtn");
      selectBtn.style.display = show ? 'block' : 'none';
    }
  }

})();

