(function () {
    'use strict';

    angular.module('Fred').service('BudgetDetailService', BudgetDetailService);
    BudgetDetailService.$inject = ['BudgetService', 'BudgetCalculator', 'BudgetDetailCalculator', 'BudgetSousDetailSdCalculator', 'BudgetSousDetailT4Calculator', '$filter'];

    function BudgetDetailService(BudgetService, BudgetCalculator, BudgetDetailCalculator, BudgetSousDetailSdCalculator, BudgetSousDetailT4Calculator, $filter) {
        var _sousDetailItemViewId = 0;
        var _numberFilter = $filter('number');
        var _sousDetailCalculator = null;

        var service = {

            // Enumérations
            ViewModeType: {
                None: 0,
                Detail: 1,
                SousDetail: 2
            },

            // Provient de BudgetSousDetailViewMode
            SousDetailViewModeType: {
                T4: 0,
                SD: 1,
                PU: 2
            },

            NavigateT4Enum: {
                PreviousT4: 0,
                NextT4: 1
            },

            // Action sélectionnée dans l'écran de copie / déplacement de T4
            CopierDeplacerT4ActionEnum: {
                Copier: "Copier",
                Deplacer: "Deplacer",
                Remplacer: "Remplacer"
            },

            // Mode de copie
            CopierT4Mode: {
                Ajouter: "Ajouter",
                Remplacer: "Remplacer"
            },

            // Functions
            InitialiseBudget: InitialiseBudget,
            InitialiseTaches: InitialiseTaches,
            InitialiseTache1To3: InitialiseTache1To3,
            InitialiseTache4: InitialiseTache4,
            InitialiseSousDetail: InitialiseSousDetail,
            InitialiseSousDetailItem: InitialiseSousDetailItem,
            ProcessSousDetail: ProcessSousDetail,
            ResetSousDetailItemViewIdCounter: ResetSousDetailItemViewIdCounter,

            SousDetailItemUniteChanged: SousDetailItemUniteChanged,
            SousDetailItemQuantiteSdChanged: SousDetailItemQuantiteSdChanged,
            SousDetailItemQuantiteSdFormuleChanged: SousDetailItemQuantiteSdFormuleChanged,
            SousDetailItemQuantiteChanged: SousDetailItemQuantiteChanged,
            SousDetailItemQuantiteFormuleChanged: SousDetailItemQuantiteFormuleChanged,
            SousDetailItemPrixUnitaireChanged: SousDetailItemPrixUnitaireChanged,
            SousDetailItemAdded: SousDetailItemAdded,
            SousDetailItemDeleted: SousDetailItemDeleted,
            QuantiteDeBaseChanged: QuantiteDeBaseChanged,
            QuantiteARealiserChanged: QuantiteARealiserChanged,

            GetTooltip: GetTooltip
        };

        // Variables
        service.ViewMode = service.ViewModeType.None;
        _sousDetailCalculator = null;

        return service;

        // Initialise un budget.
        // - budget : le budget concerné.
        function InitialiseBudget(budget) {
            budget.Montant = 0;

            // Retourne toutes les tâches.
            budget.GetTaches = function* () {
                if (this.Taches1) {
                    for (let tache1 of this.Taches1) {
                        yield tache1;
                        for (let tache2 of tache1.Taches2) {
                            yield tache2;
                            for (let tache3 of tache2.Taches3) {
                                yield tache3;
                                for (let tache4 of tache3.Taches4) {
                                    yield tache4;
                                }
                            }
                        }
                    }
                }
            };

            // Retourne toutes les tâches de niveau 4.
            budget.GetTaches4 = function* () {
                if (this.Taches1) {
                    for (let tache1 of this.Taches1) {
                        for (let tache2 of tache1.Taches2) {
                            for (let tache3 of tache2.Taches3) {
                                for (let tache4 of tache3.Taches4) {
                                    yield tache4;
                                }
                            }
                        }
                    }
                }
            };

            // Indique si le détail a changé
            budget.DetailHasChanged = function () {
                return GetDetailChanges(this, true);
            };

            // Retourne les changements du détail
            // - return : les changements du détail
            budget.GetDetailChanges = function () {
                return GetDetailChanges(this, false);
            };

            // Edition une tache, flag la tache éditée, deflag les autres
            budget.EditTache = function (tache) {
                for (let tache4 of this.GetTaches4()) {
                    tache4.IsEdited = false;
                }

                if (tache !== null && tache.Niveau === 4)
                    tache.IsEdited = true;
            };
        }

        // Initialise les tâches d'un budget.
        // - budget : le budget concerné.
        // - onlyParents : indique si seuls les parents des tâches doivent être affectés.
        // Note : l'ajout des parents aux tâches ne peut pas être fait coté back à cause des références cycliques.
        // Note : onlyParent = true est utilisé dans le détail budget pour un chargement complet
        //        onlyParent = false est utilisé dans la copie / déplacement de T4 pour un chargement light
        function InitialiseTaches(budget, onlyParents) {
            if (!budget || !budget.Taches1) {
                return;
            }
            for (let tache1 of budget.Taches1) {
                if (!onlyParents) {
                    service.InitialiseTache1To3(tache1, 1);
                }
                for (let tache2 of tache1.Taches2) {
                    tache2.Tache1 = tache1;
                    if (!onlyParents) {
                        service.InitialiseTache1To3(tache2, 2);
                    }
                    for (let tache3 of tache2.Taches3) {
                        tache3.Tache2 = tache2;
                        if (!onlyParents) {
                            service.InitialiseTache1To3(tache3, 3);
                        }
                        for (let tache4 of tache3.Taches4) {
                            tache4.Tache3 = tache3;
                            if (!onlyParents) {
                                service.InitialiseTache4(tache4);
                            }
                        }
                    }
                }
            }
        }

        // Initialise une tâche de niveau 1 à 3.
        // - tache : la tâche concernée.
        // - niveau : le niveau de la tâche.
        function InitialiseTache1To3(tache, niveau) {
            tache.Niveau = niveau;
            var info = tache.Info;
            if (!info) {
                // Simule un [BudgetDetailLoad.Tache1To3InfoModel]
                info = BudgetService.CreateBudgetDetailLoadTache1To3InfoModel();
                tache.Info = info;
            }
            info.View = {
                Commentaire: info.Commentaire
            };
            info.HasChanged = function () {
                if (this.View.Commentaire !== this.Commentaire.trim()) {
                    return true;
                }
                return false;
            };
            info.Saved = function () {
                this.Commentaire = this.View.Commentaire.trim();
            };
        }

        // Initialise une tâche de niveau 4.
        // - tache : la tâche concernée.
        function InitialiseTache4(tache) {
            tache.Niveau = 4;
            tache.Deleted = false;

            var budgetT4 = tache.BudgetT4;
            if (!budgetT4) {
                // Simule un [BudgetDetailLoad.BudgetT4Model]
                budgetT4 = BudgetService.CreateBudgetDetailLoadBudgetT4Model(service.SousDetailViewModeType.SD);
                tache.BudgetT4 = budgetT4;
            }
            else {
                budgetT4.QuantiteDeBase = BudgetCalculator.GetValue(budgetT4.QuantiteDeBase);
                budgetT4.QuantiteARealiser = BudgetCalculator.GetValue(budgetT4.QuantiteARealiser);
                budgetT4.MontantT4 = BudgetCalculator.GetValue(budgetT4.MontantT4);
                budgetT4.MontantSD = null;
                if (budgetT4.Commentaire === null) {
                    budgetT4.Commentaire = "";
                }
                else {
                    budgetT4.Commentaire = budgetT4.Commentaire.trim();
                }
            }
            budgetT4.Tache = tache;
            budgetT4.Current = {
                QuantiteDeBase: budgetT4.QuantiteDeBase,
                QuantiteARealiser: budgetT4.QuantiteARealiser
            };
            budgetT4.View = {
                TypeAvancement: budgetT4.TypeAvancement,
                Commentaire: budgetT4.Commentaire,
                QuantiteDeBase: budgetT4.QuantiteDeBase,
                QuantiteARealiser: budgetT4.QuantiteARealiser,
                Unite: budgetT4.Unite,
                MontantT4: budgetT4.MontantT4,
                MontantSD: budgetT4.MontantSD,
                PU: budgetT4.PU,
                VueSD: budgetT4.VueSD,
                Tache3Id: budgetT4.Tache3Id
            };
            budgetT4.HasChangedInDetail = function () {
                if (this.View.TypeAvancement !== this.TypeAvancement) {
                    return true;
                }
                if (this.View.Commentaire.trim() !== this.Commentaire) {
                    return true;
                }
                if (this.View.Tache3Id !== this.Tache3Id) {
                    return true;
                }
                return false;
            };
            budgetT4.SavedForDetail = function () {
                this.TypeAvancement = this.View.TypeAvancement;
                this.Commentaire = this.View.Commentaire.trim();
                this.Tache3Id = this.View.Tache3Id;
            };
            budgetT4.HasChangedInSousDetail = function () {
                if (BudgetCalculator.GetValue(this.View.QuantiteDeBase) !== this.QuantiteDeBase) {
                    return true;
                }
                if (BudgetCalculator.GetValue(this.View.QuantiteARealiser) !== this.QuantiteARealiser) {
                    return true;
                }
                if (!UniteAreEqual(this.View.Unite, this.Unite)) {
                    return true;
                }
                if (!BudgetCalculator.EqualsRounded(this.View.MontantT4, this.MontantT4, 3)) {
                    return true;
                }
                if (!BudgetCalculator.EqualsRounded(this.View.PU, this.PU, 3)) {
                    return true;
                }
                if (this.View.VueSD !== this.VueSD) {
                    return true;
                }
                return false;
            };
            budgetT4.SavedForSousDetail = function () {
                this.QuantiteDeBase = BudgetCalculator.GetValue(this.View.QuantiteDeBase);
                this.QuantiteARealiser = BudgetCalculator.GetValue(this.View.QuantiteARealiser);
                this.Unite = this.View.Unite;
                this.MontantT4 = this.View.MontantT4;
                this.MontantSD = this.View.MontantSD;
                this.PU = this.View.PU;
                this.VueSD = this.View.VueSD;
            };
            budgetT4.ResetForSousDetail = function () {
                this.View.QuantiteDeBase = this.QuantiteDeBase;
                this.View.QuantiteARealiser = this.QuantiteARealiser;
                this.View.Unite = this.Unite;
                this.View.MontantT4 = this.MontantT4;
                this.View.MontantSD = this.MontantSD;
                this.View.PU = this.PU;
                this.View.VueSD = this.VueSD;
                this.Current.QuantiteDeBase = this.QuantiteDeBase;
                this.Current.QuantiteARealiser = this.QuantiteARealiser;
            };
        }

        // Initialise un sous-détail.
        // Note : les sous-détails doivent être initialisés avant.
        // - sousDetail : le sous détail concerné.
        // - budgetT4 : le budget T4 correspondant.
        // - budget : le budget correspondant.
        function InitialiseSousDetail(sousDetail, budgetT4, budget) {
            sousDetail.BudgetT4 = budgetT4;
            sousDetail.Budget = budget;
            sousDetail.MontantBudgetOriginal = budget.Montant;
            if (budgetT4.MontantT4 !== null) {
                sousDetail.MontantBudgetSansCeT4 = budget.Montant - budgetT4.MontantT4;
            }
            else {
                sousDetail.MontantBudgetSansCeT4 = budget.Montant;
            }

            sousDetail.HasChanged = function () {
                var changed = this.BudgetT4.HasChangedInSousDetail();
                if (!changed) {
                    for (let sousDetailItem of this.Items) {
                        if (sousDetailItem.Deleted || sousDetailItem.HasChanged(this.BudgetT4.View.VueSD)) {
                            changed = true;
                            break;
                        }
                    }
                }
                return changed;
            };
        }

        // Initialise un item de sous-détail.
        // - sousDetailItem : l'item concerné.
        // - sousDetail : le sous-détail correspondant.
        function InitialiseSousDetailItem(sousDetailItem, sousDetail) {
            sousDetailItem.SousDetail = sousDetail;
            sousDetailItem.PrixUnitaire = BudgetCalculator.GetValue(sousDetailItem.PrixUnitaire);
            sousDetailItem.QuantiteSD = BudgetCalculator.GetValue(sousDetailItem.QuantiteSD);
            sousDetailItem.Quantite = BudgetCalculator.GetValue(sousDetailItem.Quantite);
            sousDetailItem.MontantSD = null;
            sousDetailItem.Montant = null;
            sousDetailItem.Deleted = false;
            if (sousDetailItem.Commentaire === null) {
                sousDetailItem.Commentaire = "";
            }
            else {
                sousDetailItem.Commentaire = sousDetailItem.Commentaire.trim();
            }
            sousDetailItem.Current = {
                PrixUnitaire: sousDetailItem.PrixUnitaire,
                QuantiteSD: sousDetailItem.QuantiteSD,
                Quantite: sousDetailItem.Quantite,
                Unite: sousDetailItem.Unite
            };
            sousDetailItem.ViewId = _sousDetailItemViewId++;
            sousDetailItem.View = {
                PrixUnitaire: sousDetailItem.PrixUnitaire,
                QuantiteSD: sousDetailItem.QuantiteSD,
                QuantiteSDFormule: sousDetailItem.QuantiteSDFormule,
                Quantite: sousDetailItem.Quantite,
                QuantiteFormule: sousDetailItem.QuantiteFormule,
                MontantSD: sousDetailItem.MontantSD,
                Montant: sousDetailItem.Montant,
                Commentaire: sousDetailItem.Commentaire,
                Unite: sousDetailItem.Unite,
                SousDetail: sousDetail
            };

            sousDetailItem.HasChanged = function (viewModeType) {
                if (!BudgetCalculator.EqualsRounded(this.View.PrixUnitaire, this.PrixUnitaire, 3)) {
                    return true;
                }
                if (!BudgetCalculator.EqualsRounded(this.View.QuantiteSD, this.QuantiteSD, 3)
                    && viewModeType === service.SousDetailViewModeType.SD) {
                    return true;
                }
                if (this.View.QuantiteSDFormule !== this.QuantiteSDFormule
                    && viewModeType === service.SousDetailViewModeType.SD) {
                    return true;
                }
                if (!BudgetCalculator.EqualsRounded(this.View.Quantite, this.Quantite, 3)) {
                    return true;
                }
                if (this.View.QuantiteFormule !== this.QuantiteFormule
                    && viewModeType === service.SousDetailViewModeType.T4) {
                    return true;
                }
                if (this.View.Commentaire.trim() !== this.Commentaire) {
                    return true;
                }
                if (!UniteAreEqual(this.View.Unite, this.Unite)) {
                    return true;
                }
                return false;
            };
            sousDetailItem.Saved = function () {
                this.PrixUnitaire = BudgetCalculator.GetValue(this.View.PrixUnitaire);
                this.QuantiteSD = BudgetCalculator.GetValue(this.View.QuantiteSD);
                this.QuantiteSDFormule = this.View.QuantiteSDFormule;
                this.Quantite = BudgetCalculator.GetValue(this.View.Quantite);
                this.QuantiteFormule = this.View.QuantiteFormule;
                this.MontantSD = BudgetCalculator.GetValue(this.View.MontantSD);
                this.Montant = BudgetCalculator.GetValue(this.View.Montant);
                this.Commentaire = this.View.Commentaire.trim();
                this.Unite = this.View.Unite;
                this.Current.PrixUnitaire = this.PrixUnitaire;
                this.Current.QuantiteSD = this.QuantiteSD;
                this.Current.Quantite = this.Quantite;
                this.Current.Unite = this.Unite;
            };
        }

        // Change la vue du sousdétail.
        // - sousDetail : le sous-détail concerné.
        // - updateQuantite : indique si les quantités doivent être swapées d'une vue à l'autre au besoin
        function ProcessSousDetail(sousDetail, updateQuantite) {
            if (sousDetail.BudgetT4.View.VueSD === service.SousDetailViewModeType.SD) {
                _sousDetailCalculator = BudgetSousDetailSdCalculator;
                _sousDetailCalculator.Calculate(sousDetail, updateQuantite);
            }
            else if (sousDetail.BudgetT4.View.VueSD === service.SousDetailViewModeType.T4) {
                _sousDetailCalculator = BudgetSousDetailT4Calculator;
                _sousDetailCalculator.Calculate(sousDetail, updateQuantite);
            }
            else {
                _sousDetailCalculator = null;
            }
        }

        // Reset le compteur pour la vue des identifiants des éléments de sous-détail.
        function ResetSousDetailItemViewIdCounter() {
            _sousDetailItemViewId = 0;
        }


        // A appeler lorsque l'unité d'un élément de sous-détail est changée.
        // - item : élement du sous-détail concerné.
        function SousDetailItemUniteChanged(item) {
            if (!UniteAreEqual(item.View.Unite, item.Current.Unite)) {
                item.Current.Unite = item.View.Unite;
                _sousDetailCalculator.UniteChanged(item);
            }
        }

        // A appeler lorsque la quantité SD d'un élément de sous-détail est changée.
        // - item : élement du sous-détail concerné.
        function SousDetailItemQuantiteSdChanged(item) {
            var value = BudgetCalculator.GetValue(item.View.QuantiteSD);
            if (value !== item.Current.QuantiteSD) {
                item.Current.QuantiteSD = value;
                _sousDetailCalculator.ItemChanged(item);
            }
        }

        // A appeler lorsque la formule de quantité SD d'un élément de sous-détail est changée.
        // - item : élement du sous-détail concerné.
        function SousDetailItemQuantiteSdFormuleChanged(item) {
            if (item.Current.QuantiteSDFormule !== item.View.QuantiteSDFormule) {
                item.Current.QuantiteSDFormule = item.View.QuantiteSDFormule;
                _sousDetailCalculator.ItemChanged(item);
            }
        }

        // A appeler lorsque la quantité T4 d'un élément de sous-détail est changée.
        // - item : élement du sous-détail concerné.
        function SousDetailItemQuantiteChanged(item) {
            var value = BudgetCalculator.GetValue(item.View.Quantite);
            if (value !== item.Current.Quantite) {
                item.Current.Quantite = value;
                _sousDetailCalculator.ItemChanged(item);
            }
        }

        // A appeler lorsque la formule de quantité T4 d'un élément de sous-détail est changée.
        // - item : élement du sous-détail concerné.
        function SousDetailItemQuantiteFormuleChanged(item) {
            if (item.View.QuantiteFormule !== item.Current.QuantiteFormule) {
                item.Current.QuantiteFormule = item.View.QuantiteFormule;
                _sousDetailCalculator.ItemChanged(item);
            }
        }

        // A appeler lorsque le prix unitaire d'un élément de sous-détail est changée.
        // - item : élement du sous-détail concerné.
        function SousDetailItemPrixUnitaireChanged(item) {
            var value = BudgetCalculator.GetValue(item.View.PrixUnitaire);
            if (value !== item.Current.PrixUnitaire) {
                item.Current.PrixUnitaire = value;
                _sousDetailCalculator.ItemChanged(item);
            }
        }

        // A appeler lorsqu'un élément du sous détail est ajouté.
        // - item : élement du sous-détail concerné.
        // - sousDetail : le sous-détail correspondant.
        // - montant : le montant à utiliser.
        function SousDetailItemAdded(item, sousDetail, ressource) {
            InitialiseSousDetailItem(item, sousDetail);
            _sousDetailCalculator.ItemAdded(item, sousDetail, ressource);
        }

        // A appeler lorsqu'un élément du sous détail est supprimé.
        // - item : élement du sous-détail concerné.
        function SousDetailItemDeleted(item) {
            _sousDetailCalculator.ItemDeleted(item);
        }

        // A appeler lorsque la quantité de base change.
        // - sousDetail : le sous-détail concerné.
        function QuantiteDeBaseChanged(sousDetail) {
            var budgetT4 = sousDetail.BudgetT4;
            var value = BudgetCalculator.GetValue(budgetT4.View.QuantiteDeBase);
            if (value !== budgetT4.Current.QuantiteDeBase) {
                budgetT4.Current.QuantiteDeBase = value;
                _sousDetailCalculator.QuantiteDeBaseChanged(sousDetail);
            }
        }

        // A appeler lorsque la quantité à réaliser change.
        // - sousDetail : le sous-détail concerné.
        function QuantiteARealiserChanged(sousDetail) {
            var budgetT4 = sousDetail.BudgetT4;
            var value = BudgetCalculator.GetValue(budgetT4.View.QuantiteARealiser);
            if (value !== budgetT4.Current.QuantiteARealiser) {
                budgetT4.Current.QuantiteARealiser = value;
                _sousDetailCalculator.QuantiteARealiserChanged(sousDetail);
            }
        }

        // Retourne le tooltip à utiliser pour un champ de type valeur.
        function GetTooltip(value, precision) {
            if (!value)
                return "";
            if (!precision) {
                precision = 12;
            }
            var ret = _numberFilter(value, precision);
            var i = ret.length;
            while (i-- > 0) {
                if (ret[i] !== '0')
                    break;
            }
            if (ret[i] === '.' || ret[i] === ',')
                i--;
            ret = ret.substring(0, i + 1);
            return ret;
        }

        // Indique si deux unités sont identiques
        function UniteAreEqual(unite1, unite2) {
            return !unite1 && !unite2
                || unite1 && unite2 && unite1.UniteId && unite2.UniteId && unite1.UniteId === unite2.UniteId;
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        // Retourne les changements du détail budget ou indique si un changement existe
        // - budget : le budget concerné
        // - check : true pour vérifier si des changements existes, false pour récupérer ces changements
        // - return : si check = true, un booléen qui indique si des changements existes
        //            si check = false, les changements du détail  
        function GetDetailChanges(budget, check) {
            if (check) {
                for (let tache of budget.GetTaches()) {
                    if (tache.Deleted) {
                        if (tache.Niveau === 4 && tache.BudgetT4.BudgetT4Id !== 0) {
                            return true;
                        }
                    }
                    else {
                        let changed = tache.Niveau === 4 ? tache.BudgetT4.HasChangedInDetail() : tache.Info.HasChanged();
                        if (changed) {
                            return true;
                        }
                    }
                }
                return false;
            }
            else {
                // Crée le [BudgetDetailSave.Model]
                var changes = {
                    Model: BudgetService.CreateBudgetDetailSaveModel(budget.BudgetId),
                    TachesChanged: [],
                    Exists: false
                };
                for (let tache of budget.GetTaches()) {
                    if (tache.Deleted) {
                        if (tache.Niveau === 4 && tache.BudgetT4.BudgetT4Id !== 0) {
                            changes.Model.BudgetT4sDeleted.push(tache.BudgetT4.BudgetT4Id);
                        }
                    }
                    else {
                        let changed = tache.Niveau === 4 ? tache.BudgetT4.HasChangedInDetail() : tache.Info.HasChanged();
                        if (changed) {
                            changes.TachesChanged.push(tache);
                            if (tache.Niveau === 4) {
                                // Ajoute un [BudgetDetailSave.Tache4Model]
                                changes.Model.Taches4.push(BudgetService.CreateBudgetDetailSaveTache4Model(tache.TacheId, tache.BudgetT4.View.TypeAvancement, tache.BudgetT4.View.Commentaire, tache.BudgetT4.View.Tache3Id));
                            }
                            else {
                                // Ajoute un [BudgetDetailSave.Tache1To3Model]
                                changes.Model.Tache1To3s.push(BudgetService.CreateBudgetDetailSaveTache1To3Model(tache.TacheId, tache.Info.View.Commentaire));
                            }
                        }
                    }
                }
                changes.Exists = changes.Model.Tache1To3s.length > 0 || changes.Model.Taches4.length > 0 || changes.Model.BudgetT4sDeleted.length > 0;
                return changes;
            }
        }
    }
})();
