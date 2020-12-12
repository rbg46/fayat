using System.Collections.Generic;
using Fred.Entities.FonctionnaliteDesactive;

namespace Fred.Business.FonctionnaliteDesactive
{
    /// <summary>
    ///   Interface du gestionnaire des FonctionnaliteDesactive.
    /// </summary>
    public interface IFonctionnaliteDesactiveManager : IManager<FonctionnaliteDesactiveEnt>
    {
        /// <summary>
        /// Retourne une liste de FonctionnaliteDesactiveEnt.
        /// Un Fonctionnalite est desactive des lors qu'il y a un 'entree'(ligne) dans la base.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Une liste de FonctionnaliteDesactiveEnt</returns>
        IEnumerable<FonctionnaliteDesactiveEnt> GetInactifFonctionnalitesForSocieteId(int societeId);

        /// <summary>
        /// Retourne une liste d'id de Fonctionnalite qui sont desactive sur au moins une societe
        /// </summary>
        /// <returns>liste d'id de Fonctionnalite</returns>
        IEnumerable<int> GetIdsOfFonctionnalitesPartiallyDisabled();


        /// <summary>
        /// Retourne la listes des societes inactives pour un fonctionnalite donnée.
        /// </summary>
        /// <param name="fonctionnaliteId">fonctionnaliteId</param>
        /// <returns>Liste d'organisationIDs des societes désactivées.</returns>
        IEnumerable<int> GetInactivesSocietesForFonctionnaliteId(int fonctionnaliteId);


        /// <summary>
        /// Desactive une fonctionnalité pour une liste de societes et un fonctionnalité donné.
        /// </summary>
        /// <param name="fonctionnaliteId">Id de la fonctionnalité </param>
        /// <param name="organisationIdsOfSocietesToDisable"> liste d'organisationId de societes a désactiver</param>
        /// <returns>Liste de societeId désactivé</returns> 
        IEnumerable<int> DisableFonctionnaliteByOrganisationIdsOfSocietesAndFonctionnaliteId(int fonctionnaliteId, List<int> organisationIdsOfSocietesToDisable);

        /// <summary>
        /// Active une fonctionnalité pour une liste d' organisationId de societes et une fonctionnalité donnée.
        /// </summary>
        /// <param name="fonctionnaliteId">Id de la fonctionnalité </param>
        /// <param name="organisationIdsOfSocietesToEnable"> liste d'organisationId de societes a activer</param>
        /// <returns>Liste de societeId activés</returns> 
        IEnumerable<int> EnableFonctionnaliteByOrganisationIdsOfSocietesAndFonctionnaliteId(int fonctionnaliteId, List<int> organisationIdsOfSocietesToEnable);

        /// <summary>
        /// Permet de verifer si une fonctionnalite est desactive pour une societe.
        /// </summary>
        /// <param name="fonctionnaliteId">L'id de la fonctionnalite à verifer</param>
        /// <param name="societeId">L'id de la societe</param>
        /// <returns>Boolean indique si la fonctionnalite est desactivee ou non</returns>
        bool IsFonctionnaliteDesactiveForSociete(int fonctionnaliteId, int societeId);

        /// <summary>
        /// Permet de verifer si une fonctionnalite est desactive pour une societe.
        /// </summary>
        /// <param name="fonctionnaliteCode">Le code de la fonctionnalite à verifer</param>
        /// <param name="societeId">L'id de la societe</param>
        /// <returns>Boolean indique si la fonctionnalite est desactivee ou non</returns>
        bool IsFonctionnaliteDesactiveForSociete(string fonctionnaliteCode, int societeId);
    }

}
