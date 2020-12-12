using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.ControleBudgetaire.Helpers;
using Fred.Business.Budget.ControleBudgetaire.Models;
using Fred.Business.Budget.Helpers;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Depense.Services;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Referential.Common;
using Fred.Entities.Budget;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.Budget;
using Fred.Web.Shared.Models.Budget.ControleBudgetaire;
using static Fred.Entities.Constantes;

namespace Fred.Business.Budget.ControleBudgetaire
{
    /// <summary>
    /// Implémentation de l'interface IControleBudgetaireManager
    /// </summary>
    public class ControleBudgetaireManager : Manager<ControleBudgetaireEnt, IControleBudgetaireRepository>, IControleBudgetaireManager
    {
        private readonly IBudgetManager budgetManager;
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly IAvancementManager avancementManager;
        private readonly IBudgetEtatManager budgetEtatManager;
        private readonly ICIManager ciManager;
        private readonly IDepenseServiceMediator depenseServiceMediator;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly ITacheSearchHelper taskSearchHelper;

        public ControleBudgetaireManager(
            IUnitOfWork uow,
            IControleBudgetaireRepository controleBudgetaireRepository,
            IBudgetManager budgetManager,
            IBudgetT4Manager budgetT4Manager,
            IAvancementManager avancementManager,
            IBudgetEtatManager budgetEtatManager,
            ICIManager ciManager,
            IDepenseServiceMediator depenseServiceMediator,
            IDatesClotureComptableManager datesClotureComptableManager,
            ITacheSearchHelper taskSearchHelper
            )
          : base(uow, controleBudgetaireRepository)
        {
            this.budgetManager = budgetManager;
            this.budgetT4Manager = budgetT4Manager;
            this.avancementManager = avancementManager;
            this.budgetEtatManager = budgetEtatManager;
            this.ciManager = ciManager;
            this.depenseServiceMediator = depenseServiceMediator;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.taskSearchHelper = taskSearchHelper;
        }

        public ChangeEtatResultModel SauveControleBudgetaire(ControleBudgetaireSaveModel saveModel)
        {
            //L'utilisateur a lancé une sauvegarde d'une controle budgétaire, cela veut dire que toutes les données
            //Qu'on a actuallement en base sont potentiellements obsolètes (certaines ont bougé mais d'autres non)
            //Comme on a de toute façon toutes les info nécessaires dans la variable controleBudgétaire
            //On peut sans risque effacer toutes les lignes de la table concernant ce budget et en insérer de nouvelles
            BudgetEtatEnt budgetEtatBrouillon = budgetEtatManager.GetByCode(EtatBudget.Brouillon);

            //D'abord on regarde si un ControleBudgétaire existe dejà pour ce budget
            ControleBudgetaireEnt controleBudgetaire = Repository.GetControleBudgetaire(saveModel.BudgetId, saveModel.Periode, true);
            if (controleBudgetaire != null)
            {
                SupprimeValeursControleBudgetairePourBudgetEtPeriode(saveModel.BudgetId, saveModel.Periode);

                //Quel que soit l'état du controle budgetaire si il est enregistré on repasse son etat à l'état Brouillon
                controleBudgetaire.ControleBudgetaireEtatId = budgetEtatBrouillon.BudgetEtatId;
            }
            else
            {
                controleBudgetaire = new ControleBudgetaireEnt()
                {
                    ControleBudgetaireId = saveModel.BudgetId,
                    ControleBudgetaireEtatId = budgetEtatBrouillon.BudgetEtatId,
                    Valeurs = new List<ControleBudgetaireValeursEnt>(),
                    Periode = saveModel.Periode,
                };
                Repository.Insert(controleBudgetaire);
                Save();
            }

            foreach (ControleBudgetaireSaveModelValeurs valeurs in saveModel.ControleBudgetaireValeurs)
            {
                ControleBudgetaireValeursEnt controleBudgetaireValeurs = new ControleBudgetaireValeursEnt()
                {
                    ControleBudgetaireId = controleBudgetaire.ControleBudgetaireId,
                    Periode = saveModel.Periode,
                    Ajustement = Math.Round(valeurs.Ajustement, 2),
                    CommentaireAjustement = valeurs.CommentaireAjustement,
                    TacheId = valeurs.TacheId,
                    RessourceId = valeurs.RessourceId,
                    Pfa = Math.Round(valeurs.Pfa, 2)
                };
                controleBudgetaire.Valeurs.Add(controleBudgetaireValeurs);
            }

            Repository.Update(controleBudgetaire);
            Save();

            controleBudgetaire.ControleBudgetaireEtat = budgetEtatBrouillon;
            return new ChangeEtatResultModel()
            {
                NouvelEtat = new BudgetEtatModel(budgetEtatBrouillon.Code, null)
            };
        }

