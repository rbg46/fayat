using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Organisation;

namespace Fred.Business.Organisation
{
    /// <summary>
    ///   Interface des gestionnaires des organisations
    /// </summary>
    public interface IOrganisationManager
    {
        /// <summary>
        ///   Retourne la organisation portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="organisationId">Identifiant de la organisation à retrouver.</param>
        /// <returns>La organisation retrouvée, sinon nulle.</returns>
        OrganisationEnt GetOrganisationById(int organisationId);


        /// <summary>
        /// Retourne l'organisation correspondante aux paramètres
        /// </summary>
        /// <param name="codeOrganisation">Code de l'organisation</param>
        /// <param name="codeTypeOrganisation">Type de l'organisation</param>
        /// <returns>Organisation</returns>
        OrganisationEnt GetOrganisationByCodeAndType(string codeOrganisation, string codeTypeOrganisation);

        /// <summary>
        ///   Retourne les seuils de comande définit pour une organisation
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation à retrouver.</param>
        /// <param name="roleId">Identifiant du rôle à retrouver.</param>
        /// <returns>liste des seuils de l'organisation choisi, sinon nulle.</returns>
        IEnumerable<AffectationSeuilOrgaEnt> GetSeuilByOrganisationId(int organisationId, int? roleId);

        /// <summary>
        ///   Sauvegarde les modifications d'une organisation.
        /// </summary>
        /// <param name="organisationEnt">organisation à modifier</param>
        /// <param name="mereId">Organisation mère</param>
        /// <returns>OrganisationEnt</returns>
        OrganisationEnt UpdateOrganisation(OrganisationEnt organisationEnt, int? mereId);

        /// <summary>
        ///   Retourne la liste des organisations.
        /// </summary>
        /// <returns>Renvoie la liste des organisations.</returns>
        IEnumerable<OrganisationEnt> GetList();

        /// <summary>
        ///   Renvoi la liste des organisations d'un Utilisateur
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <param name="types">Types d'organisation</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="organisationIdPere">Identifiant de l'organisation parente</param>
        /// <returns>Une liste d'organisations</returns>
        IEnumerable<OrganisationLightEnt> GetOrganisationsAvailable(string text = null, List<int> types = null, int? utilisateurId = null, int? organisationIdPere = null);

        /// <summary>
        ///   Retourne la liste des Types d'organisation.
        /// </summary>
        /// <returns>Liste des types d'organisation.</returns>
        IEnumerable<TypeOrganisationEnt> GetOrganisationTypesList();

        /// <summary>
        ///   Mise à jour d'une surcharge de devise
        /// </summary>
        /// <param name="threshold">devise</param>
        /// <returns>Association mise à jour</returns>
        AffectationSeuilOrgaEnt UpdateThresholdOrganisation(AffectationSeuilOrgaEnt threshold);

        /// <summary>
        ///   Supprimer une surcharge de devise
        /// </summary>
        /// <param name="thresholdOrganisationId">Identifiant de la surcharge à supprimer</param>
        void DeleteThresholdOrganisationById(int thresholdOrganisationId);


        /// <summary>
        ///   Retourne une liste d'organisations parentes d'une organisation fille
        /// </summary>
        /// <param name="organisationEnfantId">Organisation fille</param>
        /// <param name="orgaTypeIdToStop">Identifiant du type d'organisation où on stop la recherche de parent</param>
        /// <returns>Une liste d'organisations</returns>
        List<OrganisationEnt> GetOrganisationParentByOrganisationId(int organisationEnfantId, int? orgaTypeIdToStop = null);

        /// <summary>
        ///   Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Le texte de recherche</param>
        /// <returns>Retourne une liste des référentiels</returns>
        IEnumerable<OrganisationLightEnt> SearchLight(int page, int pageSize, string text);

        /// <summary>
        ///   Ajout d'une surcharge de devise
        /// </summary>
        /// <param name="threshold">RoleOrganisationDevise</param>
        /// <returns>RoleOrganisationDevise crée</returns>
        AffectationSeuilOrgaEnt AddOrganisationThreshold(AffectationSeuilOrgaEnt threshold);

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
        ///   Récupère la liste des organisations disponibles pour un utilisateur en fonction des types d'organisations choisis
        /// </summary>
        /// <param name="page">Numéro page</param>
        /// <param name="pageSize">Taille page</param>
        /// <param name="text">Texte recherché</param>    
        /// <param name="typeOrgaList">Liste des types d'organisation</param>
        /// <param name="bypassUser">par utilisateur ou non: Si on ignore l'utilisateur, on récupère toutes les organisations diposnibles. 
        /// Sinon, on récupère les orga dont il est habilité (Panel Habilitation dans Detail Personnel)</param>   
        /// <param name="onlyCiNoClose"> seulement avec des ci non cloturé </param>
        /// <returns>Liste Light des organisations</returns>
        IEnumerable<OrganisationLightEnt> SearchLightOrganisation(int page, int pageSize, string text, List<string> typeOrgaList, bool bypassUser = false, bool onlyCiNoClose = true);

        /// <summary>
        /// Retourne l'arbo a partir de pole 
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="poleOrganisationId">OrganisationI du pole</param>
        /// <returns>Liste a plat d'organisations</returns>
        IEnumerable<OrganisationLightEnt> GetOrganisationsForPole(int page, int pageSize, int poleOrganisationId);

        /// <summary>
        ///   Retourne l'identifiant du type d'organisation portant le code indiqué.
        /// </summary>
        /// <param name="codeTypeOrganisation">Code du type d'organisation.</param>
        /// <returns>l'id du type d'organisation retrouvé</returns>
        int GetTypeOrganisationIdByCode(string codeTypeOrganisation);

        /// <summary>
        /// Recherche les orgas a partir d'une societeId
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="recherche">recherche</param>
        /// <param name="typeOrganisation">typeOrganisation</param>
        /// <param name="societeId">societeId</param>
        /// <returns>etouner une liste  d'organisation</returns>
        IEnumerable<OrganisationLightEnt> SearchLightForSocieteId(int page, int pageSize, string recherche, List<string> typeOrganisation, int? societeId);

        /// <summary>
        /// Récuperer l'ensemble des societes dont l'utilisateur est habilité ou habilité sur leurs etab comptables
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="pageSize">page size</param>
        /// <param name="text">text</param>
        /// <returns>List des organisation (societe)</returns>
        Task<IEnumerable<OrganisationLightEnt>> SearchLightSocieteOrganisationForEtabComptable(int page, int pageSize, string text);
    }
}
