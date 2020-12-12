(function (angular) {
    'use strict';

    angular.module('Fred').controller('ExportController', ExportController);

    ExportController.$inject = ['ProgressBar', 'Notify', 'UtilisateurService', 'PointagePersonnelService'];

    function ExportController(ProgressBar, Notify, UtilisateurService, PointagePersonnelService) {

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        // méthodes exposées
        init();

        $ctrl.export = {
            Utilisateur: null,
            TypeExport: 0,
            TypePersonnel: "0",
            DateComptable: null,
            Organisation: null,
            Personnel: null,
            DateDebut: null,
            DateFin: null,
            Rapport: 0
        };

        $ctrl.errorDate = false;
        $ctrl.datePeriodeRequired = true;
        $ctrl.dateComptableRequired = true;
        $ctrl.exportPointagePersonnelField0 = "0";
        $ctrl.exportPointageInterimaireField1 = "1";
        $ctrl.exportChallengeSecuriteField2 = "2";

        $ctrl.clear = clear;
        $ctrl.handleDelete = handleDelete;
        $ctrl.handleSelectedItem = handleSelectedItem;
        $ctrl.exportPointage = exportPointage;
        $ctrl.handleDateValidation = handleDateValidation;
        $ctrl.resetPeriode = resetPeriode;


        //////////////////////////////////////////////////////////////////
        // Déclaration des fonctions publiques                          //
        //////////////////////////////////////////////////////////////////

        $ctrl.resources = resources;

        /**
         * Initialisation du controller.
         * 
         */
        function init() {
            $('#exp-exc').modal('show');
        }

        function handleDelete(type) {
            switch (type) {
                case "Personnel":
                    $ctrl.export.Personnel = null;
                    break;
                case "Organisation":
                    $ctrl.export.Organisation = null;
                    break;
            }
        };

        function handleSelectedItem(item, type) {
            switch (type) {
                case "Personnel":
                    $ctrl.export.Personnel = item;
                    break;
                case "Organisation":
                    $ctrl.export.Organisation = item;
                    break;
            }
            checkMondatoryPeriodeFields();
        };

        function handleDateValidation() {
            if (isRealValue($ctrl.export.DateDebut) || isRealValue($ctrl.export.DateFin)) {
                $ctrl.export.DateComptable = null;
            }
            if (isRealValue($ctrl.export.DateDebut) && isRealValue($ctrl.export.DateFin)) {
                $ctrl.exportForm.DateDebut.$setValidity("RangeError", $ctrl.export.DateDebut <= $ctrl.export.DateFin);
            }
            checkMondatoryPeriodeFields();
        }

        function resetPeriode() {
            if (isRealValue($ctrl.export.DateComptable)) {
                $ctrl.export.DateDebut = null;
                $ctrl.export.DateFin = null;
            }
            checkMondatoryPeriodeFields();
        }

        function exportPointage(typeExport) {
            checkMondatoryPeriodeFields();
            if (!$ctrl.exportForm.$invalid) {
                $ctrl.export.TypeExport = typeExport;

                if ($ctrl.export.TypePersonnel === $ctrl.exportPointagePersonnelField0) {
                    VerificationDate();
                    if (!$ctrl.errorDate) {
                        exportHebdomadaire();
                    }
                } else if ($ctrl.export.TypePersonnel === $ctrl.exportPointageInterimaireField1) {
                    exportInterimaire();
                } else if ($ctrl.export.TypePersonnel === $ctrl.exportChallengeSecuriteField2) {
                    VerificationDate();
                    if (!$ctrl.errorDate) {
                        exportChallengeSecurite();
                    }
                }
            }
        }

        function checkMondatoryPeriodeFields() {
            if ($ctrl.export.TypePersonnel === $ctrl.exportPointageInterimaireField1) {
                $ctrl.datePeriodeRequired = true;
                $ctrl.dateComptableRequired = false;
            } else {
                $ctrl.datePeriodeRequired = !$ctrl.export.DateComptable;
                $ctrl.dateComptableRequired = !($ctrl.export.DateDebut && $ctrl.export.DateFin);
            }
        }

        function exportInterimaire() {
            UtilisateurService.GetCurrentUser().$promise.then(function (utilisateur) {
                $ctrl.export.Utilisateur = utilisateur.Personnel;
                PointagePersonnelService.PostPointageInterimaireExport($ctrl.export).then(function (response) {
                    if (response.data === null) {
                        Notify.error($ctrl.resources.Global_Notification_AucuneDonnees);
                    }
                    else {
                        PointagePersonnelService.GetPointageInterimaireExport(response.data.id, $ctrl.export.TypeExport, $ctrl.export.DateDebut, $ctrl.export.DateFin);
                    }
                }).catch(function (error) {
                    Notify.error($ctrl.resources.Global_Notification_Error);
                    console.log(error);
                });
            }).catch(function (error) {
                Notify.error($ctrl.resources.Global_Notification_Error);
                console.log(error);
            });
        }

        function exportHebdomadaire() {
            UtilisateurService.GetCurrentUser().$promise.then(function (utilisateur) {
                $ctrl.export.Utilisateur = utilisateur.Personnel;
                PointagePersonnelService.PostPointageHebdomadaireExport($ctrl.export).then(function (response) {
                    if (response.data === null) {
                        Notify.error($ctrl.resources.Global_Notification_AucuneDonnees);
                    }
                    else {
                        PointagePersonnelService.GetPointageHebdomadaireExport(response.data.id, $ctrl.export.TypeExport, $ctrl.export.DateComptable, $ctrl.export.DateDebut, $ctrl.export.DateFin)
                    }
                }).catch(function (error) {
                    Notify.error($ctrl.resources.Global_Notification_Error);
                    console.log(error);
                });
            }).catch(function (error) {
                Notify.error($ctrl.resources.Global_Notification_Error);
                console.log(error);
            });
        }

        function VerificationDate() {
            if (isRealValue($ctrl.export.DateComptable) || (isRealValue($ctrl.export.DateDebut) && isRealValue($ctrl.export.DateFin))) {
                $ctrl.errorDate = false;
            }
            else {
                $ctrl.errorDate = true;
            }
        }

        function isRealValue(obj) {
            return obj && obj != null && obj != undefined;
        }

        function clear() {
            $ctrl.export.Utilisateur = null;
            $ctrl.export.TypeExport = 0;
            $ctrl.export.DateComptable = null;
            $ctrl.export.Organisation = null;
            $ctrl.export.Personnel = null;
            $ctrl.export.DateDebut = null;
            $ctrl.export.DateFin = null;
            $ctrl.export.Rapport = 0;

            $ctrl.exportForm.$setPristine();
        }

        function exportChallengeSecurite() {
            ProgressBar.start();
            UtilisateurService.GetCurrentUser().$promise.then(u => {
                $ctrl.export.Utilisateur = u.Personnel;
                PointagePersonnelService.PostPointageChallengeSecuriteExport($ctrl.export).then(r => {
                    if (r.data === null) {
                        Notify.error($ctrl.resources.Global_Notification_AucuneDonnees);
                    }
                    else {
                        PointagePersonnelService.GetPointageChallengeSecuriteExport(r.data.id, $ctrl.export.DateComptable, $ctrl.export.DateDebut, $ctrl.export.DateFin)
                    }
                }).catch(e => {
                    Notify.error($ctrl.resources.Global_Notification_Error);
                    console.log(e);
                });
            }).catch(e => {
                Notify.error($ctrl.resources.Global_Notification_Error);
                console.log(e);
            }).finally(ProgressBar.complete());
        }

    }
}(angular));