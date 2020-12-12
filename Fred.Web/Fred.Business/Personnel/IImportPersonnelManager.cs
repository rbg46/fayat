using System.Collections.Generic;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Personnel
{
    /// <summary>
    /// Manager pour les import personnel de FES et FON
    /// </summary>
    public interface IImportPersonnelManager
    {

        /// <summary>
        ///   Retourne la société portant le code société paye indiqué.
        /// </summary>
        /// <param name="codeSocietePaye">Code de la société de paye dont l'identifiant est à retrouver.</param>
        /// <returns>Societe retrouvé, sinon nulle. Souleve une exception si plusieurs societe porte le meme codeSocietePaye</returns>
        SocieteEnt GetSocieteByCodeSocietePaye(string codeSocietePaye);

        /// <summary>
        /// Recupere les etablissements de paye pour plusieurs societes
        /// </summary>
        /// <param name="societeIds">id des societes</param>
        /// <returns>Liste EtablissementPaieEnt</returns>
        IEnumerable<EtablissementPaieEnt> GetEtablissementPaiesBySocieteIds(List<int> societeIds);

        IEnumerable<EtablissementComptableEnt> GetEtablissementComptablesBySocieteIds(List<int> societeIds);

        /// <summary>
        /// Recupere la liste des ressources epour un groupe
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <returns>Liste des ressources pour un groupe</returns>
        IEnumerable<RessourceEnt> GetRessourceListByGroupeId(int groupId);

        /// <summary>
        ///  Recupere les personnels  pour plusieurs societes
        /// </summary>
        /// <param name="societeIds">societeIds</param>
        /// <returns>les personnels  pour plusieurs societes</returns>
        IEnumerable<PersonnelEnt> GetPersonnelListBySocieteIds(List<int> societeIds);

        /// <summary>
        /// Recupere la liste des societes
        /// </summary>
        /// <returns> liste des societes</returns>
        IEnumerable<SocieteEnt> GetSocieteList();

        /// <summary>
        /// Recupere les ci de fred.
        /// Le filtre sur la societe sera fait plutard, Il ne faut pas que l'unicite se fait sur le code et la societe.
        /// </summary>
        /// <param name="ciCodes">Liste des Code des CI a recuperes</param>
        /// <returns>Liste de ci</returns>
        IEnumerable<CIEnt> GetCIsByCodes(IEnumerable<string> ciCodes);

        /// <summary>
        /// Obtient toutes les affectations par default
        /// </summary>
        /// <returns>return toutes les affectations par default </returns>
        IEnumerable<AffectationEnt> GetDefaultAffectations();

        /// <summary>
        /// Recupere l'utilisateur FredIE
        /// </summary>
        /// <returns>fredIe</returns>
        UtilisateurEnt GetFredIe();
    }
}
