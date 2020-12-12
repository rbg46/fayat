using Fred.Entities.Referential;
using System.Collections.Generic;

namespace Fred.Business.Referential.Service
{
    /// <summary>
    /// Interface Import Fournisseur Manager
    /// </summary>
    public interface IFournisseursImportService : IService
    {
        /// <summary>
        /// Ajouter ou mettre à jour un fournisseur et les agences associées
        /// </summary>
        /// <param name="fournisseurFromSap">fournisseur à importer</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        void AddOrUpdateFournisseurs(FournisseurEnt fournisseurFromSap, int userId);

        /// <summary>
        /// Mise à jour de l'agence
        /// </summary>
        /// <param name="agence">Agence</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        void UpdateAgence(AgenceEnt agence, int userId);
    }
}
