(function (angular) {
  'use strict';

  angular.module('Fred').controller('EtablissementComptableController', EtablissementComptableController);

  EtablissementComptableController.$inject = ['$scope', 'Notify', 'EtablissementComptableService', 'PieceJointeValidatorService', 'ProgressBar', 'confirmDialog', 'UserService', 'Notification'];

  function EtablissementComptableController($scope, Notify, EtablissementComptableService, PieceJointeValidatorService, ProgressBar, confirmDialog, UserService, Notification) {


    /* -------------------------------------------------------------------------------------------------------------
     *                                            INIT
     * -------------------------------------------------------------------------------------------------------------
     */

    // Instanciation Objet permissionKeys
    $scope.permissionKeys = PERMISSION_KEYS;

    // Instanciation Objet Ressources
      $scope.resources = resources;

    // Initialisation de la variable societeId
    $scope.societeId = undefined;

    // Instanciation de la recherche
    $scope.recherche = "";

    // Instanciation la section 'Zones libres modèles de commande' ZLMC
    $scope.ZLMC = {
      isOpen: false,
      acceptFiles: ".doc,.docx",
      CGATypes: ['CGAFourniture', 'CGALocation', 'CGAPrestation']
    };

    // Attribut d'affichage de la liste
    $scope.checkDisplayOptions = "close-right-panel";
    $scope.checkFormatOptions = "small";

    // RefPicklist
    $scope.refDictionnary = {};
    $scope.refname = null;
    $scope.checkDisplayReferential = "closeReferentials";

    UserService.getCurrentUser().then(function(user) {
      $scope.currentUser = user.Personnel;
      $scope.userOrganizationId = $scope.currentUser.Societe.Organisation.OrganisationId;
      if ($scope.currentUser.Societe.Groupe.Code.trim() === "GFES") {
          $scope.isCurrentUserFes = true;
      }
    });

    // Selection dans la Picklist société
    $scope.loadData = function () {
      $scope.societeId = $scope.societe.SocieteId;
      $scope.societeLibelle = $scope.societe.CodeLibelle;
      // Chargement des données
      $scope.actionInitSearch();
      $scope.actionLoad(true);
      $scope.actionNewEtablissementComptable();
    };

    /* -------------------------------------------------------------------------------------------------------------
     *                                            HANDLERS
     * -------------------------------------------------------------------------------------------------------------
     */

    // Handler de sélection d'une ligne de le repeater Angular
    $scope.handleSelect = function (item) {
      $scope.etablissementComptable = angular.copy(item);
      /*Open modal*/
      $scope.checkDisplayOptions = "open";
      $scope.changeFormModel = false;
    };

    // Handler de click sur le bouton Ajouter
    $scope.handleClickCreateNew = function () {
      if ($scope.societeId !== undefined) {
        $scope.formEtablissementComptable.$setPristine();
        $scope.formEtablissementComptable.Code.$setValidity('exist', true);
        $scope.actionNewEtablissementComptable();
        $scope.checkDisplayOptions = "open";
        $scope.changeFormModel = false;
      }
    };

    // Handler de click sur le bouton Enregistrer
    $scope.handleClickAddOrUpdate = function () {
      $scope.actionAddOrUpdate(false, true);
    };

    // Handler de click sur le bouton Enregistrer et Nouveau
    $scope.handleClickAddOrUpdateAndNew = function () {
      $scope.actionAddOrUpdate(true, true);
    };

    // Handler de click sur le bouton Cancel
    $scope.handleClickCancel = function () {
      $scope.actionCancel();
    };

    // Handler de frappe clavier dans le champs recherche
    $scope.handleSearch = function (recherche) {
      $scope.recherche = recherche;
      $scope.actionLoad();
    };

    // Handler de frappe clavier dans le champs code
    $scope.handleChangeCode = function () {
      if (!$scope.formEtablissementComptable.Code.$error.pattern) {
        var idCourant;

        if ($scope.etablissementComptable.EtablissementComptableId !== undefined)
          idCourant = $scope.etablissementComptable.EtablissementComptableId;
        else
          idCourant = 0;
        if ($scope.societe !== null)
          $scope.existCodeEtablissementComptable(idCourant, $scope.etablissementComptable.Code, $scope.societe.SocieteId);
      }
    };

    // Handle 'Commande' checkbox change
    $scope.handleChangeModuleCommande = function () {
      const isChecked = $scope.etablissementComptable.ModuleCommandeEnabled;
      if (!isChecked) {
        $scope.ZLMC.isOpen = false;
      }
    };

    // Handle 'CGA' files change
    $scope.handleCGAChange = async function (type, event) {
      const file = event && event.target && event.target.files && event.target.files[0];
      /*Validation du format*/
      if (file) {
        const isFormatValid = PieceJointeValidatorService.isValidFileFormat(file, $scope.ZLMC.acceptFiles);
        if (!isFormatValid)
          return;
      }

      /*Conversion en base 64*/
      const base64File = file != null ? await $scope.convertFileToBase64(file) : null;
      $scope.$apply(() => {
        $scope.etablissementComptable = {
          ...$scope.etablissementComptable,
          [type]: base64File,
          [type+'FileName']: file != null ? file.name : null,
        };
      });
    };

    /* -------------------------------------------------------------------------------------------------------------
     *                                            ACTIONS
     * -------------------------------------------------------------------------------------------------------------
     */

    // Download CGA file
      $scope.downloadCGAFile = function (CGA) {
        const CGAPath = $scope.etablissementComptable[CGA+'FilePath'];
        const CGAName = $scope.etablissementComptable[CGA+'FileName'];
        if (CGAPath != null) {
          EtablissementComptableService.DownloadCGA(CGAName);
        } else {
          Notification({
            title: $scope.resources.Global_Notification_Titre,
            message: $scope.resources.Global_PieceJointe_Error_FichierNonTeleverse.replace('{0}', CGAName)
          });
        }
    };

    // Convert file to base 64
    $scope.convertFileToBase64 = async function (file) {
      return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
      });
    };

    // Action click sur les boutons Enregistrer
    $scope.actionAddOrUpdate = function (newItem, withNotif) {
      if ($scope.formEtablissementComptable.$invalid)
        return;
      if ($scope.etablissementComptable.EtablissementComptableId === 0)
        $scope.actionCreate(newItem, withNotif);
      else
        $scope.actionUpdate(newItem, withNotif);
    };

    // Action Create
    $scope.actionCreate = function (newItem, withNotif) {
      EtablissementComptableService.Create($scope.etablissementComptable).then(function () {
        $scope.actionLoad(false);
        if (newItem) {
          $scope.handleClickCreateNew();
        }
        else {
          $scope.actionCancel();
        }
        ProgressBar.complete();
        if (withNotif) Notify.message(resources.Global_Notification_Enregistrement_Success);
      }, function (reason) {
        console.log(reason);
        ProgressBar.complete();
        if (withNotif) Notify.error(resources.Global_Notification_Error);
      });
    };

    // Action Update
    $scope.actionUpdate = function (newItem, withNotif) {
      EtablissementComptableService.Update($scope.etablissementComptable).then(function () {
        $scope.actionLoad(false);
        if (newItem) {
          $scope.handleClickCreateNew();
        }
        else {
          $scope.actionCancel();
        }
        ProgressBar.complete();
        if (withNotif) Notify.message(resources.Global_Notification_Enregistrement_Success);
      }, function (reason) {
        console.log(reason);
        ProgressBar.complete();
        if (withNotif) Notify.error(resources.Global_Notification_Error);
      });
    };

    // Action Cancel
    $scope.actionCancel = function () {
      $scope.checkDisplayOptions = "close-right-panel";
      $scope.formEtablissementComptable.$setPristine();
      $scope.formEtablissementComptable.Code.$setValidity('exist', true);
    };

    // Action Delete
    $scope.handleClickDelete = function (etablissementComptable) {
      $scope.checkDisplayOptions = "close-right-panel";
      confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
        EtablissementComptableService.Delete(etablissementComptable).then(function () {
          $scope.actionLoad(false);
          ProgressBar.complete();
          $scope.actionCancel(true);
        }, function (reason) {
          console.log(reason);
          Notify.error(resources.Global_Notification_Suppression_Error);
        });
      });
    };

    // Action initalisation d'une nouveeau Etablissement Comptable
    $scope.actionNewEtablissementComptable = function () {
        EtablissementComptableService.New($scope.societeId).then(function (value) {
            if (value.Facturation == null) {
                value.Facturation = resources.EtablissementComptable_Zones_FacturationPlaceholder;
            }
            if (value.Paiement == null) {
                value.Paiement = resources.EtablissementComptable_Zones_PaiementPlaceholder;
            }
            $scope.etablissementComptable = value;
      }, function (reason) {
        console.log(reason);
      });
    };

    // Action Load
    $scope.actionLoad = function (withNotif) {
      EtablissementComptableService.Search($scope.filters, $scope.societeId, $scope.recherche).then(function (values) {
        /*Default init*/
        values = values.map(v => {
          if (v.Facturation == null) {
              v.Facturation = resources.EtablissementComptable_Zones_FacturationPlaceholder;
          }
          if (v.Paiement == null) {
              v.Paiement = resources.EtablissementComptable_Zones_PaiementPlaceholder;
          }
          return v;
        });
        $scope.items = values;
        if (values && values.length === 0) {
          Notify.error(resources.Global_Notification_AucuneDonnees);
        }

      }, function (reason) {
        console.log(reason);
        if (withNotif) Notify.error(resources.Global_Notification_Error);
      });
    };

    // Action d'initialisation de la recherche muli-critère des Etablissement Comptable
    $scope.actionInitSearch = function () {
      $scope.filters = { Code: true, Libelle: true };
    };

    // Action de test d'existence de EtablissementComptable
    $scope.existCodeEtablissementComptable = function (idCourant, codeEtablissementComptable, societeId) {
      EtablissementComptableService.Exists(idCourant, codeEtablissementComptable, societeId).then(function (value) {
        if (value) {
          $scope.formEtablissementComptable.Code.$setValidity('exist', false);
        } else {
          $scope.formEtablissementComptable.Code.$setValidity('exist', true);
        }
      }, function (reason) {
        console.log(reason);
      });
    };

  }


})(angular);
