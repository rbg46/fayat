(function (angular) {
  'use strict';

  angular.module('Fred').controller('DatesCalendrierPaieController', DatesCalendrierPaieController);

  DatesCalendrierPaieController.$inject = ['$scope', '$http', 'Notification', 'DatesCalendrierPaieService', 'ngProgressFactory'];

  function DatesCalendrierPaieController($scope, $http, Notification, DatesCalendrierPaieService, ngProgressFactory) {
    // Instanciation Objet Ressources
    $scope.resources = resources;

    // Instanciation Objet ProgressBar
    $scope.progressBar = ngProgressFactory.createInstance();
    $scope.progressBar.setHeight("7px");
    $scope.progressBar.setColor("#FDD835");
    $scope.progressBar.start();

    // Instanciation Objet CalendrierAnnuel
    $scope.calendrierAnnuel = {};

    // Valeurs du formulaire
    $scope.anneeCourante = new Date().getFullYear();
    $scope.minAnnee = $scope.anneeCourante;
    $scope.radioTypeSaisie = 0;
    $scope.SocieteId = 0;
    $scope.societe = {};

    // Définition des options du ComboBox Kendo des sociétés
    $scope.optionsComboSociete = {
      dataTextField: "Libelle",
      dataValueField: "SocieteId"
    };

    $scope.withNotif = true;

    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////
    // Handler de click sur le bouton Valider
    $scope.handleClickValidate = function () {
      if ($scope.actionCheckFormBeforeAddOrUpdate()) {
        $scope.actionAddOrUpdateCalendriers();
      }
    };

    // Handler de changement d'année
    $scope.handleClickOnChangeDate = function (obj) {
      $scope.actionOnChangeDate(obj);
    };

    // Handler de click sur le bouton Loupe
    $scope.handleClickUpdateYear = function () {
      $scope.actionUpdateYear();
    };

    // Handler de click sur le bouton Annuler
    $scope.handleClickReset = function () {
      $scope.actionLoad();
    };

    //////////////////////////////////////////////////////////////////
    // Actions                                                      //
    //////////////////////////////////////////////////////////////////
    $scope.actionUpdateYear = function () {
      $scope.actionGetCalendriers();
    };

    $scope.actionOnChangeDate = function (obj) {
      if ($scope.radioTypeSaisie == 0) {
        obj.calendrierMensuel.JourFinPointages = obj.calendrierMensuel.JourSaisi.getDate();
      } else {
        obj.calendrierMensuel.JourTransfertPointages = obj.calendrierMensuel.JourSaisi.getDate();
      }

      obj.calendrierMensuel.kendoCalendarOptions = $scope.actionGetNewKendoCalendarOptions(obj.calendrierMensuel, false);
    };

    $scope.actionGetCalendriers = function () {
      //TODO: SHE récupérer l'identifiant de la société à passer en parmaètre(session?)
      DatesCalendrierPaieService.GetBySocieteAndYear($scope.SocieteId, $scope.anneeCourante).then(function (value) {
        $scope.calendrierAnnuel = value;

        for (var i = 0; i < $scope.calendrierAnnuel.length; i++) {
          calendrierMensuel = $scope.calendrierAnnuel[i];

          calendrierMensuel.OldJourFinPointages = calendrierMensuel.JourFinPointages;
          calendrierMensuel.OldJourTransfertPointages = calendrierMensuel.JourTransfertPointages;

          calendrierMensuel.kendoCalendarOptions = $scope.actionGetNewKendoCalendarOptions(calendrierMensuel, true);

          $scope.radioTypeSaisie = 0;
        }
      }, function (reason) {
        console.log(reason);
        if ($scope.withNotif) $scope.sendNotificationError(resources.Data_Load_Fail);
      });

      $scope.progressBar.complete();
    };

    $scope.actionAddOrUpdateCalendriers = function () {
      DatesCalendrierPaieService.AddOrUpdate($scope.calendrierAnnuel).then(function (value) {
        //OK, on avertit l'utilisateur
        if ($scope.withNotif) $scope.sendNotification(resources.Action_Update_Success);
        $scope.actionGetCalendriers();
      }, function (reason) {
        console.log(reason);
        if ($scope.withNotif) $scope.sendNotificationError(reason.Message);
      });

      $scope.progressBar.complete();
    };

    $scope.actionGetNewKendoCalendarOptions = function (calendrierMensuel, firstTime) {
      return new (function () {
        this.value = new Date(calendrierMensuel.Annee, calendrierMensuel.Mois - 1, 1);

        this.min = new Date(calendrierMensuel.Annee, calendrierMensuel.Mois - 1, 1);
        this.max = new Date(calendrierMensuel.Annee, calendrierMensuel.Mois, 0);

        this.oldJourFinPointages = calendrierMensuel.OldJourFinPointages;
        this.oldJourTransfertPointages = calendrierMensuel.OldJourTransfertPointages;
        this.jourFinPointages = calendrierMensuel.JourFinPointages;
        this.jourTransfertPointages = calendrierMensuel.JourTransfertPointages;

        this.month = {
          content: '# if (data.value ==' + this.oldJourFinPointages + '){#' +
          '<span class="text-primary-old">#= data.value #</span>' +
          '# } else if (data.value ==' + this.oldJourTransfertPointages + '){#' +
          '<span class="text-danger-old">#= data.value #</span>' +
          '# } else if (data.value ==' + this.jourFinPointages + '){#' +
          '<span class="text-primary-selected">#= data.value #</span>' +
          '# } else if (data.value ==' + this.jourTransfertPointages + '){#' +
          '<span class="text-danger-selected">#= data.value #</span>' +
          '# } else { #' +
          '#= data.value #' +
          '# } #'
        };
        this.footer = false;
      })();
    };

    // Test du formulaire
    $scope.actionCheckFormBeforeAddOrUpdate = function () {
      var bOK = true;

      // Test de présence d'un CI
      if ($scope.SocieteId == 0) {
        bOK = false;
        $scope.sendNotificationWarning(resources.SocieteObligatoire_lb);
      }

      return bOK;
    };

    //////////////////////////////////////////////////////////////////
    // Gestion diverses                                             //
    //////////////////////////////////////////////////////////////////
    // Fonction de récupération des Societe
    //dataSourceSociete = new kendo.data.DataSource({
    //  transport: {
    //    read: function (e) {
    //      $http.get("/api/Societe").success(function (data, status, headers, config) {
    //        e.success(data);
    //      })
    //      .error(function (data, status, headers, config) {
    //        e.error(data);
    //      });
    //    }
    //  }
    //});


    /*
     * Picklist 
     */

    // Fonction d'affichage de la PickList (V2) sur PickListCaller
    $scope.showPickList = function (val, source) {
      $scope.source = source;
      var basePrimeControllerUrl = '/api/' + val + '/SearchLight/?page={1}';
      $scope.apiController = val;
      //switch (val) {
      //  case "Personnel":
      //    basePrimeControllerUrl = String.format(basePrimeControllerUrl, 1);
      //    break;
      //}

      return basePrimeControllerUrl;
    };

    $scope.loadData = function (item) {
      $scope.SocieteId = item.SocieteId;
      $scope.societe = item;
      $scope.actionGetCalendriers();
    };


    $scope.handleDeletePickListItem = function () {
      $scope.societe = null;
    };

    // Gestion des notifications de succes
    $scope.sendNotification = function (message) {
      Notification({ message: message, title: 'Notification Fred' });
    };

    // Gestion des notifications d'erreur
    $scope.sendNotificationError = function (message) {
      Notification.error({ message: message, positionY: 'bottom', positionX: 'right' });
    };

    // Gestion des notifications warning
    $scope.sendNotificationWarning = function (message) {
      Notification.warning({ message: message, positionY: 'bottom', positionX: 'right' });
    };

    // Action Load
    $scope.actionLoad = function () {
      $scope.actionGetCalendriers();
    };

    // Chargement des données
    $scope.actionLoad();
  }


})(angular);