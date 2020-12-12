(function (angular) {
    'use strict';

    angular.module('Fred').controller('mainController', mainController);

    mainController.$inject = ['$scope', '$rootScope', '$q', 'UserService', '$location', 'menuAuthorizationService', 'FeatureFlags'];

    function mainController($scope, $rootScope, $q, UserService, $location, menuAuthorizationService, FeatureFlags) {

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;
        $ctrl.utilisateur = null;
        $ctrl.panelUserIsOpen = false;
        $ctrl.logo = null;
        $ctrl.version = "";
        $ctrl.resources = resources;
        $ctrl.headerMobileIsOpen = false;
        $ctrl.ModernMenuIsOpen = false;
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
            $q.when()
                .then(getCurrentUser)
                .then(getVersion)
                .then(function () {
                    $ctrl.photoProfil = UserService.getPhotoProfil();
                });

            /**  récupération du mode menu dans les paramètres Utilisateur */
            var modeMenu = UserService.getModeMenu();
            $ctrl.modeMenu = modeMenu;

            $rootScope.$on('logo.changed', function (event, img) {
                $ctrl.logo = img.logoImage.Path;
            });

            $rootScope.$on('menuMode.changed', function (event, val) {
                $ctrl.modeMenu = val;
            });

            $scope.$on('open.user.panel', function (event, taskToOpen) {
                $ctrl.panelUserIsOpen = true;
            });

            $scope.$on('close.user.panel', function (event, taskToOpen) {
                $ctrl.panelUserIsOpen = false;
            });

            $rootScope.$on('photoProfil.changed', function (event, img) {
                $ctrl.photoProfil = UserService.getPhotoProfil();
            });

            $rootScope.$on('login.changed', function (event, img) {
                $ctrl.backgroundImgUrl = img.loginImage.Path;
            });

            /*Détection si l'utilisateur ouvre le menu dit moderne*/
            $scope.$on('open.close.modern.menu', function (event, val) {
                $ctrl.ModernMenuIsOpen = !$ctrl.ModernMenuIsOpen;
            });

            /*Détecte si on est sur la home page*/
            $scope.isHomePage = function () {
                return $location.path() === "/Home/Index";
            };

            $ctrl.modules =
                [
                    {
                        "ModuleId": 1,
                        "libelle": "Pointage",
                        "ShortDescription": "Gestion des pointages journaliers",
                        "position": "left",
                        "Features": [
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuAreasPointageViewsRapportIndex,
                                "Type": "Feature",
                                "URI": "/Pointage/Rapport",
                                "icon": "sapicon-E0A5",
                                "ShortDescription": "Liste des rapports",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Rapport Pointage Workflow Verrouillage Personnel Matériel"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuAreasPointageViewsRapportNew,
                                "Type": "Feature",
                                "URI": "/Pointage/Rapport/New",
                                "icon": "flaticon flaticon-add",
                                "ShortDescription": "Nouveau rapport",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Rapport Pointage Intérimaire absence majoration tâche taches code déplacement chantier mapi météo",
                                "ModificationRigthRequired": "true"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuAreasPointageRapportHebdomadaire,
                                "Type": "Feature",
                                "URI": "/Pointage/Rapport/PointageHebdoRapportOuvrier",
                                "icon": "flaticon flaticon-add",
                                "ShortDescription": "Rapport hebdomadaire ouvrier",
                                "LongDescription": "Rapport hebdomadaire ouvrier",
                                "KeyWords": "Rapport Pointage Hebdomadaire ouvrier",
                                "ModificationRigthRequired": "true"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuAreasPointageRapportETAMIAC,
                                "Type": "Feature",
                                "URI": "/Pointage/Rapport/PointageHebdoRapportETAMIAC",
                                "icon": "flaticon flaticon-add",
                                "ShortDescription": "Rapport hebdomadaire ETAM IAC",
                                "LongDescription": "Rapport hebdomadaire ETAM IAC",
                                "KeyWords": "Rapport hebdomadaire ETAM IAC",
                                "ModificationRigthRequired": "true"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuAreasPointageRapportSyntheseMensuelle,
                                "Type": "Feature",
                                "URI": "/Pointage/Rapport/PointageEtamIacSyntheseMensuelle",
                                "icon": "flaticon flaticon-add",
                                "ShortDescription": "Validation de mon service",
                                "LongDescription": "Validation de mon service",
                                "KeyWords": "Validation de mon service",
                                "ModificationRigthRequired": "true"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuPointagePersonnelIndex,
                                "Type": "Feature",
                                "URI": "/PointagePersonnel/PointagePersonnel",
                                "icon": "flaticon flaticon-users-1",
                                "ShortDescription": "Liste des pointages personnel",
                                "LongDescription": "Description Longue",
                                "KeyWords": "week end week-end semaine jour fériés absence congé prime majoration"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuValidationPointageIndex,
                                "Type": "Feature",
                                "URI": "/ValidationPointage/ValidationPointage",
                                "icon": "flaticon flaticon-price-tag",
                                "ShortDescription": "Validation Lot de Pointage",
                                "LongDescription": "Description Longue",
                                "KeyWords": "paie controle vrac chantier transfert anomalie "
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuRapportPrimesIndex,
                                "Type": "Feature",
                                "URI": "/RapportPrime/RapportPrime",
                                "icon": "flaticon flaticon-calendar-1",
                                "ShortDescription": "Rapports de primes",
                                "LongDescription": "Rapports de primes",
                                "KeyWords": "Rapport Prime Pointage Workflow Personnel"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuAreasValidationAffairesOuvriersForResponsable,
                                "Type": "Feature",
                                "URI": "/Pointage/Rapport/ValidationAffairesOuvriers",
                                "icon": "flaticon flaticon-add",
                                "ShortDescription": "Validation de mes affaires",
                                "LongDescription": "Validation de mes affaires",
                                "KeyWords": "Validation de mes affaires",
                                "ModificationRigthRequired": "true"
                            }
                        ]
                    },
                    {
                        "ModuleId": 2,
                        "libelle": "Achat",
                        "ShortDescription": "Gestion des commandes et réceptions",
                        "position": "left",
                        "Features": [
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuCommandeDetail,
                                "Type": "Feature",
                                "URI": "/Commande/Commande/Detail",
                                "icon": "flaticon flaticon-add-2",
                                "ShortDescription": "Nouvelle commande",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Commande HA achat ligne ventilation tâche ressources article seuil fournisseur adresse bon de commande prix unité",
                                "ModificationRigthRequired": "true"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuCommandeIndex,
                                "Type": "Feature",
                                "URI": "/Commande/Commande/Index",
                                "icon": "flaticon flaticon-menu-3",
                                "ShortDescription": "Gérer les commandes",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Commande HA achat ligne ventilation tâche ressources article seuil fournisseur adresse bon de commande"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuReceptionIndex,
                                "Type": "Feature",
                                "URI": "/Reception/Reception",
                                "icon": "flaticon flaticon-checked",
                                "ShortDescription": "Gérer les réceptions",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Réception commande bon de livraison"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuReceptionTableau,
                                "Type": "Feature",
                                "URI": "/Reception/Reception/Tableau",
                                "icon": "flaticon flaticon-notebook-4",
                                "ShortDescription": "Tableau des réceptions",
                                "LongDescription": "Gestion des réceptions",
                                "KeyWords": "Tableau Réception commande bon de livraison"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuPreparerEnergies,
                                "Type": "Feature",
                                "URI": "/CommandeEnergie/CommandeEnergie/Index",
                                "icon": "flaticon flaticon-calendar-4",
                                "ShortDescription": "Préparer les énergies",
                                "LongDescription": "Préparation des Commandes d'énergies",
                                "KeyWords": "Préaprer Commande Energie"
                            }
                        ]
                    },
                    {
                        "ModuleId": 1,
                        "libelle": "Exploitation",
                        "ShortDescription": "Exploitation",
                        "position": "left",
                        "Features": [
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuDepenseExplorateur,
                                "Type": "Feature",
                                "URI": "/Depense/Depense/Explorateur",
                                "icon": "flaticon-file-2",
                                "ShortDescription": "Explorateur de dépenses",
                                "LongDescription": "Description Longue",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuCompteExploitationIndex,
                                "Type": "Feature",
                                "URI": "/CompteExploitation/Edition",
                                "icon": "flaticon-calculator-1",
                                "ShortDescription": "Editions d'Exploitation",
                                "LongDescription": "Editions d'Exploitation",
                                "KeyWords": "edition exploitation"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuBilanFlashIndex,
                                "Type": "Feature",
                                "URI": "/BilanFlash/BilanFlash",
                                "icon": "flaticon flaticon-network",
                                "ShortDescription": "Gérer les bilans flash",
                                "LongDescription": "Gérer les bilans flash",
                                "KeyWords": "bilans flash bilanFlash"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuOperationDiverseIndex,
                                "Type": "Feature",
                                "URI": "/OperationDiverse/OperationDiverse",
                                "icon": "flaticon flaticon-notebook-4",
                                "ShortDescription": "Rapprochement Compta/Gestion",
                                "LongDescription": "Rapprochement Compta/Gestion",
                                "KeyWords": "opération diverses OD écart"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuBaremeExploitationOrganisationIndex,
                                "Type": "Feature",
                                "URI": "/Bareme/Exploitation/Organisation",
                                "icon": "flaticon flaticon-calculator",
                                "ShortDescription": "Barèmes organisation",
                                "LongDescription": "Barèmes organisation",
                                "KeyWords": "barème barême bareme orga organisation"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuBaremeExploitationCIIndex,
                                "Type": "Feature",
                                "URI": "/Bareme/Exploitation/CI",
                                "icon": "flaticon flaticon-calculator",
                                "ShortDescription": "Barèmes CI",
                                "LongDescription": "Barèmes CI",
                                "KeyWords": "barème barême bareme ci"
                            }
                        ]
                    },
                    {
                        "ModuleId": 1,
                        "libelle": "Affaires",
                        "ShortDescription": "Affaire",
                        "position": "left",
                        "Features": [
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuCIIndex,
                                "Type": "Feature",
                                "URI": "/CI/CI",
                                "icon": "flaticon flaticon-target",
                                "ShortDescription": "Gérer les centres d'imputation",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Chantier affaire GPS coordonnées devise code majoration prime coordonnées adresse"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuDatesClotureComptableIndex,
                                "Type": "Feature",
                                "URI": "/DatesClotureComptable/DatesClotureComptable/Index",
                                "icon": "flaticon flaticon-calendar-4",
                                "ShortDescription": "Gérer les clôtures",
                                "LongDescription": "Gérer les clôtures",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuDatesClotureComptableEnMasseIndex,
                                "Type": "Feature",
                                "URI": "/CloturesPeriodes/CloturesPeriodes/Index",
                                "icon": "flaticon flaticon-calendar-4",
                                "ShortDescription": "Gérer les clôtures en masse",
                                "LongDescription": "Gérer les clôtures en masse",
                                "KeyWords": "A définir, A définir, A définir"
                            }
                        ]
                    },
                    {
                        "ModuleId": 1,
                        "libelle": "Budget",
                        "ShortDescription": "Budget",
                        "position": "left",
                        "Features": [
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuReferentielTachesIndex,
                                "Type": "Feature",
                                "URI": "/ReferentielTaches/ReferentielTaches/",
                                "icon": "flaticon flaticon-layers",
                                "ShortDescription": "Plan de tâches",
                                "LongDescription": "Description Longue",
                                "KeyWords": "tache taches tâche tâches"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuParamTarifsReferentielsIndex,
                                "Type": "Feature",
                                "URI": "/ParamTarifsReferentiels/ParamTarifsReferentiels",
                                "icon": "flaticon flaticon-notepad-1",
                                "ShortDescription": "Barèmes Budget",
                                "LongDescription": "",
                                "KeyWords": "barème bareme"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuBibliothequePrix,
                                "Type": "Feature",
                                "URI": "/Budget/Budget/BibliothequePrix",
                                "icon": "flaticon flaticon-notepad-1",
                                "ShortDescription": "Bibliothèque des prix",
                                "LongDescription": "Bibliothèque des prix",
                                "KeyWords": "bibliotheque, prix"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuBudgetIndex,
                                "Type": "Feature",
                                "URI": "/Budget/Budget/Liste",
                                "icon": "flaticon flaticon-calculator",
                                "ShortDescription": "Liste des budgets",
                                "LongDescription": "Description Longue",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuControleBudgetaire,
                                "Type": "Feature",
                                "URI": "/Budget/Budget/ControleBudgetaire",
                                "icon": "flaticon flaticon-calculator",
                                "ShortDescription": "Contrôle Budgétaire",
                                "LongDescription": "Description Longue",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuAvancement,
                                "Type": "Feature",
                                "URI": "/Budget/Budget/Avancement",
                                "icon": "flaticon flaticon-calculator",
                                "ShortDescription": "Avancement",
                                "LongDescription": "Avancement",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuAvancementRecette,
                                "Type": "Feature",
                                "URI": "/Budget/Budget/AvancementRecette",
                                "icon": "flaticon flaticon-calculator",
                                "ShortDescription": "Avancement Recette",
                                "LongDescription": "Avancement Recette",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuBudgetComparaison,
                                "Type": "Feature",
                                "URI": "/Budget/Budget/BudgetComparaison",
                                "icon": "flaticon flaticon-calculator",
                                "ShortDescription": "Comparaison de budget",
                                "LongDescription": "Comparaison de budget",
                                "KeyWords": "A définir, A définir, A définir"
                            }
                        ]
                    },
                    {
                        "ModuleId": 1,
                        "libelle": "Moyen",
                        "ShortDescription": "Moyen",
                        "position": "left",
                        "Features": [
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageGestionMoyenIndex,
                                "Type": "Feature",
                                "URI": "/Moyen/Moyen/GestionMoyen",
                                "icon": "flaticon flaticon-layers",
                                "ShortDescription": "Gestion des moyens",
                                "LongDescription": "Gestion des moyens",
                                "KeyWords": "Gestion des départs"
                            }
                        ]
                    },
                    {
                        "ModuleId": 1,
                        "libelle": "Ressources Humaines",
                        "ShortDescription": "Administration des codes",
                        "position": "right",
                        "Features": [
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuPersonnelIndex,
                                "Type": "Feature",
                                "URI": "/Personnel/Personnel",
                                "icon": "flaticon flaticon-user-3",
                                "ShortDescription": "Gérer le personnel",
                                "LongDescription": "Intérimaire",
                                "KeyWords": "Intérimaire, Personnel, Utilisateur, Personnels, Utilisateurs, Mot de passe"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuCodeDeplacementIndex,
                                "Type": "Feature",
                                "URI": "/CodeDeplacement/CodeDeplacement",
                                "icon": "flaticon flaticon-map-location",
                                "ShortDescription": "Gérer les codes déplacement",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Code déplacement RH"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuCodeZoneDeplacementIndex,
                                "Type": "Feature",
                                "URI": "/CodeZoneDeplacement/CodeZoneDeplacement",
                                "icon": "flaticon flaticon-compass",
                                "ShortDescription": "Gérer les codes zone déplacement",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Code zone déplacement RH"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuCodeMajorationIndex,
                                "Type": "Feature",
                                "URI": "/CodeMajoration/CodeMajoration",
                                "icon": "flaticon flaticon-calculator",
                                "ShortDescription": "Gérer les codes heures spécifiques",
                                "LongDescription": "Description Longue",
                                "KeyWords": "code majoration RH"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuPrimeIndex,
                                "Type": "Feature",
                                "URI": "/Prime/Prime",
                                "icon": "flaticon flaticon-diamond",
                                "ShortDescription": "Gérer les codes primes",
                                "LongDescription": "Code prime RH"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuCodeAbsenceIndex,
                                "Type": "Feature",
                                "URI": "/CodeAbsence/CodeAbsence",
                                "icon": "fa fa-braille",
                                "ShortDescription": "Gérer les codes absence",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Code Absence RH"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuExportPointagePersonnelIndex,
                                "Type": "Feature",
                                "URI": "/PointagePersonnel/PointagePersonnel/Export",
                                "icon": "flaticon flaticon-document",
                                "ShortDescription": "Export pointage personnel",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Export pointage personnel RH"
                            }
                        ]
                    },
                    {
                        "ModuleId": 1,
                        "libelle": "Habilitations",
                        "ShortDescription": "Droits et habilitations",
                        "position": "right",
                        "Features": [

                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuRoleIndex,
                                "Type": "Feature",
                                "URI": "/Role/Role",
                                "icon": "flaticon flaticon-user-7",
                                "ShortDescription": "Gérer les rôles",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Administration Rôle Role Droit module Fonctionnalité"
                            },
                            {
                                // Code commenté si demande retour en arriere
                                // La permission __SuperAdmin___ n'existe pas donc seul les admin verrons ce menu
                                "permissionKey": "__SuperAdmin___",// PERMISSION_KEYS.AffichageMenuModuleIndex,
                                "Type": "Feature",
                                "URI": "/Module/Module",
                                "icon": "flaticon flaticon-settings",
                                "ShortDescription": "Gérer les modules",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Administration Rôle Droit Module"
                            },
                            {
                                "permissionKey": "__SuperAdmin___",//PERMISSION_KEYS.AffichageMenuAuthentificationLogIndex,
                                "Type": "Feature",
                                "URI": "/AuthentificationLog/AuthentificationLog/Index",
                                "icon": "flaticon flaticon-settings",
                                "ShortDescription": "Gérer le journal de connexion",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Administration Authentification"
                            },
                            {
                                "permissionKey": "__SuperAdmin___",//PERMISSION_KEYS.AffichageMenuFeatureFlipping,
                                "Type": "Feature",
                                "URI": "/FeatureFlipping/FeatureFlipping/Index",
                                "icon": "flaticon flaticon-settings",
                                "ShortDescription": "Gérer les features flipping",
                                "LongDescription": "Description Longue",
                                "KeyWords": "Administration Feature Flipping"
                            }
                        ]
                    },
                    {
                        "ModuleId": 1,
                        "libelle": "Organisation",
                        "ShortDescription": "Administration des modules relatives à l'organisation",
                        "position": "right",
                        "Features": [
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuEtablissementPaieIndex,
                                "Type": "Feature",
                                "URI": "/EtablissementPaie/EtablissementPaie",
                                "icon": "flaticon flaticon-house",
                                "ShortDescription": "Gérer les établissements de paie",
                                "LongDescription": "Description Longue",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuEtablissementComptableIndex,
                                "Type": "Feature",
                                "URI": "/EtablissementComptable/EtablissementComptable",
                                "icon": "flaticon flaticon-home-2",
                                "ShortDescription": "Gérer les établissements comptables",
                                "LongDescription": "Description Longue",
                                "KeyWords": "A définir, A définir, A définir"
                            },

                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuSocieteIndex,
                                "Type": "Feature",
                                "URI": "/Societe/Societe",
                                "icon": "flaticon flaticon-share-2",
                                "ShortDescription": "Gérer les sociétés",
                                "LongDescription": "Description Longue",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuFournisseurIndex,
                                "Type": "Feature",
                                "URI": "/Fournisseur/Fournisseur",
                                "icon": "flaticon flaticon-television-1",
                                "ShortDescription": "Gérer les fournisseurs",
                                "LongDescription": "Description Longue",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuClassificationSocietesIndex,
                                "Type": "Feature",
                                "URI": "/ClassificationSocietes/ClassificationSocietes",
                                "icon": "flaticon flaticon-television-1",
                                "ShortDescription": "Gérer les classifications sociétés",
                                "LongDescription": "Description Longue",
                                "KeyWords": "A définir, A définir, A définir"
                            }
                        ]
                    },
                    {
                        "ModuleId": 1,
                        "libelle": "Paramétrage",
                        "ShortDescription": "Administration des modules de paramétrage",
                        "position": "right",
                        "Features": [
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuAreasReferentialViewsDeviseIndex,
                                "Type": "Feature",
                                "URI": "/Referential/Devise/Index",
                                "icon": "flaticon flaticon-diamond",
                                "ShortDescription": "Gérer les devises",
                                "LongDescription": "",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuJournalComptableIndex,
                                "Type": "Feature",
                                "URI": "/JournalComptable/JournalComptable",
                                "icon": "flaticon flaticon-network",
                                "ShortDescription": "Gérer les journaux comptables",
                                "LongDescription": "Gérer les journaux comptables",
                                "KeyWords": ""
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuNatureIndex,
                                "Type": "Feature",
                                "URI": "/Nature/Nature",
                                "icon": "flaticon flaticon-network",
                                "ShortDescription": "Gérer les natures analytiques",
                                "LongDescription": "Gérer les natures analytiques",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuReferentielFixesIndex,
                                "Type": "Feature",
                                "URI": "/ReferentielFixes/ReferentielFixes",
                                "icon": "flaticon flaticon-notebook-4",
                                "ShortDescription": "Gérer les référentiels fixes",
                                "LongDescription": "",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuRessourcesSpecifiquesCiIndex,
                                "Type": "Feature",
                                "URI": "/RessourcesSpecifiquesCi/RessourcesSpecifiquesCi",
                                "icon": "flaticon flaticon-notebook-4",
                                "ShortDescription": "Gérer les ressources spécifiques CI",
                                "LongDescription": "Gérer les ressources spécifiques CI",
                                "KeyWords": "ressources, spécifiques"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuReferentielEtenduIndex,
                                "Type": "Feature",
                                "URI": "/ReferentielEtendu/ReferentielEtendu/Index",
                                "icon": "flaticon flaticon-notebook-4",
                                "ShortDescription": "Gérer les natures analytiques / ressources",
                                "LongDescription": "",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuRessourcesRecommandeesIndex,
                                "Type": "Feature",
                                "URI": "/RessourcesRecommandees/RessourcesRecommandees/Index",
                                "icon": "flaticon flaticon-notebook-4",
                                "ShortDescription": "Gérer les ressources recommandées",
                                "LongDescription": "",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuMaterielIndex,
                                "Type": "Feature",
                                "URI": "/Materiel/Materiel",
                                "icon": "flaticon flaticon-settings-9",
                                "ShortDescription": "Gérer les matériels",
                                "LongDescription": "",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                //"permissionKey": PERMISSION_KEYS.AffichageMenuMaterielIndex, // TODO : gérer les permissions pour cette area 
                                "Type": "Feature",
                                "URI": "/Cache/Cache",
                                "icon": "flaticon flaticon-settings-9",
                                "ShortDescription": "Gérer le cache",
                                "LongDescription": "",
                                "KeyWords": "A définir, A définir, A définir"
                            },
                            {
                                "permissionKey": PERMISSION_KEYS.AffichageMenuFamilleOperationDiverse,
                                "Type": "Feature",
                                "URI": "/FamilleOperationDiverse/FamilleOperationDiverse",
                                "icon": "flaticon flaticon-settings-9",
                                "ShortDescription": "Gérer les familles OD",
                                "LongDescription": "",
                                "KeyWords": "A définir, A définir, A définir"
                            }
                        ]
                    }
                ];
            if (FeatureFlags.getFlagStatus('ActivationUS13085_6000')) {
                var foundIndex = $ctrl.modules.findIndex(x => x.ModuleId === 1 && x.libelle === "Paramétrage");
                if (foundIndex >= 0) {
                    $ctrl.modules[foundIndex].Features.push(...[
                        {
                            "permissionKey": PERMISSION_KEYS.AffichageMenuListFamilleOperationDiverse,
                            "Type": "Feature",
                            "URI": "/FamilleOperationDiverse/FamilleOperationDiverse/List",
                            "icon": "flaticon flaticon-network",
                            "ShortDescription": "Familles opérations diverses",
                            "LongDescription": "",
                            "KeyWords": "A définir, A définir, A définir"
                        },
                        {
                            "permissionKey": PERMISSION_KEYS.AffichageMenuAssociationFodJournauxNatures,
                            "Type": "Feature",
                            "URI": "/FamilleOperationDiverse/FamilleOperationDiverse/AssociationFodNaturesJournaux",
                            "icon": "flaticon flaticon-network",
                            "ShortDescription": "Association Famille OD / Natures / Journaux",
                            "LongDescription": "",
                            "KeyWords": "A définir, A définir, A définir"
                        }
                    ]);
                }
            }
            $ctrl.modulesAutorized = menuAuthorizationService.filterMenus($ctrl.modules);
            $ctrl.menuAdminIsVisible = menuAuthorizationService.menuAdministationIsVisible($ctrl.modules);


        }

        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        function getCurrentUser() {
            return UserService.getCurrentUser()
                .then(function (response) {
                    $ctrl.utilisateur = response;
                    return response;
                })
                .then(loadImages)
                .finally(getSavedImages);
        }

        function loadImages(utilisateur) {
            return UserService.loadImages(utilisateur);
        }

        function getSavedImages() {
            var images = UserService.getSavedImages();
            $ctrl.logo = images.logo;
        }

        function getVersion() {
            return UserService.getVersionInfo().then(function (response) {
                $ctrl.version = response;
            });
        }
    }
}(angular));

