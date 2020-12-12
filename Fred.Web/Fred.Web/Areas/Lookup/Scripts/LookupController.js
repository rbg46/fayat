(function () {
  'use strict';

  angular.module('Fred').controller('LookupController', LookupController);

  LookupController.$inject = ['$q', '$timeout', '$scope', '$log', '$uibModal', 'confirmDialog'];

  function LookupController($q, $timeout, $scope, $log, $uibModal, confirmDialog) {


    var $ctrl = this;


    $ctrl.searchTitle = 'Recherchez quelque chose';
    $ctrl.Placeholder = 'Tapez quelque chose';
    $ctrl.CILookup5 = null;
    $ctrl.CILookup5 = { info: 'bidon', Code: 'alexandre' };
    $ctrl.resources = resources;



    $ctrl.initReferentialPicklistCaller1 = initReferentialPicklistCaller1;
    $ctrl.fnSelectReferentialPicklistCaller1 = fnSelectReferentialPicklistCaller1;
    $ctrl.fnDeleteReferentialPicklistCaller1 = fnDeleteReferentialPicklistCaller1;




    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////

    // #region name



    $ctrl.showPickList1 = showPickList1;
    $ctrl.loadData1 = loadData1;

    function showPickList1() {
      return '/api/Ressource/SearchLight?societeId=1';
    }

    function loadData1(item) {
      $log.log('lookup call with');
      $log.log(item);
    }


    function loadData(item) {
      var selectedRef;
      if (item === null) {
        $ctrl.selectedRef = $ctrl.selectedRef;
      }
      else {
        $ctrl.selectedRef = item;
      }
      $scope.Organisation = {};
      $scope.libelleOrganisation = $ctrl.selectedRef.Libelle;
      $scope.Organisation.OrganisationId = $ctrl.selectedRef.IdRef;
      $scope.Organisation.Libelle = $ctrl.selectedRef.Libelle;
    }


    function showPickList(val, refLigne) {
      return '/api/Ressource/SearchLight?societeId=1';
    }






    // #region ReferentialPicklistCaller1


    function initReferentialPicklistCaller1() {
      return '/api/Ressource/SearchLight?societeId=1';
    }

    function fnSelectReferentialPicklistCaller1() {
      $ctrl.CIReferentialPicklistCaller1JSON = JSON.stringify($ctrl.CIReferentialPicklistCaller1);
    }

    function fnDeleteReferentialPicklistCaller1() {
      $ctrl.CIReferentialPicklistCaller1 = null;
    }


    // #endregion


    // #region lookup1
    $ctrl.showPickList = showPickList;
    $ctrl.loadData = loadData;
    $ctrl.CILookup1 = null;
    $ctrl.initLookup1 = initLookup1;
    $ctrl.onSelectLookup1 = onSelectLookup1;
    $ctrl.onDeleteLookup1 = onDeleteLookup1;


    $ctrl.showPickList = function () {
      return '/api/Ressource/SearchLight?societeId=1';
    };

    $ctrl.loadData = function (item) {
      $log.log(item);
    };



    function initLookup1() {
      return '/api/Ressource/SearchLight?societeId=1';

    }

    function onSelectLookup1(lookupName, item) {

    }

    function onDeleteLookup1() {
      $ctrl.CILookup1 = null;
    }


    // #endregion


    // #region lookup2
    $ctrl.CILookup2 = null;
    $ctrl.initLookup2 = initLookup2;
    $ctrl.onSelectLookup2 = onSelectLookup2;
    $ctrl.onDeleteLookup2 = onDeleteLookup2;


    function initLookup2() {
      return '/api/Ressource/SearchLight?societeId=1';

    }

    function onSelectLookup2(lookupName, itemSelected) {

    }

    function onDeleteLookup2() {
      $ctrl.CILookup2 = null;
    }


    // #endregion

    // #region lookup3
    $ctrl.CILookup3 = null;
    $ctrl.initLookup3 = initLookup3;
    $ctrl.onSelectLookup3 = onSelectLookup3;
    $ctrl.onDeleteLookup3 = onDeleteLookup3;


    function initLookup3() {
      return '/api/Ressource/SearchLight?societeId=1';

    }

    function onSelectLookup3(lookupName, item) {
      $log.log('onSelectLookup3 call with');
      $log.log(lookupName);
      $log.log(item);
    }

    function onDeleteLookup3() {
      $ctrl.CILookup3 = null;
    }


    // #endregion



    $ctrl.openModal = function () {
      var modalInstance = $uibModal.open({
        component: 'lookupPopupComponent',
        resolve: {
          info: function () {
            return "";
          }

        }
      });

      return modalInstance;
    };



    $ctrl.confirmActionPromise = confirmActionPromise;
    function confirmActionPromise() {
      return confirmDialog.confirm($ctrl.resources, "Confirmer vous l'ouverture du lookup ?")
        .then(function () {
          console.log("Vous avez cliquez sur oui !");
        });
    }


    $ctrl.confirmAction = confirmAction;
    function confirmAction() {
      return false;
    }

  }
}());
