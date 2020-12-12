(function () {
    'use strict';

    var pointageHebdoRapportMajorationTabComponent = {
        templateUrl: '/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-majoration-tab.html',
        bindings: {
            resources: '<',
            isEtamIac: '<',
            groupeId: '<',
            mondayDate: '<',
            isManagerPointing: '<'
        },
        controller: PointageHebdoRapportMajorationTabController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('pointageHebdoRapportMajorationTabComponent', pointageHebdoRapportMajorationTabComponent);

    angular.module('Fred').controller('PointageHebdoRapportMajorationTabController', PointageHebdoRapportMajorationTabController);

    PointageHebdoRapportMajorationTabController.$inject = ['$scope', 'PointageHedboService', 'Notify', 'UserService'];

    function PointageHebdoRapportMajorationTabController($scope, PointageHedboService, Notify, UserService) {
        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.subNodeList = [];
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
        };

        $ctrl.NmbreMaxMajorations = 5;
        $ctrl.isAffichageParOuvrier = true;

        UserService.getCurrentUser().then(function(user) {
            $ctrl.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;
        });

        $ctrl.Math = Math;
        $ctrl.affectedMajorationList;
        $scope.$on('event.refresh.pointage.hebdo.majoration.panel', function (evt, data) {
            $ctrl.data = data.rapportMajorationPanelData;
            $ctrl.status = $ctrl.data[0].Statut;
            $ctrl.isAffichageParOuvrier = data.isAffichageParOuvrier;
            if ($ctrl.isEtamIac === undefined || $ctrl.isEtamIac === null) {
                $ctrl.isEtamIac = false;
            }
        });

        $scope.$on('event.refresh.prime.personnel.affected', function (evt, data) {
            $ctrl.personnelIdList = data.personnelIdList;
            if (data.data !== undefined) {
                $ctrl.subNodeList = data.data[0].SubNodeList;
            }

            majorationPersonnelAffected();
        });

        function majorationPersonnelAffected() {
            var model = { datePointage: $ctrl.mondayDate, personnelIdList: $ctrl.personnelIdList };
            return PointageHedboService.MajorationPersonnelAffected(model)
                .$promise
                .then(function (value) {
                    $ctrl.affectedMajorationList = value;
                });
        }

        //Fonction de chargement des données de l'item sélectionné dans la picklist
        $scope.handleLookupSelection = function (item, subNode, ciAffichageCI) {
            var ciId = $ctrl.isAffichageParOuvrier ? subNode.NodeId : ciAffichageCI.NodeId;
            var personnelId = $ctrl.isAffichageParOuvrier ? ciAffichageCI.NodeId : subNode.NodeId;
            var majorationList = subNode.SubNodeList[0].Items;
            if (majorationList.length < $ctrl.NmbreMaxMajorations) {
                if (!actionContainsObject(item, majorationList)) {
                    $ctrl.rapportLignesStatutList = [];
                    PointageHedboService.GetRapportLigneStatutForNewPointage({ personnelId: personnelId, ciId: ciId, mondayDate: $ctrl.mondayDate }).$promise
                        .then(function (value) {
                            $ctrl.rapportLignesStatutList = value;
                            var majorationHeurePerDayList = [];
                            for (var i = 0; i <= 6; i++) {
                                var rapportLigneStatut = getRapportLigneStatut(i);
                                var pointagePerDay = {
                                    DayOfWeek: i,
                                    HeureMajoration: 0,
                                    RapportLigneId: 0,
                                    HeureMojorationOldValue: 0,
                                    datePointage: moment($ctrl.mondayDate).add(i, 'days'),
                                    PersonnelVerrouille: rapportLigneStatut.IsRapportLigneVerouiller,
                                    RapportValide: rapportLigneStatut.IsRapportLigneValide2
                                };
                                majorationHeurePerDayList.push(pointagePerDay);
                            }
                            var majorationToAdd = {
                                CodeMajorationId: item.CodeMajorationId,
                                CodeMajoration: item.Code,
                                IsHeureNuit: item.IsHeureNuit,
                                MajorationHeurePerDayList: majorationHeurePerDayList
                            };

                            majorationList.push(majorationToAdd);
                        });
                }
                else {
                    Notify.warning($scope.resources.Rapport_RapportController_MajorationExisteDeja_Info);
                }
            }
            else {
                Notify.warning($scope.resources.Rapport_RapportController_NbrMaxMajoration_Info);
            }
            // Test d'existance de la majoration dans le rapport

        };

        // Fonction de test d'existence d'un objet de type rérential dans une liste
        function actionContainsObject(obj, list) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].CodeMajorationId === obj.CodeMajorationId) {
                    return true;
                }
            }
            return false;
        }

        /// Function calculate total hours majoration per day
        $scope.handleTotalMajorationPerDay = function (personnelSubNodeList, day) {
            var res = 0;
            if (personnelSubNodeList[0]) {
                var personnelMajoration = personnelSubNodeList[0].Items;
                var total = 0;
                for (var i = 0; i < personnelMajoration.length; i++) {
                    var majorationHeurePerDayList = personnelMajoration[i].MajorationHeurePerDayList[day].HeureMajoration;
                    total += isNaN(majorationHeurePerDayList) ? 0 : majorationHeurePerDayList;
                }

                res = isNaN(total) ? 0 : total;
                $scope.$emit('event.refresh.visible.commentaire', { Index: day, HasHour: res > 0, Ecran: "Majoration", Id: personnelSubNodeList[0].NodeId });
            }

            return res > 10 ? 10 : res;
        };


        /*
        * @description calcul du total des éléments d'une liste.
        */
        $ctrl.calculateSumFieldsTotal = function (SubNodeList) {
            var result = 0;
            for (var i = 0; i <= 6; i++) {
                result += $ctrl.totalMajorationParDayForAllCi(SubNodeList, i);
            }
            return result;
        };


        /// Function calculate total majoration per day for all CI
        $ctrl.totalMajorationParDayForAllCi = function (SubNodeList, day) {
            var data = SubNodeList;
            var totalDay = 0;
            for (var i = 0; i < data.length; i++) {
                if (!data[i].IsPersonnelCiDesaffected && !data[i].IsAbsence) {
                    var ListByCi = data[i].SubNodeList[0].Items;
                    if (ListByCi === undefined) continue;
                    for (var j = 0; j < ListByCi.length; j++) {
                        totalDay += isNaN(ListByCi[j].MajorationHeurePerDayList[day].HeureMajoration) ? 0 : ListByCi[j].MajorationHeurePerDayList[day].HeureMajoration;
                    }
                }
            }

            return totalDay;
        };


        /// Function calculate max add hours majoration per day in display by CI
        $scope.stopAddMajorationPerDayInAffichageParCi = function (personnelSubNodeList, day, value) {
            return value === undefined || value === null || value.length <= 0 ? 10 - $scope.handleTotalMajorationPerDay(personnelSubNodeList, day) : 10 - $scope.handleTotalMajorationPerDay(personnelSubNodeList, day) + value;
        };





        /// Function calculate max add hours majoration per day for all CI  in display by Ouvrier
        $scope.disableAddMajorationPerDayForAllCi = function (day, value) {
            var data = $ctrl.data[0].SubNodeList;
            var totalDay = 0;
            for (var i = 0; i < data.length; i++) {
                if (data[i].SubNodeList && data[i].SubNodeList.length > 0) {
                    var ListByCi = data[i].SubNodeList[0].Items;
                    for (var j = 0; j < ListByCi.length; j++) {
                        totalDay += isNaN(ListByCi[j].MajorationHeurePerDayList[day].HeureMajoration) ? 0 : ListByCi[j].MajorationHeurePerDayList[day].HeureMajoration;
                    }
                }
            }
            return value === undefined || value === null || value.length <= 0 ? 10 - totalDay : 10 - totalDay + value;
        };

        /// Function calculate hours majoration per a majoartion
        $scope.handleTotalDayMajoration = function (majoration) {
            var heureMajoration = 0;
            for (var i = 0; i < majoration.MajorationHeurePerDayList.length; i++) {
                var heureMajorationPerDay = majoration.MajorationHeurePerDayList[i].HeureMajoration;
                heureMajoration += isNaN(heureMajorationPerDay) ? 0 : heureMajorationPerDay;
            }

            return isNaN(heureMajoration) ? 0 : heureMajoration;
        };

        /// Function calculate total hours majoartion per week
        $scope.handleTotalWeekPourOuvrier = function (majorationList) {
            var totalHeureMajoration = 0;
            if (majorationList) {
                for (var i = 0; i < 7; i++) {
                    totalHeureMajoration += $scope.handleTotalMajorationPerDay(majorationList, i);
                }
            }

            return isNaN(totalHeureMajoration) ? 0 : totalHeureMajoration;
        };

        /// Function handle change hour majoration value
        $ctrl.handleChangeHeureMajoration = function (majoration, list, sub, data, courante) {
            if ($ctrl.isUserFes) {
                checkMajorationAllerRetour(majoration, list, sub, data, courante);
            }
            if (majoration) {
                if (majoration.HeureMajoration !== majoration.HeureMojorationOldValue && majoration.RapportLigneId > 0) {
                    majoration.IsUpdated = true;
                }
                if (majoration.RapportLigneId === 0 && majoration.HeureMajoration > 0) {
                    majoration.IsCreated = true;
                }
            }
        };
        // Vérifier les majoration de trajet Aller/Retour pour Fes
        function checkMajorationAllerRetour(majoration, list, sub, data, courante) {
            var day = majoration.DayOfWeek;
            var haveMajorationRouteDbo = false;
            $ctrl.listGlobalOuvrier = new Array();
            $ctrl.listGlobalci = new Array();
            var majorationAller = 5;
            var majorationRetour = 6;
            var majorationcourante = 0;

            //Récupération des donnés depuis la base
            for (var a = 0; a < $ctrl.affectedMajorationList[0].MajorationList.length; a++) {
                if ($ctrl.affectedMajorationList[0].MajorationList[a].CodeMajorationId === majorationAller && $ctrl.affectedMajorationList[0].MajorationList[a].AffectationDay === day ||
                    $ctrl.affectedMajorationList[0].MajorationList[a].CodeMajorationId === majorationRetour && $ctrl.affectedMajorationList[0].MajorationList[a].AffectationDay === day) {
                    haveMajorationRouteDbo = true;
                }
            }

            //Par ouvrier
            if ($ctrl.isAffichageParOuvrier) {
                var personnelcourant = list[0].NodeId;
                if (courante.CodeMajorationId === majorationAller || courante.CodeMajorationId === majorationRetour) {
                    //Verifier sur un Ci
                    for (var y = 0; y < list[0].Items.length; y++) {
                        if (list[0].Items[y].CodeMajorationId === majorationAller && list[0].Items[y].MajorationHeurePerDayList[day].HeureMajoration > 0 ||
                            list[0].Items[y].CodeMajorationId === majorationRetour && list[0].Items[y].MajorationHeurePerDayList[day].HeureMajoration > 0) {
                            if (haveMajorationRouteDbo) {
                                alertMajorationAllerRetour(majoration);
                            }
                            else {
                                majorationcourante = list[0].Items[y].CodeMajorationId;
                                break;
                            }
                        }
                    }

                    for (var i = 0; i < list[0].Items.length; i++) {
                        if (majorationcourante !== undefined) {
                            if (list[0].Items[i].CodeMajorationId !== majorationcourante && list[0].Items[i].MajorationHeurePerDayList[day].HeureMajoration > 0 &&
                                (list[0].Items[i].CodeMajorationId === majorationAller || list[0].Items[i].CodeMajorationId === majorationRetour)) {
                                alertMajorationAllerRetour(majoration);
                            }
                            else {
                                //Verifier sur tout les ci
                                $ctrl.listGlobalOuvrier = sub.SubNodeList.filter(x => x.NodeId !== personnelcourant);
                                for (var ci = 0; ci < $ctrl.listGlobalOuvrier.length; ci++) {
                                    if ($ctrl.listGlobalOuvrier[ci].SubNodeList && $ctrl.listGlobalOuvrier[ci].SubNodeList.length) {
                                        for (var node = 0; node < $ctrl.listGlobalOuvrier[ci].SubNodeList[0].Items.length; node++) {
                                            if ($ctrl.listGlobalOuvrier[ci].SubNodeList[0].Items[node].CodeMajorationId === majorationAller && $ctrl.listGlobalOuvrier[ci].SubNodeList[0].Items[node].MajorationHeurePerDayList[day].HeureMajoration > 0 ||
                                                $ctrl.listGlobalOuvrier[ci].SubNodeList[0].Items[node].CodeMajorationId === majorationRetour && $ctrl.listGlobalOuvrier[ci].SubNodeList[0].Items[node].MajorationHeurePerDayList[day].HeureMajoration > 0) {
                                                alertMajorationAllerRetour(majoration);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            //Par Ci
            else {
                var ciicourant = sub.NodeId;
                var personnelcourants = list[0].NodeId;
                haveMajorationRouteDbo = false;
                if (courante.CodeMajorationId === majorationAller || courante.CodeMajorationId === majorationRetour) {
                    //verifier sur un ci
                    for (var b = 0; b < list[0].Items.length; b++) {
                        if (list[0].Items[b].CodeMajorationId === majorationAller && list[0].Items[b].MajorationHeurePerDayList[day].HeureMajoration > 0 ||
                            list[0].Items[b].CodeMajorationId === majorationRetour && list[0].Items[b].MajorationHeurePerDayList[day].HeureMajoration > 0) {
                            if (haveMajorationRouteDbo) {
                                alertMajorationAllerRetour(majoration);
                            }
                            else {
                                majorationcourante = list[0].Items[b].CodeMajorationId;
                                break;
                            }
                        }
                    }
                    for (var index = 0; index < list[0].Items.length; index++) {
                        if (majorationcourante !== undefined) {
                            if (list[0].Items[index].CodeMajorationId !== majorationcourante && list[0].Items[index].MajorationHeurePerDayList[day].HeureMajoration > 0 &&
                                (list[0].Items[index].CodeMajorationId === majorationAller || list[0].Items[index].CodeMajorationId === majorationRetour)) {
                                alertMajorationAllerRetour(majoration);
                            }
                            else {
                                // verifier sur tout les ci
                                $ctrl.listGlobalOuvrier = data.filter(x => x.NodeId !== ciicourant);
                                for (ci = 0; ci < $ctrl.listGlobalOuvrier.length; ci++) {
                                    for (node = 0; node < $ctrl.listGlobalOuvrier[ci].SubNodeList.length; node++) {
                                        if ($ctrl.listGlobalOuvrier[ci].SubNodeList[node].NodeId === personnelcourants) {
                                            for (var subnode = 0; subnode < $ctrl.listGlobalOuvrier[ci].SubNodeList[node].SubNodeList[0].Items.length; subnode++) {
                                                if ($ctrl.listGlobalOuvrier[ci].SubNodeList[node].SubNodeList[0].Items[subnode].CodeMajorationId === majorationAller && $ctrl.listGlobalOuvrier[ci].SubNodeList[node].SubNodeList[0].Items[subnode].MajorationHeurePerDayList[day].HeureMajoration > 0 ||
                                                    $ctrl.listGlobalOuvrier[ci].SubNodeList[node].SubNodeList[0].Items[subnode].CodeMajorationId === majorationRetour && $ctrl.listGlobalOuvrier[ci].SubNodeList[node].SubNodeList[0].Items[subnode].MajorationHeurePerDayList[day].HeureMajoration > 0) {
                                                    alertMajorationAllerRetour(majoration);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        function alertMajorationAllerRetour(majoration) {
            Notify.error($scope.resources.RapportHebdo_Error_MajorationAller_Retour);
            majoration.HeureMajoration = 0;
            return;
        }

        function getRapportLigneStatut(index) {
            var newIndex = index + 1;
            if (newIndex === 7) {
                newIndex = 0;
            }
            return $ctrl.rapportLignesStatutList.find(function (e) { return e.DayOfWeekIndex === newIndex; });
        }
    }
})();