(function (angular) {
    'use strict';

    angular.module('Fred').controller('BudgetControleBudgetaireController', BudgetControleBudgetaireController);

    BudgetControleBudgetaireController.$inject = ['$scope', '$filter', '$timeout', 'BudgetService', 'ProgressBar', 'Notify', '$uibModal', 'ControleBudgetaireCalculService', 'CIService', '$q', 'fredDialog', '$window', 'favorisService', 'BudgetDateService', 'TachesService', 'ReferentielFixeService'];

    function BudgetControleBudgetaireController($scope, $filter, $timeout, BudgetService, ProgressBar, Notify, $uibModal, ControleBudgetaireCalculService, CIService, $q, fredDialog, $window, favorisService, BudgetDateService, TachesService, ReferentielFixeService) {
        var $ctrl = this;

        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.DisplayControleBudgetaireValidationVerrouillagePrecedentsDialogValidated, demandeDeValidationConfirmee);
        };

        $ctrl.resources = resources;
        //Events déclaration
        $ctrl.displayBanner = false;
        $ctrl.onCiSelected = onCiSelected;
        $ctrl.periode = null;
        $ctrl.axeTree = null;
        $ctrl.isAvancementValide = false;
        $ctrl.isPeriodeCloturee = false;
        $ctrl.filterActiveOnAxe = false;

        //Représente toutes les colonnes pouvant être affichées ou cachées
        $ctrl.colonnesAffichable = ['BudgetQte',
            'BudgetPu',
            'DadQte',
            'DadPu',
            'DepQte',
            'DepPu',
            'EcartQte',
            'EcartPu',
            'RadQte',
            'RadPu',
            'AjustementCommentaire',
            'PrevisionnelFinAffaire',
            'PfaMoisPrecedent',
            'ProjectionLineaireDepense'];

        $ctrl.colonnesProperties = {
            BudgetQte: 'Valeurs.QuantiteBudget.Quantite',
            BudgetPu: 'Valeurs.PuBudget',
            BudgetMontant: 'Valeurs.MontantBudget',
            DadPourcent: 'Valeurs.PourcentageDad',
            DadQte: 'Valeurs.QuantiteDad',
            DadPu: 'Valeurs.PuBudget',
            DadMontant: 'Valeurs.MontantDad',
            DepPourcentage: 'Valeurs.PourcentageDepense',
            DepQte: 'Valeurs.QuantiteDepense.Quantite',
            DepPu: 'Valeurs.PuDepense',
            DepMontant: 'Valeurs.MontantDepense',
            EcartQte: 'Valeurs.QuantiteEcart',
            EcartPu: 'Valeurs.PuEcart',
            EcartMontant: 'Valeurs.MontantEcart',
            RadQte: 'Valeurs.QuantiteRad',
            RadPu: 'Valeurs.PuBudget',
            RadMontant: 'Valeurs.MontantRadTheorique',
            AjustementMontant: 'Valeurs.MontantAjustement',
            AjustementCommentaire: 'Valeurs.CommentaireAjustement',
            PrevisionnelFinAffaire: 'Valeurs.Pfa',
            PfaMoisPrecedent: 'Valeurs.PfaMoisPrecedent',
            ProjectionLineaireDepense: 'Valeurs.ProjectionLineaireDepenses'
        };

        $ctrl.colonnesOrder = {
            BudgetQte: null,
            BudgetPu: null,
            BudgetMontant: null,
            DadPourcent: null,
            DadQte: null,
            DadPu: null,
            DadMontant: null,
            DepPourcentage: null,
            DepQte: null,
            DepPu: null,
            DepMontant: null,
            EcartQte: null,
            EcartPu: null,
            EcartMontant: null,
            RadQte: null,
            RadPu: null,
            RadMontant: null,
            AjustementMontant: null,
            AjustementCommentaire: null,
            PrevisionnelFinAffaire: null,
            PfaMoisPrecedent: null,
            ProjectionLineaireDepense: null
        };

        $ctrl.showColumn = {
            Budget: true,
            Dad: true,
            Dep: true,
            Ecart: true,
            Rad: true,
            Ajustement: true
        };
        //Les colonnes a afficher
        $ctrl.colonnesAffichees = angular.copy($ctrl.colonnesAffichable);

        ///Constantes ces valeurs ne devraient jamais être modifiées par le code
        $ctrl.AxePrincipalTaches = "TacheRessource";
        $ctrl.AxePrincipalRessources = "RessourceTache";
        $ctrl.axeAnalytiqueAffichable =
            [
                { Name: "T1", Type: "T", Order: 0 },
                { Name: "T2", Type: "T", Order: 1 },
                { Name: "T3", Type: "T", Order: 2 },
                { Name: "Chapitre", Type: "R", Order: 0 },
                { Name: "SousChapitre", Type: "R", Order: 1 },
                { Name: "Ressource", Type: "R", Order: 2 }
            ];

        $ctrl.axesAnalytiquesAffiches = angular.copy($ctrl.axeAnalytiqueAffichable);

        $ctrl.filters = [
            { id: 'Ressource', value: { _label: "Ress. :", label: "Ress. :", active: false } },
            { id: 'Tache', value: { _label: "Tâche :", label: "Tâche :", active: false } }
        ];

        //Dans ce tableau seront répertoriées les choix de l'utilisateur sur l'affichage des colonnes
        //Dans le bandeau latéral. e.g si l'utilisateur choisi dans le bandeau d'afficher la colonne BudgetQte
        //alors cette valeur sera ajoutée à ce tableau
        $ctrl.colonnesParametreesAffichees;
        $ctrl.axesAnalytiquesParametresAffiches = angular.copy($ctrl.axeAnalytiqueAffichable);

        //Ces valeurs viennent du CSS
        $ctrl.enteteColonneBudgetLargeur = 350;
        $ctrl.enteteColonneDadLargeur = 410;
        $ctrl.enteteColonneDepensesLargeur = 410;
        $ctrl.enteteColonneEcartLargeur = 350;
        $ctrl.enteteColonneRadLargeur = 350;
        $ctrl.enteteColonneAjustementLargeur = 500;

        //Temporaire avant de passer à la selection des devises
        $ctrl.symboleDeviseSelectionnee = '€';

        /**
         * Cette variable si true alors un clique signifiera un reset a 0 des montants de l'ajustement
         * si c'est faux, alors cela signifiera un chargement des valeurs de l'ajustement du mois précédent
         */
        $ctrl.resetOrDownloadAjustement = true;
        $ctrl.displayBanner = false;

        $ctrl.ci;
        $ctrl.axePrincipal = $ctrl.AxePrincipalTaches;
        $ctrl.filter = {};
        $ctrl.filter.AxePrincipal = $ctrl.axePrincipal;
        $ctrl.filter.AxeAffichees = ["T1", "T2", "T3", "Chapitre", "SousChapitre", "Ressource"];
        $ctrl.filter.Cumul = true;
        $ctrl.msgAxeAnalytique = "";

        $ctrl.onChangePeriod = onChangePeriod;
        $ctrl.onSaveParametresControleBudgetaire = onSaveParametresControleBudgetaire;
        $ctrl.onCancelParametresChangesControleBudgetaire = onCancelParametresChangesControleBudgetaire;
        $ctrl.onClickBoutonCacheColonne = onClickBoutonCacheColonne;
        $ctrl.onCacheOuAfficheColonneDansParametre = onCacheOuAfficheColonneDansParametre;
        $ctrl.onClickCacheOuAfficheAxe = onClickCacheOuAfficheAxe;
        $ctrl.onAfficheBandeauParametres = onAfficheBandeauParametres;
        $ctrl.onClickBoutonAnnuler = onClickBoutonAnnuler;
        $ctrl.onClickBoutonEnregistrer = onClickBoutonEnregistrer;
        $ctrl.onChangeAxePrincipalDansParametrage = onChangeAxePrincipalDansParametrage;
        $ctrl.onAxePrincipalChanged = onAxePrincipalChanged;
        $ctrl.onClickBoutonAValider = onClickBoutonAValider;
        $ctrl.onClickBoutonValider = onClickBoutonValider;
        $ctrl.onCLickBoutonModifier = onCLickBoutonModifier;
        $ctrl.onClickButtonDeverouiller = onClickButtonDeverouiller;
        $ctrl.hideAllColumnByTitle = hideAllColumnByTitle;
        $ctrl.showAllColumnByTitle = showAllColumnByTitle;
        $ctrl.onExportExcel = onExportExcel;
        $ctrl.onClearCiLookup = onClearCiLookup;
        $ctrl.onToutPlier = onToutPlier;
        $ctrl.onToutDeplier = onToutDeplier;

        // calculs
        $ctrl.getQuantiteEcart = ControleBudgetaireCalculService.calculQuantiteEcart;
        $ctrl.getPuEcart = ControleBudgetaireCalculService.calculPuEcart;
        $ctrl.getMontantEcart = ControleBudgetaireCalculService.calculMontantEcart;
        $ctrl.getMontantRadTheorique = ControleBudgetaireCalculService.calculMontantRadTheorique;
        $ctrl.getPfa = ControleBudgetaireCalculService.calculPfa;
        $ctrl.getPourcentageDepense = ControleBudgetaireCalculService.calculPourcentageDepense;
        $ctrl.getPourcentageDad = ControleBudgetaireCalculService.calculPourcentageDad;
        $ctrl.getQuantiteRad = ControleBudgetaireCalculService.calculQuantiteRad;
        $ctrl.getPuDepense = ControleBudgetaireCalculService.calculPuDepense;
        $ctrl.getQuantiteDad = ControleBudgetaireCalculService.calculQuantiteDad;
        $ctrl.getPuDad = ControleBudgetaireCalculService.calculPuDad;

        $ctrl.getTotalMontantBudget = ControleBudgetaireCalculService.calculTotalMontantBudget;
        $ctrl.getTotalDaD = ControleBudgetaireCalculService.calculTotalDaD;
        $ctrl.getTotalDepense = ControleBudgetaireCalculService.calculTotalDepense;
        $ctrl.getTotalEcart = ControleBudgetaireCalculService.calculTotalEcart;
        $ctrl.getTotalRad = ControleBudgetaireCalculService.calculTotalRad;
        $ctrl.getTotalAjustement = ControleBudgetaireCalculService.calculTotalAjustement;
        $ctrl.getTotalPfa = ControleBudgetaireCalculService.calculTotalPfa;
        $ctrl.getTotalPfaMoisPrecedent = ControleBudgetaireCalculService.calculTotalPfaMoisPrecedent;
        $ctrl.getTotalProjectionLineaireDepenses = ControleBudgetaireCalculService.calculTotalProjectionLineaireDepenses;
        $ctrl.getTooltipFavoris = getTooltipFavoris;
        $ctrl.addFilter2Favoris = addFilter2Favoris;
        $ctrl.getFilterOrFavoris = getFilterOrFavoris;
        $ctrl.getDescriptionAxes = getDescriptionAxes;

        $ctrl.isExportAllowed = isExportAllowed;
        $ctrl.IsAxeAnalytiqueSelected = IsAxeAnalytiqueSelected;
        $ctrl.isLastLevelDisplayed = isLastLevelDisplayed;
        $ctrl.onClickCumulButton = onClickCumulButton;
        $ctrl.onClickDownloadPreviousAjustement = onClickDownloadPreviousAjustement;
        $ctrl.onClickResetAjustement = onClickResetAjustement;
        $ctrl.exportCIList = [];
        $ctrl.onOrderChange = onOrderChange;

        $scope.$on('MontantAjustementChanged', onMontantAjustementChanged);

        $scope.$on('ShowOngletDepenses', function (event, axe) {
            initializeFilters();
            $q.all([TachesService.getTache(axe.TacheId), ReferentielFixeService.getRessourceById(axe.RessourceId)])
                .then(function (response) {
                    $ctrl.tache = response[0].data;
                    $ctrl.ressource = response[1].data;
                    if ($ctrl.tache !== null) {
                        $ctrl.explorateurFilter.Tache = $ctrl.tache;
                        $ctrl.explorateurFilter.TacheId = axe.TacheId;
                        $filter('filter')($ctrl.filters, { id: 'Tache' }, true)[0].value.active = true;
                        $filter('filter')($ctrl.filters, { id: 'Tache' }, true)[0].value.label = $ctrl.tache.CodeLibelle;
                    }
                    if ($ctrl.ressource !== null) {
                        $ctrl.explorateurFilter.RessourceId = axe.RessourceId;
                        $ctrl.explorateurFilter.Ressource = $ctrl.ressource;
                        $filter('filter')($ctrl.filters, { id: 'Ressource' }, true)[0].value.active = true;
                        $filter('filter')($ctrl.filters, { id: 'Ressource' }, true)[0].value.label = $ctrl.ressource.CodeLibelle;
                    }

                    $ctrl.explorateurFilter.CI = $ctrl.ci;
                    $ctrl.explorateurFilter.CiId = $ctrl.ci.CiId;
                    $ctrl.explorateurFilter.IsCumul = $ctrl.filter.Cumul;
                    $ctrl.explorateurFilter.AxePrincipal = ["T1", "T2", "T3"];
                    $ctrl.explorateurFilter.AxeSecondaire = ["Chapitre", "SousChapitre", "Ressource"];
                    $ctrl.explorateurFilter.PeriodeDebut = moment($ctrl.periode).startOf('month').format('YYYY-MM-DD');
                    $ctrl.explorateurFilter.PeriodeFin = moment($ctrl.periode).endOf('month').format('YYYY-MM-DD 23:59:59');
                    BudgetService.ShowOngletDepenses($ctrl.explorateurFilter, $ctrl.filters);
                })
        });

        function initializeFilters() {
            sessionStorage.removeItem('explorateurDepenseFilter');
            $ctrl.explorateurFilter = {};
            $filter('filter')($ctrl.filters, { id: 'Ressource' }, true)[0].value.active = false;
            $filter('filter')($ctrl.filters, { id: 'Tache' }, true)[0].value.active = false;
            $filter('filter')($ctrl.filters, { id: 'Ressource' }, true)[0].value.label = $filter('filter')($ctrl.filters, { id: 'Ressource' }, true)[0].value._label;
            $filter('filter')($ctrl.filters, { id: 'Tache' }, true)[0].value.label = $filter('filter')($ctrl.filters, { id: 'Tache' }, true)[0].value._label;
        }

        var controleBudgetaireFilterChanged = true;

        return $ctrl;

        //Cette variable tempon est utilisée pour gérer les annulations de modifications
        var axeTreeCopie;

        function onToutPlier() {
            $(".collapse").collapse('hide');
            resetAxe($ctrl.axeTree, true);
        }

        function onToutDeplier() {
            $(".collapse").collapse('show');
            resetAxe($ctrl.axeTree, false);
        }

        // Gestion du tri
        function onOrderChange(columnName, initialLoad) {
            let orderAsc = $ctrl.colonnesOrder[columnName];
            let orderByProperty = $ctrl.colonnesProperties[columnName];

            if (orderAsc === null) {
                orderByProperty = "Valeurs.DefaultOrder";
            }

            for (let key in $ctrl.colonnesOrder) {
                if (key !== columnName) {
                    $ctrl.colonnesOrder[key] = null;
                }
            }

            // pas de tri si pas de données
            if (!$ctrl.axeTree) {
                $ctrl.colonnesOrder[columnName] = null;
                return;
            }

            // Dans le cas du chargement initial pas besoin d'afficher la progress bar
            if (initialLoad) {
                $ctrl.axeTree = orderAxe($ctrl.axeTree, orderByProperty, orderAsc);
                return;
            }
            // Affichage de la progress bar en time out pour pouvoir rafraichir l'écran avant le digest
            ProgressBar.start(true);
            $timeout(() => {
                $ctrl.axeTree = orderAxe($ctrl.axeTree, orderByProperty, orderAsc);
                ProgressBar.complete();
                sessionStorage.setItem('controleBudgetaireFilter', JSON.stringify({ Filter: $ctrl.filter, ColonnesAffichees: $ctrl.colonnesAffichees, ColonneOrder: getColonneOrderName(), ColonneOrderAsc: getColonneOrderAsc() }));
            });
        }

        // Order by recursif sur les sous axes
        function orderAxe(axeTree, columnName, reverseOrder) {
            axeTree.forEach(function (axe) {
                if (axe.SousAxe) {
                    axe.SousAxe = orderAxe(axe.SousAxe, columnName, reverseOrder);
                }
            });
            return $filter('orderBy')(axeTree, columnName, reverseOrder, numericComparator);
        }

        // Custom Comparator pour gérer uniquement les valeurs numériques,
        // les autres valeurs seront traitées comme des 0 (non triés)
        function numericComparator(v1, v2) {
            let value1 = 0;
            let value2 = 0;

            if (v1.type === "number") {
                value1 = v1.value;
            }

            if (v2.type === "number") {
                value2 = v2.value;
            }

            if (value1 === value2) {
                return 0;
            }
            if (value1 > value2) {
                return 1;
            }
            return -1;
        }

        function getColonneOrderName() {
            for (let key in $ctrl.colonnesOrder) {
                if ($ctrl.colonnesOrder[key] !== null) {
                    return key;
                }
            }
            return null;
        }

        function getColonneOrderAsc() {
            for (let key in $ctrl.colonnesOrder) {
                if ($ctrl.colonnesOrder[key] !== null) {
                    return $ctrl.colonnesOrder[key];
                }
            }
            return null;
        }

        function resetAxe(axes, hide) {
            axes.forEach(function reset(axe) {
                axe.hiddenChildren = hide;
                if (axe.SousAxe) {
                    resetAxe(axe.SousAxe, hide);
                }
            });
        }

        function isExportAllowed() {
            return $ctrl.axeTree !== null && $ctrl.filter.Cumul === true;
        }

        function IsAxeAnalytiqueSelected(axeType) {
            if ($ctrl.axesAnalytiquesParametresAffiches) {
                return $ctrl.axesAnalytiquesParametresAffiches.findIndex((axe) => axe.Name === axeType.Name) !== -1;
            }
        }

        function onShowOngletDepense() {
            BudgetService.ShowOngletDepenses($ctrl.ci);
        }

        function onMontantAjustementChanged() {
            $ctrl.axeTree.forEach((axeT1) => {
                axeT1.Valeurs.MontantAjustement = ControleBudgetaireCalculService.calculMontantAjustementEnfant(axeT1);
            });
        }

        function onClickBoutonEnregistrer() {
            if (!$ctrl.ci || !$ctrl.periode) {
                Notify.error(resources.EnregistrementControleBudgetaireError);
                return;
            }

            if (isLastLevelDisplayed()) {
                ProgressBar.start(true);
                var saveModel = createSaveModel();
                BudgetService.EnregistreControleBudgetaire(saveModel)
                    .then(onEnregistrerControleBudgetaireSuccess)
                    .catch(onEnregistrerControleBudgetaireErreur);
            }
        }

        /**
         * Cette fonction retourne true si le niveau le plus fin est affiché
         * Niveau Ressource pour l'axe principale Tache Ressource
         * Niveau Taches pour l'axe principale Ressource Tache
         * @returns {any} un booleen : true si le niveau le plus fin est affiché, false sinon
         */
        function isLastLevelDisplayed() {
            if ($ctrl.axePrincipal === $ctrl.AxePrincipalRessources) {
                return $ctrl.axesAnalytiquesAffiches.findIndex((axe) => axe.Name === "T3") !== -1;
            } else if ($ctrl.axePrincipal === $ctrl.AxePrincipalTaches) {
                return $ctrl.axesAnalytiquesAffiches.findIndex((axe) => axe.Name === "Ressource") !== -1;
            }
        }

        function createSaveModel() {
            var modelArray = [];

            $ctrl.axeTree.forEach((axeT1) => {
                if (axeT1.SousAxe) {
                    modelArray = construitSaveModel(modelArray, axeT1);
                }
                else {
                    let model = CreateSaveModelPourAxe(axeT1);
                    modelArray.push(model);
                }
            });

            var saveModel = {
                BudgetId: $ctrl.budgetId,
                Periode: BudgetDateService.formatPeriodeToApiYYYYMMFormat($ctrl.periode),
                ControleBudgetaireValeurs: modelArray
            };

            return saveModel;
        }

        function construitSaveModel(modelArray, axe) {
            axe.SousAxe.forEach((sousAxe) => {
                if (AxeDevraitEtreSauvegarde(sousAxe)) {
                    var model = CreateSaveModelPourAxe(sousAxe);
                    modelArray.push(model);
                }

                if (sousAxe.SousAxe !== null) {
                    return construitSaveModel(modelArray, sousAxe);
                } else {
                    return modelArray;
                }
            });

            return modelArray;
        }

        function CreateSaveModelPourAxe(axe) {
            if (!axe.Valeurs.MontantAjustement) {
                axe.Valeurs.MontantAjustement = 0;
            }

            var model = {
                Ajustement: axe.Valeurs.MontantAjustement,
                CommentaireAjustement: axe.Valeurs.CommentaireAjustement,
                Pfa: ControleBudgetaireCalculService.calculPfa(axe),
                TacheId: axe.TacheId,
                RessourceId: axe.RessourceId
            };

            return model;
        }

        function AxeDevraitEtreSauvegarde(axe) {
            return $ctrl.axePrincipal === $ctrl.AxePrincipalRessources && axe.AxeType === 'T3' ||
                $ctrl.axePrincipal === $ctrl.AxePrincipalTaches && axe.AxeType === 'Ressource';
        }

        function onClickBoutonAnnuler() {
            $ctrl.axeTree = angular.copy(axeTreeCopie);
        }

        //Events Implementation
        function onCiSelected() {
            if ($ctrl.filter.CiId !== $ctrl.ci.CiId) {
                $ctrl.filter.CiId = $ctrl.ci.CiId;
                controleBudgetaireFilterChanged = true;
                getControleBudgetaire();
            }
        }

        function getControleBudgetaire() {
            //On vérifie d'abord qu'un Ci a été choisi
            //Pas besoin de vérifier le choix de l'axe principal ou de la période ils ont tous les deux une valeur par défaut
            if (!$ctrl.ci || !$ctrl.periode || !controleBudgetaireFilterChanged) {
                return;
            }
            ProgressBar.start(true);
            sessionStorage.setItem('controleBudgetaireFilter', JSON.stringify({ Filter: $ctrl.filter, ColonnesAffichees: $ctrl.colonnesAffichees, ColonneOrder: getColonneOrderName(), ColonneOrderAsc: getColonneOrderAsc() }));
            $ctrl.filter.AxeAffichees = convertiAxesAnalytiqueAffichesVersTableauAxePourFiltre();
            BudgetService.GetControleBudgetaire($ctrl.filter)
                .then(onGetControleBudgetaireSuccess)
                .catch(onGetControleBudgetaireError);
        }

        /**
         * L'API attend en paramètre un tableau ordonné : l'indice 0 du tableau représente le premier niveau de l'arbre.
         * Cette fonction retourne un nouveau tableau respectant cette contrainte à partir du tableau d'axe affichés
         * @returns {any} un tableau ordonné avec en premier element l'axe ayant la priorité la plus haute et en dernier l'axe avec la priorité la plus basse
         */
        function convertiAxesAnalytiqueAffichesVersTableauAxePourFiltre() {
            resetOrder();
            switch ($ctrl.axePrincipal) {
                case $ctrl.AxePrincipalTaches:
                    DiminuePrioriteAxesPourType("R");
                    break;
                case $ctrl.AxePrincipalRessources:
                    DiminuePrioriteAxesPourType("T");
                    break;
            }

            $ctrl.axesAnalytiquesAffiches.sort(compareOrdreAxeAnalytique);
            return $ctrl.axesAnalytiquesAffiches.map(axe => axe.Name);
        }

        function compareOrdreAxeAnalytique(a, b) {
            if (a.Order < b.Order)
                return -1;
            if (a.Order > b.Order)
                return 1;
            return 0;
        }

        function DiminuePrioriteAxesPourType(typeAxe) {
            //Avec cette axe principal les tâches doivent être plus prioritaires que les ressources
            //donc il faut diminuer la priorité des ressources dans le tableau
            $ctrl.axesAnalytiquesAffiches.forEach(axe => {
                if (axe.Type === typeAxe) {
                    axe.Order += 3; //3 pour que le plus prioritaire des axes du type choisi (T ou R) soit moins prioritaire que le moins prioritaire des axes de l'autre type
                }
            });
        }

        function onGetControleBudgetaireSuccess(response) {
            if (response.data === "") {
                Notify.error(resources.GetControleBudgetaireAucuneDonnee);
                sessionStorage.setItem('controleBudgetaireFilter', JSON.stringify({ Filter: null, ColonnesAffichees: null, ColonneOrder: null, ColonneOrderAsc: null }));
                $ctrl.tree = null;
                $ctrl.axeTree = null;
                ProgressBar.complete();
                return;
            }

            controleBudgetaireFilterChanged = false;
            $ctrl.budgetId = response.data.BudgetId;
            $ctrl.budgetVersion = response.data.BudgetVersion;
            $ctrl.dateBudget = response.data.DateBudget;
            $ctrl.etat = response.data.CodeEtat;
            $ctrl.axeTree = response.data.Tree;
            $ctrl.isAvancementValide = response.data.AvancementValide;
            $ctrl.isPeriodeCloturee = response.data.PeriodeCloturee;
            $ctrl.exportCIList = [];
            //Encore une fois, un controle budgétaire locké est pratiquement impossible a délocker et cela arrive lorsqu'un controle budgétaire futur est validé
            //Un controle budgétaire readonly peut parfaitement changer d'état, cela arrive quand l'état n'est pas brouillon par exemple
            $ctrl.Locked = response.data.Locked;
            $ctrl.Readonly = response.data.Readonly;

            axeTreeCopie = angular.copy($ctrl.axeTree);
            if ($ctrl.axeTree === null) {
                //Il n'y a pas de budget en application actuellement donc pas de controle budgétaire
                //donc on signale l'erreur et on quitte
                Notify.error(resources.Budget_Controller_Notification_CISansOrganisation_Erreur);
                return;
            }

            //L'utilisateur a potentiellement déjà choisi quelles colonnes il veut afficher ou cacher
            //Donc on cache les colonnes qui ne doivent pas être affichées
            $ctrl.colonnesAffichable.forEach((colonne) => {
                if (!$ctrl.colonnesAffichees.includes(colonne)) {
                    $scope.$broadcast('HideColumn', colonne);
                }
            });

            // propriétés calculées
            calculateProperties($ctrl.axeTree);
            // application du tri si il existe
            let orderColumnName = getColonneOrderName();
            if (orderColumnName) {
                onOrderChange(orderColumnName, true);
            }
            ProgressBar.complete();
        }

        // Calcul des propriétés en javascript
        function calculateProperties(axeTree) {
            let defaultOrderIndex = 0;
            axeTree.forEach(function (axe) {
                if (axe.SousAxe) {
                    calculateProperties(axe.SousAxe);
                    axe.Valeurs.ProjectionLineaireDepenses = axe.SousAxe.reduce((a, b) => a + b.Valeurs.ProjectionLineaireDepenses, 0);
                } else {
                    axe.Valeurs.ProjectionLineaireDepenses = axe.Valeurs.ProjectionLineaire;
                }

                axe.Valeurs.DefaultOrder = defaultOrderIndex;
                axe.Valeurs.QuantiteEcart = $ctrl.getQuantiteEcart(axe);
                axe.Valeurs.PuEcart = $ctrl.getPuEcart(axe);
                axe.Valeurs.MontantEcart = $ctrl.getMontantEcart(axe);
                axe.Valeurs.MontantRadTheorique = $ctrl.getMontantRadTheorique(axe);
                axe.Valeurs.Pfa = $ctrl.getPfa(axe);
                axe.Valeurs.PourcentageDepense = $ctrl.getPourcentageDepense(axe);
                axe.Valeurs.PourcentageDad = $ctrl.getPourcentageDad(axe);
                axe.Valeurs.QuantiteRad = $ctrl.getQuantiteRad(axe);
                axe.Valeurs.PuDepense = $ctrl.getPuDepense(axe);
                axe.Valeurs.QuantiteDad = $ctrl.getQuantiteDad(axe);
                axe.Valeurs.PuDad = $ctrl.getPuDad(axe);
                defaultOrderIndex++;
            });
        }

        function onClearCiLookup() {
            $ctrl.ci = null;
            $ctrl.axeTree = null;
            $ctrl.filter.CiId = null;
        }

        function onChangePeriod() {
            $q.when()
                .then(() => {
                    //L'axe principal ne fait pas partie du filtre mais il est utilisé pour ordonner le tableau d'axe à afficher
                    var selectedPeriode = BudgetDateService.formatPeriodeToApiYYYYMMFormat($ctrl.periode);
                    if ($ctrl.filter.PeriodeComptable !== selectedPeriode) {
                        $ctrl.filter.PeriodeComptable = selectedPeriode;
                        controleBudgetaireFilterChanged = true;
                        getControleBudgetaire();
                    }
                });
        }

        function onClickBoutonAValider() {
            if (!$ctrl.ci || !$ctrl.periode) {
                Notify.error(resources.DemandeValidationControleBudgetaireError);
                return;
            }

            ProgressBar.start(true);

            BudgetService.GetPeriodeControleBudgetaireBrouillon($ctrl.budgetId, $ctrl.filter.PeriodeComptable)
                .then(continueWithDemandeAValider)
                .catch(onDemandeValidationControleBudgetaireErreur);
        }

        function continueWithDemandeAValider(response) {
            let periodes = response.data.map(periode => {
                return intPeriodeToStringPeriode(periode);
            });

            $scope.$broadcast(BudgetService.Events.DisplayControleBudgetaireValidationVerrouillagePrecedentsDialog,
                { Periodes: periodes });
            ProgressBar.complete();
        }

        function demandeDeValidationConfirmee() {
            ProgressBar.start(true);
            if (isLastLevelDisplayed()) {
                var saveModel = createSaveModel();
                BudgetService.EnregistreControleBudgetaire(saveModel)
                    .then(demandeValidationControleBudgetaire)
                    .catch(onEnregistrerControleBudgetaireErreur);
            } else {
                //On n'enregistre pas si on n'a rien a enregistrer (car le niveau le plus fin n'est pas affiché)
                demandeValidationControleBudgetaire();
            }
        }

        function demandeValidationControleBudgetaire() {
            BudgetService.DemandeValidationControleBudgetaire($ctrl.budgetId, BudgetDateService.formatPeriodeToApiYYYYMMFormat($ctrl.periode))
                .then(onDemandeValidationControleBudgetaireSuccess)
                .catch(onDemandeValidationControleBudgetaireErreur);
        }

        function onClickBoutonValider() {
            if (!$ctrl.ci || !$ctrl.periode) {
                Notify.error(resources.ValidationControleBudgetaireError);
                return;
            }

            ProgressBar.start(true);
            //Pas besoin de l'enregistrer ici parce que si le budget a été modifié il sera à l'état brouillon
            //et ne pourra pas passer à l'état Validé directement
            BudgetService.ValideControleBudgetaire($ctrl.budgetId, BudgetDateService.formatPeriodeToApiYYYYMMFormat($ctrl.periode))
                .then(onValidationSuccess)
                .catch(onValidationError);
        }

        function onClickButtonDeverouiller() {
            ProgressBar.start(true);
            BudgetService.RepasseControleBudgetaireEtatBrouillon($ctrl.budgetId, BudgetDateService.formatPeriodeToApiYYYYMMFormat($ctrl.periode))
                .then(onPassageEtatBrouillonSuccess)
                .catch(onPassageEtatBrouillonError);
        }

        function onCLickBoutonModifier() {
            ProgressBar.start(true);
            BudgetService.RepasseControleBudgetaireEtatBrouillon($ctrl.budgetId, BudgetDateService.formatPeriodeToApiYYYYMMFormat($ctrl.periode))
                .then(onPassageEtatBrouillonSuccess)
                .catch(onPassageEtatBrouillonError);
        }

        function onCancelParametresChangesControleBudgetaire() {
            $ctrl.axePrincipal = $ctrl.filter.AxePrincipal;
            $ctrl.axesAnalytiquesParametresAffiches = angular.copy($ctrl.axesAnalytiquesAffiches);
            $ctrl.colonnesParametreesAffichees = angular.copy($ctrl.colonnesAffichees);
        }

        function onSaveParametresControleBudgetaire() {
            $ctrl.displayBanner = false;
            $ctrl.axePrincipal = $ctrl.parametreAxePrincipal;
            //Maintenant on peut modifier la taille des entêtes des colonnes qu'on a décidé de masquer
            $ctrl.colonnesAffichable.forEach((colonne) => {
                //Est-ce qu'on cache la colonne parce qu'elle a toujours était cachée ou parce que l'utilisateur vient de décider de la cacher
                if (!$ctrl.colonnesParametreesAffichees.includes(colonne) &&
                    $ctrl.colonnesAffichees.includes(colonne)) {
                    metAJourEnteteColonnesApresCachage(colonne);
                } else if ($ctrl.colonnesParametreesAffichees.includes(colonne) &&
                    !$ctrl.colonnesAffichees.includes(colonne)) {
                    metAJourEnteteColonnesApresAffichage(colonne);
                }
            });

            $ctrl.colonnesAffichees = angular.copy($ctrl.colonnesParametreesAffichees);

            //Si il y a eu un changement qui nécessite un rechargement des données on met le flag à true
            if (!angular.equals($ctrl.axesAnalytiquesAffiches, $ctrl.axesAnalytiquesParametresAffiches)) {
                controleBudgetaireFilterChanged = true;
            }
            else if ($ctrl.filter.AxePrincipal !== $ctrl.axePrincipal) {
                controleBudgetaireFilterChanged = true;
            }

            $ctrl.axesAnalytiquesAffiches = angular.copy($ctrl.axesAnalytiquesParametresAffiches);
            //Il faut d'abord vérifier la cohérence entre l'axe principal choisi et les axes à afficher.
            //e.g Si l'utilisateur n'a choisi que axes de types ressources alors l'axe principal choisi
            //ne peut pas être Taches
            if ($ctrl.axesAnalytiquesAffiches.every(axe => axe.Type === 'R')) {
                //Si on est ici c'est que l'utilisateur n'a choisi que des ressources donc l'axe principal est ressource
                $ctrl.axePrincipal = $ctrl.AxePrincipalRessources;
            } else if ($ctrl.axesAnalytiquesAffiches.every(axe => axe.Type === 'T')) {
                //Si on est ici c'est que l'utilisateur n'a choisi que des Taches donc l'axe principal est Taches
                $ctrl.axePrincipal = $ctrl.AxePrincipalTaches;
            }

            $ctrl.filter.AxePrincipal = $ctrl.axePrincipal;
            $ctrl.filter.AxeAffichees = convertiAxesAnalytiqueAffichesVersTableauAxePourFiltre();

            sessionStorage.setItem('controleBudgetaireFilter', JSON.stringify({ Filter: $ctrl.filter, ColonnesAffichees: $ctrl.colonnesAffichees, ColonneOrder: getColonneOrderName(), ColonneOrderAsc: getColonneOrderAsc() }));

            //On appelle la fonction pour récupérer la nouvelle forme du controle budgétaire
            //La fonction ne fera rien si l'axe principal choisi n'a pas été modifié
            getControleBudgetaire();
        }

        function resetOrder() {
            $ctrl.axesAnalytiquesAffiches.forEach((axe) => {
                let axeAffichable = $ctrl.axeAnalytiqueAffichable.find(a => a.Name === axe.Name);
                axe.Order = axeAffichable.Order;
            });
        }

        function metAJourEnteteColonnesApresCachage(colonne) {
            switch (colonne) {
                case 'BudgetQte':
                case 'BudgetPu':
                    $ctrl.enteteColonneBudgetLargeur -= 100;
                    break;
                case 'DadQte':
                case 'DadPu':
                    $ctrl.enteteColonneDadLargeur -= 100;
                    break;
                case 'DepQte':
                case 'DepPu':
                    $ctrl.enteteColonneDepensesLargeur -= 100;
                    break;
                case 'EcartQte':
                case 'EcartPu':
                    $ctrl.enteteColonneEcartLargeur -= 100;
                    break;
                case 'RadQte':
                case 'RadPu':
                    $ctrl.enteteColonneRadLargeur -= 100;
                    break;
                case 'AjustementCommentaire':
                    $ctrl.enteteColonneAjustementLargeur -= 300;
                    break;
            }
        }

        function metAJourEnteteColonnesApresAffichage(colonne) {
            switch (colonne) {
                case 'BudgetQte':
                case 'BudgetPu':
                    $ctrl.enteteColonneBudgetLargeur += 100;
                    break;
                case 'DadQte':
                case 'DadPu':
                    $ctrl.enteteColonneDadLargeur += 100;
                    break;
                case 'DepQte':
                case 'DepPu':
                    $ctrl.enteteColonneDepensesLargeur += 100;
                    break;
                case 'EcartQte':
                case 'EcartPu':
                    $ctrl.enteteColonneEcartLargeur += 100;
                    break;
                case 'RadQte':
                case 'RadPu':
                    $ctrl.enteteColonneRadLargeur += 100;
                    break;
                case 'AjustementCommentaire':
                    $ctrl.enteteColonneAjustementLargeur += 300;
                    break;
            }
        }

        function onChangeAxePrincipalDansParametrage(axe) {
            $ctrl.parametreAxePrincipal = axe;
        }

        function onAxePrincipalChanged(axe) {
            if ($ctrl.axePrincipal !== axe) {
                if (axe === $ctrl.AxePrincipalTaches && $ctrl.axesAnalytiquesParametresAffiches.filter(a => a.Type === "T").length === 0) {
                    Notify.error("Veuillez sélectionner au moins niveau de tâche");
                }
                else if (axe === $ctrl.AxePrincipalRessources && $ctrl.axesAnalytiquesParametresAffiches.filter(a => a.Type === "R").length === 0) {
                    Notify.error("Veuillez sélectionner au moins niveau de resource");
                }
                else {
                    controleBudgetaireFilterChanged = true;
                    $ctrl.axePrincipal = axe;
                    $ctrl.filter.AxePrincipal = $ctrl.axePrincipal;
                    getControleBudgetaire();
                }
            }
        }

        function onClickBoutonCacheColonne(colonne) {
            //On ne peut pas cliquer sur ce bouton si la colonne est déjà cachée, donc pas besoin de vérifier la valeur de l'index
            var indexOfColumn = $ctrl.colonnesAffichees.indexOf(colonne);
            $ctrl.colonnesAffichees.splice(indexOfColumn, 1);
            sessionStorage.setItem('controleBudgetaireFilter', JSON.stringify({ Filter: $ctrl.filter, ColonnesAffichees: $ctrl.colonnesAffichees, ColonneOrder: getColonneOrderName(), ColonneOrderAsc: getColonneOrderAsc() }));

            metAJourEnteteColonnesApresCachage(colonne);
            $scope.$broadcast('HideColumn', colonne);
        }

        function onCacheOuAfficheColonneDansParametre(colonne) {
            var indexOfColumn = $ctrl.colonnesParametreesAffichees.indexOf(colonne);
            if (indexOfColumn !== -1) {
                $ctrl.colonnesParametreesAffichees.splice(indexOfColumn, 1);
            } else {
                $ctrl.colonnesParametreesAffichees.push(colonne);
            }
            checkFilterActiveOnAxe();
        }

        function onClickCacheOuAfficheAxe(axe) {
            var indexOfAxe = $ctrl.axesAnalytiquesParametresAffiches.findIndex(a => a.Name === axe.Name);
            if (indexOfAxe !== -1) {
                $ctrl.axesAnalytiquesParametresAffiches.splice(indexOfAxe, 1);
            } else {
                $ctrl.axesAnalytiquesParametresAffiches.push(axe);
            }
            checkFilterActiveOnAxe();
        }

        function onAfficheBandeauParametres() {
            $ctrl.parametreAxePrincipal = $ctrl.axePrincipal;
            $ctrl.colonnesParametreesAffichees = angular.copy($ctrl.colonnesAffichees);
            $ctrl.axesAnalytiquesParametresAffiches = angular.copy($ctrl.axesAnalytiquesAffiches);
            $ctrl.displayBanner = true;
        }

        function checkFilterActiveOnAxe() {
            if ($ctrl.colonnesParametreesAffichees.length !== $ctrl.colonnesAffichable.length || $ctrl.axesAnalytiquesParametresAffiches.length !== $ctrl.axeAnalytiqueAffichable.length) {
                $ctrl.filterActiveOnAxe = true;
            }
            else {
                $ctrl.filterActiveOnAxe = false;
            }
            if ($ctrl.axesAnalytiquesParametresAffiches.length === 0) {
                $ctrl.msgAxeAnalytique = "Veuillez sélectionner au moins un axe.";
            }
            else {
                $ctrl.msgAxeAnalytique = "";
            }
        }

        function onClickResetAjustement() {
            if (!$ctrl.axeTree) {
                return;
            }

            fredDialog.question(" Etes-vous sûr de vouloir supprimer les ajustements de ce contrôle budgétaire?", "", "", "", "", function () { return resetAjustementToZero($ctrl.axeTree); });
        }

        function resetAjustementToZero(axeArray) {
            axeArray.forEach(axe => {
                axe.Valeurs.MontantAjustement = 0;
                if (axe.SousAxe) {
                    resetAjustementToZero(axe.SousAxe);
                }
            });
        }

        function onClickDownloadPreviousAjustement() {
            if (!$ctrl.axeTree) {
                return;
            }

            fredDialog.confirmation("Les ajustements du dernier contrôle budgétaires vont être importés. Si le contrôle budgétaire actuel contient des ajustements, ils seront remplacés.", "", "flaticon flaticon-warning")
                .then(() => {
                    let periodePrecedente = $ctrl.filter.PeriodeComptable - 1;
                    ProgressBar.start(true);
                    BudgetService.GetAjustement($ctrl.budgetId, periodePrecedente)
                        .then(getPreviousAjustementSuccess)
                        .catch(getPreviousAjustementError);
                });
        }

        function getPreviousAjustementSuccess(response) {
            resetAjustementWithPreviousValeurs($ctrl.axeTree, response.data);
            onMontantAjustementChanged();
            Notify.message("Ajustement précédent récupéré avec succes");
            ProgressBar.complete();
        }

        function getPreviousAjustementError(error) {
            Notify.error("Impossible de récupérer l'ajustement précédent");
            ProgressBar.complete();
        }

        function resetAjustementWithPreviousValeurs(axeArray, previousValuesArray) {
            axeArray.forEach(axe => {
                if (axe.SousAxe) {
                    resetAjustementWithPreviousValeurs(axe.SousAxe, previousValuesArray);
                } else {
                    //Si on est au niveau feuille alors on peut connaitre le couple Tache3 Ressource pour cette feuille
                    //et donc récupérer la valeur de l'ajustement du mois précédent
                    let index = previousValuesArray.findIndex(v => v.Tache3Id === axe.TacheId && v.RessourceId === axe.RessourceId);
                    if (index === -1) {
                        //Si on est ici c'est qu'aucun montant d'ajustement n'a été saisie pour cette ressource
                        //le mois dernier donc on met 0
                        axe.Valeurs.MontantAjustement = 0;
                        return;
                    }

                    let previousValues = previousValuesArray[index];
                    axe.Valeurs.MontantAjustement = previousValues.Ajustement;
                }
            });
        }

        function onClickCumulButton() {
            controleBudgetaireFilterChanged = true;
            $ctrl.filter.Cumul = !$ctrl.filter.Cumul;
            getControleBudgetaire();
        }

        function showModalCI() {
            let periode = BudgetDateService.formatPeriodeToApiYYYYMMFormat($ctrl.periode);
            let modalInstance = $uibModal.open({
                animation: true,
                windowClass: 'app-modal-window',
                size: 'lg',
                backdrop: "static",
                component: 'budgetCIComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    ci: function () { return $ctrl.ci; },
                    periode: function () { return periode; },
                    ciSelectedlist: function () { return $ctrl.exportCIList; }
                }
            });
            return modalInstance.result;
        }

        function onExportExcel(isPdfConverted) {
            if ($ctrl.filter.Cumul === false) {
                return;
            }
            showModalCI().then(
                function (ciList) {
                    let ciIdList = ciList.map(x => x.CiId);
                    var budgetModel = createExportExcelLoadModel(isPdfConverted, ciIdList);
                    ProgressBar.start(true);
                    $q.when()
                        .then(() => {
                            BudgetService.EnregistreControleBudgetaire(createSaveModel());
                            Notify.message(resources.EnregistrementControleBudgetaireSuccess);
                        })
                        .then(continueWithExportExcel(budgetModel))
                        .catch(onEnregistrerControleBudgetaireErreur)
                        .finally(ProgressBar.complete());
                }
            );
        }

        function createExportExcelLoadModel(isPdfConverted, ciIdList) {
            let tree = angular.copy($ctrl.axeTree);
            tree = resolveHiddenAxe(tree);

            //Le back n'a besoin que du type d'axe et des sous axes
            tree = tree.map(function mapper(axe) {
                let sousAxe = null;
                if (axe.SousAxe !== null) {
                    sousAxe = axe.SousAxe.map(mapper);
                }

                return {
                    AxeType: axe.AxeType,
                    SousAxe: sousAxe
                };
            });

            var model = {
                BudgetId: $ctrl.budgetId,
                Periode: GetPeriodeFormat($ctrl.periode),
                IsPdfConverted: isPdfConverted,
                AxePrincipal: $ctrl.filter.AxePrincipal,
                AxeAffichees: $ctrl.filter.AxeAffichees,
                Tree: tree,
                ciIdList: ciIdList
            };

            return model;
        }

        function GetPeriodeFormat(date) {
            var year = date.getFullYear();
            var month = date.getMonth() + 1;
            return year * 100 + month;
        }

        function resolveHiddenAxe(axe) {
            axe.map(function resolve(tache) {
                if (tache.hiddenChildren) {
                    tache.SousAxe = null;
                }
                else {
                    if (tache.SousAxe) {
                        tache.SousAxe.map(resolve);
                    }
                }
            });

            return axe;
        }

        function continueWithExportExcel(model) {
            ProgressBar.start(true);
            BudgetService.ControleBudgetaireExportExcel(model)
                .then(response => actionGetExportExcel(response, model.IsPdfConverted))
                .catch(exportExcelError);
        }

        function actionGetExportExcel(response, isPdf) {
            BudgetService.DownloadExportFile(response.data.id, isPdf, 'ControleBudgetaire' + $ctrl.budgetId);
            ProgressBar.complete();
        }

        function exportExcelError(error) {
            Notify.error(resources.ExportExcelError);
            ProgressBar.complete();
        }

        function onGetControleBudgetaireError(error) {
            let message = resources.GetControleBudgetaireError;
            if (error.data.ExceptionMessage) {
                message = error.data.ExceptionMessage;
            }
            else if (error.data.Message)
                message = error.data.Message;

            $ctrl.tree = null;
            $ctrl.axeTree = null;
            sessionStorage.setItem('controleBudgetaireFilter', JSON.stringify({ Filter: null, ColonnesAffichees: null, ColonneOrder: null, ColonneOrderAsc: null }));
            Notify.error(message);
            ProgressBar.complete();
        }

        function onEnregistrerControleBudgetaireSuccess(response) {
            ProgressBar.complete();
            $ctrl.etat = response.data.NouvelEtat.Code;
            $ctrl.Readonly = false;
            Notify.message(resources.EnregistrementControleBudgetaireSuccess);
        }

        function onEnregistrerControleBudgetaireErreur(error) {
            ProgressBar.complete();
            Notify.error(resources.EnregistrementControleBudgetaireError);
        }

        function onDemandeValidationControleBudgetaireSuccess(response) {
            //La demande de validation est tout le temps acceptée, cependant on se sert de la valeur de retour
            //pour mettre à jours les deux indicateurs (avancement et cloture du CI)
            $ctrl.etat = response.data.NouvelEtat.Code;
            $ctrl.Readonly = true;

            $ctrl.isAvancementValide = response.data.AvancementValide;
            $ctrl.isPeriodeCloturee = response.data.PeriodeComptableCloturee;
            Notify.message(resources.DemandeValidationControleBudgetaireSuccess);
            ProgressBar.complete();
        }

        function onDemandeValidationControleBudgetaireErreur(error) {
            Notify.error(resources.DemandeValidationControleBudgetaireError);
            ProgressBar.complete();
        }

        function onValidationSuccess(response) {
            if (response.data.AllOkay) {
                $ctrl.etat = response.data.NouvelEtat.Code;
                $ctrl.Readonly = true;

                //Le passage à l'état validé ne peut pas réussir si ces deux valeurs ne sont pas
                $ctrl.isAvancementValide = true;
                $ctrl.isPeriodeCloturee = true;

                Notify.message(resources.ValidationControleBudgetaireSuccess);
                ProgressBar.complete();
            } else {
                let periodeString = intPeriodeToStringPeriode($ctrl.filter.PeriodeComptable);
                $scope.$broadcast(BudgetService.Events.DisplayControleBudgetaireValidationErreurDialog,
                    {
                        Periode: periodeString,
                        AvancementValide: response.data.AvancementValide,
                        PeriodeComptableCloturee: response.data.PeriodeComptableCloturee
                    });

                ProgressBar.complete();
            }
        }

        function onValidationError(error) {
            Notify.error(resources.ValidationControleBudgetaireError);
        }

        function onPassageEtatBrouillonSuccess(response) {
            if (response.data.AllOkay) {
                $ctrl.etat = response.data.NouvelEtat.Code;
                $ctrl.Readonly = false;
                Notify.message(resources.PassageControleBudgetaireEtatBrouillonSuccess);
                if (response.data.BudgetIdHasChanged) {
                    controleBudgetaireFilterChanged = true;
                    getControleBudgetaire();
                }
                ProgressBar.complete();
            } else {
                Notify.error("Impossible de passer à l'état brouillon");
                ProgressBar.complete();
            }
        }

        function onPassageEtatBrouillonError(error) {
            Notify.error(resources.PassageControleBudgetaireEtatBrouillonError);
        }

        function intPeriodeToStringPeriode(periode) {
            let year = Math.round(periode / 100);
            let month = periode % 100;
            let periodeString = month + "/" + year;
            return periodeString;
        }

        function getDescriptionAxes() {
            switch ($ctrl.axePrincipal) {
                case $ctrl.AxePrincipalTaches:
                    return $ctrl.resources.Budget_ControleBudgetaire_Titre_AxePrincipalTaches;
                case $ctrl.AxePrincipalRessources:
                    return $ctrl.resources.Budget_ControleBudgetaire_Titre_AxePrincipalRessource;
                default:
                    return $ctrl.resources.Budget_ControleBudgetaire_Titre_AxeInconnu;
            }
        }

        function getTooltipFavoris() {
            if ($ctrl.ci && $ctrl.periode) {
                var periode = $ctrl.periode.getMonth() + 1 + "/" + $ctrl.periode.getFullYear();
                return resources.ControleBudgetaire_Tooltip_Favoris_Part1 + $ctrl.ci.CodeLibelle + resources.ControleBudgetaire_Tooltip_Favoris_Part2 + periode;
            }
            return "";
        }

        /*
        * @function addFilter2Favoris()
        * @description Crée un nouveau favori
        */
        function addFilter2Favoris() {
            var filter = {
                ColonnesAffichees: $ctrl.colonnesAffichees,
                Filter: $ctrl.filter,
                ColonneOrder: getColonneOrderName(),
                ColonneOrderAsc: getColonneOrderAsc()
            };
            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("ControleBudgetaire", url, filter);
        }

        function getFilterOrFavoris(favoriId) {
            $ctrl.favoriId = parseInt(favoriId);
            if ($ctrl.favoriId !== 0) {
                favorisService.getFilterByFavoriIdOrDefault({ favoriId: $ctrl.favoriId, defaultFilter: $ctrl.filter })
                    .then(function (response) {
                        $ctrl.filter = response.Filter;
                        $ctrl.colonnesAffichees = response.ColonnesAffichees;
                        let colonneOrder = response.ColonneOrder;
                        if (colonneOrder) {
                            $ctrl.colonnesOrder[colonneOrder] = response.ColonneOrderAsc;
                        }
                        TraitmentFilterAndFavoris();
                    }).catch(function (error) { console.log(error); });
            }
            else {
                if (sessionStorage.getItem('controleBudgetaireFilter') !== null) {
                    let sessionData = JSON.parse(sessionStorage.getItem('controleBudgetaireFilter'));
                    $ctrl.filter = sessionData.Filter;
                    $ctrl.colonnesAffichees = sessionData.ColonnesAffichees;
                    let colonneOrder = sessionData.ColonneOrder;
                    if (colonneOrder) {
                        $ctrl.colonnesOrder[colonneOrder] = sessionData.ColonneOrderAsc;
                    }
                    $ctrl.colonnesAffichable.forEach((colonne) => {
                        //On vient de récupérer les colonnes masquées par l'utilisateur, il faut maintenant mettre à jour la taille des entêtes
                        if (!$ctrl.colonnesAffichees.includes(colonne)) {
                            metAJourEnteteColonnesApresCachage(colonne);
                        }
                    });

                    TraitmentFilterAndFavoris();
                }
            }
        }

        function TraitmentFilterAndFavoris() {
            $ctrl.periode = new Date($ctrl.filter.PeriodeComptable / 100, $ctrl.filter.PeriodeComptable % 100 - 1);
            CIService.GetById({ CiId: $ctrl.filter.CiId }).$promise.then((response) => {
                var preSelectedCi = response;
                if (preSelectedCi !== null) {
                    $ctrl.ci = preSelectedCi;
                    getControleBudgetaire();
                }
            });

            $ctrl.axePrincipal = $ctrl.filter.AxePrincipal;
            if ($ctrl.filter.AxeAffichees) {
                $ctrl.axesAnalytiquesAffiches = [];
                $ctrl.filter.AxeAffichees.forEach(axe => {
                    $ctrl.axesAnalytiquesAffiches.push($ctrl.axeAnalytiqueAffichable.find(axe2 => axe2.Name === axe));
                });
                $ctrl.colonnesParametreesAffichees = angular.copy($ctrl.colonnesAffichees);
                $ctrl.axesAnalytiquesParametresAffiches = angular.copy($ctrl.axesAnalytiquesAffiches);
                checkFilterActiveOnAxe();
            }
        }

        function hideAllColumnByTitle(title) {
            var colonnes = angular.copy($ctrl.colonnesAffichees);
            colonnes.forEach(colonne => {
                if (colonne.startsWith(title)) {
                    onClickBoutonCacheColonne(colonne);
                }
            });
            majIconChevronByTitle(title);
        }

        function showAllColumnByTitle(title) {
            var colonnes = angular.copy($ctrl.colonnesAffichable);
            colonnes.forEach(colonne => {
                if (colonne.startsWith(title) && !$ctrl.colonnesAffichees.includes(colonne)) {
                    $ctrl.colonnesAffichees.push(colonne);
                    metAJourEnteteColonnesApresAffichage(colonne);
                }
            });
            majIconChevronByTitle(title);
        }

        function majIconChevronByTitle(title) {
            switch (title) {
                case "Budget":
                    $ctrl.showColumn.Budget = !$ctrl.showColumn.Budget;
                    break;
                case "Dad":
                    $ctrl.showColumn.Dad = !$ctrl.showColumn.Dad;
                    break;
                case "Dep":
                    $ctrl.showColumn.Dep = !$ctrl.showColumn.Dep;
                    break;
                case "Ecart":
                    $ctrl.showColumn.Ecart = !$ctrl.showColumn.Ecart;
                    break;
                case "Rad":
                    $ctrl.showColumn.Rad = !$ctrl.showColumn.Rad;
                    break;
                case "Ajustement":
                    $ctrl.showColumn.Ajustement = !$ctrl.showColumn.Ajustement;
                    break;
            }
        }
    }
}(angular));