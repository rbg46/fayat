using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Budget.BudgetManager.Dtao;
using Fred.Business.Budget.Helpers;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Recette;
using static Fred.Entities.Constantes;

namespace Fred.Business.Budget.BudgetManager
{
    public class BudgetCopieManager : Manager<BudgetEnt, IBudgetRepository>, IBudgetCopieManager
    {
        private readonly IBudgetRepository budgetRepository;
        private readonly IBudgetEtatManager budgetEtatManager;
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly ICopierBudgetSourceToCible copierBudgetSourceToCible;
        private readonly IBudgetBibliothequePrixItemRepository budgetBibliothequePrixItemRepository;
        private readonly IBudgetT4Repository budgetT4Repository;

        public BudgetCopieManager(
            IUnitOfWork uow,
            IBudgetRepository budgetRepository,
            IBudgetEtatManager budgetEtatManager,
            IBudgetT4Manager budgetT4Manager,
            ICopierBudgetSourceToCible copierBudgetSourceToCible,
            IBudgetBibliothequePrixItemRepository budgetBibliothequePrixItemRepository,
            IBudgetT4Repository budgetT4Repository)
            : base(uow, budgetRepository)
        {
            this.budgetRepository = budgetRepository;
            this.budgetEtatManager = budgetEtatManager;
            this.budgetT4Manager = budgetT4Manager;
            this.copierBudgetSourceToCible = copierBudgetSourceToCible;
            this.budgetBibliothequePrixItemRepository = budgetBibliothequePrixItemRepository;
            this.budgetT4Repository = budgetT4Repository;
        }

        public async Task<BudgetEnt> CopierBudgetDansMemeCiAsync(int budgetACopierId, int utilisateurConnecteId, bool useBibliothequeDesPrix)
        {
            var budgetACopier = budgetRepository.GetBudget(budgetACopierId, true);
            var etatBrouillon = budgetEtatManager.GetByCode(EtatBudget.Brouillon);
            var nouvelleVersion = budgetRepository.GetBudgetMaxVersion(budgetACopier.BudgetId);
            nouvelleVersion = VersionHelper.IncrementVersionMineur(nouvelleVersion);

            //Copie du budget en lui même
            var budgetCopie = new BudgetEnt
            {
                BudgetEtatId = etatBrouillon.BudgetEtatId,
                CiId = budgetACopier.CiId,
                DeviseId = budgetACopier.DeviseId,
                Version = nouvelleVersion,
                Partage = false
            };

            //Création d'un nouveau workflow pour le budget ne contenant qu'une entrée à l'état brouillon.
            budgetCopie.Workflows = new List<BudgetWorkflowEnt>()
            {
                new BudgetWorkflowEnt()
                {
                    AuteurId = utilisateurConnecteId,
                    Budget = budgetCopie,
                    EtatInitialId = null,
                    EtatCibleId = etatBrouillon.BudgetEtatId,
                    Date = DateTime.Now
                }
            };

            CopierBudgetRecettes(budgetACopier, budgetCopie);
            CopierBudgetT4EtSousDetails(budgetACopier, budgetCopie, useBibliothequeDesPrix);

            List<BudgetT4Ent> tachesWithVueT4 = budgetCopie.BudgetT4s.Where(t4 => t4.VueSD == 0).ToList();

            var budgetInsere = budgetRepository.Insert(budgetCopie);
            Save();

            //Sauvegarde de la liste des budgets T4 avec la vue T4;
            //Obligation de forcer l'update a 0, parce que l'insert entity framework core ne les détectent pas.
            await budgetT4Repository.UpdateVueSDToZero(tachesWithVueT4);

            return budgetInsere;
        }

