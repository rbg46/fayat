using System;
using Fred.Business.CI;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.Referential;

namespace Fred.Business.Referential.Materiel
{
    /// <summary>
    /// Le gestionnaire des matériels externes
    /// </summary>
    public class MaterielExterneManager : Manager<MaterielEnt, IMaterielRepository>, IMaterielExterneManager
    {
        private readonly ICIManager ciManager;
        private readonly IFournisseurManager fournisseurManager;

        public MaterielExterneManager(IUnitOfWork uow, IMaterielRepository materielRepository, ICIManager ciManager, IFournisseurManager fournisseurManager)
          : base(uow, materielRepository)
        {
            this.ciManager = ciManager;
            this.fournisseurManager = fournisseurManager;
        }

        /// <summary>
        /// Ajout d'un matériel externe
        /// </summary>
        /// <param name="commandeLigne">Ligne de commande</param>
        /// <param name="commande">commande</param>
        /// <param name="auteurCreationId">identifiant unique de l'auteur de création du matériel externe</param>
        /// <returns>materiel</returns>
        public MaterielEnt AddMaterielExterne(CommandeLigneEnt commandeLigne, CommandeEnt commande, int auteurCreationId)
        {
            var numeroCommande = string.IsNullOrEmpty(commande.NumeroCommandeExterne) ? commande.Numero : commande.NumeroCommandeExterne;
            var fournisseurCode = fournisseurManager.GetFournisseur((int)commande.FournisseurId, null).Code;

            MaterielEnt materielExterne = new MaterielEnt()
            {
                SocieteId = (int)ciManager.GetCiById((int)commande.CiId).SocieteId,
                RessourceId = (int)commandeLigne.RessourceId,
                Code = $"{fournisseurCode} - {numeroCommande} - {commandeLigne.NumeroLigne}",
                Libelle = commandeLigne.Libelle,
                Actif = true,
                AuteurCreationId = auteurCreationId,
                DateCreation = DateTime.UtcNow,
                MaterielLocation = true,
                FournisseurId = commande.FournisseurId
            };
            Repository.AddMateriel(materielExterne);
            return materielExterne;
        }

        /// <summary>
        /// Désactive le matériel externe
        /// </summary>
        /// <param name="materielId">identifiant unique du materielExterne</param>
        /// <param name="statut">statut du matériel externe</param>
        /// <param name="auteurModificationId">identifiant unique de l'auteur de la modification</param>
        public void ChangeStatutMaterielExterne(int materielId, bool statut, int? auteurModificationId)
        {
            MaterielEnt materielExterne = Repository.GetMaterielById(materielId);

            materielExterne.Actif = statut;
            materielExterne.AuteurModificationId = auteurModificationId;
            materielExterne.DateModification = DateTime.UtcNow;
            materielExterne.Fournisseur = null;
            materielExterne.Ressource = null;
            Repository.UpdateMateriel(materielExterne);
        }
    }
}
