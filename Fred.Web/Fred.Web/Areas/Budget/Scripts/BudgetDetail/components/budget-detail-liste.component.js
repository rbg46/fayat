(function () {
    'use strict';

    var budgetDetailListeComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetDetail/components/budget-detail-liste.component.html',
        bindings: {
            budget: '<',
            resources: '<',
            disabledTasksDisplayed: '<'
        },
        controller: budgetDetailListeController
    };
    angular.module('Fred').component('budgetDetailListeComponent', budgetDetailListeComponent);
    angular.module('Fred').controller('budgetDetailListeController', budgetDetailListeController);
    budgetDetailListeController.$inject = ['$rootScope', '$scope', 'BudgetService', 'BudgetDetailService', 'BudgetDetailCalculator', 'ProgressBar', 'Notify', '$timeout'];

    function budgetDetailListeController($rootScope, $scope, BudgetService, BudgetDetailService, BudgetDetailCalculator, ProgressBar, Notify, $timeout) {
        var $ctrl = this;

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                            //
        //////////////////////////////////////////////////////////////////
        $ctrl.Budget = $ctrl.budget;
        $ctrl.BudgetService = BudgetService;
        $ctrl.BudgetDetailService = BudgetDetailService;
        $ctrl.AddT4 = AddT4;
        $ctrl.ChangeT4 = ChangeT4;
        $ctrl.DeleteT4 = DeleteT4;
        $ctrl.DeleteBudgetT4 = DeleteBudgetT4;
        $ctrl.ShowComment = ShowComment;
        $ctrl.ToggleTypeAvancement = ToggleTypeAvancement;
        $ctrl.LoadSousDetailRequested = LoadSousDetailRequested;
        $ctrl.GetShowCommentButtonTooltip = GetShowCommentButtonTooltip;
        $ctrl.onToutPlier = onToutPlier;
        $ctrl.onToutDeplier = onToutDeplier;
        $ctrl.ShowCopyMoveDialog = ShowCopyMoveDialog;
        $ctrl.ShouldDisplayTaskLine = ShouldDisplayTaskLine;
        $ctrl.IsColumnVisible = IsColumnVisible;
        $ctrl.ColumnsVisible = [BudgetService.DetailColumnEnum.Unite
            , $ctrl.BudgetService.DetailColumnEnum.Quantite
            , $ctrl.BudgetService.DetailColumnEnum.PU];
        $ctrl.NiveauxVisible = [1, 2, 3, 4];
        $ctrl.Col2SizeClass = "col-2-small";
        $ctrl.codeTacheEcart = '99';
        $ctrl.isTacheEcart = isTacheEcart;

        if ($ctrl.Budget.IsTypeAvancementBudgetDynamique)
            $ctrl.Col2SizeClass = "col-2-large";
        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.SaveDetail, function (event, args) { Save(args.continueWithValidation); });
            $scope.$on(BudgetService.Events.PanelCommentaireModified, function (event, arg) { CommentaireHasChanged(arg); });
            $scope.$on(BudgetService.Events.DisplayDetail, restoreScrollPosition);
            $scope.$on(BudgetService.Events.PanelCustomizeDisplayModified, function (event, args) { CustomizeDisplay(args); });
        };

        function isTacheEcart(tache) {
            return tache.Code.startsWith($ctrl.codeTacheEcart);
        }

        // Appelé lors de l'affichage du detail apres edition d'un T4.
        function restoreScrollPosition() {
            $timeout(function () {
                //Wait for DOM has finished rendering before restoring scrollpos
                let fullQualifiedId = '#FLEX_TABLE';
                $(fullQualifiedId).scrollTop($ctrl.scrollPosition);
            });
        }

        //////////////////////////////////////////////////////////////////
        // Enregistrement                                               //
        //////////////////////////////////////////////////////////////////
        function Save(continueWithValidation) {
            // Vérifie s'il y a des changements
            var changes = $ctrl.Budget.GetDetailChanges();
            if (!changes.Exists) {
                Notify.message($ctrl.resources._Budget_PasDeModificationAEnregistrer);
                if (continueWithValidation) {
                    $rootScope.$broadcast(BudgetService.Events.DisplayValidationDialog);
                }
            }
            else {
                ProgressBar.start();
                BudgetService.SaveDetail(changes.Model)
                    .then(result => SaveDetailThen(result, changes.TachesChanged, changes.Model.BudgetT4sDeleted, continueWithValidation))
                    .catch(SaveDetailCatch);
            }
        }

        function SaveDetailThen(result, tachesChanged, budgetT4sDeleted, continueWithValidation) {
            var data = result.data;
            if (data.Erreur) {
                Notify.error(data.Erreur);
                ProgressBar.complete();
            }
            else {
                // Affecte les nouveaux identifiants et flag les éléments comme enregistrés
                for (let tache of tachesChanged) {
                    if (tache.Niveau === 4) {
                        for (var i = 0; i < data.BudgetT4sCreated.length; i++) {
                            var budgetT4Created = data.BudgetT4sCreated[i];
                            if (tache.TacheId === budgetT4Created.TacheId) {
                                tache.BudgetT4.BudgetT4Id = budgetT4Created.BudgetT4Id;
                                data.BudgetT4sCreated.splice(i, 1);
                                break;
                            }
                        }
                        tache.BudgetT4.SavedForDetail();
                    }
                    else {
                        for (var j = 0; j < data.BudgetTachesCreated.length; j++) {
                            var budgetTacheCreated = data.BudgetTachesCreated[j];
                            if (tache.TacheId === budgetTacheCreated.TacheId) {
                                tache.Info.BudgetTacheId = budgetTacheCreated.BudgetTacheId;
                                data.BudgetTachesCreated.splice(j, 1);
                                break;
                            }
                        }
                        tache.Info.Saved();
                    }
                }

                // Budget T4 supprimés
                for (var k = budgetT4sDeleted.length; k-- > 0;) {
                    var budgetT4Deleted = budgetT4sDeleted[k];
                    if (DeleteBudgetT4FromView(budgetT4Deleted)) {
                        budgetT4sDeleted.splice(k);
                    }
                }

                Notify.message($ctrl.resources._Budget_Enregistrement_Effectue);
                ProgressBar.complete();
                if (continueWithValidation) {
                    $rootScope.$broadcast(BudgetService.Events.DisplayValidationDialog);
                }
            }
        }

        function SaveDetailCatch() {
            ProgressBar.complete();
            Notify.error($ctrl.resources._Budget_Erreur_Enregistrement);
        }


        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        // Affiche la boite de dialogue d'ajout d'une tâche T4.
        // - tache3 : la tâche parente de niveau 3
        function AddT4(tache3) {
            $scope.$broadcast(BudgetService.Events.DisplayDialogToAddT4, {
                Tache3: tache3,
                OnValidate: function (tacheCreated) {
                    // Transforme arg.Tache4 [TacheResultModel] en [BudgetDetailLoad.Tache4Model] pour l'ajouter à la liste des tâches
                    var tache4 = BudgetService.CreateBudgetDetailLoadTache4Model(tacheCreated.TacheId, tacheCreated.Code, tacheCreated.Libelle, tacheCreated.IsActive);

                    // Ajoute les élements pour la vue et crée le BudgetT4
                    tache4.Tache3 = tache3;

                    BudgetDetailService.InitialiseTache4(tache4);

                    // Par défaut le type d'avancement est en pourcent
                    tache4.BudgetT4.View.TypeAvancement = BudgetService.TypeAvancementEnum.Pourcentage;
                    tache4.BudgetT4.View.Tache3Id = tache4.Tache3.TacheId;

                    // Le $timeout permet d'ajouter la ligne et de rafraîchir la table flex hors du $digest, sinon une
                    //  exception se produit : [$rootScope:inprog]
                    $timeout(() => {
                        tache3.Taches4.push(tache4);
                        $ctrl.Budget.EditTache(tache4);

                        // Un rafraichissement est requis lors de l'ajout dynamique d'une ligne
                        document.getElementById("FLEX_TABLE").refresh();
                    });
                }
            });
        }

        // Affiche la boite de dialogue de changement d'une tâche T4.
        // - tache4 : la tâche concernée
        function ChangeT4(tache4) {
            $ctrl.Budget.EditTache(tache4);
            $scope.$broadcast(BudgetService.Events.DisplayDialogToChangeT4, {
                Tache4: tache4,
                OnValidate: function (code, libelle) {
                    tache4.Code = code;
                    tache4.Libelle = libelle;
                }
            });
        }

        // Affiche la boite de dialogue de suppression d'une tâche T4.
        // - tache4 : la tâche concernée
        function DeleteT4(tache4) {
            $ctrl.Budget.EditTache(tache4);
            $scope.$broadcast(BudgetService.Events.DisplayDialogToDeleteT4, {
                Tache4: tache4,
                OnValidate: function () {
                    var taches4 = tache4.Tache3.Taches4;
                    for (var i = 0; i < taches4.length; i++) {
                        if (taches4[i].TacheId === arg.Tache4Id) {
                            taches4.splice(i, 1);
                            break;
                        }
                    }
                }
            });
        }

        // Affiche la boite de dialogue de suppression d'une ligne de détail.
        // - tache4 : la tâche concernée
        function DeleteBudgetT4(tache4) {
            $ctrl.Budget.EditTache(tache4);
            $scope.$broadcast(BudgetService.Events.DisplayDialogToDeleteBudgetT4, {
                Tache4: tache4,
                OnValidate: function () {
                    tache4.Deleted = true;
                    BudgetDetailCalculator.Calculate($ctrl.Budget);
                }
            });
        }

        // Ouvre le panneau de gestion des commentaires.
        function ShowComment(tache) {
            $ctrl.Budget.EditTache(tache);
            $scope.$broadcast(BudgetService.Events.OpenPanelCommentaire, { Tache: tache });
        }

        // Bascule le type d'avancement entre % et quantité
        function ToggleTypeAvancement(tache4) {
            $ctrl.Budget.EditTache(tache4);
            if (!tache4.BudgetT4.IsReadOnly) {
                if (tache4.BudgetT4.View.TypeAvancement === BudgetService.TypeAvancementEnum.Pourcentage) {
                    tache4.BudgetT4.View.TypeAvancement = BudgetService.TypeAvancementEnum.Quantite;
                }
                else if (tache4.BudgetT4.View.TypeAvancement === BudgetService.TypeAvancementEnum.Quantite) {
                    tache4.BudgetT4.View.TypeAvancement = null;
                }
                else {
                    tache4.BudgetT4.View.TypeAvancement = BudgetService.TypeAvancementEnum.Pourcentage;
                }
            }
        }

        // Informe que le chargement d'une tâche de niveau 4 est demandé
        function LoadSousDetailRequested(tache4) {
            $ctrl.Budget.EditTache(tache4);
            //Avant de passer à l'écran du sous détail on enregistre la position de la scrollbar vertical
            $ctrl.scrollPosition = $('#FLEX_TABLE').scrollTop();
            $scope.$emit(BudgetService.Events.LoadSousDetail, { Tache4: tache4 });
        }

        // Appelé lorsque l'utilisateur souhaite copier / déplacer / remplacer des T4
        function ShowCopyMoveDialog(tache) {
            DisplayDialogToCopyMoveT4(tache);
        }


        //////////////////////////////////////////////////////////////////
        // Copier / déplacer des T4                                     //
        //////////////////////////////////////////////////////////////////

        // Affiche la fenêtre de copie / déplacement / remplacement de T4
        function DisplayDialogToCopyMoveT4(tache) {
            $scope.$broadcast(BudgetService.Events.DisplayDialogToCopyMoveT4, {
                Tache: tache,
                Budget: $ctrl.budget,
                OnAction: function (arg) {
                    DisplayDialogToConfirmCopyMoveT4(arg.Action, arg.Taches4, tache, arg.Budget);
                }
            });
        }

        // Affiche la fenêtre de confirmation de copie / déplacement / remplacement de T4
        function DisplayDialogToConfirmCopyMoveT4(action, taches4, tache, budgetSource) {
            $scope.$broadcast(BudgetService.Events.DisplayDialogToConfirmCopyMoveT4, {
                Action: action,
                Budget: $ctrl.Budget,
                Tache: tache,
                Taches4: taches4,
                OnConfirm: function (arg) {
                    switch (action) {
                        case BudgetDetailService.CopierDeplacerT4ActionEnum.Copier:
                            CopierTaches4(arg.CopierMode, taches4, budgetSource, arg.Taches4, tache);
                            break;

                        case BudgetDetailService.CopierDeplacerT4ActionEnum.Deplacer:
                            DeplacerTaches4(taches4, tache);
                            break;

                        //case BudgetDetailService.CopierDeplacerT4ActionEnum.Remplacer
                    }
                }
            });
        }

        // Copier des T4 dans un autre T3
        // - copierMode : type de copie (ajout ou remplacement)
        // - taches4Source : les T4 à copier
        // - budgetSource : le budget parent des T4 sources
        // - taches4CibleInfo : informations sur les T4 cible
        // - tache3Target : le T3 cible
        function CopierTaches4(copierMode, taches4Source, budgetSource, taches4CibleInfo, tache3Target) {
            // Vide la tâche 3 cible en cas de remplacement
            if (copierMode === BudgetDetailService.CopierT4Mode.Remplacer) {
                for (let tache4 of tache3Target.Taches4) {
                    tache4.Deleted = true;
                }
            }

            // Ajoute les BudgetT4
            var model = BudgetService.CreateBudgetSousDetailCopierModel($ctrl.Budget.BudgetId, budgetSource.CI.CiId);
            var taches4Copied = [];
            for (var i = 0; i < taches4Source.length; i++) {
                var tache4Source = taches4Source[i];
                var tache4CibleInfo = taches4CibleInfo[i];

                // Créé le [BudgetDetailLoad.Tache4Model] pour l'ajouter à la liste des tâches
                var tache4Cible = BudgetService.CreateBudgetDetailLoadTache4Model(tache4CibleInfo.TacheId, tache4CibleInfo.Code, tache4CibleInfo.Libelle, true);
                tache4Cible.BudgetT4 = BudgetService.CreateBudgetDetailLoadBudgetT4Model(BudgetDetailService.SousDetailViewModeType.SD);

                // Si la source est le budget courant, on récupère les éléments de la vue
                var dataSource = budgetSource.BudgetId === $ctrl.Budget.BudgetId
                    ? tache4Source.BudgetT4.View
                    : tache4Source.BudgetT4;
                tache4Cible.BudgetT4.TypeAvancement = dataSource.TypeAvancement;
                tache4Cible.BudgetT4.Commentaire = dataSource.Commentaire;
                tache4Cible.BudgetT4.QuantiteDeBase = dataSource.QuantiteDeBase;
                tache4Cible.BudgetT4.QuantiteARealiser = dataSource.QuantiteARealiser;
                tache4Cible.BudgetT4.Unite = dataSource.Unite;
                tache4Cible.BudgetT4.MontantT4 = dataSource.MontantT4;
                tache4Cible.BudgetT4.MontantSD = dataSource.MontantSD;
                tache4Cible.BudgetT4.PU = dataSource.PU;
                tache4Cible.BudgetT4.VueSD = dataSource.VueSD;

                BudgetDetailService.InitialiseTache4(tache4Cible);
                tache4Cible.Tache3 = tache3Target;
                tache4Cible.IsActive = tache4Cible.Tache3.IsActive;
                taches4Copied.push(tache4Cible);
                tache3Target.Taches4.push(tache4Cible);
                tache4Cible.BudgetT4.View.Tache3Id = tache4Cible.Tache3.TacheId;
                tache4Cible.BudgetT4.Tache3Id = tache4Cible.Tache3.TacheId;

                //On appelle pas la fonction editTache car cette fonction ne permet d'avoir qu'une seule tache à l'état modifiée à la fois
                //Dans notre cas on veut que toutes les tâches ajoutées soit considérées comme éditées.
                tache4Cible.IsEdited = true;

                var budgetT4CibleModel = BudgetService.CreateBudgetSousDetailSaveTache4Model(tache4CibleInfo.TacheId, tache4Cible.BudgetT4);
                var copierModel = BudgetService.CreateBudgetSousDetailCopierItemModel(tache4Source.BudgetT4.BudgetT4Id, budgetT4CibleModel);
                model.Items.push(copierModel);
            }

            ProgressBar.start();
            BudgetService.CopySousDetails(model)
                .then(result => CopySousDetailsThen(result, taches4Copied))
                .catch(CopySousDetailsCatch);
        }
        function CopySousDetailsThen(result, taches4Copied) {
            // Remet les bon identifiants
            for (var i = 0; i < taches4Copied.length; i++) {
                taches4Copied[i].BudgetT4.BudgetT4Id = result.data.BudgetT4sIdCreated[i];
            }

            // Un rafraichissement est requis lors de l'ajout dynamique d'une ligne
            $timeout(() => {
                document.getElementById("FLEX_TABLE").refresh();
            });

            // Recalcule l'ensemble du budget
            BudgetDetailCalculator.Calculate($ctrl.Budget);

            ProgressBar.complete();
        }
        function CopySousDetailsCatch() {
            Notify.error($ctrl.resources.Global_Notification_Error);
            ProgressBar.complete();
        }

        // Deplacer des T4 dans un autre T3
        // - taches4Source : les T4 à déplacer
        // - tache3Target : le T3 cible
        function DeplacerTaches4(taches4Source, tache3Target) {
            for (let tache4Source of taches4Source) {
                // Enlève la tâche 4 de son ancien parent
                for (var i = 0; i < tache4Source.Tache3.Taches4.length; i++) {
                    var tache4InTache3Source = tache4Source.Tache3.Taches4[i];
                    if (tache4InTache3Source.TacheId === tache4Source.TacheId) {
                        tache4Source.Tache3.Taches4.splice(i, 1);
                        break;
                    }
                }

                // Ajoute la tâche 4 a son nouveau parent
                tache4Source.BudgetT4.View.Tache3Id = tache3Target.TacheId;
                tache4Source.Tache3 = tache3Target;
                tache4Source.IsActive = tache3Target.IsActive;
                tache3Target.Taches4.push(tache4Source);

                //On appelle pas la fonction editTache car cette fonction ne permet d'avoir qu'une seule tache à l'état modifiée à la fois
                //Dans notre cas on veut que toutes les tâches ajoutées soit considérées comme éditées.
                tache4Source.IsEdited = true;
            }

            // Recalcule l'ensemble du budget
            BudgetDetailCalculator.Calculate($ctrl.Budget);
        }


        //////////////////////////////////////////////////////////////////
        // Evènements                                                   //
        //////////////////////////////////////////////////////////////////

        // Appelé après la modification d'un commentaire via le panneau.
        function CommentaireHasChanged(arg) {
            if (arg.Tache) {
                if (arg.Tache.Niveau === 4) {
                    arg.Tache.BudgetT4.View.Commentaire = arg.Commentaire;
                }
                else {
                    arg.Tache.Info.View.Commentaire = arg.Commentaire;
                }
            }
        }

        // Appelé après la customization de l'affichage
        function CustomizeDisplay(arg) {
            $ctrl.ColumnsVisible = arg.ColumnsVisible;
            $ctrl.NiveauxVisible = arg.NiveauxVisible;
        }

        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////
        function GetShowCommentButtonTooltip() {
            return $ctrl.Budget.Readonly
                ? $ctrl.resources.Budget_Detail_Tooltip_VoirCommentaire
                : $ctrl.resources.Budget_Detail_Tooltip_ModifierCommentaire;
        }


        function onToutPlier() {
            $(".collapse").collapse('hide');
        }

        function onToutDeplier() {
            $(".collapse").collapse('show');
        }



        // Supprime un budget T4 de la vue
        // - budgetT4Id : identifiant du budget T4
        function DeleteBudgetT4FromView(budgetT4Id) {
            for (let tache1 of $ctrl.Budget.Taches1) {
                for (let tache2 of tache1.Taches2) {
                    for (let tache3 of tache2.Taches3) {
                        for (var i = 0; i < tache3.Taches4.length; i++) {
                            if (tache3.Taches4[i].BudgetT4.BudgetT4Id === budgetT4Id) {
                                tache3.Taches4.splice(i, 1);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        function ShouldDisplayTaskLine(tache) {

            let hasMontant = false;
            if (tache.BudgetT4) {
                hasMontant = tache.BudgetT4.MontantT4 !== null;
            } else {
                hasMontant = tache.Montant !== null;
            }

            return ($ctrl.disabledTasksDisplayed || (tache.Code.indexOf('T4REV') > -1) || hasMontant || tache.IsActive)
                && $ctrl.NiveauxVisible.includes(tache.Niveau);
        }

        function IsColumnVisible(columnName) {
            return $ctrl.ColumnsVisible.includes(columnName);
        }
    }
})();