        private void CopierBudgetT4EtSousDetails(BudgetEnt budgetACopier, BudgetEnt budgetCopie, bool useBibliothequePrix)
        {
            var bibliothequePrix = budgetBibliothequePrixItemRepository.GetAllBibliothequePrixItemForCi(
                budgetACopier.CiId,
                budgetACopier.DeviseId,
                new BudgetCopieBibliothequePrixItemDao().Selector);

            //Copie des T4 associés à ce budget
            var budgetT4ACopier = budgetT4Manager.GetByBudgetId(budgetACopier.BudgetId, true);
            budgetT4ACopier = budgetT4ACopier.Where(b => !b.T4.Code.StartsWith("T4REV", StringComparison.OrdinalIgnoreCase));
            budgetCopie.BudgetT4s = new List<BudgetT4Ent>(budgetT4ACopier.Count());
            foreach (var t4ACopier in budgetT4ACopier)
            {
                //Un t4 contient une liste de sous détails donc d'abord on copie le t4
                var t4Copie = new BudgetT4Ent()
                {
                    Budget = budgetCopie,
                    MontantT4 = t4ACopier.MontantT4,
                    QuantiteDeBase = t4ACopier.QuantiteDeBase,
                    QuantiteARealiser = t4ACopier.QuantiteARealiser,
                    TypeAvancement = t4ACopier.TypeAvancement,
                    PU = t4ACopier.PU,
                    T4Id = t4ACopier.T4Id,
                    UniteId = t4ACopier.UniteId,
                    T3Id = t4ACopier.T3Id,
                    VueSD = t4ACopier.VueSD,
                    IsReadOnly = t4ACopier.IsReadOnly
                };

                //Ensuite on copie les sous détails de ce t4
                t4Copie.BudgetSousDetails = new List<BudgetSousDetailEnt>(t4ACopier.BudgetSousDetails.Count);
                foreach (var sousDetailsACopier in t4ACopier.BudgetSousDetails)
                {
                    BudgetCopieBibliothequePrixItemDao prixFromBibliotheque = null;

                    //On n'utilise pas la bibliotheque des prix si le budget T4 vient d'un recalage
                    if (useBibliothequePrix && t4ACopier.T4.BudgetId == null)
                    {
                        //Il est possible que dans la bibliotheque des prix, deux valeures soient saisies,
                        //Une pour l'etablissement, une pour le CI, si c'est le cas il faut prendre la valeur pour le CI
                        prixFromBibliotheque = bibliothequePrix.SingleOrDefault(i => i.RessourceId == sousDetailsACopier.RessourceId);
                    }

                    var pu = sousDetailsACopier.PU;
                    var uniteId = sousDetailsACopier.UniteId;

                    if (prixFromBibliotheque != null)
                    {
                        pu = prixFromBibliotheque.Prix;
                        uniteId = prixFromBibliotheque.UniteId;
                    }

                    t4Copie.BudgetSousDetails.Add(new BudgetSousDetailEnt()
                    {
                        BudgetT4 = t4Copie,
                        Montant = sousDetailsACopier.Montant,
                        PU = pu,
                        Quantite = sousDetailsACopier.Quantite,
                        QuantiteFormule = sousDetailsACopier.QuantiteFormule,
                        QuantiteSD = sousDetailsACopier.QuantiteSD,
                        QuantiteSDFormule = sousDetailsACopier.QuantiteSDFormule,
                        RessourceId = sousDetailsACopier.RessourceId,
                        UniteId = uniteId,
                        Commentaire = sousDetailsACopier.Commentaire
                    });
                }

                t4Copie.MontantT4 = t4Copie.BudgetSousDetails.Sum(sd => sd.PU * sd.Quantite);
                t4Copie.PU = (t4Copie.QuantiteARealiser ?? 0) == 0 ? null : t4Copie.MontantT4 / t4Copie.QuantiteARealiser;

                budgetCopie.BudgetT4s.Add(t4Copie);
            }
        }

        private static void CopierBudgetRecettes(BudgetEnt budgetACopier, BudgetEnt budgetCopie)
        {
            //Un budget peut parfaitement ne pas avoir de recette (s'il est en brouillon par exemple)
            if (budgetACopier.Recette != null)
            {
                //Copie des recettes
                var recette = new BudgetRecetteEnt()
                {
                    TravauxSupplementaires = budgetACopier.Recette.TravauxSupplementaires,
                    AutresRecettes = budgetACopier.Recette.AutresRecettes,
                    MontantAvenants = budgetACopier.Recette.MontantAvenants,
                    MontantMarche = budgetACopier.Recette.MontantMarche,
                    PenalitesEtRetenues = budgetACopier.Recette.PenalitesEtRetenues,
                    Revision = budgetACopier.Recette.Revision,
                    SommeAValoir = budgetACopier.Recette.SommeAValoir,
                };

                budgetCopie.Recette = recette;
            }
        }

        public async Task<CopierBudgetDto.Result> CopierBudgetAsync(CopierBudgetDto.Request request)
        {
            return await copierBudgetSourceToCible.CopierAsync(request);
        }
    }
}
