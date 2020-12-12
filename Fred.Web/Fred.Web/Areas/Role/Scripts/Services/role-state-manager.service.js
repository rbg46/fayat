(function () {
  "use strict";


  angular.module('Fred').service("RoleStateManagerService", RoleStateManagerService);

  RoleStateManagerService.$inject = ['$uibModal', '$timeout', 'Notification', 'RoleService', 'thresholdValidator'];

  function RoleStateManagerService($uibModal, $timeout, Notification, RoleService, thresholdValidator) {
    return {

      setState: function (newStateName, stateModel) {
       

        if (stateModel == null) {
          stateModel = {};
          stateModel.isBusy = false;
          stateModel.tileCreateManuallySelected = false;
          stateModel.tileCopySelected = false;
          stateModel.tileCompanySelected = false;
          stateModel.showTileCompany = false;
          stateModel.tileExportFonctionnalitesSelected = false;
          stateModel.tileExportSeuilsSelected = false;
          stateModel.showTileExportFonctionnalites = false;
          stateModel.showTileExportSeuils = false;
          stateModel.showTileValidate = false;
        }
        if (stateModel.isBusy === true && newStateName !== 'notBusy') {
          return stateModel;
        }
        if (newStateName === 'busy') {
          stateModel.isBusy = true;
        }
        if (newStateName === 'notBusy') {
          stateModel.isBusy = false;
        }
        if (newStateName === 'initial') {
          stateModel.showDashBoard = false;
          stateModel.showList = false;
          stateModel.showMessageForSelection = true;
          stateModel.showMessageNoRole = false;
        }
        if (newStateName === 'showDashBord') {
          stateModel.showDashBoard = true;
          stateModel.showList = false;
          stateModel.showMessageForSelection = false;
          stateModel.showMessageNoRole = true;
        }
        if (newStateName === 'showList') {
          stateModel.showDashBoard = false;
          stateModel.showList = true;
          stateModel.showMessageForSelection = false;
          stateModel.showMessageNoRole = false;
        }
        if (newStateName === 'selectTileCreateManuallyState') {
          stateModel.tileCreateManuallySelected = true;
          stateModel.tileCopySelected = false;
          stateModel.tileCompanySelected = false;
          stateModel.showTileCompany = false;
          stateModel.tileExportFonctionnalitesSelected = false;
          stateModel.tileExportSeuilsSelected = false;
          stateModel.showTileExportFonctionnalites = false;
          stateModel.showTileExportSeuils = false;
          stateModel.showTileValidate = true;
        }      
        if (newStateName === 'selectTileCopyState') {
          stateModel.tileCreateManuallySelected = false;
          stateModel.tileCopySelected = true;
          stateModel.tileCompanySelected = false;
          stateModel.showTileCompany = true;
          stateModel.tileExportFonctionnalitesSelected = false;
          stateModel.tileExportSeuilsSelected = false;         
          stateModel.showTileExportFonctionnalites = false;
          stateModel.showTileExportSeuils = false;
          stateModel.showTileValidate = false;
        }
        if (newStateName === 'selectTileCompanyState') {
          stateModel.tileExportFonctionnalitesSelected = false;
          stateModel.tileExportSeuilsSelected = false;
          stateModel.tileCompanySelected = true;
          stateModel.showTileExportFonctionnalites = false;
          stateModel.showTileExportSeuils = false;
          stateModel.showTileValidate = false;
        }
        if (newStateName === 'companySelectedOnLookupState') {
          stateModel.tileCreateManuallySelected = false;
          stateModel.tileCopySelected = true;
          stateModel.tileCompanySelected = true;
          stateModel.showTileCompany = true;
          stateModel.tileExportFonctionnalitesSelected = false;
          stateModel.tileExportSeuilsSelected = false;
          stateModel.showTileExportFonctionnalites = true;
          stateModel.showTileExportSeuils = true;
          stateModel.showTileValidate = true;
        }
       
        if (newStateName === 'toogleTileExportFonctionnalitesState') {
          stateModel.tileExportFonctionnalitesSelected = !stateModel.tileExportFonctionnalitesSelected;
        }      
        if (newStateName === 'toogleTileExportSeuilsState') {
          stateModel.tileExportSeuilsSelected = !stateModel.tileExportSeuilsSelected;
        }       
        if (newStateName === 'validateState') {
          stateModel.showDashBoard = false;
          stateModel.showList = true;
          stateModel.showMessageForSelection = false;
          stateModel.showMessageNoRole = false;
          stateModel.tileCreateManuallySelected = false;
          stateModel.tileCopySelected = false;
          stateModel.tileCompanySelected = false;
          stateModel.showTileCompany = false;
          stateModel.tileExportFonctionnalitesSelected = false;
          stateModel.tileExportSeuilsSelected = false;
          stateModel.showTileExportFonctionnalites = false;
          stateModel.showTileExportSeuils = false;
          stateModel.showTileValidate = false;
        }     
        return stateModel;
      }

    };
  };


})();