        public async Task<ControleBudgetaireModel> GetControleBudgetaireAsync(ControleBudgetaireLoadModel filter)
        {
            AbstractControleBudgetaireDataSourceBuilder controleBudgetaireGenerator = GetControleBudgetaireGenerator(filter.Cumul);
            BudgetEnt budget = GetBudgetForCiAndPeriode(filter.CiId, filter.PeriodeComptable);

            if (budget == null)
            {
                throw new FredBusinessMessageResponseException("Aucun budget en application pour ce CI");
            }

            AxeTreeDataSource sources = await controleBudgetaireGenerator.BuildDataSourceAsync(filter.CiId, budget.BudgetId, filter.PeriodeComptable).ConfigureAwait(false);

            AxeTreeBuilder axe = new AxeTreeBuilder(sources, filter.AxePrincipal, taskSearchHelper, filter.AxeAffichees);
            IEnumerable<AxeTreeModel> tree = axe.GenerateTree();

            return GetControleBudgetaireModel(tree, budget, filter.PeriodeComptable);
        }

        private AbstractControleBudgetaireDataSourceBuilder GetControleBudgetaireGenerator(bool isCumul)
        {
            if (isCumul)
            {
                return new ValeursCumuleesDataSourceBuilder(this, depenseServiceMediator, budgetManager, budgetT4Manager, ciManager, avancementManager);
            }
            else
            {
                return new ValeursMoisCourantDataSourceBuilder(this, depenseServiceMediator, budgetManager, budgetT4Manager, ciManager, avancementManager);
            }
        }

        public ChangeEtatResultModel ChangeEtatControleBudgetaire(int budgetId, int periode, string codeEtat)
        {
            ControleBudgetaireEnt controleBudgetaire = Repository.GetControleBudgetaireByBudgetId(budgetId, periode);

            ChangeEtatResultModel changeEtatResult = PeutPasserAEtat(budgetId, periode, codeEtat);

            //La demande de validation est un peu spéciale, elle est toujours acceptée même si l'avancement et le CI n'est pas cloturé
            if (!changeEtatResult.AllOkay && codeEtat != EtatBudget.AValider)
            {
                return changeEtatResult;
            }

            BudgetEtatEnt nouvelEtat = budgetEtatManager.GetByCode(codeEtat);
            controleBudgetaire.ControleBudgetaireEtatId = nouvelEtat.BudgetEtatId;

            Repository.Update(controleBudgetaire);
            Save();

            // controle si l'identifiant du budget a changé suite au retour à l'état brouillon
            if (nouvelEtat.Code == EtatBudget.Brouillon)
            {
                var currentBudget = budgetManager.GetBudget(controleBudgetaire.ControleBudgetaireId);
                BudgetEnt expectedBudget = GetBudgetForCiAndPeriode(currentBudget.CiId, controleBudgetaire.Periode);
                if (currentBudget.BudgetId != expectedBudget.BudgetId)
                {
                    changeEtatResult.BudgetIdHasChanged = true;
                }
            }

            return changeEtatResult;
        }

