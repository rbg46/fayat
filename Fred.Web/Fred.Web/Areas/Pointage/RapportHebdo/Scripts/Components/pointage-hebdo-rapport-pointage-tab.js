(function () {
    'use strict';

    var pointageHebdoRapportPointageTabComponent = {
        templateUrl: '/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-pointage-tab.html',
        bindings: {
            resources: '<',
            limitHourPerDayBlocking: '@',
            limitHourPerDayBlockingForIac: '@',
            limitHourPerWeekNotBlocking: '@',
            limitHourPerWeekBlocking: '@',
            limitHourPerWeekBlockingForEtam: '@',
            isEtamIac: '<',
            isManagerPointing: '<',
            personnelIdList: '<',
            listPointagePersonnelForWeekParCi: '<',
            mondayDate: '<'
        },
        controller: PointageHebdoRapportPointageTabController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('pointageHebdoRapportPointageTabComponent', pointageHebdoRapportPointageTabComponent);

    angular.module('Fred').controller('PointageHebdoRapportPointageTabController', PointageHebdoRapportPointageTabController);

    PointageHebdoRapportPointageTabController.$inject = ['$scope', '$uibModal', 'Notify', 'PointageHedboService', 'UserService'];

    function PointageHebdoRapportPointageTabController($scope, $uibModal, Notify, PointageHedboService, UserService) {
        var $ctrl = this;
        $ctrl.data = [];
        $ctrl.isAffichageParOuvrier;
        $ctrl.blockingEntries = [];
        $ctrl.blockingEntriesForValidation = [];
        $ctrl.isEtam = false;
        $ctrl.isIac = false;
        $ctrl.Math = Math;
        $ctrl.isValidTosaveByWeek = true;
        $ctrl.isValidTosaveByDay = true;
        $ctrl.isBlockingForAllCi = false;
        $ctrl.handleClickCommentaire = handleClickCommentaire;
        $ctrl.commentaireNotNulleOrEmpty = commentaireNotNulleOrEmpty;
        $ctrl.itemWithCommentaireToDisplay = null;
        $ctrl.getLibelleTask = getLibelleTask;
        $ctrl.letterLimit = 35;
        checkPersonnelType();
        $ctrl.isOuvrier;
        $ctrl.showErrorMessageDepassement = false;
        $ctrl.errorList = [];

        UserService.getCurrentUser().then(function(user) {
            $ctrl.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;
        });

        $ctrl.getReferentielRhUrl = function (globalStatut, nodeStatut) {
            if ($ctrl.isUserFes) {
                switch (globalStatut) {
                    case "IAC": return "isCadre=true&";
                    case "ETAM": return "isETAM=true&";
                    case "Ouvrier": return "isOuvrier=true&";
                    default: switch (nodeStatut) {
                        case "IAC": return "isCadre=true&";
                        case "ETAM": return "isETAM=true&";
                        case "Ouvrier": return "isOuvrier=true&";
                        default: return "";
                    }
                }
            }
            return "";
        };

        // Rafrichissement du panel . Cet événement peut étre décelenché lors du changements des dates .
        $scope.$on('event.refresh.pointage.hebdo.task.panel', function (evt, data) {
            GetPointageByPersonnelIDAndIntervalParCi();
            $ctrl.data = data.rapportTaskAbsencePanelData;
            $ctrl.rapportMajorationPanelData = data.rapportMajorationPanelData;
            $ctrl.isAffichageParOuvrier = data.isAffichageParOuvrier;
            if ($ctrl.isAffichageParOuvrier === undefined) {
                $ctrl.isAffichageParOuvrier = false;
            }
            if ($ctrl.isAffichageParOuvrier === true && $ctrl.isEtamIac === true) {
                checkPersonnelType();
                if ($ctrl.isIac) {
                    calculateTasksAndAbsenceForIac();
                }
            }

            if ($ctrl.data) {
                makeTotalCalculationsFirstLoad($ctrl.data);
                updateGlobalTotals();
            }
        });

        $scope.$on('event.refresh.visible.commentaire.panel.pointage', function (event, data) {
            if ($ctrl.data[0] && $ctrl.data[0].SubNodeList) {
                var subNode = $ctrl.data[0].SubNodeList.filter(affaire => affaire.NodeId === data.Id);
                if (subNode && subNode[0] && subNode[0].SubNodeList) {
                    var taskAbsence = subNode[0].SubNodeList.filter(task => task.NodeText === "Absence");
                    var item = taskAbsence[0].Items[data.Index];
                    checkVisibilityPanelForCommentaire(data.Ecran, data.HasHour, item);
                }
            }
            updateGlobalTotals();
        });

        function traitmentTaskForCheckVisibilityPanel(subNode, index) {
            var tasks = subNode.SubNodeList.filter(node => node.NodeTypeLibelle === "Task");
            var value = false;
            tasks.forEach(task => {
                if (task.Items[index].TotalHours > 0) {
                    value = true;
                }
            });
            var taskAbsence = subNode.SubNodeList.filter(task => task.NodeText === "Absence");
            if (taskAbsence && taskAbsence.length > 0) {
                var item = taskAbsence[0].Items[index];
                checkVisibilityPanelForCommentaire("Pointage", value, item);
            }
        }

        function checkVisibilityPanelForCommentaire(name, value, item) {
            switch (name) {
                case "Pointage":
                    item.ValueInPanel.Pointage = value;
                    break;
                case "Majoration":
                    item.ValueInPanel.Majoration = value;
                    break;
                case "Prime":
                    item.ValueInPanel.Prime = value;
                    break;
            }

            if (item.ValueInPanel.Pointage || item.ValueInPanel.Majoration || item.ValueInPanel.Prime || item.ValueInPanel.Astreinte) {
                item.IsCommentaireVisible = true;
            } else {
                item.IsCommentaireVisible = false;
            }
        }

        //////////////////////////////////////////////////////////////////
        // Pick list urls
        /////////////////////////////////////////////////////////////////
        /**
        * Vérifier si le personnel est un ETAM ou IAC /Ouvrier
        */
        function checkPersonnelType() {
            if ($ctrl.data) {
                if ($ctrl.data[0] && $ctrl.data[0].Statut === 'IAC') {
                    $ctrl.isIac = true;
                    $ctrl.isEtam = false;
                    $ctrl.isOuvrier = false;
                }
                else if ($ctrl.data[0] && $ctrl.data[0].Statut === 'ETAM') {
                    $ctrl.isIac = false;
                    $ctrl.isEtam = true;
                    $ctrl.isOuvrier = false;
                }
                else {
                    $ctrl.isIac = false;
                    $ctrl.isEtam = false;
                    $ctrl.isOuvrier = true;
                }
            }
        }

        /**
        * Calculer les tranches pointées de la journée en fonction de nombre des heures
        */
        function calculateTasksAndAbsenceForIac() {
            angular.forEach($ctrl.data[0].SubNodeList, function (ci) {
                angular.forEach(ci.SubNodeList, function (taskOrAbsence) {
                    angular.forEach(taskOrAbsence.Items, function (item) {
                        item.TotalHoursForIac = item.TotalHours / 7;
                    });
                });
            });
        }

        //Fonction d'initialisation des données de la picklist
        $ctrl.handleLookupUrl = function (val, societedOrCiId, globalStatut, nodeStatut) {
            var lookupUrl = '/api/' + val + '/SearchLight/?societeId={0}&ciId={1}&groupeId={2}&materielId={3}&';
            switch (val) {
                case "CodeAbsence":
                    lookupUrl = String.format(lookupUrl, societedOrCiId, null, null, null);
                    lookupUrl += $ctrl.getReferentielRhUrl(globalStatut, nodeStatut);
                    break;
                case "Tache":
                    if (societedOrCiId) {
                        lookupUrl = String.format(lookupUrl, null, societedOrCiId, null, null);
                    }
                    break;
                default:
                    lookupUrl = '/api/' + val + '/SearchLight/';
                    break;
            }

            lookupUrl = String.format(lookupUrl, null, null, null, null);
            return lookupUrl;
        };

        /**
        * Handler pour la séléction de lookup des absences
        * @param {any} item l'élément séléctionné
        * @param {any} el élément du rapport
        */
        $ctrl.handleLookupSelection = function (item, el, index, subNode) {
            el.CodeAbsence = item.Code;
            el.CodeAbsenceId = item.CodeAbsenceId;
            if ($ctrl.isIac) {
                el.TotalHoursForIac = item.NBHeuresDefautETAM / 7;
                el.TotalHours = item.NBHeuresDefautETAM;
            }
            else {
                if ($ctrl.isOuvrier) {
                    el.TotalHours = item.NBHeuresDefautCO;
                }
                else {
                    el.TotalHours = item.NBHeuresDefautETAM;
                }
            }

            $ctrl.updateTotals(subNode, index);
        };

        /**
        * Handler pour la séléction de lookup des taches
        * @param {any} item l'élément séléctionné
        * @param {any} subNode rapport list node
        * @param {any} ciNode Noeud du CI
        */
        $ctrl.handleLookupSelectionTask = function (item, subNode, ciNode) {
            if (subNode.SubNodeList && subNode.SubNodeList.length > 0) {
                var existantTask = subNode.SubNodeList.find(function (e) { return e.NodeCode === item.Code; });
                if (!existantTask) {
                    if (ciNode.NodeTypeLibelle === $ctrl.resources.CI_Search_CIType_Etude && item && item.Code === $ctrl.resources.RapportHebdo_DefaultTaskCode) {
                        Notify.error($ctrl.resources.RapportHebdo_Message_Use_SpecificTask_For_CiTypeEtude);
                    }
                    else {
                        $ctrl.rapportLignesStatutList = [];
                        var ciId = item.CiId;
                        var personnel = $ctrl.isAffichageParOuvrier ? $ctrl.data[0].NodeId : subNode.NodeId;

                        PointageHedboService.GetRapportLigneStatutForNewPointage({ personnelId: personnel, ciId: ciId, mondayDate: $ctrl.mondayDate }).$promise
                            .then(function (value) {
                                $ctrl.rapportLignesStatutList = value;
                                var elements = [];
                                for (var i = 0; i < 7; i++) {
                                    var absenceEl = subNode.SubNodeList[0].Items[i];
                                    var rapportLigneStatut = getRapportLigneStatut(absenceEl.Date);
                                    elements.push(
                                        {
                                            Id: null,
                                            TotalHours: null,
                                            TotalHoursForIac: 0,
                                            Date: absenceEl.Date,
                                            RapportId: absenceEl.RapportId,
                                            RapportLigneId: absenceEl.RapportLigneId,
                                            PersonnelVerrouille: rapportLigneStatut.IsRapportLigneVerouiller,
                                            RapportValide: rapportLigneStatut.IsRapportLigneValide2
                                        });
                                }
                                subNode.SubNodeList.push(
                                    {
                                        NodeId: item.TacheId,
                                        NodeText: item.Libelle,
                                        NodeCode: item.Code,
                                        NodeType: 'Task',
                                        Items: elements
                                    }
                                );
                            });
                    }
                }
            }
        };

        //////////////////////////////////////////////////////////////////
        // Actions and Handlers                                          //
        //////////////////////////////////////////////////////////////////

        /*
       * @description Check aprés calcul de la somme des heures travaillés et déclencher un message bloquant pour la validation .
       */
        $ctrl.checkSumFieldsForValidation = function (list, index, indexItem) {
            var showError = false;
            if ($ctrl.isAffichageParOuvrier) {
                var result = $ctrl.calculateSumFields(list, index);
                var absence = $ctrl.calculateSumFieldsAbsence(list, index);
                var heurMajoNuit = GetMajorationByDayTravailNuit(index);
                showError = result + absence + heurMajoNuit > parseInt($ctrl.limitHourPerDayBlocking);
                checkBlockingEntriesForValidation(index, showError);
            }
            return showError;
        };

        /**
        * Check après calcul des tranches travaillés et déclencher un message bloquant pour la validation
        * @param {any} list List de champs
        * @param {integer} index Index des champs
        * @returns {any} True si des erreurs ont été détectées
        */
        $ctrl.checkSumFieldsForValidationIac = function (list, index) {
            var showError = false;
            if ($ctrl.isAffichageParOuvrier) {
                var result = $ctrl.calculateSumFields(list, index);
                var absence = $ctrl.calculateSumFieldsAbsence(list, index);

                showError = $ctrl.isIac && result + absence > parseInt($ctrl.limitHourPerDayBlockingForIac);
                checkBlockingEntriesForValidation(index, showError);
            }
            return showError;
        };

        /*
        * @description calcul des sommes des éléments d'un list en utilisant l'index.
        */
        $ctrl.calculateSumFields = function (list, index) {
            var result = 0;
            for (var i = 0; i < list.length; i++) {
                if (!list[i] || !list[i].Items) {
                    continue;
                }

                result += getTotalHours(list[i], index);
            }
            return result;
        };

        /*
        * @description calcul du total des éléments d'une liste.
        */
        $ctrl.calculateSumFieldsTotal = function (list) {
            var result = 0;
            for (var i = 0; i <= 6; i++) {
                result += $ctrl.calculateSumFields(list, i);
            }
            return result;
        };

        /*
        * @description Synchronisation du calcul des totaux .
        */
        $ctrl.updateTotals = function (subNode, index) {
            traitmentTaskForCheckVisibilityPanel(subNode, index)
            var result = 0;
            var absence = 0;
            for (var i = 0; i < subNode.SubNodeList.length; i++) {
                if (!subNode.SubNodeList[i] || !subNode.SubNodeList[i].Items) {
                    continue;
                }

                if (subNode.SubNodeList[i].isAbsenceNode) {
                    absence = getTotalHours(subNode.SubNodeList[i], index);
                }
                else {
                    result += getTotalHours(subNode.SubNodeList[i], index);
                }
            }

            if (!subNode.Items) {
                subNode.Items = [];
            }

            if (subNode.Items[index] === undefined) {
                subNode.Items[index] = {};
            }

            subNode.Items[index].TotalHours = result;
            subNode.Items[index].AbsenceHour = absence;
        };

        /**
        * Synchronisation du calcul des totaux pour le cas d'un IAC
        * @param {any} subNode Sous-noeud
        * @param {integer} index Index
        * @param {any} task Tâche
        */
        $ctrl.updateTotalsForIac = function (subNode, index, task) {
            if (task.Items[index].TotalHoursForIac === undefined) {
                task.Items[index].TotalHoursForIac = 0;
            }
            else if (task.isAbsenceNode === true && ![0, 0.5, 1].includes(task.Items[index].TotalHoursForIac)) {
                task.Items[index].TotalHoursForIac = 0;
            }
            else if (task.isAbsenceNode !== true && ![0, 0.25, 0.5, 0.75, 1].includes(task.Items[index].TotalHoursForIac)) {
                task.Items[index].TotalHoursForIac = 0;
            }
            $ctrl.updateHeureTacheForIac(subNode, index, task);
        };

        /**
        * Synchronisation du calcul des totaux pour le cas d'un Etam-Ouvrier
        * @param {any} subNode Sous noeud de l'arborescence pour lequel calculer les heures totales
        * @param {integer} index Index de l'entrée du sous-noeud
        * @param {any} task Ligne d'activité
        */
        $ctrl.updateTotalsForEtamOuvrier = function (subNode, index, task)
        {
            if (task.Items[index].TotalHours === undefined) {
                task.Items[index].TotalHours = 0;
            }
            var TotalHours = task.Items[index].TotalHours;
            var decimals = TotalHours - Math.floor(TotalHours);

            if (![0, 0.25, 0.50, 0.75].includes(decimals)) {
                task.Items[index].TotalHours = 0;
            }
            $ctrl.updateTotals(subNode, index);
        };

        /**
        * Synchronisation du calcul des totaux pour le cas d'un IAC
        * @param {any} subNode Sous noeud de l'arborescence pour lequel calculer les heures totales
        * @param {integer} index Index de l'entrée du sous-noeud
        * @param {any} task Ligne d'activité
        */
        $ctrl.updateHeureTacheForIac = function (subNode, index, task) {
            var item = task.Items[index];
            if (item.TotalHoursForIac === undefined) {
                item.TotalHoursForIac = 1;
            }
            item.TotalHours = item.TotalHoursForIac * 7;
            $ctrl.updateTotals(subNode, index);
        };

        /**
        * Synchronisation du calcul des totaux pour le cas d'un Ouvrier
        * @param {any} subNode Sous noeud de l'arborescence pour lequel calculer les heures totales
        * @param {integer} index Index de l'entrée du sous-noeud
        * @param {any} task Ligne d'activité
        */
        $ctrl.updateHeureTacheForEtamOuvrier = function (subNode, index, task) {
            var item = task.Items[index];
            if (item.TotalHours === undefined || item.TotalHours > 10) {
                item.TotalHours = 10;
            }

            $ctrl.updateTotals(subNode, index);
        };

        /*
        * @description Obtenir le total des heures pour une liste par index.
        */
        function getTotalHours(list, index) {
            var el = list.Items[index] ? list.Items[index].TotalHours : 0;
            var totalHours = typeof el === 'number' ? el : 0;
            return totalHours;
        }

        /*
        * @description Obtenir le total des heures par niveau
        */
        $ctrl.calculateSumFieldsTotalByLevel = function (subNode) {
            var result = 0;
            if (!subNode || !subNode.Items || subNode.Items.length === 0) {
                return result;
            }

            for (var i = 0; i < Object.keys(subNode.Items).length; i++) {
                result += getTotalHours(subNode, i);
            }
            return result;
        };

        /*
        * @description Obtenir le total des heures par personnel
        */
        $ctrl.calculateSumFieldsTotalByLevelError = function (subNode) {
            const days = 6;
            var resMaj = 0;
            var resMajByNode = 0;

            var totale = 0;
            if ($ctrl.isAffichageParOuvrier) {
                for (var i = 0; i <= days; i++) {
                    resMaj += GetMajorationByDayTravailNuit(i);
                }
                $ctrl.data.forEach(function (node) {
                    node.SubNodeList.forEach(function (subnode) {
                        subnode.Items.forEach(function (item) {
                            totale += item.TotalHours;//+ item.AbsenceHour;
                        });
                    });
                });

                totale += resMaj;
            }
            else {
                totale = 0;
                for (var node = 0; node < $ctrl.data.length; node++) {
                    for (var sub = 0; sub < $ctrl.data[node].SubNodeList.length; sub++) {
                        if (subNode !== null && subNode.NodeId && $ctrl.data[node].SubNodeList[sub].NodeId === subNode.NodeId) {
                            $ctrl.data[node].SubNodeList[sub].Items.forEach(function (item) {
                                totale += item.TotalHours + item.AbsenceHour;
                            });

                            for (var j = 0; j <= days; j++) {
                                resMajByNode += GetMajorationByDayTravailNuitByNode(j, sub, node);
                            }
                        }

                        totale += resMajByNode;
                        resMajByNode = 0;
                    }
                }
            }

            return totale > $ctrl.limitHourPerWeekNotBlocking && totale <= $ctrl.limitHourPerWeekBlocking;
        };

        /*
        * @description Obtenir le total des heures sup par niveau .
        */
        $ctrl.calculateSupHoursByLevel = function (subNode) {
            var val = $ctrl.calculateSumFieldsTotalByLevel(subNode) - $ctrl.limitHourPerWeekNotBlocking;
            return val && val >= 0 ? val : 0;
        };

        /*
        * @description Obtenir le total des heures sup par sub node item list .
        */
        $ctrl.calculateSumFieldsSupHour = function (itemSubNodeList) {
            var result = 0;
            if (!itemSubNodeList && itemSubNodeList.length === 0) {
                return result;
            }

            result = $ctrl.calculateSumFieldsTotal(itemSubNodeList) - $ctrl.limitHourPerWeekNotBlocking;

            return result && result >= 0 ? result : 0;
        };

        /*
        * @description Initialisation des calculs .
        */
        function makeTotalCalculationsFirstLoad(data) {
            if (!data && data.length === 0) {
                return;
            }

            $ctrl.blockingEntries = [];
            // Level 1
            for (var j = 0; j < data.length; j++) {
                var item = data[j];
                if (!item || !item.SubNodeList) {
                    continue;
                }

                var subNodeList = item.SubNodeList;
                for (var k = 0; k < subNodeList.length; k++) {
                    var subNode = subNodeList[k];
                    if (subNode) {
                        for (var i = 0; i < 7; i++) {
                            $ctrl.updateTotals(subNode, i);
                        }
                    }
                }
            }
        }

        /*
        * @description Calcul de la somme des heures d'absence .
        */
        $ctrl.calculateSumFieldsAbsence = function (itemSubNodeList, index) {
            if (!itemSubNodeList && itemSubNodeList.length === 0) {
                return;
            }

            var result = 0;
            for (var j = 0; j < itemSubNodeList.length; j++) {
                var subNode = itemSubNodeList[j];
                if (!subNode.SubNodeList || subNode.SubNodeList.length === 0) {
                    continue;
                }
                for (var x = 0; x < subNode.SubNodeList.length; x++) {
                    var task = subNode.SubNodeList[x];
                    if (!task || !task.isAbsenceNode) {
                        continue;
                    }

                    if (task.Items[index] && task.Items[index].CodeAbsence) {
                        result += getTotalHours(task, index);
                    }
                }
            }
            return result;
        };

        /*
        * @description Obtenir le total des heures d'absence  .
        */
        $ctrl.calculateSumFieldsAbsenceTotal = function (itemSubNodeList) {
            var result = 0;
            for (var i = 0; i < 7; i++) {
                result += $ctrl.calculateSumFieldsAbsence(itemSubNodeList, i);
            }
            return result;
        };

        function handleClickCommentaire(item) {
            $uibModal.open({
                animation: true,
                backdrop: 'static',
                component: 'popupCommentaireComponent',
                resolve: {
                    itemWithCommentaireToDisplay: function () { return item; },
                    resources: function () { return $ctrl.resources; }
                }
            });
        }


        ///////////////////////////////////////////////////////////////////////
        // Check hours
        /////////////////////////////////////////////////////////////////////////

        /*
       * @description Obtenir le total des Majorations pour une journée .
       */
        function GetMajorationByDayTravailNuit(day) {
            var result = 0;
            var rapportMajoration = $ctrl.rapportMajorationPanelData[0].SubNodeList;
            for (var nodeCi = 0; nodeCi < rapportMajoration.length; nodeCi++) {
                for (var node = 0; node < rapportMajoration[nodeCi].SubNodeList.length; node++) {
                    for (var subnode = 0; subnode < rapportMajoration[nodeCi].SubNodeList[node].Items.length; subnode++) {
                        if (rapportMajoration[nodeCi].SubNodeList[node].Items[subnode].IsHeureNuit) {
                            if (rapportMajoration[nodeCi].SubNodeList[node].Items[subnode].MajorationHeurePerDayList[day] !== undefined) {
                                result += rapportMajoration[nodeCi].SubNodeList[node].Items[subnode].MajorationHeurePerDayList[day].HeureMajoration;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /*
       * @description Obtenir le total des Majorations pour une journée .
       */
        function GetMajorationByDayTravailNuitByNode(day, SubNodeIndex, nodeIndex) {
            if (day === 7) day = 0;
            var result = 0;
            if ($ctrl.rapportMajorationPanelData[nodeIndex] && $ctrl.rapportMajorationPanelData[nodeIndex].SubNodeList) {
                var rapportMajoration = $ctrl.rapportMajorationPanelData[nodeIndex].SubNodeList;
                if (rapportMajoration[SubNodeIndex].SubNodeList[0] === undefined) return 0;
                rapportMajoration[SubNodeIndex].SubNodeList[0].Items.forEach(function (item) {
                    if (item.IsHeureNuit) {
                        if (item.MajorationHeurePerDayList[day] !== undefined) {
                            result += item.MajorationHeurePerDayList[day].HeureMajoration;

                        }
                    }
                });
                return result;
            }
            return result;
        }

        $ctrl.isBlockingHourNumberPerDay = function (list, index) {
            if ($ctrl.isAffichageParOuvrier) {
                var isBlocking = false;
                for (var i = 0; i < 7; i++) {
                    if ($ctrl.isIac === true) {
                        if ($ctrl.calculateSumFields(list.SubNodeList, i) + $ctrl.calculateSumFieldsAbsence(list.SubNodeList, i) > parseInt($ctrl.limitHourPerDayBlockingForIac)) {
                            isBlocking = true;
                            break;
                        }
                    }
                    else {
                        var totalHourInAllCiForPersonnel = 0;
                        totalHourInAllCiForPersonnel = filtrepersonnelMultiCi(list.NodeId, i, list.SubNodeList);
                        if ($ctrl.calculateSumFields(list.SubNodeList, i) + $ctrl.calculateSumFieldsAbsence(list.SubNodeList, i) + GetMajorationByDayTravailNuit(i) + totalHourInAllCiForPersonnel > parseInt($ctrl.limitHourPerDayBlocking)) {
                            isBlocking = true;
                            $ctrl.isBlockingForAllCi = true;
                            break;
                        }
                        $ctrl.isBlockingForAllCi = false;
                    }
                }
                if (isBlocking) {
                    $ctrl.isValidTosaveByDay = false;
                    $scope.$emit('event.change.save.state', false);
                    return;
                }
                else if ($ctrl.isValidTosaveByWeek && $ctrl.isValidTosaveByWeek === true) {
                    $ctrl.isValidTosaveByDay = true;
                    $scope.$emit('event.change.save.state', true);
                }
                return isBlocking;
            }
            else {
                for (var p = 0; p < $ctrl.personnelIdList.length; p++) {
                    var isBlocking1 = false;
                    var total = 0;
                    for (var d = 0; d < $ctrl.data.length; d++) {
                        for (var s = 0; s < $ctrl.data[d].SubNodeList.length; s++) {
                            if ($ctrl.personnelIdList[p] === $ctrl.data[d].SubNodeList[s].NodeId) {
                                var totalHourInAllCiForPersonnelMultiCi = 0;
                                totalHourInAllCiForPersonnelMultiCi = filtrepersonnelMultiCi($ctrl.personnelIdList[p], index, $ctrl.data);
                                total = $ctrl.data[d].SubNodeList[s].Items[index].TotalHours + $ctrl.data[d].SubNodeList[s].Items[index].AbsenceHour + totalHourInAllCiForPersonnelMultiCi;
                                if (list !== undefined) {
                                    total += (list.TotalHours ? list.TotalHours : 0);
                                }

                                total += (GetMajorationByDayTravailNuitByNode(index, s, d));
                                if (total > parseInt($ctrl.limitHourPerDayBlocking) && $ctrl.isValidTosaveByWeek && $ctrl.isValidTosaveByWeek === true) {
                                    isBlocking1 = true;
                                    $ctrl.showErrorMessageDepassement = true;
                                    if (!$ctrl.errorList.includes(index)) {
                                        $ctrl.errorList.push({
                                            key: index,
                                            val: total,
                                            personnelId: $ctrl.personnelIdList[p]
                                        });
                                    }

                                    return checkBlockingEntries(isBlocking1, $ctrl.data[d].NodeId, index, true);
                                } else {
                                    for (var i = 0; i < $ctrl.errorList.length; i++) {
                                        if ($ctrl.errorList[i].key === index && $ctrl.errorList[i].personnelId === $ctrl.personnelIdList[p]) {
                                            $ctrl.errorList.splice(i, 1);
                                        }
                                    }

                                    if ($ctrl.errorList.length === 0) {
                                        $ctrl.showErrorMessageDepassement = false;
                                    }

                                    checkBlockingEntries(isBlocking1, $ctrl.data[d].NodeId, index);
                                }
                            }
                        }
                    }
                }

                return isBlocking1;
            }
        };

        function filtrepersonnelMultiCi(personnel, day, ci) {
            var result = 0;
            var ciId = ci.map(function (v) {
                return v.NodeId;
            });
            day++;
            if (day > 6) {
                day = 0;
            }
            else {
                if ($ctrl.listPointagePersonnelForWeekParCi !== undefined) {
                    for (var p = 0; p < $ctrl.listPointagePersonnelForWeekParCi.length; p++) {
                        if ($ctrl.listPointagePersonnelForWeekParCi[p].PersonnelId === personnel) {
                            var c = $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi.length;
                            for (var d = 0; d < $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi.length; d++) {
                                var curentciId = $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi[d].CiId;
                                if (ciId.indexOf(curentciId) === -1) {
                                    result += $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi[d].TotalHours + $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi[d].TotalAbsence;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        function GetPointageByPersonnelIDAndIntervalParCi()
        {
            var model = { Mondaydate: $ctrl.mondayDate, PersonnelIds: $ctrl.personnelIdList, isForWeek: false };
            return PointageHedboService.GetPointageByPersonnelIDAndInterval(model)
                .$promise
                .then(function (value) {
                    $ctrl.listPointagePersonnelForWeekParCi = value;
                });
        }

        function filtrepersonnelForWeek(personnel, day, ci) {
            var result = 0;
            if ($ctrl.listPointagePersonnelForWeekParCi !== undefined) {
                for (var p = 0; p < $ctrl.listPointagePersonnelForWeekParCi.length; p++) {
                    if ($ctrl.listPointagePersonnelForWeekParCi[p].PersonnelId === personnel) {
                        for (var c = 0; c < $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi.length; c++) {
                            if ($ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi[c].CiId !== ci) {
                                var maj = $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi[c].TotalMajoration;
                                if (maj === undefined) maj = 0;
                                result += $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi[c].TotalHours + maj;
                            }
                        }
                    }
                }
            }
            return result;
        }

        $ctrl.isBlockingHourNumberPerDays = function (index) {
            if (!$ctrl.isAffichageParOuvrier) {
                for (var p = 0; p < $ctrl.personnelIdList.length; p++) {
                    var isBlocking1 = false;
                    var total = 0;
                    for (var d = 0; d < $ctrl.data.length; d++) {
                        for (var s = 0; s < $ctrl.data[d].SubNodeList.length; s++) {
                            if ($ctrl.personnelIdList[p] === $ctrl.data[d].SubNodeList[s].NodeId) {
                                if ($ctrl.data[d].SubNodeList[s].Items[index]) {
                                    total += $ctrl.data[d].SubNodeList[s].Items[index].TotalHours + $ctrl.data[d].SubNodeList[s].Items[index].AbsenceHour;
                                }

                                if (total + GetMajorationByDayTravailNuitByNode(index, s, d) > parseInt($ctrl.limitHourPerDayBlocking)) {
                                    isBlocking1 = true;
                                    return isBlocking1;
                                }
                            }
                        }
                    }
                }
                return isBlocking1;
            }
        };
        function checkBlockingEntries(isBlocking, nodeId, index, fromAllCI = false) {
            var exist = $ctrl.blockingEntries && $ctrl.blockingEntries.find(function (o) {
                return o.NodeId === nodeId && o.Index === index;
            });

            if (isBlocking) {
                if (!exist) {
                    $ctrl.blockingEntries.push({ NodeId: nodeId, Index: index });
                }
            } else {
                if (exist) {
                    $ctrl.blockingEntries = $ctrl.blockingEntries.filter(function (o) {
                        return o.NodeId !== nodeId && o.Index !== index;
                    });
                }
            }

            var isBlockingEntriesEmtpty = $ctrl.blockingEntries.length === 0;
            var emitEvent = $ctrl.isValidToSave !== isBlockingEntriesEmtpty;
            $ctrl.isValidToSave = isBlockingEntriesEmtpty;
            $ctrl.isValidTosaveByDay = $ctrl.isValidToSave;
            if (fromAllCI) {
                $ctrl.showErrorMessageDepassement = $ctrl.isValidToSave ? false : true;
            }

            if (emitEvent) {
                $scope.$emit('event.change.save.state', $ctrl.isValidToSave);
            }
        }

        $ctrl.isBlockingHoursNumberPerWeek = function (item) {
            var isBlocking = false;
            for (var l = 0; l < item.SubNodeList.length; l++) {
                var totale = 0;
                for (var m = 0; m < item.SubNodeList[l].Items.length; m++) {
                    totale += item.SubNodeList[l].Items[m].TotalHours + item.SubNodeList[l].Items[m].AbsenceHour;
                }
                if (totale > getLimitHourPerWeekBlocking()) {
                    isBlocking = true;
                    checkBlockingEntries(isBlocking, item.NodeId, 7);
                }
                else {
                    checkBlockingEntries(isBlocking, item.NodeId, 7);
                }
            }
        };

        /*
        * @description Obtenir le total des majorations dans une semaines  .
        */
        function GetTotalMajorationByWeek() {
            var rapportMajoration = $ctrl.rapportMajorationPanelData[0].SubNodeList;
            var totalMajNuit = 0;
            if (rapportMajoration.length > 0) {
                for (var nodeCi = 0; nodeCi < rapportMajoration.length; nodeCi++) {
                    for (var node = 0; node < rapportMajoration[nodeCi].SubNodeList.length; node++) {
                        for (var subnode = 0; subnode < rapportMajoration[nodeCi].SubNodeList[node].Items.length; subnode++) {
                            if (rapportMajoration[nodeCi].SubNodeList[node].Items[subnode].IsHeureNuit) {
                                for (var subNode = 0; subNode < rapportMajoration[nodeCi].SubNodeList[node].Items[subnode].MajorationHeurePerDayList.length; subNode++) {
                                    if (rapportMajoration[nodeCi].SubNodeList[node].Items[subnode].MajorationHeurePerDayList[subNode]) {
                                        totalMajNuit += rapportMajoration[nodeCi].SubNodeList[node].Items[subnode].MajorationHeurePerDayList[subNode].HeureMajoration;
                                    }
                                }
                            }
                        }
                    }
                }
                return totalMajNuit;
            }
        }


        /*
       * @description Obtenir le total des Majorations pour une journée .
       */
        function GetTotalMajorationByWeekAffichageParCi(personnel) {
            var result = 0;
            $ctrl.rapportMajorationPanelData.forEach(function (NodeCi) {
                var rapportMajoration = NodeCi.SubNodeList;
                if (rapportMajoration !== undefined && rapportMajoration.filter(x => x.NodeId === personnel).length > 0) {
                    var personnelRapportMajoration = rapportMajoration.filter(x => x.NodeId === personnel && !x.IsPersonnelCiDesaffected && !x.IsAbsence);
                    if (personnelRapportMajoration[0] !== undefined &&
                        personnelRapportMajoration[0].SubNodeList[0] !== undefined &&
                        personnelRapportMajoration[0].SubNodeList[0].Items !== undefined) {
                        personnelRapportMajoration[0].SubNodeList[0].Items.forEach(function (item) {
                            if (item.IsHeureNuit) {
                                item.MajorationHeurePerDayList.forEach(function (value) {
                                    if (value !== undefined) {
                                        result += value.HeureMajoration;
                                    }
                                });
                            }
                        });
                    }
                }
            });

            return result;
        }

        function CalculPerWeekOuvrier(personnel, ci) {
            var totalAllci = 0;
            for (var i = 0; i <= 6; i++) {
                totalAllci += filtrepersonnelForWeekAffichageOuvrier(personnel, i, ci);
            }
            return totalAllci;
        }

        $ctrl.isBlockingTotalHoursNumberPerWeek = function (item) {
            if ($ctrl.isAffichageParOuvrier) {
                var total = 0;
                if (item.SubNodeList) {
                    for (var j = 0; j < item.SubNodeList.length; j++) {
                        if (item.SubNodeList[j] && item.SubNodeList[j].Items) {
                            for (var k = 0; k < item.SubNodeList[j].Items.length; k++) {
                                total += item.SubNodeList[j].Items[k].TotalHours + item.SubNodeList[j].Items[k].AbsenceHour;
                                if (total + GetTotalMajorationByWeek() + CalculPerWeekOuvrier(item.NodeId, item.SubNodeList) > getLimitHourPerWeekBlocking()) {
                                    $ctrl.isValidTosaveByWeek = false;
                                    $scope.$emit('event.change.save.state', false);
                                }
                                else if ($ctrl.isValidTosaveByDay && $ctrl.isValidTosaveByDay === true) {
                                    $ctrl.isValidTosaveByWeek = true;
                                    $scope.$emit('event.change.save.state', true);
                                }
                            }
                        }
                    }
                }
            }
            else {
                for (var p = 0; p < $ctrl.personnelIdList.length; p++) {
                    var totale = 0;
                    var totalAllCi = 0;
                    var personnel = 0;
                    var ci = $ctrl.data[0].NodeId;
                    $ctrl.data.forEach(function (node) {
                        if (node.SubNodeList) {
                            node.SubNodeList.forEach(function (subnode) {
                                if ($ctrl.personnelIdList[p] === subnode.NodeId) {
                                    personnel = $ctrl.personnelIdList[p];
                                    if (subnode.Items) {
                                        subnode.Items.forEach(function (item) {
                                            totale += item.TotalHours + item.AbsenceHour;
                                        });
                                    }
                                }
                            });
                        }
                    });
                    for (var day = 0; day <= 6; day++) {
                        totalAllCi += filtrepersonnelForWeek(personnel, day, ci);
                    }
                    var getTotalMaj = GetTotalMajorationByWeekAffichageParCi(personnel);
                    if ($ctrl.data.length > 1) {
                        if (totale + getTotalMaj > getLimitHourPerWeekBlocking()) {
                            $scope.$emit('event.change.save.state', false);
                            $ctrl.isValidTosaveByWeek = false;
                            break;
                        }
                        else if ($ctrl.isValidTosaveByDay && $ctrl.isValidTosaveByDay === true) {
                            $ctrl.isValidTosaveByWeek = true;
                            $scope.$emit('event.change.save.state', true);
                        }
                    }
                    else {
                        if (totale + getTotalMaj + totalAllCi > getLimitHourPerWeekBlocking()) {
                            $scope.$emit('event.change.save.state', false);
                            $ctrl.isValidTosaveByWeek = false;
                            break;
                        }
                        else if ($ctrl.isValidTosaveByDay && $ctrl.isValidTosaveByDay === true) {
                            $ctrl.isValidTosaveByWeek = true;
                            $scope.$emit('event.change.save.state', true);
                        }
                    }
                }
            }
            return $ctrl.isValidTosaveByWeek;
        };

        function updateGlobalTotals() {
            if (!$ctrl.data && $ctrl.data.length === 0) {
                return;
            }

            var resultSupHour = 0;
            var resultTotal = updateGlobalTotalsForbothViews();
            if ($ctrl.isAffichageParOuvrier) {
                resultSupHour = resultTotal - $ctrl.limitHourPerWeekNotBlocking;
            }

            if (!$ctrl.isAffichageParOuvrier) {
                resultSupHour = updateGlobalTotalSupAffaire();
            }

            resultSupHour = resultSupHour && resultSupHour >= 0 ? resultSupHour : 0;
            $scope.$emit('event.change.totals', { total: resultTotal, supHour: resultSupHour });
        }

        function getLimitHourPerWeekBlocking() {
            if ($ctrl.isEtamIac === true && $ctrl.isManagerPointing === false) {
                return $ctrl.limitHourPerWeekBlockingForEtam;
            }
            return $ctrl.limitHourPerWeekBlocking;
        }

        function getSumFieldsTotalByLevel(subNode) {
            var result = 0;
            if (!subNode || !subNode.Items || subNode.Items.length === 0) {
                return result;
            }
            for (var i = 0; i < Object.keys(subNode.Items).length; i++) {
                result += getTotalHours(subNode, i);
            }

            return result;
        }
        function checkBlockingEntriesForValidation(index, isBlocking) {
            var exist = $ctrl.blockingEntriesForValidation && $ctrl.blockingEntriesForValidation.indexOf(index) > -1;

            if (isBlocking) {
                if (!exist) {
                    $ctrl.blockingEntriesForValidation.push(index);
                }
            } else {
                if (exist) {
                    $ctrl.blockingEntriesForValidation = $ctrl.blockingEntriesForValidation.filter(function (o) {
                        return o !== index;
                    });
                }
            }

            var isBlockingEntriesEmtpty = $ctrl.blockingEntriesForValidation.length === 0;
            var emitEvent = $ctrl.isValidToValidate !== isBlockingEntriesEmtpty;
            $ctrl.isValidToValidate = isBlockingEntriesEmtpty;
            if (emitEvent) {
                $scope.$emit('event.change.validate.state', $ctrl.isValidToValidate);
            }
        }

        $ctrl.checkPointageError = function (isAbsenceNode, total) {
            if (isAbsenceNode === true) {
                if (total === 0 || total === 0.5 || total === 1) {
                    return false;
                }
                return true;
            }
            else {
                if (total === 0 || total === 0.25 || total === 0.5 || total === 0.75 || total === 1) {
                    return false;
                }
                return true;
            }
        };

        $ctrl.handleShowErrorIAC = function (isAbsenceNode) {
            $ctrl.IacPointageError = new Array();
            if (isAbsenceNode) {
                $ctrl.IacPointageError.push($ctrl.resources.Pointage_IAC_AbsenceSaisie_Error);
            }
            else {
                $ctrl.IacPointageError.push($ctrl.resources.Pointage_IAC_TacheSaisie_Error);
            }
        };

        $ctrl.checkMaxHoursPerDay = function (item, index) {
            const days = 6;
            var result = false;
            if ($ctrl.isAffichageParOuvrier && (!$ctrl.isIac || !$ctrl.isEtamIac)) {
                for (var i = 0; i <= days; i++) {
                    if ($ctrl.checkSumFieldsForValidation(item.SubNodeList, i, index) || $ctrl.isBlockingForAllCi) {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        };

        function filtrepersonnelForWeekAffichageOuvrier(personnel, day, ci) {
            var result = 0;
            var ciId = ci.map(function (v) {
                return v.NodeId;
            });
            if ($ctrl.listPointagePersonnelForWeekParCi !== undefined) {
                for (var p = 0; p < $ctrl.listPointagePersonnelForWeekParCi.length; p++) {
                    if ($ctrl.listPointagePersonnelForWeekParCi[p].PersonnelId === personnel) {
                        for (var c = 0; c < $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi.length; c++) {
                            var curentCiId = $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi[c].CiId;
                            if (ciId.indexOf(curentCiId) === -1) {
                                var maj = $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi[c].TotalMajoration;
                                if (maj === undefined) maj = 0;
                                result += $ctrl.listPointagePersonnelForWeekParCi[p].ListTotalHours[day].ListTotalHourByCi[c].TotalHours + maj;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /*
         * @function $ctrl.getNodeTextOrNodeCode(task,parent,rootParent)
         * @description Obtenir le code ou libelle de tache d'un noeud.
         * @param {task} un noeud de rapport de type tache
         * @param {parent} un noeud de rapport de type CI en affichage par personnel ou type personnel en cas d'affichage par CI
         * @param {rootParent} un noeud de rapport de type personnel en affivhage par personnel ou type CI en cas d'affichage par CI
         * @returns {string} le code de noeud si CI est de type Etude ou le libelle de noeud si non
         */
        $ctrl.getNodeTextOrNodeCode = function (task, parent, rootParent) {
            if (task.NodeCode && (parent.NodeTypeLibelle === $ctrl.resources.CI_Search_CIType_Etude || rootParent.NodeTypeLibelle === $ctrl.resources.CI_Search_CIType_Etude)) {
                return task.NodeCode;
            }
            return task.NodeText;
        };

        function getLibelleTask(task) {
            if (task.NodeCode && task.NodeCode.length > 0) {
                return task.NodeCode.concat(' - ', task.NodeText);
            }
            return task.NodeText;
        }

        function commentaireNotNulleOrEmpty(item) {
            if (item) {
                if (item.Commentaire && item.Commentaire.length > 0) {
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return false;
            }
        }

        function getRapportLigneStatut(date) {
            return $ctrl.rapportLignesStatutList.find(function (e) { return e.DatePointage === date.replace('Z', ''); });
        }

        function updateGlobalTotalsForbothViews() {
            var resultTotal = 0;
            var resultMajoration = GetTotalMajorationByWeek();
            for (var i = 0; i < $ctrl.data.length; i++) {
                var item = $ctrl.data[i];
                if (item) {
                    for (var x = 0; x < item.SubNodeList.length; x++) {
                        var subNode = item.SubNodeList[x];
                        if (subNode) {
                            resultTotal += getSumFieldsTotalByLevel(subNode);
                        }
                    }

                    resultTotal += $ctrl.calculateSumFieldsAbsenceTotal(item.SubNodeList);
                }
            }

            resultTotal += resultMajoration;
            return resultTotal && resultTotal >= 0 ? resultTotal : 0;
        }

        function updateGlobalTotalSupAffaire() {
            var resultTotal = 0;
            for (var i = 0; i < $ctrl.data.length; i++) {
                var item = $ctrl.data[i];
                if (item && item.SubNodeList && !item.IsAbsence) {
                    for (var j = 0; j < item.SubNodeList.length; j++) {
                        var subNode = item.SubNodeList[j];
                        var result = 0;
                        for (var k = 0; k < Object.keys(subNode.Items).length; k++) {
                            result += getTotalHours(subNode, k);
                        }

                        var resultSup = result && result - $ctrl.limitHourPerWeekNotBlocking > 0 ? result - $ctrl.limitHourPerWeekNotBlocking : 0;
                        resultTotal += resultSup;
                    }
                }
            }

            return resultTotal;
        }
    }
})();