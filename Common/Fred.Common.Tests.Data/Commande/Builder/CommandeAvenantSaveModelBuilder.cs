using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeAvenantSaveModelBuilder : ModelDataTestBuilder<CommandeAvenantSave.Model>
    {
        public CommandeAvenantSaveAbonnementModelBuilder AbonnementBuilder => new CommandeAvenantSaveAbonnementModelBuilder();

        public CommandeAvenantSaveFournisseurModelBuilder FournisseurBuilder => new CommandeAvenantSaveFournisseurModelBuilder();

        public CommandeAvenantSaveLigneModelBuilder SaveLigneBuilder => new CommandeAvenantSaveLigneModelBuilder();


        public override CommandeAvenantSave.Model New()
        {
            Model = new CommandeAvenantSave.Model();
            Model.CreatedLignes = new List<CommandeAvenantSave.LigneModel>();
            Model.UpdatedLignes = new List<CommandeAvenantSave.LigneModel>();
            Model.DeletedLigneIds = new List<int>();
            return Model;
        }

        /// <summary>
        /// A ne pas modifier !
        /// </summary>
        /// <returns></returns>
        public CommandeAvenantSaveModelBuilder ParDefaut()
        {
            Model.CommandeId = 1;
            Model.CommentaireFournisseur = "commentaire1";
            Model.CommentaireInterne = "commentaire1";
            Model.DelaiLivraison = "delai1";
            Model.Abonnement = AbonnementBuilder.ParDefaut().Build();
            Model.Fournisseur = FournisseurBuilder.ParDefaut().Build();
            return this;
        }

        public CommandeAvenantSaveModelBuilder ParDefautWithCreatedLignes()
        {
            Model = ParDefaut().Build();
            return AddCreatedLignes(SaveLigneBuilder.Libelle("CreatedLignes").Build());
        }

        public CommandeAvenantSaveModelBuilder ParDefautWithUpdatedLignes()
        {
            Model = ParDefaut().Build();
            return AddUpdatedLignes(SaveLigneBuilder.Libelle("UpdatedLignes").ParDefaut().Build());
        }

        public CommandeAvenantSaveModelBuilder ParDefautWithDeletedLignes()
        {
            Model = ParDefaut().Build();
            return AddDeletedLigneIds(1);
        }

        public CommandeAvenantSaveModelBuilder ParDefautWithCreatedAndUpdatedLignes()
        {
            Model = ParDefaut().Build();
            AddCreatedLignes(SaveLigneBuilder.Libelle("CreatedLignes").Build());
            AddUpdatedLignes(SaveLigneBuilder.Libelle("UpdatedLignes").ParDefaut().Build());
            return this;
        }

        public CommandeAvenantSaveModelBuilder ParDefautWithCreatedAndDeletedLignes()
        {
            Model = ParDefaut().Build();
            AddCreatedLignes(SaveLigneBuilder.Libelle("CreatedLignes").Build());
            AddDeletedLigneIds(1);
            return this;
        }

        public CommandeAvenantSaveModelBuilder ParDefautWithdUpdatedAndDeletedLignes()
        {
            Model = ParDefaut().Build();
            AddUpdatedLignes(SaveLigneBuilder.Libelle("UpdatedLignes").ParDefaut().Build());
            AddDeletedLigneIds(1);
            return this;
        }

        public CommandeAvenantSaveModelBuilder ParDefautWithCreatedUpdatedAndDeletedLignes()
        {
            Model = ParDefaut().Build();
            AddCreatedLignes(SaveLigneBuilder.Libelle("CreatedLignes").Build());
            AddUpdatedLignes(SaveLigneBuilder.Libelle("UpdatedLignes").ParDefaut().Build());
            AddDeletedLigneIds(1);
            return this;
        }

        public CommandeAvenantSaveModelBuilder AddLigne(CommandeAvenantSave.AbonnementModel ligne)
        {
            Model.Abonnement = ligne;
            return this;
        }

        public CommandeAvenantSaveModelBuilder CommandeId(int id)
        {
            Model.CommandeId = id;
            return this;
        }

        public CommandeAvenantSaveModelBuilder CommentaireFournisseur(string commentaire)
        {
            Model.CommentaireFournisseur = commentaire;
            return this;
        }

        public CommandeAvenantSaveModelBuilder CommentaireInterne(string commentaire)
        {
            Model.CommentaireInterne = commentaire;
            return this;
        }

        public CommandeAvenantSaveModelBuilder Abonnement(CommandeAvenantSave.AbonnementModel abonnement)
        {
            Model.Abonnement = abonnement;
            return this;
        }

        public CommandeAvenantSaveModelBuilder DelaiLivraison(string delaiLivraison)
        {
            Model.DelaiLivraison = delaiLivraison;
            return this;
        }

        public CommandeAvenantSaveModelBuilder Fournisseur(CommandeAvenantSave.FournisseurModel fournisseur)
        {
            Model.Fournisseur = fournisseur;
            return this;
        }

        public CommandeAvenantSaveModelBuilder AddCreatedLignes(CommandeAvenantSave.LigneModel createdLigne)
        {
            Model.CreatedLignes.Add(createdLigne);
            return this;
        }

        public CommandeAvenantSaveModelBuilder AddUpdatedLignes(CommandeAvenantSave.LigneModel updatedLigne)
        {
            Model.UpdatedLignes.Add(updatedLigne);
            return this;
        }

        public CommandeAvenantSaveModelBuilder AddDeletedLigneIds(int deletedLigneId)
        {
            Model.DeletedLigneIds.Add(deletedLigneId);
            return this;
        }

    }
}