        public ChangeEtatResultModel PeutPasserAEtat(int budgetId, int periode, string etatCodeDestination)
        {
            ControleBudgetaireEnt controleBudgetaire = Repository.GetControleBudgetaireByBudgetId(budgetId, periode);
            switch (etatCodeDestination)
            {
                case EtatBudget.AValider:
                    {
                        ChangeEtatResultModel changeResultModel = GetDefaultChangeEtatResultModel(budgetId, periode, etatCodeDestination);

                        //Vérification de l'état du controle budgétaire actuel
                        changeResultModel.EtatPrecedentOkay = controleBudgetaire.ControleBudgetaireEtat.Code == EtatBudget.Brouillon;
                        return changeResultModel;
                    }

                case EtatBudget.EnApplication:
                    {
                        ChangeEtatResultModel changeResultModel = GetDefaultChangeEtatResultModel(budgetId, periode, etatCodeDestination);

                        //Vérification de l'état du controle budgétaire actuel
                        changeResultModel.EtatPrecedentOkay = controleBudgetaire.ControleBudgetaireEtat.Code == EtatBudget.AValider;
                        return changeResultModel;
                    }

                case EtatBudget.Brouillon:
                    {
                        bool etatPrecedentOkay = controleBudgetaire.ControleBudgetaireEtat.Code == EtatBudget.AValider || controleBudgetaire.ControleBudgetaireEtat.Code == EtatBudget.EnApplication;
                        return new ChangeEtatResultModel()
                        {
                            EtatPrecedentOkay = etatPrecedentOkay,
                            NouvelEtat = new BudgetEtatModel(etatCodeDestination, null)
                        };
                    }
                default:
                    throw new FredBusinessMessageResponseException("Impossible de changer l'état du controle budgétaire. Etat de destination inconnu");
            }
        }

        private ControleBudgetaireModel GetControleBudgetaireModel(IEnumerable<AxeTreeModel> tree, BudgetEnt budgetEnApplication, int periode)
        {
            int ciId = budgetManager.GetCiIdAssociatedToBudgetId(budgetEnApplication.BudgetId);
            bool isAvancementValide = avancementManager.IsAvancementValide(ciId, budgetEnApplication.BudgetId, periode);
            bool isPeriodeCloturee = CheckIfPeriodeIsCloturee(ciId, periode);

            //Maintenant que l'axe est généré on construit le model
            ControleBudgetaireModel controleBudgetaireModel = new ControleBudgetaireModel()
            {
                BudgetId = budgetEnApplication.BudgetId,
                Locked = false,
                Readonly = false,
                Periode = periode,
                AvancementValide = isAvancementValide,
                PeriodeCloturee = isPeriodeCloturee,
                BudgetVersion = budgetEnApplication.Version,
                DateBudget = DateTime.ParseExact(budgetEnApplication.PeriodeDebut.Value.ToString(), "yyyyMM", CultureInfo.CurrentCulture).ToString("MMMM yyyy")
            };

            ControleBudgetaireEnt controleBudgetaire = Repository.GetControleBudgetaireByBudgetId(budgetEnApplication.BudgetId, periode);

            if (controleBudgetaire == null)
            {
                //Cela veut dire qu'aucun controle budgétaire n'existe encore pour ce budget en application
                //On ne le crée pas tout de suite mais on peut donner au model le code Brouillon
                controleBudgetaireModel.CodeEtat = EtatBudget.Brouillon;
            }
            else
            {
                controleBudgetaireModel.CodeEtat = controleBudgetaire.ControleBudgetaireEtat.Code;
                controleBudgetaireModel.Readonly = controleBudgetaireModel.CodeEtat != EtatBudget.Brouillon;
            }

            int lastValidation = Repository.GetLastValidationPeriode(ciId);

            //Un controle budgétaire ne peut être modifié que s'il n'existe aucun autre controle validé dans une période future
            //(Futur de la période choisie par l'utilisateur, on peut très bien être en décembre et l'utilisateur choisi la periode de Février)
            //Le controle n'est pas non plus modifiable s'il n'est pas à l'état brouillon
            if (lastValidation > periode)
            {
                controleBudgetaireModel.Locked = true;
                controleBudgetaireModel.Readonly = true;
            }

            controleBudgetaireModel.Tree = tree;

            return controleBudgetaireModel;
        }

        public void SupprimeValeursControleBudgetairePourBudgetEtPeriode(int budgetId, int? periode = null)
        {
            Repository.RemoveAllValeursForBudgetEtPeriode(budgetId, periode);
            Save();
        }

