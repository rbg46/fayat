
(function () {
  'use strict';

  angular.module('Fred').component('listeFactureComponent', {
    templateUrl: '/Areas/RapprochementFacture/Scripts/ListeFacture/ListeFactureTemplate.html',
    bindings: {
      resources: '<'
    },
    controller: 'listeFactureComponentController'
  });

  angular.module('Fred').controller('listeFactureComponentController', listeFactureComponentController);

  listeFactureComponentController.$inject = ['$scope'];

  function listeFactureComponentController ($scope) {

    var $ctrl = this;
    var parser = null;


    //function getNew() {
    //  var dataMock = {
    //    Niveau: 4,
    //    Code: "code",
    //    Libelle: "Libelle",
    //    QuantiteBase: 1,
    //    Unite: "kg",
    //    Quantite: 10,
    //    PrixUnitaireQB: null,
    //    PrixTotalQB: null,
    //    PrixTotalT4: null,
    //    Ressources: [
    //      {
    //        Chapitre: "505",
    //        SousChapitre: "505-00",
    //        Ressource: { Code: "505-00-00", Libelle: "Mallettes transport" },
    //        Unite: "piece",
    //        QuantiteBase: null,
    //        PuHt: null,
    //        Montant: null,
    //        Formule: null,
    //        Quantite: null,
    //        MontantTotal: null
    //      },
    //      {
    //        Chapitre: "666",
    //        SousChapitre: "666-00",
    //        Ressource: { Code: "666-00-00", Libelle: "Bombes" },
    //        Unite: "kg",
    //        QuantiteBase: null,
    //        PuHt: null,
    //        Montant: null,
    //        Formule: null,
    //        Quantite: null,
    //        MontantTotal: null
    //      }
    //    ]

    //  };
    //  return dataMock;
    //}


    //////////////////////////////////////////////////////////////////
    // Déclaration des fonctions publiques                          //
    //////////////////////////////////////////////////////////////
    //$ctrl.handleTacheQuantiteBaseChange = handleTacheQuantiteBaseChange;
    //$ctrl.handleTacheQuantiteChange = handleTacheQuantiteChange;
    //$ctrl.handleRessourceQuantiteBaseChange = handleRessourceQuantiteBaseChange;
    //$ctrl.handleRessourcePuHtChange = handleRessourcePuHtChange;
    //$ctrl.handleRessourceFormuleChange = handleRessourceFormuleChange;
    $ctrl.GetStatusStyle = GetStatusStyle;

    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////

    $ctrl.$onInit = function () {

      init();
      parser = new exprEval.Parser();


      $scope.$on('budgetCrtl.displayList', function (event, newTaskSelected) {
        init();
      });


      $scope.$on('budgetCrtl.SaveTask', function (event, newTaskSelected) {

      });

      $scope.$on('budgetCrtl.addRessourceToTask', function (event, newRessource) {
        $ctrl.taskSelected.Ressources.push(newRessource);
      });

    };

    function init() {

      $ctrl.liste =



[{ "NumeroFMFI": 93903, "NumeroFacture": "RB9016000652", "DateFacture": "15/05/2009", "Journ": "NDF", "Fournisseur": "Office Dépôt", "MontantTTC": 155.06, "Devise": "EUR", "DateCompatable": "30/05/2009", "Folio": null, "MontantHT": 124.048, "MontantTVA": 31.012, "SocieteID": 11, "EtablissementID": 6, "CiLibelle": "Matmut Atlantique", "CICode": "MULTI_CI", "Statut": 0 },
{ "NumeroFMFI": 85392, "NumeroFacture": "RB9016000652", "DateFacture": "17/10/2009", "Journ": "SST", "Fournisseur": "Office Dépôt", "MontantTTC": 281.06, "Devise": "USD", "DateCompatable": "01/11/2009", "Folio": null, "MontantHT": 224.848, "MontantTVA": 56.212, "SocieteID": 9, "EtablissementID": 6, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 1 },
{ "NumeroFMFI": 86436, "NumeroFacture": "RB9016000652", "DateFacture": "21/03/2010", "Journ": "ACH", "Fournisseur": "Office Dépôt", "MontantTTC": 365.25, "Devise": "LGB", "DateCompatable": "05/04/2010", "Folio": null, "MontantHT": 292.2, "MontantTVA": 73.05, "SocieteID": 10, "EtablissementID": 15, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 2 },
{ "NumeroFMFI": 82208, "NumeroFacture": "RB9016000652", "DateFacture": "23/08/2010", "Journ": "NDF", "Fournisseur": "Office Dépôt", "MontantTTC": 394.95, "Devise": "EUR", "DateCompatable": "07/09/2010", "Folio": null, "MontantHT": 315.96, "MontantTVA": 78.99, "SocieteID": 3, "EtablissementID": 5, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 3 },
{ "NumeroFMFI": 68695, "NumeroFacture": "RB9016000652", "DateFacture": "25/01/2011", "Journ": "SST", "Fournisseur": "Office Dépôt", "MontantTTC": 520.95, "Devise": "EUR", "DateCompatable": "09/02/2011", "Folio": null, "MontantHT": 416.76, "MontantTVA": 104.19, "SocieteID": 14, "EtablissementID": 5, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 4 },
{ "NumeroFMFI": 67182, "NumeroFacture": "RB9016000652", "DateFacture": "29/06/2011", "Journ": "ACH", "Fournisseur": "Office Dépôt", "MontantTTC": 365.25, "Devise": "EUR", "DateCompatable": "14/07/2011", "Folio": null, "MontantHT": 292.2, "MontantTVA": 73.05, "SocieteID": 11, "EtablissementID": 13, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 0 },
{ "NumeroFMFI": 99808, "NumeroFacture": "RB9016000652", "DateFacture": "01/12/2011", "Journ": "NDF", "Fournisseur": "Office Dépôt", "MontantTTC": 634.84, "Devise": "EUR", "DateCompatable": "16/12/2011", "Folio": null, "MontantHT": 507.872, "MontantTVA": 126.968, "SocieteID": 1, "EtablissementID": 7, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 1 },
{ "NumeroFMFI": 91490, "NumeroFacture": "RB9016000652", "DateFacture": "04/05/2012", "Journ": "SST", "Fournisseur": "Office Dépôt", "MontantTTC": 760.84, "Devise": "USD", "DateCompatable": "19/05/2012", "Folio": null, "MontantHT": 608.672, "MontantTVA": 152.168, "SocieteID": 9, "EtablissementID": 10, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 2 },
{ "NumeroFMFI": 62234, "NumeroFacture": "RB9016000652", "DateFacture": "06/10/2012", "Journ": "ACH", "Fournisseur": "Office Dépôt", "MontantTTC": 365.25, "Devise": "LGB", "DateCompatable": "21/10/2012", "Folio": null, "MontantHT": 292.2, "MontantTVA": 73.05, "SocieteID": 6, "EtablissementID": 13, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 3 },
{ "NumeroFMFI": 56758, "NumeroFacture": "RB9016000652", "DateFacture": "10/03/2013", "Journ": "NDF", "Fournisseur": "Office Dépôt", "MontantTTC": 874.73, "Devise": "EUR", "DateCompatable": "25/03/2013", "Folio": null, "MontantHT": 699.784, "MontantTVA": 174.946, "SocieteID": 4, "EtablissementID": 10, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 4 },
{ "NumeroFMFI": 68390, "NumeroFacture": "RB9016000652", "DateFacture": "12/08/2013", "Journ": "SST", "Fournisseur": "Office Dépôt", "MontantTTC": 1000.73, "Devise": "EUR", "DateCompatable": "27/08/2013", "Folio": null, "MontantHT": 800.584, "MontantTVA": 200.146, "SocieteID": 8, "EtablissementID": 8, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 0 },
{ "NumeroFMFI": 60033, "NumeroFacture": "RB9016000652", "DateFacture": "14/01/2014", "Journ": "ACH", "Fournisseur": "Office Dépôt", "MontantTTC": 365.25, "Devise": "EUR", "DateCompatable": "29/01/2014", "Folio": null, "MontantHT": 292.2, "MontantTVA": 73.05, "SocieteID": 2, "EtablissementID": 10, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 1 },
{ "NumeroFMFI": 79337, "NumeroFacture": "RB9016000652", "DateFacture": "18/06/2014", "Journ": "NDF", "Fournisseur": "Office Dépôt", "MontantTTC": 1114.62, "Devise": "EUR", "DateCompatable": "03/07/2014", "Folio": null, "MontantHT": 891.696, "MontantTVA": 222.924, "SocieteID": 4, "EtablissementID": 15, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 2 },
{ "NumeroFMFI": 64525, "NumeroFacture": "RB9016000652", "DateFacture": "20/11/2014", "Journ": "SST", "Fournisseur": "Office Dépôt", "MontantTTC": 1240.62, "Devise": "USD", "DateCompatable": "05/12/2014", "Folio": null, "MontantHT": 992.496, "MontantTVA": 248.124, "SocieteID": 6, "EtablissementID": 1, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 3 },
{ "NumeroFMFI": 71320, "NumeroFacture": "RB9016000652", "DateFacture": "24/04/2015", "Journ": "ACH", "Fournisseur": "Office Dépôt", "MontantTTC": 365.25, "Devise": "LGB", "DateCompatable": "09/05/2015", "Folio": null, "MontantHT": 292.2, "MontantTVA": 73.05, "SocieteID": 14, "EtablissementID": 13, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 4 },
{ "NumeroFMFI": 84831, "NumeroFacture": "RB9016000652", "DateFacture": "26/09/2015", "Journ": "NDF", "Fournisseur": "Office Dépôt", "MontantTTC": 1354.51, "Devise": "EUR", "DateCompatable": "11/10/2015", "Folio": null, "MontantHT": 1083.608, "MontantTVA": 270.902, "SocieteID": 1, "EtablissementID": 10, "CiLibelle": "Matmut Atlantique", "CICode": "34559", "Statut": 0 },
{ "NumeroFMFI": 62400, "NumeroFacture": "RB9016000963", "DateFacture": "28/02/2016", "Journ": "SST", "Fournisseur": "Point P", "MontantTTC": 1480.51, "Devise": "EUR", "DateCompatable": "14/03/2016", "Folio": null, "MontantHT": 1184.408, "MontantTVA": 296.102, "SocieteID": 9, "EtablissementID": 15, "CiLibelle": "Grand Paris - EOLE", "CICode": "MULTI_CI", "Statut": 1 },
{ "NumeroFMFI": 73753, "NumeroFacture": "RB9016000963", "DateFacture": "01/08/2016", "Journ": "ACH", "Fournisseur": "Point P", "MontantTTC": 1606.51, "Devise": "EUR", "DateCompatable": "16/08/2016", "Folio": null, "MontantHT": 1285.208, "MontantTVA": 321.302, "SocieteID": 9, "EtablissementID": 7, "CiLibelle": "Grand Paris - EOLE", "CICode": "78159", "Statut": 2 },
{ "NumeroFMFI": 63914, "NumeroFacture": "RB9016000963", "DateFacture": "03/01/2017", "Journ": "NDF", "Fournisseur": "Point P", "MontantTTC": 365.25, "Devise": "EUR", "DateCompatable": "18/01/2017", "Folio": null, "MontantHT": 292.2, "MontantTVA": 73.05, "SocieteID": 4, "EtablissementID": 15, "CiLibelle": "Grand Paris - EOLE", "CICode": "78159", "Statut": 3 },
{ "NumeroFMFI": 93835, "NumeroFacture": "RB9016000963", "DateFacture": "07/06/2017", "Journ": "SST", "Fournisseur": "Point P", "MontantTTC": 1720.4, "Devise": "USD", "DateCompatable": "22/06/2017", "Folio": null, "MontantHT": 1376.32, "MontantTVA": 344.08, "SocieteID": 7, "EtablissementID": 7, "CiLibelle": "Grand Paris - EOLE", "CICode": "78159", "Statut": 4 },
{ "NumeroFMFI": 82024, "NumeroFacture": "RB9016000963", "DateFacture": "09/11/2017", "Journ": "ACH", "Fournisseur": "Point P", "MontantTTC": 1846.4, "Devise": "LGB", "DateCompatable": "24/11/2017", "Folio": null, "MontantHT": 1477.12, "MontantTVA": 369.28, "SocieteID": 14, "EtablissementID": 12, "CiLibelle": "Grand Paris - EOLE", "CICode": "78159", "Statut": 0 },
{ "NumeroFMFI": 89731, "NumeroFacture": "RB9016000963", "DateFacture": "13/04/2018", "Journ": "NDF", "Fournisseur": "Point P", "MontantTTC": 1972.4, "Devise": "EUR", "DateCompatable": "28/04/2018", "Folio": null, "MontantHT": 1577.92, "MontantTVA": 394.48, "SocieteID": 5, "EtablissementID": 15, "CiLibelle": "Grand Paris - EOLE", "CICode": "MULTI_CI", "Statut": 1 },
{ "NumeroFMFI": 63310, "NumeroFacture": "RB9016000963", "DateFacture": "15/09/2018", "Journ": "SST", "Fournisseur": "Point P", "MontantTTC": 365.25, "Devise": "EUR", "DateCompatable": "30/09/2018", "Folio": null, "MontantHT": 292.2, "MontantTVA": 73.05, "SocieteID": 15, "EtablissementID": 5, "CiLibelle": "Grand Paris - EOLE", "CICode": "78159", "Statut": 2 },
{ "NumeroFMFI": 72005, "NumeroFacture": "RB9016000963", "DateFacture": "17/02/2019", "Journ": "ACH", "Fournisseur": "Point P", "MontantTTC": 2086.29, "Devise": "EUR", "DateCompatable": "04/03/2019", "Folio": null, "MontantHT": 1669.032, "MontantTVA": 417.258, "SocieteID": 8, "EtablissementID": 8, "CiLibelle": "Grand Paris - EOLE", "CICode": "78159", "Statut": 3 },
{ "NumeroFMFI": 89682, "NumeroFacture": "RB9016000963", "DateFacture": "22/07/2019", "Journ": "NDF", "Fournisseur": "Point P", "MontantTTC": 2212.29, "Devise": "EUR", "DateCompatable": "06/08/2019", "Folio": null, "MontantHT": 1769.832, "MontantTVA": 442.458, "SocieteID": 12, "EtablissementID": 2, "CiLibelle": "Grand Paris - EOLE", "CICode": "78159", "Statut": 4 },
{ "NumeroFMFI": 68584, "NumeroFacture": "RB9016000963", "DateFacture": "24/12/2019", "Journ": "SST", "Fournisseur": "Point P", "MontantTTC": 2338.29, "Devise": "USD", "DateCompatable": "08/01/2020", "Folio": null, "MontantHT": 1870.632, "MontantTVA": 467.658, "SocieteID": 13, "EtablissementID": 15, "CiLibelle": "Grand Paris - EOLE", "CICode": "78159", "Statut": 0 },
{ "NumeroFMFI": 64636, "NumeroFacture": "RB9016000963", "DateFacture": "27/05/2020", "Journ": "ACH", "Fournisseur": "Point P", "MontantTTC": 365.25, "Devise": "LGB", "DateCompatable": "11/06/2020", "Folio": null, "MontantHT": 292.2, "MontantTVA": 73.05, "SocieteID": 1, "EtablissementID": 10, "CiLibelle": "Grand Paris - EOLE", "CICode": "MULTI_CI", "Statut": 1 },
{ "NumeroFMFI": 68045, "NumeroFacture": "RB9016000963", "DateFacture": "29/10/2020", "Journ": "NDF", "Fournisseur": "Point P", "MontantTTC": 2452.18, "Devise": "EUR", "DateCompatable": "13/11/2020", "Folio": null, "MontantHT": 1961.744, "MontantTVA": 490.436, "SocieteID": 5, "EtablissementID": 8, "CiLibelle": "Grand Paris - EOLE", "CICode": "78159", "Statut": 2 },
{ "NumeroFMFI": 99510, "NumeroFacture": "RB9016000963", "DateFacture": "02/04/2021", "Journ": "SST", "Fournisseur": "Point P", "MontantTTC": 2578.18, "Devise": "EUR", "DateCompatable": "17/04/2021", "Folio": null, "MontantHT": 2062.544, "MontantTVA": 515.636, "SocieteID": 11, "EtablissementID": 15, "CiLibelle": "Grand Paris - EOLE", "CICode": "78159", "Statut": 3 },
{ "NumeroFMFI": 65832, "NumeroFacture": "RB9016000963", "DateFacture": "04/09/2021", "Journ": "ACH", "Fournisseur": "Point P", "MontantTTC": 2704.18, "Devise": "EUR", "DateCompatable": "19/09/2021", "Folio": null, "MontantHT": 2163.344, "MontantTVA": 540.836, "SocieteID": 4, "EtablissementID": 11, "CiLibelle": "Grand Paris - EOLE", "CICode": "78159", "Statut": 4 }]


    }





    /* Définition du style de la colonne  Statut
     * 
     */
    function GetStatusStyle(status) {
      var rtn;
      switch (status) {
        case 0:
          rtn = "status-rapproche-todo";
          break;
        case 1:
          rtn = "status-encours";
          break;
        case 2:
          day = "status-rapproche-ok";
          break;
        default:
          rtn = "status-rapproche-todo";
      }
      return rtn;

    }

    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////
    function handleTacheQuantiteBaseChange() {
      ChangeTacheQuantite();
      calculPrix();
    }

    function handleTacheQuantiteChange(ressource) {
      ChangeTacheQuantite();
      calculPrix();
    }

    function handleRessourceQuantiteBaseChange(ressource) {
      ressource.Formule = null;
      ressource.hasFormuleError = false;
      calculTotauxRessource(ressource);
      calculPrix();
    }

    function handleRessourcePuHtChange(ressource) {
      calculMontantBase(ressource);
      calculMontantTotal(ressource);
      calculPrix();
    }

    function handleRessourceFormuleChange(ressource) {
      if (ressource.Formule === '') {
        ressource.hasFormuleError = false;
        calculTotauxRessource(ressource);
        return;
      }
      if (canCalculQuantiteBaseCalculWithFormule(ressource)) {
        ressource.QuantiteBase = calculQuantiteBaseAvecFormule(ressource);
        calculTotauxRessource(ressource);
      } else {
        resetRessource(ressource);
      }
      calculPrix();
    }

    function handleDeleteRessource(ressource) {
      deleteRessource(ressource);
      calculPrix();
    }


    //////////////////////////////////////////////////////////////////
    // Actions                                                      //
    //////////////////////////////////////////////////////////////////

    function initQuantiteDeBaseSiNonDefini() {
      if ($ctrl.taskSelected.QuantiteBase === null || $ctrl.taskSelected.QuantiteBase === undefined) {
        $ctrl.taskSelected.QuantiteBase = 1;
      }
    }

    function initQuantiteARealiseeSiNonDefini() {
      if ($ctrl.taskSelected.Quantite === null || $ctrl.taskSelected.Quantite === undefined) {
        $ctrl.taskSelected.Quantite = 1;
      }
    }

    //////////////////////////////////////////////////////////////////
    // Actions - Calcul sur les ressources                          //
    //////////////////////////////////////////////////////////////////

    function calculTotauxRessource(ressource) {
      if (!ressourceIsInError($ctrl.taskSelected, ressource)) {
        calculMontantBase(ressource);
        calculQuantite(ressource);
        calculMontantTotal(ressource);
      } else {
        resetRessource(ressource);
      }
    }

    function canCalculQuantiteBaseCalculWithFormule(ressource) {
      var hasError = true;
      try {
        calculQuantiteBaseAvecFormule(ressource);
        ressource.hasFormuleError = false;
        hasError = false;
      } catch (e) {
        hasError = true;
        ressource.hasFormuleError = true;
      }
      return !hasError;
    }

    function calculQuantiteBaseAvecFormule(ressource) {
      var formule = ressource.Formule.replace('X', 'x');
      var expr = parser.parse(formule);
      var quantiteBase = expr.evaluate({ x: $ctrl.taskSelected.QuantiteBase });
      return quantiteBase;
    }


    function calculMontantBase(ressource) {
      ressource.Montant = ressource.QuantiteBase * ressource.PuHt;
    }


    function calculQuantite(ressource) {
      ressource.Quantite = $ctrl.taskSelected.Quantite / $ctrl.taskSelected.QuantiteBase * ressource.QuantiteBase;
    }

    function calculMontantTotal(ressource) {
      ressource.MontantTotal = ressource.Quantite * ressource.PuHt;
    }

    //////////////////////////////////////////////////////////////////
    // Actions - Calcul des totaux                                  //
    //////////////////////////////////////////////////////////////////

    function ChangeTacheQuantite() {
      if (taskIsInError($ctrl.taskSelected)) {
        resetAll();
      } else {
        reCalculAll();
      }
    }

    function resetAll() {
      for (var i = 0; i < $ctrl.taskSelected.Ressources.length; i++) {
        var ressource = $ctrl.taskSelected.Ressources[i];
        resetRessource(ressource);
      }

    }

    function resetRessource(ressource) {
      ressource.Montant = null;
      ressource.MontantTotal = null;
      ressource.Quantite = null;
    }

    function reCalculAll() {
      for (var i = 0; i < $ctrl.taskSelected.Ressources.length; i++) {
        var ressource = $ctrl.taskSelected.Ressources[i];
        if (ressource.Formule === null) {
          calculQuantite(ressource);
          calculMontantTotal(ressource);
        } else {
          handleRessourceFormuleChange(ressource);
        }
      }
    }

    function taskIsInError(task) {
      if (task.QuantiteBase === null || task.PuHt === null) {
        return true;
      }
      return false;
    }



    function calculPrix() {
      if (taskIsInError($ctrl.taskSelected)) {
        resetTotaux($ctrl.taskSelected);
      } else {
        calculPrixTotalQB($ctrl.taskSelected);
        calculPrixUnitaireQB($ctrl.taskSelected);//faire d'abord le prix total Qb obligatoire         
        calculPrixTotalT4($ctrl.taskSelected);
      }
    }

    function resetTotaux(task) {
      task.PrixUnitaireQB = null;
      task.PrixTotalQB = null;
      task.PrixTotalT4 = null;

    }

    function calculPrixUnitaireQB(task) {
      task.PrixUnitaireQB = task.PrixTotalQB / task.QuantiteBase;

    }

    function calculPrixTotalQB(task) {
      task.PrixTotalQB = 0;
      for (var i = 0; i < task.Ressources.length; i++) {
        var ressource = task.Ressources[i];
        if (!ressourceIsInError(task, ressource)) {
          task.PrixTotalQB += ressource.Montant;
        }
      }
    }

    function calculPrixTotalT4(task) {
      task.PrixTotalT4 = 0;
      for (var i = 0; i < task.Ressources.length; i++) {
        var ressource = task.Ressources[i];
        if (!ressourceIsInError(task, ressource)) {
          task.PrixTotalT4 += ressource.MontantTotal;
        }
      }
    }

    function ressourceIsInError(task, ressource) {
      if (taskIsInError(task)) {
        return true;
      }
      if (ressource.QuantiteBase === null && ressource.Formule === null) {
        return true;
      }
      if (ressource.hasFormuleError && ressource.Quantite === null) {
        return true;
      }
      return false;
    }

    function deleteRessource(ressource) {
      var index = $ctrl.taskSelected.Ressources.indexOf(ressource);
      $ctrl.taskSelected.Ressources.splice(index, 1);
    }

  }

})();