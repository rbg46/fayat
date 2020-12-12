
using System.Collections.Generic;
using Fred.Entities.Delegation;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données des délégations
    /// </summary>
    public interface IDelegationRepository : IRepository<DelegationEnt>
    {
        /// <summary>
        /// Permet de récupérer une liste de délégations lié à un personnel de référence
        /// </summary>
        /// <param name="id">Personnel de référence.</param>
        /// <returns>Les délégations pour un personnel de référence.</returns>
        List<DelegationEnt> GetDelegationByPersonnelId(int id);

        /// <summary>
        /// Permet de récupérer une délégations en fonction de son identifiant
        /// </summary>
        /// <param name="id">identifiant unique de la délégation</param>
        /// <returns>La délégation</returns>
        DelegationEnt GetDelegationById(int id);

        /// <summary>
        /// Permet de récupérer une liste de délégation qui s'active aujourd'hui
        /// </summary>
        /// <returns>La délégation</returns>
        List<DelegationEnt> GetDelegationStartNow();

        /// <summary>
        /// Permet de récupérer une liste de délégation qui se désactive aujourd'hui et qui n'est pas déjà désactivée
        /// </summary>
        /// <returns>La délégation</returns>
        List<DelegationEnt> GetDelegationStopNow();

        /// <summary>
        /// Permet de récupérer si une délégation est active ou non dans une période donnée à un personnel délégué
        /// </summary>
        /// <param name="delegationEnt">identifiant unique de la délégation</param>
        /// <returns>0 si aucune délégation active dans la période sinon 1</returns>
        int GetDelegationAlreadyActive(DelegationEnt delegationEnt);

        /// <summary>
        /// Permet d'ajouter une délégation
        /// </summary>
        /// <param name="delegationEnt">Délégation</param>
        /// <returns>La délégation enregistrée</returns>
        DelegationEnt AddDelegation(DelegationEnt delegationEnt);

        /// <summary>
        /// Permet de modifier une délégation
        /// </summary>
        /// <param name="delegationEnt">Délégation</param>
        /// <returns>La délégation modifiée</returns>
        DelegationEnt UpdateDelegation(DelegationEnt delegationEnt);

        /// <summary>
        /// Permet d'activer et de désactiver une délégation si le jour de  début et de fin est aujourd'hui
        /// </summary>
        void ActivateAndDesactivateDelegation();

        /// <summary>
        /// Permet de désactiver une délégation
        /// </summary>
        /// <param name="delegationEnt">Délégation</param>
        /// <returns>La délégation modifiée</returns>
        DelegationEnt DesactivateDelegation(DelegationEnt delegationEnt);

        /// <summary>
        /// Permet de supprimer une délégation en fonction de son identifiant
        /// </summary>
        /// <param name="delegationEnt">délégation</param>
        void DeleteDelegationById(DelegationEnt delegationEnt);
    }
}
