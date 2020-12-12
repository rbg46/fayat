(function () {
  'use strict';


  angular.module('Fred').component('moduleArboComponent', {
    templateUrl: '/Areas/Module/Scripts/components/module-arbo.tpl.html',
    bindings: {
      resources: '<'
    },
    controller: 'moduleArboComponentController'
  });

  angular.module('Fred').controller('moduleArboComponentController', moduleArboComponentController);

  moduleArboComponentController.$inject = ['$scope', 'ProgressBar', 'Notify', 'moduleArboService', 'moduleArboStoreService', 'fredSubscribeService', '$q', 'moduleArboDataService','$document'];

  function moduleArboComponentController($scope, ProgressBar, Notify, moduleArboService, moduleArboStoreService, fredSubscribeService, $q, moduleArboDataService, $document) {

    var $ctrl = this;
    var bodyRef = angular.element($document[0].body);
    $ctrl.resources = resources;
    $ctrl.isVisible = false;
    //var page = 0;
    var PAGE_SIZE = 25;
    var isLoading = false;
    var isSaving = false;
    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////
    $ctrl.$onInit = function () {
      fredSubscribeService.subscribe({ eventName: 'module-open-arbo', callback: openPanel });

      FredToolBox.bindScrollEnd("#arboScroll", actionLoadMore);
    };


    $ctrl.$onDestroy = function () {
        fredSubscribeService.unsubscribe({ eventName: 'module-open-arbo', callback: openPanel });
    };


    function actionLoadMore() {
      if (isLoading) {
        return;
      }
      isLoading = true;
      ProgressBar.start();
      var page = moduleArboStoreService.getNextPage();
      var societeId = moduleArboStoreService.get('societeId');
      moduleArboService.getOrganisationTreeForSocieteId(page, PAGE_SIZE, societeId).then(function (response) {
        moduleArboStoreService.addOrganisations(response.data);
        isLoading = false;
      }).finally(function () {
        ProgressBar.complete();
      });
    }


    function openPanel(info) {
      $ctrl.isVisible = true;
      bodyRef.addClass('ovh');
      if (moduleArboStoreService.contextHasChanged(info)) {
        moduleArboStoreService.changeContext(info);      
        $ctrl.pole = moduleArboStoreService.get('pole');
        $ctrl.module = moduleArboStoreService.get('moduleSelected');
        $ctrl.fonctionnalite = moduleArboStoreService.get('fonctionnaliteSelected');
        ProgressBar.start();
        moduleArboStoreService.requestArboStarted();
        moduleArboDataService.getArbo($ctrl.module, $ctrl.fonctionnalite)
             .then(getArboOnSuccess)
             .catch(getArboOnError)
             .finally(getArboFinally);
      }
    }


    function getArboOnSuccess(responses) {
      $ctrl.pole = moduleArboStoreService.get('pole');
    }


    function getArboOnError(error) {
      Notify.error($ctrl.resources.Global_Notification_Error);
    }

    function getArboFinally() {
      ProgressBar.complete();
      moduleArboStoreService.requestArboFinish();
    }


    /////////////////////////////////////////////////////////////////////////////////////////
    // SAUVEGARDE                                                                          //
    /////////////////////////////////////////////////////////////////////////////////////////

      $ctrl.save = function () {
          if (isSaving) {
              return;
          }

          isSaving = true;

          var organisationIdsOfSocietesToEnable = moduleArboStoreService.getSocietesEnabledAfterInit();
          var organisationIdsOfSocietesToDisabled = moduleArboStoreService.getSocietesDisabledAfterInit();

          if (organisationIdsOfSocietesToEnable.length > 0 || organisationIdsOfSocietesToDisabled.length > 0) {
              ProgressBar.start();
              var module = moduleArboStoreService.get('moduleSelected');
              var fonctionnalite = moduleArboStoreService.get('fonctionnaliteSelected');
              if (module) {
                  moduleArboService.enableOrDisableModuleByOrganisationIdsOfSocietesAndModuleId(module.ModuleId, organisationIdsOfSocietesToEnable, organisationIdsOfSocietesToDisabled)
                      .then(onSaveSuccess)
                      .catch(onSaveFail)
                      .finally(onSaveFinally);
              } else {
                  moduleArboService.enableOrDisableFonctionnaliteByOrganisationIdsOfSocietesAndFonctionnaliteId(fonctionnalite.FonctionnaliteId, organisationIdsOfSocietesToEnable, organisationIdsOfSocietesToDisabled)
                      .then(onSaveSuccess)
                      .catch(onSaveFail)
                      .finally(onSaveFinally);
              }

          } else {
              isSaving = false;
          }


      };

    function onSaveSuccess(response) {
      moduleArboStoreService.arboSaveSuccess(response.data);
      Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
      fredSubscribeService.raiseEvent('module-reload-infos-inactives', null);     
      $ctrl.isVisible = false;
    }

    function onSaveFail(error) {
      Notify.error($ctrl.resources.Global_Notification_Error);
    }

    function onSaveFinally() {
      isSaving = false;
      ProgressBar.complete();
    }

    /////////////////////////////////////////////////////////////////////////////////////////
    // CANCEL                                                                              //
    /////////////////////////////////////////////////////////////////////////////////////////

      $ctrl.cancel = function () {
          moduleArboStoreService.cancel();
          bodyRef.removeClass('ovh');
          $ctrl.isVisible = false;
      };


      $ctrl.close = function () {
          bodyRef.removeClass('ovh');
          $ctrl.isVisible = false;
      };

  }
})();