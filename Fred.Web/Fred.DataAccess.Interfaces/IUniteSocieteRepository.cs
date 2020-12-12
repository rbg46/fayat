
using System.Collections.Generic;
using Fred.Entities.Societe;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les associations entre devises et sociétés.
    /// </summary>
    public interface IUniteSocieteRepository : IRepository<UniteSocieteEnt>
    {
        /// <summary>
        /// Enregistre l'entité en base
        /// </summary>
        /// <param name="uniteSocieteEnt">Entité à enregistré</param>
        void AddSocieteUnite(UniteSocieteEnt uniteSocieteEnt);

        /// <summary>
        /// Mets à jour l'entité en base
        /// </summary>
        /// <param name="uniteSocieteEnt">Entité à mettre à jour</param>
        void UpdateSocieteUnite(UniteSocieteEnt uniteSocieteEnt);

        /// <summary>
        /// Supprime l'entité de la base
        /// </summary>
        /// <param name="uniteSocieteId">Identifiant de l'entité</param>
        void DeleteSocieteUnite(int uniteSocieteId);

        /// <summary>
        /// Retourne la liste de société/unité
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste de société/unité</returns>
        List<UniteSocieteEnt> GetListSocieteUnite(int societeId);
    }
}