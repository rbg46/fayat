(function () {
    'use strict';

    var pointageHebdoRapportPrimeTabComponent = {
        templateUrl: '/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-prime-tab.html',
        bindings: {
            resources: '<',
            isEtamIac: '<',
            groupeId: '<',
            mondayDate: '<',
            isAffichageWorker: '<',
            isManagerPointing: '<'
        },
        controller: PointageHebdoRapportPrimeTabController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('pointageHebdoRapportPrimeTabComponent', pointageHebdoRapportPrimeTabComponent);

    angular.module('Fred').controller('PointageHebdoRapportPrimeTabController', PointageHebdoRapportPrimeTabController);

    PointageHebdoRapportPrimeTabController.$inject = ['$scope', 'PointageHedboService', 'Notify', 'UserService'];

    function PointageHebdoRapportPrimeTabController($scope, PointageHedboService, Notify, UserService) {
        var $ctrl = this;
        $scope.resources = resources;
        $ctrl.isAffichageParOuvrier = false;
        $ctrl.subNodeList = [];

        UserService.getCurrentUser().then(function (user) {
            $ctrl.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;
        });

        $scope.PrimeCodes = {
            PrimeIR: "IR",
            PrimeTR: "TR",
            PrimeES: "ES",
            PrimeINS: "INS",
            PrimeAE: "AE"
        };

        $ctrl.affectedPrimeList;
        $ctrl.PrimePersonnelId = 0;
        $ctrl.PrimeCiId = 0;
        if (!$ctrl.isEtamIac) {
            $ctrl.isEtamIac = false;
        }

        $scope.$on('event.refresh.pointage.hebdo.prime.panel', function (evt, data) {
            $ctrl.data = data.rapportPrimePanelData;
            $ctrl.isAffichageParOuvrier = data.isAffichageParOuvrier;
        });
        $scope.$on('event.refresh.prime.personnel.affected', function (evt, data) {
            $ctrl.personnelIdList = data.personnelIdList;
            if (data.data !== undefined) {
                $ctrl.subNodeList = data.data[0].SubNodeList;
            }

            primePersonnelAffected();
        });

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        //Fonction d'initialisation des données de la picklist 
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

        $scope.handleLookupUrl = function (ciId) {
            var basePrimeByGroupeControllerUrl = '/api/Prime/SearchLight/?groupeId={0}&ciId={1}&isRapportPrime={2}&';
            basePrimeByGroupeControllerUrl = String.format(basePrimeByGroupeControllerUrl, $ctrl.groupeId, ciId, $ctrl.isEtamIac);
            return basePrimeByGroupeControllerUrl;
        };

        //Fonction de chargement des données de l'item sélectionné dans la picklist
        $scope.handleLookupSelection = function (prime, subNode, ciAffichageCI) {
            var primeList = subNode.SubNodeList[0].Items;
            // Test d'existance de la prime dans le rapport
            if (!actionContainsObject(prime, primeList)) {
                var ciId = $ctrl.isAffichageParOuvrier ? subNode.NodeId : ciAffichageCI.NodeId;
                var personnelId = $ctrl.isAffichageParOuvrier ? ciAffichageCI.NodeId : subNode.NodeId;
                $ctrl.rapportLignesStatutList = [];
                PointageHedboService.GetRapportLigneStatutForNewPointage({ personnelId: personnelId, ciId: ciId, mondayDate: $ctrl.mondayDate }).$promise
                    .then(function (value) {
                        $ctrl.rapportLignesStatutList = value;
                        var primeHeurePerDayList = [];
                        for (var i = 0; i <= 6; i++) {
                            var rapportLigneStatut = getRapportLigneStatut(i);
                            var primePerDay = {
                                DayOfWeek: i,
                                HeurePrime: prime.PrimeType === 0 ? null : 0,
                                RapportLigneId: 0,
                                HeurePrimeOldValue: prime.PrimeType === 0 ? null : 0,
                                IsChecked: false,
                                datePointage: moment($ctrl.mondayDate).add(i, 'days'),
                                PersonnelVerrouille: rapportLigneStatut.IsRapportLigneVerouiller,
                                RapportValide: rapportLigneStatut.IsRapportLigneValide2
                            };
                            primeHeurePerDayList.push(primePerDay);
                        }
                        var primeToAdd = {
                            PrimeId: prime.PrimeId,
                            PrimeCode: prime.Code,
                            PrimeLibelle: prime.Libelle,
                            IsPrimeJournaliere: prime.PrimeType === 0 ? true : false,
                            RapportHebdoPrimePerDayList: primeHeurePerDayList
                        };

                        primeList.push(primeToAdd);
                    });
            }
            else {
                Notify.warning($scope.resources.Rapport_RapportController_PrimeExisteDeja_Info);
            }

        };

        // Fonction de test d'existence d'un objet de type rérential dans une liste
        function actionContainsObject(obj, list) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].PrimeId === obj.PrimeId) {
                    return true;
                }
            }
            return false;
        }

        $scope.handleTotalsPrimesPerDay = function (SubNodes, day) {
            var somme = 0;
            if (SubNodes) {
                SubNodes.forEach(function (SubNode) {
                    if (!SubNode.IsPersonnelCiDesaffected && !SubNode.IsAbsence) {
                        somme += $scope.handleTotalPrimePerDay(SubNode.SubNodeList, day);
                    }
                });
            }

            return somme;
        };


        $scope.calculateSumFieldsTotal = function (SubNode) {
            var somme = 0;
            if (SubNode) {
                for (var i = 0; i <= 6; i++) {
                    somme += $scope.handleTotalsPrimesPerDay(SubNode, i);
                }
            }

            return somme;
        };


        /// Function calculate total hours prime per day
        $scope.handleTotalPrimePerDay = function (personnelSubNodeList, day) {
            var total = 0;
            var res = 0;
            if (personnelSubNodeList && personnelSubNodeList[0]) {
                var personnelPrime = personnelSubNodeList[0].Items;
                if (personnelPrime.length > 0) {
                    for (var i = 0; i < personnelPrime.length; i++) {
                        if (personnelPrime[i].RapportHebdoPrimePerDayList[day] !== undefined && personnelPrime[i].RapportHebdoPrimePerDayList[day].IsChecked) {
                            total++;
                        }
                    }
                }

                res = isNaN(total) ? 0 : total;
                $scope.$emit('event.refresh.visible.commentaire', { Index: day, HasHour: res > 0, Ecran: "Prime", Id: personnelSubNodeList[0].NodeId });
            }

            return res;
        };

        /// Function calculate total hours majoartion per week
        $scope.handleTotalWeekPourOuvrier = function (primeList) {
            var totalHeurePrime = 0;
            for (var i = 0; i < 7; i++) {
                totalHeurePrime += $scope.handleTotalPrimePerDay(primeList, i);
            }
            return isNaN(totalHeurePrime) ? 0 : totalHeurePrime;
        };

        /// Function handle change prime value
        $ctrl.handleChangePrime = function (prime, jour, ciIdAffichageCi, ciIdAffichageworker, personnelCiPrimeList, list) {
            $ctrl.affectedPersonnelPrime = [];
            GetPersonnelPrimeAffected(ciIdAffichageCi, ciIdAffichageworker);

            if (prime.RapportHebdoPrimePerDayList[jour]) {
                if (prime.PrimeCode.trim() === $scope.PrimeCodes.PrimeIR.trim() || prime.PrimeCode.trim() === $scope.PrimeCodes.PrimeTR.trim()) {
                    handlePrimeIRTRAffectation(prime, jour);
                }

                if (prime.PrimeCode.trim() === $scope.PrimeCodes.PrimeES.trim() || prime.PrimeCode.trim() === $scope.PrimeCodes.PrimeINS.trim()) {
                    if ($ctrl.isAffichageWorker) {
                        handlePrimeEsInsAffectation(prime, jour, ciIdAffichageworker);
                    }
                    else {
                        handlePrimeEsInsAffectation(prime, jour, ciIdAffichageCi);
                    }
                }

                if (prime.PrimeCode.trim() === $scope.PrimeCodes.PrimeAE.trim()) {
                    if ($ctrl.isAffichageWorker) {
                        handlePrimeAEAffectation(prime, jour, ciIdAffichageworker, personnelCiPrimeList);
                    }
                    else {
                        handlePrimeAEAffectation(prime, jour, ciIdAffichageCi, personnelCiPrimeList);
                    }
                }
                if (prime.PrimeCode.trim() !== $scope.PrimeCodes.PrimeES.trim() && prime.PrimeCode.trim() !== $scope.PrimeCodes.PrimeIR.trim()
                    && prime.PrimeCode.trim() !== $scope.PrimeCodes.PrimeTR.trim() && prime.PrimeCode.trim() !== $scope.PrimeCodes.PrimeAE.trim()) {
                    if (prime.RapportHebdoPrimePerDayList[jour].RapportLigneId > 0) {
                        prime.RapportHebdoPrimePerDayList[jour].IsUpdated = true;
                    }
                    else {
                        prime.RapportHebdoPrimePerDayList[jour].IsCreated = true;
                    }
                }
                ReaffectNewPersonnelPrime();
            }
        };

        /// Function calculate hours prime per a majoartion
        $scope.handleTotalDayPrime = function (prime) {
            var heurePrime = 0;
            for (var i = 0; i < prime.RapportHebdoPrimePerDayList.length; i++) {
                if (prime.RapportHebdoPrimePerDayList[i] !== undefined && prime.RapportHebdoPrimePerDayList[i].IsChecked) {
                    heurePrime++;
                }
            }
            return heurePrime;
        };

        function primePersonnelAffected() {
            var model = { datePointage: $ctrl.mondayDate, personnelIdList: $ctrl.personnelIdList };
            return PointageHedboService.PrimePersonnelAffected(model)
                .$promise
                .then(function (value) {
                    $ctrl.affectedPrimeList = value;
                });
        }

        /// Function handle affectation des primes IR | TR
        function handlePrimeIRTRAffectation(prime, jour) {
            if (prime.PrimeCode.trim() === $scope.PrimeCodes.PrimeIR.trim()) {
                handlePrimeTRIR(prime, jour, $scope.PrimeCodes.PrimeIR.trim(), $scope.PrimeCodes.PrimeTR.trim());
            }
            else {
                handlePrimeTRIR(prime, jour, $scope.PrimeCodes.PrimeTR.trim(), $scope.PrimeCodes.PrimeIR.trim());
            }
        }

        /// function handle affectation des primes ES | INS
        function handlePrimeEsInsAffectation(prime, jour, ciId) {
            var primeAEAffected = $ctrl.affectedPersonnelPrime.find(function (element) {
                return element.AffectationDay === jour && element.CodePrime.trim() === $scope.PrimeCodes.PrimeAE.trim() && element.CiId === ciId;
            });
            if (prime.RapportHebdoPrimePerDayList[jour].RapportLigneId > 0) {
                if (primeAEAffected && primeAEAffected.IsAffected) {
                    prime.RapportHebdoPrimePerDayList[jour].IsChecked = false;
                    prime.RapportHebdoPrimePerDayList[jour].IsUpdated = true;
                    Notify.warning($scope.resources.Rapport_RapportController_PrimeAE_Info);
                }
                else {
                    $ctrl.affectedPersonnelPrime.push({
                        AffectationDay: jour,
                        CodePrime: prime.PrimeCode.trim(),
                        CiId: ciId,
                        IsAffected: prime.RapportHebdoPrimePerDayList[jour].IsChecked
                    });
                    prime.RapportHebdoPrimePerDayList[jour].IsUpdated = true;
                }
            }
            else {
                prime.RapportHebdoPrimePerDayList[jour].IsCreated = true;
                if (primeAEAffected && primeAEAffected.IsAffected) {
                    prime.RapportHebdoPrimePerDayList[jour].IsChecked = false;
                    Notify.warning($scope.resources.Rapport_RapportController_PrimeAE_Info);
                }
                else {
                    $ctrl.affectedPersonnelPrime.push({
                        AffectationDay: jour,
                        CodePrime: prime.PrimeCode.trim(),
                        CiId: ciId,
                        IsAffected: prime.RapportHebdoPrimePerDayList[jour].IsChecked
                    });
                }
            }
        }

        /// Function handle AE prime
        function handlePrimeAEAffectation(prime, jour, ciId, personnelCiPrimeList) {
            var primeAEAffected = $ctrl.affectedPersonnelPrime.find(function (element) {
                return element.AffectationDay === jour && element.CodePrime === $scope.PrimeCodes.PrimeAE.trim() && element.CiId === ciId;
            });
            var primeAEAffectedIndex = $ctrl.affectedPersonnelPrime.indexOf(primeAEAffected);

            if (prime.RapportHebdoPrimePerDayList[jour].RapportLigneId > 0) {
                if (primeAEAffected) {
                    if (!prime.RapportHebdoPrimePerDayList[jour].IsChecked) {
                        $ctrl.affectedPersonnelPrime[primeAEAffectedIndex].IsAffected = prime.RapportHebdoPrimePerDayList[jour].IsChecked;
                    }
                    else {
                        $ctrl.affectedPersonnelPrime[primeAEAffectedIndex].IsAffected = prime.RapportHebdoPrimePerDayList[jour].IsChecked;
                        UpdateEsInsPrimesWhenAEPrimeAffected(jour, personnelCiPrimeList, ciId);
                    }
                }
                prime.RapportHebdoPrimePerDayList[jour].IsUpdated = true;
            }
            else {
                prime.RapportHebdoPrimePerDayList[jour].IsCreated = true;
                if (primeAEAffected) {
                    $ctrl.affectedPersonnelPrime[primeAEAffectedIndex].IsAffected = prime.RapportHebdoPrimePerDayList[jour].IsChecked;
                    if (prime.RapportHebdoPrimePerDayList[jour].IsChecked) {
                        UpdateEsInsPrimesWhenAEPrimeAffected(jour, personnelCiPrimeList, ciId);
                    }
                }
                else {
                    $ctrl.affectedPersonnelPrime.push({
                        AffectationDay: jour,
                        CodePrime: prime.PrimeCode.trim(),
                        CiId: ciId,
                        IsAffected: prime.RapportHebdoPrimePerDayList[jour].IsChecked
                    });
                    UpdateEsInsPrimesWhenAEPrimeAffected(jour, personnelCiPrimeList, ciId);
                }
            }
        }

        /// Update Prime ES | INS mettre isChecked false if AE prime affected
        function UpdateEsInsPrimesWhenAEPrimeAffected(jour, personnelCiPrimeList, ciId) {
            var primeASAffected = $ctrl.affectedPersonnelPrime.find(function (element) {
                return element.AffectationDay === jour && element.CodePrime === $scope.PrimeCodes.PrimeES.trim() && element.CiId === ciId;
            });
            var primeES = personnelCiPrimeList.find(function (element) {
                return element.PrimeCode === $scope.PrimeCodes.PrimeES.trim();
            });
            if (primeASAffected && primeES && primeES.RapportHebdoPrimePerDayList[jour].IsChecked) {
                var primeAEAffectedIndex = $ctrl.affectedPersonnelPrime.indexOf(primeASAffected);
                $ctrl.affectedPersonnelPrime[primeAEAffectedIndex].IsAffected = false;
                primeES.RapportHebdoPrimePerDayList[jour].IsChecked = false;
                primeES.RapportHebdoPrimePerDayList[jour].IsUpdated = true;
            }

            var primeINSAffected = $ctrl.affectedPersonnelPrime.find(function (element) {
                return element.AffectationDay === jour && element.CodePrime === $scope.PrimeCodes.PrimeINS.trim() && element.CiId === ciId;
            });
            var primeINS = personnelCiPrimeList.find(function (element) {
                return element.PrimeCode === $scope.PrimeCodes.PrimeINS.trim();
            });
            if (primeINSAffected && primeINS.RapportHebdoPrimePerDayList[jour].IsChecked) {
                var primeINSAffectedIndex = $ctrl.affectedPersonnelPrime.indexOf(primeINSAffected);
                $ctrl.affectedPersonnelPrime[primeINSAffectedIndex].IsAffected = false;
                primeINS.RapportHebdoPrimePerDayList[jour].IsChecked = false;
                primeINS.RapportHebdoPrimePerDayList[jour].IsUpdated = true;
            }
        }

        function GetPersonnelPrimeAffected(ciIdAffichageCi, ciIdAffichageworker) {
            if ($ctrl.isAffichageWorker) {
                $ctrl.PrimePersonnelId = ciIdAffichageCi;
                $ctrl.PrimeCiId = ciIdAffichageworker;
            }
            else {
                $ctrl.PrimePersonnelId = ciIdAffichageworker;
                $ctrl.PrimeCiId = ciIdAffichageCi;
            }

            if ($ctrl.affectedPrimeList) {
                for (var i = 0; i < $ctrl.affectedPrimeList.length; i++) {
                    if ($ctrl.PrimePersonnelId === $ctrl.affectedPrimeList[i].PersonnelId) {
                        $ctrl.affectedPersonnelPrime = $ctrl.affectedPrimeList[i].PrimeList;
                        break;
                    }
                }
            }
        }

        function ReaffectNewPersonnelPrime() {
            if ($ctrl.affectedPrimeList) {
                for (var i = 0; i < $ctrl.affectedPrimeList.length; i++) {
                    if ($ctrl.PrimePersonnelId === $ctrl.affectedPrimeList[i].PersonnelId) {
                        $ctrl.affectedPrimeList[i].PrimeList = $ctrl.affectedPersonnelPrime;
                        break;
                    }
                }
            }
        }

        function handlePrimeTRIR(prime, jour, newPrimeCode, existPrimeCode) {
            var isPrimeAffected = $ctrl.affectedPersonnelPrime.find(function (element) {
                return element.AffectationDay === jour && element.CodePrime === newPrimeCode && element.CiId === $ctrl.PrimeCiId;
            });

            var isOppositePrimeAffected = $ctrl.affectedPersonnelPrime.find(function (element) {
                return element.AffectationDay === jour && element.CodePrime === existPrimeCode && element.IsAffected;
            });

            if (isOppositePrimeAffected) {
                prime.RapportHebdoPrimePerDayList[jour].IsChecked = false;
                prime.RapportHebdoPrimePerDayList[jour].IsUpdated = true;
                Notify.warning($scope.resources.Rapport_RapportController_PrimeIRTRExisteDeja_Info);
            }
            else {
                if (isPrimeAffected) {
                    var primeNewAffectedIndex = $ctrl.affectedPersonnelPrime.indexOf(isPrimeAffected);
                    if (prime.RapportHebdoPrimePerDayList[jour].RapportLigneId > 0) {
                        $ctrl.affectedPersonnelPrime[primeNewAffectedIndex].IsAffected = prime.RapportHebdoPrimePerDayList[jour].IsChecked;
                        prime.RapportHebdoPrimePerDayList[jour].IsUpdated = true;
                    }
                    else {
                        $ctrl.affectedPersonnelPrime[primeNewAffectedIndex].IsAffected = prime.RapportHebdoPrimePerDayList[jour].IsChecked;
                        if ($ctrl.affectedPersonnelPrime[primeNewAffectedIndex].IsAffected) {
                            prime.RapportHebdoPrimePerDayList[jour].IsCreated = true;
                        }
                    }
                }
                else {
                    $ctrl.affectedPersonnelPrime.push({
                        CodePrime: prime.PrimeCode.trim(),
                        AffectationDay: jour,
                        CiId: $ctrl.PrimeCiId,
                        IsAffected: prime.RapportHebdoPrimePerDayList[jour].IsChecked
                    });
                    prime.RapportHebdoPrimePerDayList[jour].IsCreated = true;
                }
            }
        }

        function getRapportLigneStatut(index) {
            return $ctrl.rapportLignesStatutList.find(function (e) { return e.DayOfWeekIndex === index; });
        }
    }
})();