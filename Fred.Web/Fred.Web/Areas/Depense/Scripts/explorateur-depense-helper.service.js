(function () {
    'use strict';

    angular.module('Fred').service('ExplorateurDepenseHelperService', ExplorateurDepenseHelperService);

    ExplorateurDepenseHelperService.$inject = ['Notify', '$filter'];

    function ExplorateurDepenseHelperService(Notify, $filter) {
        var service = {
            actionOnNoData: actionOnNoData,
            actionFormattingData: actionFormattingData,
            actionCheckAxeAnalytique: actionCheckAxeAnalytique,
            actionCheckAllAxeAnalytique: actionCheckAllAxeAnalytique,
            actionGetTuple: actionGetTuple,
            actionDeleteAxeEnfants: actionDeleteAxeEnfants,
            actionSelectDepense: actionSelectDepense,
            actionSelectAllDepenses: actionSelectAllDepenses,
            deselectAllDepense: deselectAllDepense,
            isChecked: isChecked
        };

        return service;

        /**
         * Message d'erreur quand aucune donnée retrouvée
         * @param {any} data reponse
         * @return {any} reponse
         */
        function actionOnNoData(data) {
            if (!data.data || data.data.length === 0 || data.data.Depenses && data.data.Depenses.length === 0) {
                Notify.error(resources.Global_Notification_AucuneDonnees);
            }
            return data;
        }

        /**
         * Permet de setter tous les parents de chaque sous axes
         * @param {any} data liste de niveaux 1
         * @returns {any} liste de niveaux 1
         */
        function actionFormattingData(data) {

            angular.forEach(data.data, function (level, k) {
                level.Id = k;
                level.Niveau = k;
                level.Checked = false;
                level.Picked = false;
                actionInitData(level, k);
            });

            return data;
        }

        /**
         * Gestion de la case à cocher
         * @param {any} level axe courant
         * @param {any} data arbre complet
         */
        function actionCheckAxeAnalytique(level, data) {
            actionAxeSelectionDown(level);
            actionAxeSelectionUp(level, data);
        }

        /**
         * Check all
         * @param {any} data liste niveau 1
         * @param {any} value valeur boolén : coché ou pas 
         */
        function actionCheckAllAxeAnalytique(data, value) {
            angular.forEach(data, function (level) { level.Checked = value; actionAxeSelectionDown(level); });
        }

        /**
         * Récupération du tuple axe 1 et axe 2 après sélection
         * @param {any} levelNumber numéro niveau
         * @param {any} currentLevel niveau courant
         * @param {any} l level1
         * @param {any} ll level2
         * @param {any} lll level3
         * @param {any} llll level4
         * @param {any} axePCount count axe 1
         * @param {any} axeSCount count axe 2
         * @param {any} typeAxes liste type d'axe
         * @returns {any} tuple
         */
        function actionGetTuple(levelNumber, currentLevel, l, ll, lll, llll, axePCount, axeSCount, typeAxes) {
            var tuple = { Axe1: null, Axe2: null, Id: null };
            var levels = [l, ll, lll, llll];

            if (levelNumber === 1) {
                tuple = { Axe1: l, Axe2: null, Id: currentLevel.Id };
            }
            else if (levelNumber === 2) {
                if (actionGetTypeDetail(typeAxes, l) !== actionGetTypeDetail(typeAxes, ll)) {
                    tuple = { Axe1: l, Axe2: ll, Id: currentLevel.Id };
                }
                else {
                    tuple = { Axe1: ll, Axe2: null, Id: currentLevel.Id };
                }
            }
            else if (levelNumber === 3) {
                tuple = { Axe1: levels[axePCount - 1], Axe2: levels[axePCount + axeSCount - 1], Id: currentLevel.Id };
            }
            else if (levelNumber === 4) {
                tuple = { Axe1: levels[axePCount - 1], Axe2: llll, Id: currentLevel.Id };
            }
            else if (levelNumber === 5) {
                if (actionGetTypeDetail(typeAxes, ll) !== actionGetTypeDetail(typeAxes, lll)) {
                    tuple = { Axe1: ll, Axe2: lll, Id: currentLevel.Id };
                }
                else {
                    tuple = { Axe1: l, Axe2: lll, Id: currentLevel.Id };
                }
            }
            else if (levelNumber === 6) {
                tuple = { Axe1: l, Axe2: ll, Id: currentLevel.Id };
            }

            return tuple;
        }

        /**
         * Gestion de la sélection des dépenses
         * @param {any} allDepenses Tableau complet des dépenses
         * @param {any} currentDepense Dépense     
         * @param {any} value booléen est sélectionné ou pas
         * @param {any} selectedDepenses liste des dépenses sélectionnées
         */
        function actionSelectDepense(allDepenses, currentDepense, value, selectedDepenses) {
            if (value !== undefined && value !== null) {
                currentDepense.Selected = value;
            }

            if (currentDepense) {

                var isAlreadyIn = $filter('filter')(selectedDepenses, { Id: currentDepense.Id }, true)[0];

                if (currentDepense.Selected) {

                    if (!isAlreadyIn) {
                        selectedDepenses.push(currentDepense);
                    }
                }
                else {
                    selectedDepenses.splice(selectedDepenses.indexOf(currentDepense), 1);
                }
            }
        }

        function isChecked(selectedDepenses, currentDepense) {
            angular.forEach(selectedDepenses, function (key, value) {
                if (key.Id === currentDepense.Id) {
                    currentDepense.Selected = true;
                }
            });
        }
        

        /**
         * Gestion de la sélection ou déselection complète des lignes de dépenses
         * @param {any} isSelectingAll Sélection ou Déselection
         * @param {any} depenses Toutes les dépenses
         * @param {any} selectedDepenses Toutes les dépenses sélectionnées
         */
        function actionSelectAllDepenses(isSelectingAll, depenses, selectedDepenses) {
            angular.forEach(depenses, function (d) {
                if (d.TacheRemplacable) {
                    actionSelectDepense(depenses, d, isSelectingAll, selectedDepenses);
                }
            });
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            PRIVATE
         * -------------------------------------------------------------------------------------------------------------
         */

        /**
         * Suppression des axes enfants dans le filtre
         * @param {any} list liste des axes enfants
         * @param {any} axes filtre axes
         */
        function actionDeleteAxeEnfants(list, axes) {
            // Si sélection d'un père dont au moins un enfant a été sélectionné, on déselectionne l'enfant et on supprime l'axe de la liste de filtre
            angular.forEach(list, function (val) {
                var found = $filter('filter')(axes, { Id: val.Id }, true)[0];

                if (found) {
                    axes.splice(axes.indexOf(found), 1);
                }
            });
        }

        /**
         * Détermine si axe de type T ou R
         * @param {any} typeAxes liste types d'axe
         * @param {any} level niveau
         * @returns {any} T ou R
         */
        function actionGetTypeDetail(typeAxes, level) {
            switch (level.Type) {
                case typeAxes.T1.name:
                case typeAxes.T2.name:
                case typeAxes.T3.name:
                    return "T";
                case typeAxes.Chapitre.name:
                case typeAxes.SousChapitre.name:
                case typeAxes.Ressource.name:
                    return "R";
            }
        }

        /**
         * Fonction récursive : sélection de tous les niveaux inférieurs
         * @param {any} level axe courant
         */
        function actionAxeSelectionDown(level) {
            if (level.SousExplorateurAxe && level.SousExplorateurAxe.length > 0) {

                angular.forEach(level.SousExplorateurAxe, function (val, key) {
                    val.Checked = level.Checked;

                    actionAxeSelectionDown(val);
                });

            }
        }

        /**
         * Fonction récursive : sélection des niveaux supérieurs si tous les niveaux enfants sont sélectionnés
         * @param {any} level axe courant
         * @param {any} data arbre complet (tableau de gauche)
         */
        function actionAxeSelectionUp(level, data) {
            var parent = actionGetParent(data, level.ParentId);

            if (parent) {
                parent.Checked = actionIsAllChecked(parent);
                actionAxeSelectionUp(parent, data);
            }
        }

        /**
         * Récupération du parent
         * @param {any} data arbre
         * @param {any} parentId identifiant parent
         * @returns {any} Retourne le parent
         */
        function actionGetParent(data, parentId) {

            for (var i = 0; i < data.length; i++) {
                if (data[i].Id === parentId) {
                    return data[i];
                }
                else {
                    var res = actionFindByParentId(data[i], parentId);
                    if (res) return res;
                }
            }

        }

        /**
         * Recherche le parent par ID (parcours de l'arbre depuis la racine)
         * @param {any} level niveau courant
         * @param {any} parentId identifiant du parent
         * @returns {any} Retourne le parent par l'identifiant
         */
        function actionFindByParentId(level, parentId) {

            if (level.SousExplorateurAxe && level.SousExplorateurAxe.length > 0) {

                for (var i = 0; i < level.SousExplorateurAxe.length; i++) {

                    if (level.SousExplorateurAxe[i].Id === parentId) {

                        return level.SousExplorateurAxe[i];
                    }
                    else {
                        var res = actionFindByParentId(level.SousExplorateurAxe[i], parentId);
                        if (res) return res;
                    }
                }

            }
        }

        /**
         * Set des id uniques pour chaque axe
         * @param {any} level niveau courant
         * @param {any} k id
         */
        function actionInitData(level, k) {
            if (level.SousExplorateurAxe && level.SousExplorateurAxe.length > 0) {

                angular.forEach(level.SousExplorateurAxe, function (val, key) {
                    val.Id = k + '_' + key;
                    val.ParentId = level.Id;
                    val.Niveau = level.Niveau + 1;
                    val.Checked = false;
                    val.Picked = false;
                    actionInitData(val, val.Id);
                });

            }
        }

        /**
         * Vérifie si tous les sous axe d'un axe sont cochés
         * @param {any} level axe courant
         * @returns {any} bool tout coché ou pas
         */
        function actionIsAllChecked(level) {
            var allChecked = true;

            for (var i = 0; i < level.SousExplorateurAxe.length; i++) {
                var axe = level.SousExplorateurAxe[i];
                if (!axe.Checked) {
                    allChecked = false;
                    break;
                }
            }

            return allChecked;
        }

        // vider la liste depense seletionnee
       
        function deselectAllDepense(selectedDepenses) {

            selectedDepenses.length = 0;
        }

    }

})();