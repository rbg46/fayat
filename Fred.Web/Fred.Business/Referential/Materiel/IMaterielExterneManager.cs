using Fred.Entities.Commande;
using Fred.Entities.Referential;

namespace Fred.Business.Referential.Materiel
{
    /// <summary>
    ///   Le gestionnaire des matériels externes
    /// </summary>
    public interface IMaterielExterneManager : IManager<MaterielEnt>
    {
        /// <summary>
        ///   Ajout d'un matériel externe
        /// </summary>
        /// <param name="commandeLigne">Ligne de commande</param>
        /// <param name="commande">commande</param>
        /// <param name="auteurCreationId">identifiant unique de l'auteur de création du matériel externe</param>
        /// <returns>materiel</returns>
        MaterielEnt AddMaterielExterne(CommandeLigneEnt commandeLigne, CommandeEnt commande, int auteurCreationId);

        /// <summary>
        ///   Désactive le matériel externe
        /// </summary>
        /// <param name="materielId">identifiant unique du materielExterne</param>
        /// <param name="statut">statut du matériel externe</param>
        /// <param name="auteurModificationId">identifiant unique de l'auteur de la modification</param>
        void ChangeStatutMaterielExterne(int materielId, bool statut, int? auteurModificationId);
    }
}
