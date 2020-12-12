(function () {
  'use strict';

  angular
    .module('Fred')
    .component('createBudgetRessourceComponent', {
      templateUrl: '/Areas/Budget/Scripts/Ressources/Ressource/CreateBudgetRessourceTemplate.html',
      bindings: {
        resources: '<',
        resolve: '<',
        close: '&',
        dismiss: '&'
      },
      controller: 'createBudgetRessourceComponentController'
    });

  angular.module('Fred').controller('createBudgetRessourceComponentController', createBudgetRessourceComponentController);

  createBudgetRessourceComponentController.$inject = ['$scope', 'Notify', 'BudgetDataService', 'TachePriceManagerService', 'ModelStateErrorManager',
                                                      'RessourceManagerService', 'RessourceUpdaterService', 'UniteManagerService',
                                                      'CiManagerService', 'ParametrageReferentielEtenduService'];

  function createBudgetRessourceComponentController ($scope,
                                                    Notify,
                                                    BudgetDataService,
                                                    TachePriceManagerService,
                                                    ModelStateErrorManager,
                                                    RessourceManagerService,
                                                    RessourceUpdaterService,
                                                    UniteManagerService,
                                                    CiManagerService,
                                                    ParametrageReferentielEtenduService) {

    var $ctrl = this;

    $ctrl.isBusy = false;
    $ctrl.uniteSelected = null;

    //////////////////////////////////////////////////////////////////
    // Déclaration des fonctions publiques                          //
    //////////////////////////////////////////////////////////////////

    $ctrl.handleSave = handleSave;
    $ctrl.handleCancel = handleCancel;
    $ctrl.handleUpdate = handleUpdate;

    $ctrl.handleSelectUnite = handleSelectUnite;
    $ctrl.handleDeleteUnite = handleDeleteUnite;

    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////

    $ctrl.$onInit = function () {
      $ctrl.errorMessage = null;
      $ctrl.resources = $ctrl.resolve.resources;
      $ctrl.chapitre = $ctrl.resolve.chapitre;
      $ctrl.sousChapitre = $ctrl.resolve.sousChapitre;
      $ctrl.ressourceParent = $ctrl.resolve.ressourceParent;
      $ctrl.isEditMode = $ctrl.resolve.isEditMode;
      if ($ctrl.isEditMode) {
        //pour la mise a jour d'une resource, on copie la ressource selectionnée
        //ce qui ne modifie pas la ressource selectionnée. Et si la mise a jour reussie
        //nous remplacerons cette ressource dans la liste des ressources
        $ctrl.ressource = {};
        angular.copy($ctrl.resolve.ressource, $ctrl.ressource);
        init();
      } else {
        $ctrl.ressource = RessourceManagerService.createNewRessourceWithRessourceParent($ctrl.ressourceParent, $ctrl.resolve.deviseSelected);
        init();
      }

    };

    function init() {

      var param = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDeviseByRessource($ctrl.ressource, $ctrl.resolve.deviseSelected);
      if (param) {
        $ctrl.unite = param.Unite;
        $ctrl.montant = param.Montant;
        $ctrl.symbole = param.Devise.Symbole;
      } else {
        param = ParametrageReferentielEtenduService.createParametrageReferentielEtenduForDevise(null, $ctrl.ressource, $ctrl.resolve.deviseSelected);
        $ctrl.unite = param.Unite;
        $ctrl.montant = param.Montant;
        $ctrl.symbole = param.Devise.Symbole;
      }

    }


    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////

    function handleSave() {
      save();
    }

    function handleCancel() {
      $ctrl.dismiss({ $value: 'cancel' });
    }

    function handleUpdate() {
      update();
    }

    function handleSelectUnite(unite) {
      UniteManagerService.changeUniteForAllParametrageReferentielEtendus($ctrl.ressource, unite);
    }


    function handleDeleteUnite() {
      $ctrl.unite = null;
    }


    //////////////////////////////////////////////////////////////////
    // Actions - Creation d'une nouvelle ressource                  //
    //////////////////////////////////////////////////////////////////

    function save() {
      $ctrl.isBusy = true;
      var param = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDeviseByRessource($ctrl.ressource, $ctrl.resolve.deviseSelected);
      param.Montant = $ctrl.montant;
      var ciId = CiManagerService.getCi().CiId;
      BudgetDataService
        .CreateRessource($ctrl.ressource, ciId)
        .then(saveSuccess)
        .catch(saveError)
        .finally(saveEnd);
    }

    function saveSuccess(response) {
      $ctrl.close({ $value: response.data });
    }

    function saveError(error) {
      manageErrors(error);
    }

    function saveEnd() {
      //todo signal graphique
      $ctrl.isBusy = false;
    }

    //////////////////////////////////////////////////////////////////
    // Actions - Update d'une ressource                             //
    //////////////////////////////////////////////////////////////////

    function update() {
      $ctrl.isBusy = true;
      var param = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDeviseByRessource($ctrl.ressource, $ctrl.resolve.deviseSelected);
      param.Montant = $ctrl.montant;
      var ciId = CiManagerService.getCi().CiId;
      BudgetDataService
        .UpdateRessource($ctrl.ressource, ciId)
        .then(updateSuccess)
        .catch(updateError)
        .finally(updateEnd);
    }

    function updateSuccess(response) {
      RessourceUpdaterService.modifyRessourceInTasksView(response.data);
      $ctrl.close({ $value: response.data });
    }

    function updateError(error) {
      manageErrors(error);
    }

    function updateEnd() {
      //todo signal graphique
      $ctrl.isBusy = false;
    }

    //////////////////////////////////////////////////////////////////
    // Actions -Gestion des erreurs                                 //
    //////////////////////////////////////////////////////////////////

    function manageErrors(error) {
      var validationError = ModelStateErrorManager.getErrors(error);
      if (error.status === 409) {
        $ctrl.errorMessage = $ctrl.resources.Global_Control_Code_Exist_Erreur;
      }
      else if (validationError) {
        $ctrl.errorMessage = validationError;
      }
      else if (error.data.Message) {
        $ctrl.errorMessage = error.data.Message;
      }
      else {
        $ctrl.errorMessage = $ctrl.resources.CreateBudgetRessourceCpt_saveError;
      }
    }

  }


})();