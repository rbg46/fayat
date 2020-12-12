(function (angular) {
  'use strict';
  angular.module('Fred').controller('ParamTarifsReferentielsCtrl', ParamTarifsReferentielsCtrl);

  ParamTarifsReferentielsCtrl.$inject = ['$scope', '$http', '$filter', 'Notify', 'ParamTarifsReferentielsService', 'ProgressBar', 'ReferentielEtenduFilterService', 'ParamTarifsReferentielsHelperService', '$q'];

  function ParamTarifsReferentielsCtrl($scope, $http, $filter, Notify, ParamTarifsReferentielsService, ProgressBar, ReferentielEtenduFilterService, ParamTarifsReferentielsHelperService, $q) {

    /* -------------------------------------------------------------------------------------------------------------
     *                                            INIT
     * -------------------------------------------------------------------------------------------------------------
     */

    // Instanciation Objet Ressources
    $scope.resources = resources;    
    $scope.loadingBusy = false;
    $scope.responseMessage = "";
    $scope.organisationId;
    $scope.devise = 0;
    $scope.ListDevises = [];
    $scope.ListParamRefEtenduModified = [];
    $scope.search = '';
    $scope.selected = {};
    $scope.chapitresFilter = ReferentielEtenduFilterService.chapitresFilter;
    $scope.sousChapitresFilter = ReferentielEtenduFilterService.sousChapitresFilter;
    $scope.ressourcesFilter = ReferentielEtenduFilterService.ressourcesFilter;

    /*
     * Selectionne une ressource.
     * Utile pour la selection de la ligne et donc au passage du mode edit - display
     */
    $scope.select = function (ressource) {
      $scope.selected = ressource;
    };

    /*
     * Action effectuée lors de la selection d'une nouvelle unité
     */
    $scope.handleSelectUnite = function (item, ressource) {
      var lastParametrageReferentielEtendu = getLastParametrageReferentielEtendus(ressource);
      $scope.inlineUpdateParametrage(lastParametrageReferentielEtendu);

      //Rajout de l'Id de l'unité pour la sauvegarde back
      lastParametrageReferentielEtendu.UniteId = lastParametrageReferentielEtendu.Unite.UniteId;
    };

    /*
    * Action effectuée lors de la suppression d'une unité :
    * Rajout dans liste des modifs puis suppression unite du ParametrageReferentielEtendu.
    */
    $scope.handleDeleteUnite = function (ressource) {
      var lastParametrageReferentielEtendu = getLastParametrageReferentielEtendus(ressource);
      $scope.inlineUpdateParametrage(lastParametrageReferentielEtendu);

      // Suppression de l'unité sur le ParametrageReferentielEtendu
      lastParametrageReferentielEtendu.Unite = null;
      lastParametrageReferentielEtendu.UniteId = null;

    };

    /*
     * Retourne le dernier ParametrageReferentielEtendu de la ressource.
     */
    function getLastParametrageReferentielEtendus(ressource) {
      var indexOfLastParametrageReferentielEtendus = ressource.ReferentielEtendus[0].ParametrageReferentielEtendus.length - 1;
      var lastParametrageReferentielEtendus = ressource.ReferentielEtendus[0].ParametrageReferentielEtendus[indexOfLastParametrageReferentielEtendus];
      return lastParametrageReferentielEtendus;
    }

    function loadDevises() {
      return ParamTarifsReferentielsService.getListDevises($scope.organisationId).then(function (response) {
        if (response.status === 500) {
          Notify.error(resources.Global_Notification_Error);
        }
        else {
          $scope.ListDevises = response.data;
          if ($scope.ListDevises.length > 0) {
            for (var i = 0; i < $scope.ListDevises.length; i++) {
              if ($scope.ListDevises[i].Reference)
                $scope.devise = $scope.ListDevises[i];
            }
          }
          else {
            Notify.error(resources.Global_Notification_Error);
          }
        }
      });
    }

    $scope.loadData = function (needDevises) {
      $scope.organisationId = $scope.organisation.OrganisationId;
      ProgressBar.start();
      $scope.responseMessage = "";
      $scope.loadingBusy = true;
      $scope.headersTitles = [];
      $scope.tarifsReferentiels = [];

      if (needDevises) {
        loadDevises().then(function () {
          getTarifsReferentiels();
        });
      }
      else getTarifsReferentiels();
    };

    function getTarifsReferentiels() {
      ParamTarifsReferentielsService.getTarifsReferentiels($scope.organisationId, $scope.devise.DeviseId, $scope.search).then(function (t) {
        if (t.data.Referentiels.length > 0) {
          $scope.headersTitles = t.data.HeaderColumns;
          $scope.tarifsReferentiels = t.data.Referentiels;

          ParamTarifsReferentielsHelperService.setSynthese($scope.tarifsReferentiels);
          
          $scope.loadingBusy = false;
          ProgressBar.complete();
        }
        else {
          $scope.loadingBusy = false;
          $scope.responseMessage = resources.ParamTarifsReferentiels_Controler_AucunReferentiel;
          ProgressBar.complete();
        }
      });
    }

    $scope.inlineUpdateParametrage = function (paramRefEtendu) {


      var found = $filter('filter')($scope.ListParamRefEtenduModified, {
        OrganisationId: paramRefEtendu.OrganisationId,
        DeviseId: paramRefEtendu.DeviseId,
        ReferentielEtenduId: paramRefEtendu.ReferentielEtenduId
      }, true)[0];
      if (!found) {
        $scope.ListParamRefEtenduModified.push(paramRefEtendu);
      }

    };

    $scope.changeDevise = function (devise) {
      $scope.devise = devise;
      $scope.loadData(false);
    };

    $scope.cancel = function () {
      $scope.loadData(false);
    };


    $scope.save = function () {

      //Permet de savoir si un des parametres n'est pas valide, cad, qu 'il manque soit le prix soit l'unité.
      var hasMissingInfoOnParam = ParamTarifsReferentielsHelperService.hasMissingInfoOnParam($scope.ListParamRefEtenduModified);
      if (hasMissingInfoOnParam) {
        Notify.error("Enregistrement impossible : toutes les valeurs d'unités associés aux prix doivent être renseignées.");
        return;
      }

      var listToDelete = ParamTarifsReferentielsHelperService.getListParamToDelete($scope.ListParamRefEtenduModified);
      var listToUptade = ParamTarifsReferentielsHelperService.getListParamToUpdate($scope.ListParamRefEtenduModified);

      //check si il y a des action a effectué
      if (listToDelete.length === 0 && listToUptade.length === 0) {
        return;
      }

      //Permet de savoir si l'on peux sauvegarder.
      var canSave = ParamTarifsReferentielsHelperService.canSave($scope.ListParamRefEtenduModified);
      if (canSave) {

        var allDeletePromise = deleteAll(listToDelete);
        var allUpdatePromise = updateOrCreate(listToUptade);


        $q.all([allDeletePromise, allUpdatePromise])
          .then(function () {
            Notify.message(resources.Global_Notification_Enregistrement_Success);
          })
          .catch(function (error) {
            Notify.error(error);
          })
          .finally(function () {
            $scope.ListParamRefEtenduModified = [];
            ParamTarifsReferentielsHelperService.setWarnings($scope.tarifsReferentiels);
          });


      }
    };

    /*
     * Supression des ligne sans prix
     */
    function deleteAll(listToDelete) {
      var listToDeletePromises = [];

      for (var i = 0; i < listToDelete.length; i++) {
        var paramToDelete = listToDelete[i];
        var deletePromise = ParamTarifsReferentielsService.delete(paramToDelete)
                                                           .then(function (response) {
                                                             response.row.ParametrageReferentielEtenduId = 0;
                                                             response.row.Unite = null;
                                                             ParamTarifsReferentielsHelperService.setSynthese($scope.tarifsReferentiels);
                                                           });
        listToDeletePromises.push(deletePromise);
      }

      var allDeletePromise = $q.all(listToDeletePromises);

      return allDeletePromise;
    }



    /*
     * Creation ou mise a jour des parametres
     */
    function updateOrCreate(listToUptade) {
      var listToUpdatePromises = [];

      for (var i = 0; i < listToUptade.length; i++) {
        var paramToUpdate = listToUptade[i];
        var updatePromise = ParamTarifsReferentielsService.save(paramToUpdate)
                                                          .then(function (response) {
                                                            response.row.ParametrageReferentielEtenduId = response.data.ParametrageReferentielEtenduId;
                                                            response.row.DateCreation = response.data.DateCreation;
                                                            response.row.AuteurCreationId = response.data.AuteurCreationId;
                                                            ParamTarifsReferentielsHelperService.setSynthese($scope.tarifsReferentiels);
                                                          });

        listToUpdatePromises.push(updatePromise);
      }


      var allUpdatePromise = $q.all(listToUpdatePromises);

      return allUpdatePromise;
    }



    $scope.handleSearchTextChanged = function (text) {
      $scope.search = text;
      $scope.loadData(false);
    };

    $scope.closeall2 = function () {
      $('.panel-collapse')
        .collapse('hide');
      $('.accordion-toggle').removeClass('collapsed').addClass('collapsed');
      $('.accordion-toggle').removeClass('collapsed').addClass('collapsed');
    };

    $scope.openall2 = function () {
      $('.panel-collapse:not(".in")')
        .collapse('show');
      $('.accordion-toggle').removeClass('collapsed');
      $('.accordion-toggle').removeClass('collapsed');
    };

    //// Fonction d'extraction au format excel
    $scope.extractExcel = function () {
      $http({
        url: "/api/ParamReferentielEtendu/GenerateExcel?orgaId=" + $scope.organisationId + "&deviseId=" + $scope.devise.DeviseId + "&filter=" + $scope.search,
        method: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json'
      }).success(function (response) {
        window.location.href = '/api/ParamReferentielEtendu/ExtractExcel/' + response.id;
      });
    };
  }



})(angular);