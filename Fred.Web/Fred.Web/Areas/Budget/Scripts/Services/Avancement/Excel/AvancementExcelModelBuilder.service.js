(function () {

    'use strict';

    angular.module('Fred').service('AvancementExcelModelBuilderService', AvancementExcelModelBuilderService);

    AvancementExcelModelBuilderService.$inject = ['BudgetService'];

    function AvancementExcelModelBuilderService(BudgetService) {

        var service = this;

        //L'API demande de préfixer le code tache par le type de la tache, T1, T2, T3 ou T4
        var t1CodeTachePrefix = "T1";
        var t2CodeTachePrefix = "T2";
        var t3CodeTachePrefix = "T3";
        var t4CodeTachePrefix = "T4";

        service.CreateExcelModel = CreateExcelModel;

        /**
         * Retourne un objet contenant le model de l'avancement utilisé par l'export excel
         * @param {any} ciId le CiId contenant le budget en application sur lequel on travail
         * @param {any} periode la période en cours
         * @param {any} tachesNiveau1 la liste des taches de niveau 1 de ce budget, cette fonction ne prend que des taches marquées comment étant visibles
         * @param {any} shownAnalyticalAxis shownAnalyticalAxis
         * @param {any} isPdfConverted flag de convertion pdf
         * c'est à dire que la tache parent ne contient pas de variable nommé childrenHidden ou alors cette variable est à False
         * @returns {any} un model tel qu'attendu par l'export excel voir l'API
         */
        function CreateExcelModel(ciId, periode, tachesNiveau1, shownAnalyticalAxis, isPdfConverted) {

            //D'abord il faut ordonner les taches par code
            let taches = angular.copy(tachesNiveau1);
            sortByCode(taches);

            var model = {
                CiId: ciId,
                Periode: periode,
                Valeurs: GetValeursFromTaches1(taches),
                AnalyticalAxis:shownAnalyticalAxis,
                IsPdfConverted: isPdfConverted

            };

            return model;
        }

        function sortByCode(tachesNiveau1) {
            tachesNiveau1.sort(compare);
            tachesNiveau1.forEach((t1) => {
                t1.TachesNiveau2.sort(compare);
                t1.TachesNiveau2.forEach(t2 => {
                    t2.TachesNiveau3.sort(compare);
                    t2.TachesNiveau3.forEach(t3 => {
                        t3.TachesNiveau4.sort(compare);
                    });
                });

            });
        }

        function compare(a, b) {
            if (a.Code < b.Code)
                return -1;
            if (a.Code > b.Code)
                return 1;
            return 0;
        }



        function GetValeursFromTaches1(tachesNiveau1) {

            var valeurs = [];

            for (let tache1 of tachesNiveau1) {
                valeurs.push(CreateTacheModelForTache(tache1, t1CodeTachePrefix));
                if (tache1.childrenHidden !== true) {
                    valeurs = valeurs.concat(GetChildrenValeursFromTache1(tache1));
                }
            }

            return valeurs;
        }

        function GetChildrenValeursFromTache1(tache1) {

            var valeurs = [];
            for (let tache2 of tache1.TachesNiveau2) {
                valeurs.push(CreateTacheModelForTache(tache2, t2CodeTachePrefix));

                if (tache2.childrenHidden !== true) {
                    valeurs = valeurs.concat(GetChildrenValeursFromTache2(tache2));
                }
            }

            return valeurs;
        }

        function GetChildrenValeursFromTache2(tache2) {

            var valeurs = [];
            for (let tache3 of tache2.TachesNiveau3) {
                valeurs.push(CreateTacheModelForTache(tache3, t3CodeTachePrefix));
                if (tache3.childrenHidden !== true) {
                    valeurs = valeurs.concat(GetChildrenValeursFromTache3(tache3));
                }
            }

            return valeurs;
        }

        function GetChildrenValeursFromTache3(tache3) {
            return tache3.TachesNiveau4.map(t4 => CreateTacheModelForTache(t4, t4CodeTachePrefix ,true));
        }

        function CreateTacheModelForTache(tache,prefixCodeTache, isTache4) {

            let uniteCode = null;
            let quantiteBudgete = null;
            let puBudgete = null;
            let valeursAvancementMoisPrecedent = null;
            let valeursAvancementMoisCourant = null;
            let UniteAvancement = null;

            if (isTache4 === true) {
                UniteAvancement = getUniteAvancement(tache);
                uniteCode = tache.Unite ? tache.Unite.Code : "";
                quantiteBudgete = tache.Quantite;
                puBudgete = tache.PU;
                valeursAvancementMoisPrecedent = getMoisPrecedentAvancementT4(tache);
                valeursAvancementMoisCourant = getMoisCourantAvancementT4(tache);
            }
            else {
                UniteAvancement = '%';
                uniteCode = '%';
                valeursAvancementMoisPrecedent = tache.AvancementPourcentPrevious;
                valeursAvancementMoisCourant = tache.AvancementPourcent;
            }

            var tacheModel = {
                NiveauTache: prefixCodeTache,
                CodeTache: tache.Code,
                LibelleTache: tache.Libelle,
                Commentaire: tache.Commentaire,
                UniteCode: uniteCode,
                QuantiteBudgetee: quantiteBudgete,
                PuBudgete: puBudgete,
                MontantBudgete: tache.Montant,
                UniteAvancement: UniteAvancement,
                ValeursAvancementMoisCourant: valeursAvancementMoisCourant,
                ValeursAvancementMoisPrecedent: valeursAvancementMoisPrecedent
            };

            return tacheModel;
        }

        function getUniteAvancement(tache4){
            if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Quantite) {
                return tache4.Unite ? tache4.Unite.Code : "";
            }

            if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Pourcentage) {
                return '%';
            }
        }
  

        function getMoisCourantAvancementT4(tache4) {

            if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Quantite) {
                return tache4.AvancementQte;
            }

            if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Pourcentage) {
                return tache4.AvancementPourcent;
            }
        }

        function getMoisPrecedentAvancementT4(tache4) {

            if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Quantite) {
                return tache4.AvancementQtePrevious;
            }

            if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Pourcentage) {
                return tache4.AvancementPourcentPrevious;
            }
        }

    }

})();