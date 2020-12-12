/*
 * Ce service sert a l'edition d'un contact
 */
(function () {
  'use strict';

  angular.module('Fred').service('PersonnelEditContactService', PersonnelEditContactService);

  function PersonnelEditContactService() {

    var service = {
      copyContact: copyContact,
      setContactAdress: setContactAdress,
      setContactAddressWithInverseGeocode: setContactAddressWithInverseGeocode,
      getPersonnelIsNew: getPersonnelIsNew
    };
    return service;

    /*
     * copie les infos d'un contact
     */
    function copyContact(source, destination) {
      destination.Adresse1 = source.Adresse1;
      destination.Adresse2 = source.Adresse2;
      destination.Adresse3 = source.Adresse3;
      destination.CodePostal = source.CodePostal;
      destination.Ville = source.Ville;
      destination.PaysLabel = source.PaysLabel;
      destination.PaysId = source.PaysId;
      destination.Pays = source.Pays;
      destination.Telephone1 = source.Telephone1;
      destination.Telephone2 = source.Telephone2;
      destination.Email = source.Email;
      destination.LatitudeDomicile = source.LatitudeDomicile;
      destination.LongitudeDomicile = source.LongitudeDomicile;
    }


    /*
    * Met a jour les coordonnées du personnel
    */
    function setContactAdress(persoForContact, info) {
      persoForContact.Adresse1 = info.Adresse.Adresse1;
      persoForContact.Adresse2 = info.Adresse.Adresse2;
      persoForContact.Adresse3 = info.Adresse.Adresse3;
      persoForContact.CodePostal = info.Adresse.CodePostal;
      persoForContact.Ville = info.Adresse.Ville;
      persoForContact.PaysLabel = info.Adresse.Pays;
      persoForContact.LatitudeDomicile = info.Geometry.Location.Lat;
      persoForContact.LongitudeDomicile = info.Geometry.Location.Lng;
    }

    function setContactAddressWithInverseGeocode(persoForContact, info, lat, lng) {

      persoForContact.Adresse1 = info.Adresse.Adresse1;
      persoForContact.Adresse2 = info.Adresse.Adresse2;
      persoForContact.Adresse3 = info.Adresse.Adresse3;
      persoForContact.CodePostal = info.Adresse.CodePostal;
      persoForContact.Ville = info.Adresse.Ville;
      persoForContact.PaysLabel = info.Adresse.Pays;
      persoForContact.LatitudeDomicile = Number(lat);
      persoForContact.LongitudeDomicile = Number(lng);
    }



    function getPersonnelIsNew(personnelId) {
      if (personnelId === 0) {
        return true;
      }
      return false;
    }



  }
})();