        public void SupprimeControleBudgetairePourBudgetEtPeriode(int budgetId, int? periode = null)
        {
            Repository.RemoveControleBudgetairePourBudgetEtPeriode(budgetId, periode);
            Save();
        }

        public bool IsControleBudgetaireValide(int budgetId, int periode)
        {
            string code = Repository.GetEtatControleBudgetaire(budgetId, periode);
            return code == EtatBudget.EnApplication;
        }

        public ControleBudgetaireValeursEnt GetValeursForPeriodeByTacheEtRessource(int controleBudgetaireId, int periode, int tache3Id, int ressourceId)
        {
            return Repository.GetValeursForPeriodeByTacheEtRessource(controleBudgetaireId, periode, tache3Id, ressourceId);
        }

        public IEnumerable<ControleBudgetaireValeursEnt> GetControleBudgetaireValeurs(int budgetId, int periode)
        {
            ControleBudgetaireEnt controleBudgetaire = Repository.GetControleBudgetaire(budgetId, periode, true);
            return controleBudgetaire?.Valeurs;
        }

        public int? GetControleBudgetaireLatestPeriode(int budgetId, int? maxPeriode)
        {
            return Repository.GetControleBudgetaireLatestPeriode(budgetId, maxPeriode);
        }

        public IEnumerable<int> GetAllPeriodesBetweenPeriodeAndLastValidation(int budgetId, int periode)
        {
            var budget = budgetManager.GetBudget(budgetId, false);
            if (budget == null)
            {
                return new List<int>() { periode };
            }

            int lastValidation = Repository.GetLastValidationPeriode(budget.CiId, periode);
            if (lastValidation != 0)
            {
                return GetAllPeriodeBetweenPeriodes(PeriodeHelper.GetNextPeriod(lastValidation).Value, periode);
            }
            else
            {
                //Ici pas de date de validation on prend donc la periode de début du budget
                int? periodeDebut = budgetManager.GetPeriodeDebutBudget(budgetId);
                if (!periodeDebut.HasValue)
                {
                    return new List<int>() { periode };
                }

                return GetAllPeriodeBetweenPeriodes(periodeDebut.Value, periode);
            }
        }

        private IEnumerable<int> GetAllPeriodeBetweenPeriodes(int oldest, int newest)
        {
            for (int p = oldest; p < newest; p = PeriodeHelper.GetNextPeriod(p).Value)
            {
                yield return p;
            }
        }


        private bool CheckIfPeriodeIsCloturee(int ciId, int periode)
        {
            //Vérification de la cloture de la période
            int year = periode / 100;
            int month = periode % 100;
            return datesClotureComptableManager.IsPeriodClosed(ciId, year, month);
        }

        private ChangeEtatResultModel GetDefaultChangeEtatResultModel(int budgetId, int periode, string codeEtatDestination)
        {
            //Vérification de la cloture de l'avancement
            int ciId = budgetManager.GetCiIdAssociatedToBudgetId(budgetId);
            bool isAvancementValide = avancementManager.IsAvancementValide(ciId, budgetId, periode);
            bool isPeriodeCloturee = CheckIfPeriodeIsCloturee(ciId, periode);

            return new ChangeEtatResultModel()
            {
                AvancementValide = isAvancementValide,
                PeriodeComptableCloturee = isPeriodeCloturee,
                NouvelEtat = new BudgetEtatModel(codeEtatDestination, null)
            };
        }

        public BudgetEnt GetBudgetForCiAndPeriode(int ciId, int periode)
        {
            // si un controle budgétaire a été "validé" ou est "à valider" pour la période, 
            // le budget retourné est le budget liè à ce controle budgétaire
            ControleBudgetaireEnt controleBudgetaireValide = Repository.GetControleBudgetaireValideForCiAndPeriode(ciId, periode);
            if (controleBudgetaireValide != null)
            {
                return controleBudgetaireValide.Budget;
            }
            // dans tous les autres cas, retourne le budget en application
            return budgetManager.GetBudgetEnApplication(ciId);
        }

        public List<int> GetListPeriodeControleBudgetaireValide(int budgetId)
        {
            return Repository.GetListPeriodeControleBudgetaireValide(budgetId);
        }
    }
}
