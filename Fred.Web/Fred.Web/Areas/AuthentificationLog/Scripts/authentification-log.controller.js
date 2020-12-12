
(function (angular) {
  'use strict';

  angular.module('Fred').controller('authentificationLogController', authentificationLogController);

  authentificationLogController.$inject = ['Notify', 'authentificationLogService', 'ProgressBar', '$uibModal'];

  function authentificationLogController(Notify, authentificationLogService, ProgressBar, $uibModal) {

    var $ctrl = this;
    //////////////////////////////////////////////////////////////////
    // Déclaration des variables privées                          //
    //////////////////////////////////////////////////////////////////
    var lastLoginText = "";
    var lastSkip = 0;
    var take = 25;
    var canLoadMoreData = true;
    var logToDelete = null;

    //////////////////////////////////////////////////////////////////
    // Déclaration des variables publiques                          //
    //////////////////////////////////////////////////////////////////
    $ctrl.authentificationLogs = [];
    $ctrl.loginText = "";
    $ctrl.resources = resources;
    $ctrl.isBusy = true;
    //////////////////////////////////////////////////////////////////
    // Déclaration des fonctions publiques                          //
    //////////////////////////////////////////////////////////////////
    $ctrl.getByLogin = getByLogin;
    $ctrl.loginTextChanged = loginTextChanged;

    $ctrl.selectAll = selectAll;
    $ctrl.deleteAll = deleteAll;
    $ctrl.deleteLog = deleteLog;
    $ctrl.select = select;
    $ctrl.showDetail = showDetail;

    init();

    return $ctrl;

    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////


    /**
     * Initialisation du controller.
     * 
     */
    function init() {
      ProgressBar.start();
      authentificationLogService.getByLogin($ctrl.loginText, lastSkip, take)
        .then(getByLoginSucess)
        .catch(getByLoginError)
        .finally(getByLoginFinally);

      FredToolBox.bindScrollEnd('#authentificationLogList', getByLogin);
    }


    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////
    // #region GET
    function getByLogin() {
      if (lastLoginText !== $ctrl.loginText) {
        canLoadMoreData = true;
      }
      if (!$ctrl.isBusy && canLoadMoreData) {
        $ctrl.isBusy = true;
        if (lastLoginText !== $ctrl.loginText) {
          lastLoginText = $ctrl.loginText;
          lastSkip = 0;
          $ctrl.authentificationLogs = [];
          canLoadMoreData = true;
        } else {
          lastSkip += take;
        }

        authentificationLogService.getByLogin($ctrl.loginText, lastSkip, take)
                                  .then(getByLoginSucess)
                                  .catch(getByLoginError)
                                  .finally(getByLoginFinally);
      }

    }

    //////////////////////////////////////////////////////////////////
    //  Actions    - GET                                            //
    //////////////////////////////////////////////////////////////////
    // #region Actions    - GET 
    function getByLoginSucess(response) {
      for (var i = 0; i < response.data.length; i++) {
        var log = response.data[i];
        log.isSelected = false;
        $ctrl.authentificationLogs.push(response.data[i]);
      }
      if (response.data.length < take) {
        canLoadMoreData = false;
      }

    }

    function getByLoginError() {
      Notify.error($ctrl.resources.Global_Notification_Chargement_Error);
    }

    function getByLoginFinally() {
      $ctrl.isBusy = false;
      ProgressBar.complete();
    }
    // #endregion




    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////
    function loginTextChanged() {
      if (lastLoginText !== $ctrl.loginText) {
        canLoadMoreData = true;
      }
    }


    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////
    // #region DELETION
    function deleteAll() {
      if (!$ctrl.isBusy) {
        ProgressBar.start();
        var listToDelete = getAllLogsSelected($ctrl.authentificationLogs);

        authentificationLogService.deleteAuthentificationLogs(listToDelete)
                                  .then(deleteAuthentificationLogsSucess)
                                  .catch(deleteAuthentificationLogsError)
                                  .finally(deleteAuthentificationLogsFinally);
      }

    }

    //////////////////////////////////////////////////////////////////
    // Actions    - DELETE   ALL                                    //
    //////////////////////////////////////////////////////////////////
    // #region Actions    - DELETE   ALL  
    function deleteAuthentificationLogsSucess(response) {
      var selectedLogs = getAllLogsSelected($ctrl.authentificationLogs);
      angular.forEach(selectedLogs, function (key) {
        deleteLogOnScreen(key, $ctrl.authentificationLogs);
      });
      Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
    }

    function deleteAuthentificationLogsError() {
      Notify.error($ctrl.resources.Global_Notification_Error);
    }

    function deleteAuthentificationLogsFinally() {
      $ctrl.isBusy = false;
      ProgressBar.complete();
      reloadIfNecessary($ctrl.authentificationLogs);
    }
    // #endregion


    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////
    function deleteLog(itemToDelete) {
      if (!$ctrl.isBusy) {
        ProgressBar.start();
        logToDelete = itemToDelete;
        var listToDelete = [itemToDelete.AuthentificationLogId];
        authentificationLogService.deleteAuthentificationLogs(listToDelete)
                                  .then(deleteAuthentificationLogSucess)
                                  .catch(deleteAuthentificationLogError)
                                  .finally(deleteAuthentificationLogFinally);
      }

    }


    //////////////////////////////////////////////////////////////////
    // Actions    - DELETE   ONE                                    //
    //////////////////////////////////////////////////////////////////
    // #region  Actions    - DELETE   ONE
    function deleteAuthentificationLogSucess(response) {
      deleteLogOnScreen(logToDelete.AuthentificationLogId, $ctrl.authentificationLogs);
      Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
    }

    function deleteAuthentificationLogError() {
      logToDelete = null;
      Notify.error($ctrl.resources.Global_Notification_Error);
    }

    function deleteAuthentificationLogFinally() {
      $ctrl.isBusy = false;
      ProgressBar.complete();
      reloadIfNecessary($ctrl.authentificationLogs);
    }
    // #endregion
    // #endregion




    // #endregion

    // #region SELECTION
    function selectAll() {
      for (var i = 0; i < $ctrl.authentificationLogs.length; i++) {
        var log = $ctrl.authentificationLogs[i];
        log.isSelected = true;
      }
    }

    function select(log) {
      log.isSelected = !log.isSelected;

    }


    // #endregion


    // #endregion


    //////////////////////////////////////////////////////////////////
    // COMMON                                                       //
    //////////////////////////////////////////////////////////////////

    function getAllLogsSelected(authentificationLogs) {
      var listOfSelected = [];
      for (var i = 0; i < authentificationLogs.length; i++) {
        var log = authentificationLogs[i];
        if (log.isSelected === true) {
          listOfSelected.push(log.AuthentificationLogId);
        }
      }
      return listOfSelected;
    }

    function deleteLogOnScreen(authentificationLogId, authentificationLogs) {
      var index = authentificationLogs.map(function (e) {
        return e.AuthentificationLogId;
      }).indexOf(authentificationLogId);

      if (index !== -1) {
        authentificationLogs.splice(index, 1);
      }
    }


    function reloadIfNecessary(authentificationLogs) {
      if (authentificationLogs.length < take) {
        lastSkip = authentificationLogs.length;
        if (!$ctrl.isBusy && canLoadMoreData) {
          $ctrl.isBusy = true;
          authentificationLogService.getByLogin($ctrl.loginText, lastSkip, take)
                            .then(getByLoginSucess)
                            .catch(getByLoginError)
                            .finally(getByLoginFinally);
        }
      }
    }


    function showDetail(log) {

      $uibModal.open({
        templateUrl: '/Areas/AuthentificationLog/Scripts/Dialogs/authentification-log-detail.tpl.html',
        backdrop: 'static',
        controller: 'authentificationLogdetailController',
        size: 'lg',
        resolve: {
          resources: function () {
            return $ctrl.resources;
          },
          id: function () {
            return log.AuthentificationLogId;
          }
        }
      });


    }
  }




}(angular));



