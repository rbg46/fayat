using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities;
using Fred.Entities.Organisation;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les organisation.
    /// </summary>
    public interface IOrganisationRepository : IRepository<OrganisationEnt>
    {
        /// <summary>
        ///   Retourne la liste des organisations.
        /// </summary>
        /// <returns>La liste des organisations.</returns>
        IEnumerable<OrganisationEnt> GetList();

        /// <summary>
        ///   Retourne le organisation dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="organisationId">Identifiant du organisation à retrouver.</param>
        /// <returns>Le organisation retrouvé, sinon nulle.</returns>
        OrganisationEnt GetOrganisationById(int organisationId);

        /// <summary>
        ///   Retourne les seuils de comande définit pour une organisation
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation à retrouver.</param>
        /// <param name="roleId">Identifiant du role de l'organisation à retrouver.</param>
        /// <returns>liste des seuils de l'organisation choisi, sinon nulle.</returns>
        IEnumerable<AffectationSeuilOrgaEnt> GetSeuilByOrganisationId(int organisationId, int? roleId);

        /// <summary>
        ///   Retourne une nouvelle organisation
        /// </summary>
        /// <param name="codeOrganisation"> code du type d'organisation</param>
        /// <param name="pereId"> identifiant de l'organisation du parent</param>
        /// <returns> L'organisation générée</returns>
        OrganisationEnt GenerateOrganisation(string codeOrganisation, int? pereId);

        /// <summary>
        ///   Retourne une nouvelle organisation
        /// </summary>
        /// <param name="codeOrganisation"> code du type d'organisation</param>
        /// <param name="pere">L'organisation du parent</param>
        /// <returns> L'organisation générée</returns>
        OrganisationEnt GenerateOrganisation(string codeOrganisation, OrganisationEnt pere);

        /// <summary>
        ///   Sauvegarde les modifications d'une organisation.
        /// </summary>
        /// <param name="organisationEnt">l'organisation à modifier</param>
        /// <param name="pereId">identifiant du père à modifier</param>
        /// <returns>OrganisationEnt</returns>
        OrganisationEnt UpdateOrganisation(OrganisationEnt organisationEnt, int? pereId);

        /// <summary>
        ///   Supprime un organisation
        /// </summary>
        /// <param name="id">L'identifiant du organisation à supprimer</param>
        void DeleteOrganisationById(int id);

        /// <summary>
        ///   Retourne la liste des organisations.
        /// </summary>
        /// <returns>La liste des organisations.</returns>
        IEnumerable<TypeOrganisationEnt> GetOrganisationTypesList();

        /// <summary>
        ///   Retourne l'identifiant du type d'organisation portant le code indiqué.
        /// </summary>
        /// <param name="codeTypeOrganisation">Code du type d'organisation.</param>
        /// <returns>l'id du type d'organisation retrouvé</returns>
        [Obsolete("Prefer to use " + nameof(GetTypeOrganisationIdByCodeAsync) + " instead")]
        int GetTypeOrganisationIdByCode(string codeTypeOrganisation);

        /// <summary>
        ///   Retourne l'identifiant du type d'organisation portant le code indiqué de manière async
        /// </summary>
        /// <param name="codeTypeOrganisation">Code du type d'organisation.</param>
        /// <returns>l'id du type d'organisation retrouvé</returns>
        Task<int> GetTypeOrganisationIdByCodeAsync(string codeTypeOrganisation);

        /// <summary>
        ///   Mise à jour d'une surcharge de devise
        /// </summary>
        /// <param name="threshold">devise à mettre à jour</param>
        /// <returns>Association mise à jour</returns>
        AffectationSeuilOrgaEnt UpdateThresholdOrganisation(AffectationSeuilOrgaEnt threshold);

        /// <summary>
        ///   Supprimer une surcharge de devise
        /// </summary>
        /// <param name="thresholdOrganisationId">Identifiant de la surcharge à supprimer</param>
        void DeleteThresholdOrganisationById(int thresholdOrganisationId);

        /// <summary>
        ///   Récupère la liste des organisations parentes
        /// </summary>
        /// <param name="organisationEnfantId">Identifiant de l'organisation fille</param>
        /// <param name="orgaTypeIdToStop">Identifiant du type d'organisation parent où l'on doit stopper la recherche</param>
        /// <param name="organisationParentListe">Liste des entité OrganisationEnt représentant les parents</param>
        /// <returns>Liste d'organisation représentant toutes les organisations parents.</returns>
        List<OrganisationEnt> GetOrganisationParentList(int organisationEnfantId, int? orgaTypeIdToStop = null, List<OrganisationEnt> organisationParentListe = null);

        /// <summary>
        ///   Ajout une nouvelle association à une organisation
        /// </summary>
        /// <param name="threshold"> Association à ajouter </param>
        /// <returns>L'assiociation ajoutée</returns>
        AffectationSeuilOrgaEnt AddOrganisationThreshold(AffectationSeuilOrgaEnt threshold);

        /// <summary>
        /// Recupere la liste des organisation parentes avec l'organisation passé en parametre.
        /// La liste retournée commence par l'organisation dont on passe l'id en parametre et continue en remontant l'arbre.
        /// </summary>
        /// <param name="organisationEnfantId">organisationEnfantId</param>
        /// <returns>Liste d'organisation</returns>
        List<OrganisationEnt> GetOrganisationParentsWithCurrent(int organisationEnfantId);

        /// <summary>
        ///   Renvoi la liste des organisations d'un Utilisateur
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <param name="types">Types d'organisation</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="organisationIdPere">Identifiant de l'organisation parente</param>
        /// <returns>Une liste d'organisations</returns>
        [Obsolete("Utiliser le OrganisationTreeService ou OrganisationTreeRepository à la place")]
        IEnumerable<OrganisationLightEnt> GetOrganisationsAvailable(string text = null, List<int> types = null, int? utilisateurId = null, int? organisationIdPere = null);

        /// <summary>
        /// Retourne l'identifiant de l'organisation parente demandée.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation concernée.</param>
        /// <param name="organisationParentType">Le type d'organisation parente.</param>
        /// <returns>L'identifiant de l'organisation parente.</returns>
        int? GetParentId(int organisationId, OrganisationType organisationParentType);

        /// <summary>
        /// Retourne l'identifiant d'une société en fonction de l'identifiant de l'organisation d'un de ses enfants.
        /// </summary>
        /// <param name="childOrganisationId">L'identifiant de l'organisation d'un enfant de la société.</param>
        /// <returns>L'identifiant de la société ou null si non trouvé.</returns>
        int? GetSocieteId(int childOrganisationId);
    }
}
