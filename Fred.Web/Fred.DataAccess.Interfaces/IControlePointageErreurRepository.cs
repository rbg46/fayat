
using System.Collections.Generic;
using Fred.Entities.ValidationPointage;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Interface IControlePointageErreurRepository
    /// </summary>
    public interface IControlePointageErreurRepository : IRepository<ControlePointageErreurEnt>
    {
        /// <summary>
        ///   Ajoute un ControlePointageErreurEnt en base
        /// </summary>
        /// <param name="cpErreur">ControlePointageErreur à ajouter</param>
        /// <returns>ControlePointageErreur ajouté</returns>
        ControlePointageErreurEnt AddControlePointageErreur(ControlePointageErreurEnt cpErreur);

        /// <summary>
        ///   Récupère l'ensemble des erreurs d'un contrôle pointage
        /// </summary>
        /// <param name="controlePointageId">Identifiant du contrôle pointage</param>
        /// <param name="searchText">Texte à rechercher</param>
        /// <returns>Liste des erreurs</returns>
        IEnumerable<ControlePointageErreurEnt> GetControlePointageErreurList(int controlePointageId, string searchText);
    }
}
