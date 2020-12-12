(function () {
  "use strict";


  angular.module('Fred').service("fonctionnaliteModeService", fonctionnaliteModeService);

  fonctionnaliteModeService.$inject = [];

  function fonctionnaliteModeService() {

    this.getModes = function () {
      return [
        {
          value: FONCTIONNALITE_TYPE_MODE.UNAFFECTED,
          text: resources.Role_Index_ModalFonctionnalite_PrivilegeUnAffected,
          description: resources.Role_Index_ModalFonctionnalite_PrivilegeUnAffected_Description
        },
        {
          value: FONCTIONNALITE_TYPE_MODE.READ,
          text: resources.Role_Index_ModalFonctionnalite_PrivilegeRead,
          description: resources.Role_Index_ModalFonctionnalite_PrivilegeRead_Description
        },
        {
          value: FONCTIONNALITE_TYPE_MODE.WRITE,
          text: resources.Role_Index_ModalFonctionnalite_PrivilegeWrite,
          description: resources.Role_Index_ModalFonctionnalite_PrivilegeWrite_Description
        }];
    }

    this.getModeText = function (mode) {
      var matches = this.getModes().filter(function myfunction(modeItem) {
        return modeItem.value === mode;
      });
      if (matches.length > 0) {
        return matches[0].text;
      }
      return null;
    }
  };


})();