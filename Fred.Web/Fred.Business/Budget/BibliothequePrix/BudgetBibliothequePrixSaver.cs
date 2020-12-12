using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Web.Shared.Models.Budget.BibliothequePrix;

namespace Fred.Business.Budget.BibliothequePrix
{
    /// <summary>
    /// Permet d'enregistrer une bibliothèque des prix.
    /// </summary>
    public class BudgetBibliothequePrixSaver : ManagersAccess
    {
        private readonly BibliothequePrixSave.Model model;
        private readonly IUnitOfWork uow;
        private readonly IBudgetBibliothequePrixRepository budgetBibliothequePrixRepository;
        private DateTime now;
        private int utilisateurId;
        private BudgetBibliothequePrixEnt bibliothequePrixEnt;
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="model">Données à enregistrer.</param>
        public BudgetBibliothequePrixSaver(BibliothequePrixSave.Model model, IBudgetBibliothequePrixRepository budgetBibliothequePrixRepository)
        {
            this.model = model;
            uow = ServiceLocator.Current.GetInstance<IUnitOfWork>();
            this.budgetBibliothequePrixRepository = budgetBibliothequePrixRepository;
        }

        /// <summary>
        /// Enregistre une bibliothèque des prix.
        /// </summary>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public BibliothequePrixSave.ResultModel Save()
        {
            var ret = new BibliothequePrixSave.ResultModel();
            now = DateTime.UtcNow;
            ret.DateSave = now;
            utilisateurId = Managers.Utilisateur.GetContextUtilisateurId();

            LoadOrInsertBibliothequePrix();
            ProcessBibliothequePrixItems();
            uow.Save();

            return ret;
        }

        /// <summary>
        /// Charge ou insert la bibliothèque des prix à enregistrer.
        /// </summary>
        private void LoadOrInsertBibliothequePrix()
        {
            // Note : ici on charge tous les champs alors que seul "Items" suffirait
            // Pour le faire, il faudrait pouvoir faire un EnsureAttach, non disponible ici
            bibliothequePrixEnt = budgetBibliothequePrixRepository.GetBibliothequePrixByOrganisationIdAndDeviseId(model.OrganisationId, model.DeviseId);

            if (bibliothequePrixEnt == null)
            {
                bibliothequePrixEnt = new BudgetBibliothequePrixEnt()
                {
                    OrganisationId = model.OrganisationId,
                    DeviseId = model.DeviseId,
                    AuteurCreationId = utilisateurId,
                    DateCreation = now,
                    Items = new List<BudgetBibliothequePrixItemEnt>()
                };
                budgetBibliothequePrixRepository.Insert(bibliothequePrixEnt);
            }
        }

        /// <summary>
        /// Traite les éléments de la bibliothèque des prix.
        /// </summary>
        private void ProcessBibliothequePrixItems()
        {
            foreach (var itemModel in model.Items)
            {
                var itemEnt = bibliothequePrixEnt.Items.FirstOrDefault(i => i.RessourceId == itemModel.RessourceId);
                if (itemEnt == null)
                {
                    itemEnt = new BudgetBibliothequePrixItemEnt
                    {
                        BudgetBibliothequePrixId = bibliothequePrixEnt.BudgetBibliothequePrixId,
                        RessourceId = itemModel.RessourceId,
                        AuteurCreationId = utilisateurId,
                        DateCreation = now,
                        Prix = itemModel.Prix,
                        UniteId = itemModel.UniteId
                    };
                    bibliothequePrixEnt.Items.Add(itemEnt);
                }
                else
                {
                    var itemHisto = new BudgetBibliothequePrixItemValuesHistoEnt
                    {
                        ItemId = itemEnt.BudgetBibliothequePrixItemId,
                        DateInsertion = itemEnt.DateModification ?? itemEnt.DateCreation,
                        Prix = itemEnt.Prix,
                        UniteId = itemEnt.UniteId
                    };
                    itemEnt.ItemValuesHisto.Add(itemHisto);
                    itemEnt.AuteurModificationId = utilisateurId;
                    itemEnt.DateModification = now;
                    itemEnt.Prix = itemModel.Prix;
                    itemEnt.UniteId = itemModel.UniteId;
                }
            }
        }
    }
}
