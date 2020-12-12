using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.GroupSpecific.Infrastructure;
using Fred.Web.Models.CI;

namespace Fred.Business.CI
{
    /// <summary>
    ///   Gestionnaire des cis.
    /// </summary>
    public interface ICIManager : IManager<CIEnt>, IGroupAwareService
    {
        #region Gestion des CI

        /// <summary>
        /// Recherche un ci selon les filtres definis
        /// </summary>
        /// <param name="filters">les filtres</param>
        /// <param name="orderBy">les orderby</param>
        /// <param name="includeProperties">les includes</param>
        /// <param name="page">la page corrante</param>
        /// <param name="pageSize">la taille de la page</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Liste de ci</returns>
        List<CIEnt> Search(List<Expression<Func<CIEnt, bool>>> filters,
                                      Func<IQueryable<CIEnt>, IOrderedQueryable<CIEnt>> orderBy = null,
                                      List<Expression<Func<CIEnt, object>>> includeProperties = null,
                                      int? page = null,
                                      int? pageSize = null,
                                      bool asNoTracking = true);

        /// <summary>
        ///   Retourne la liste des cis.
        /// </summary>
        /// <param name="onlyChantierFred">Indique si seuls les chantiers gérés par FRED sont retournés</param>
        /// <param name="groupeId">L'identifiant du groupe concerné ou null pour tous les groupes.</param>
        /// <returns>Renvoie la liste des cis.</returns>
        IEnumerable<CIEnt> GetCIList(bool onlyChantierFred = false, int? groupeId = null);

        /// <summary>
        ///   Retourne le ci par rapport à son identifiant unique 
        /// </summary>
        /// <param name="id">Identifiant Unique</param>
        /// <param name="withSocieteInclude">Indique si on inclut la société</param>
        /// <returns>Renvoie le ci.</returns>
        CIEnt GetCiById(int id, bool withSocieteInclude = false);

        /// <summary>
        ///   Retourne la liste CI du profil paie selon l'organisation choisie
        /// </summary>
        /// <param name="organisationId">OrganisationId choisie</param>
        /// <returns>Renvoie la liste des CI choisie par profil paie.</returns>
        IEnumerable<int> GetAllCIbyOrganisation(int organisationId);

        /// <summary>
        ///   Retourne la liste CI en fonction d'une organisation
        /// </summary>
        /// <param name="organisationId">OrganisationId choisie</param>
        /// <param name="period">Période</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Renvoie la liste des CI en fonction de l'organisation</returns>
        IEnumerable<CIEnt> GetCIList(int organisationId, DateTime? period, int page = 1, int pageSize = 10);

        /// <summary>
        ///   Retourne la liste CI en fonction d'une organisation
        /// </summary>
        /// <param name="organisationId">OrganisationId choisie</param>    
        /// <returns>Renvoie la liste des CI en fonction de l'organisation</returns>
        IEnumerable<CIEnt> GetCIList(int organisationId);

        /// <summary>
        ///   Retourne le ci portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="ciId">Identifiant de l'ci à retrouver.</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="byPassCheckAccess">ByPass la vérification d'accès à l'entité</param>
        /// <param name="overrideSocietyByOrganisationParent">Remplace la société du CI par celle de l'arboresence des organisations</param>
        /// <returns>L'ci retrouvée, sinon nulle.</returns>
        CIEnt GetCIById(int ciId, int userId = 0, bool byPassCheckAccess = false, bool overrideSocietyByOrganisationParent = false);

        /// <summary>
        ///   Retourne le ci portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="id">Identifiant du CI à retrouver.</param>    
        /// <returns>CI retrouvé, sinon null.</returns>
        CIEnt GetCI(int id);

        /// <summary>
        /// Retourne le CI portant le code Ci et le Code société.
        /// </summary>
        /// <param name="code">Le code du CI.</param>
        /// <param name="codeSocieteCompta">le code de la société comptable</param>
        /// <returns>Un CI.</returns>
        CIEnt GetCI(string code, string codeSocieteCompta);

        /// <summary>
        ///   Ajout une nouvelle ci.
        /// </summary>
        /// <param name="ciEnt">CI à ajouter.</param>
        /// <returns>CI ajouté.</returns>
        CIEnt AddCI(CIEnt ciEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'une ci.
        /// </summary>
        /// <param name="ciEnt">CI à modifier</param>
        /// <returns>CI mis à jour</returns>
        CIEnt UpdateCI(CIEnt ciEnt);

        /// <summary>
        ///   Supprime une ci.
        /// </summary>
        /// <param name="id">L'identifiant de l'ci à supprimer.</param>
        void DeleteCIById(int id);

        /// <summary>
        /// recuperer la liste de CI afféctés à un personnel
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">page size</param>
        /// <param name="personnelId">personnel id</param>
        /// <param name="societeId">societe id</param>
        /// <returns>retourne liste des CI d'un personnel donné</returns>
        Task<IEnumerable<CIEnt>> SearchLightByPersonnelId(string text, int page, int pageSize, int personnelId, int? societeId = null);

        /// <summary>
        ///   Permet de récupérer les champs de recherche lié au personnel
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche du personnel</returns>
        SearchCIEnt GetFilter();

        /// <summary>
        ///   Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte de recherche</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>        
        /// <param name="personnelSocieteId">Identifiant de la société du Personnel</param>
        /// <returns>Retourne une liste des référentiels</returns>
        Task<IEnumerable<CIEnt>> SearchLightAsync(string text, int page, int pageSize, int? personnelSocieteId = null);

        /// <summary>
        /// Méthode de recherche des CIs liés à une société
        /// </summary>
        /// <param name="text">Le texte de recherche</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>        
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="includeSep">inclusion des SEP</param>
        /// <returns>La liste des CIs</returns>
        IEnumerable<CIEnt> GetCisOfUserFilteredBySocieteId(string text, int page, int pageSize, int societeId, bool includeSep = true);

        /// <summary>
        ///   Méthode de recherche des CI qui sont eligibles au affectation d'un moyen en fonction des rôles de l'utilisateur
        /// </summary>
        /// <param name="text">Le texte de recherche</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Retourne une liste des référentiels</returns>
        IEnumerable<CIEnt> CiSearchLightForAffectationMoyen(string text, int page, int pageSize);

        /// <summary>
        ///   Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte de recherche</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="interimaireId">Identifiant personnel de l'intérimaire</param>
        /// <param name="date">Date du pointage</param>
        /// <returns>Retourne une liste des référentiels</returns>
        IEnumerable<CIEnt> SearchLightForInterimaire(string text, int page, int pageSize, int interimaireId, DateTime date);

        /// <summary>
        /// Retourne la liste de CI filtrée en fonction de critères liés au budget
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">page size</param>
        /// <param name="periodeApplication">période de mise en application du budget</param>
        /// <returns>retourne liste des CI filtrés par période de mise en application</returns>
        IEnumerable<CIEnt> SearchLightForBudget(string text, int page, int pageSize, int? periodeApplication);

        /// <summary>
        /// Retourne l'organisationId pour un ci
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <returns>L organisationId</returns>
        int? GetOrganisationIdByCiId(int ciId);

        /// <summary>
        ///   Vérifie si un CI est clôturé pour une période donnée
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="period">Période</param>
        /// <returns>Vrai si le CI est clôturé, sinon faux</returns>
        bool IsCIClosed(int ciId, DateTime period);

        /// <summary>
        /// Retourne l'établissement parent du CI en paramètre
        /// </summary>
        /// <param name="ciId">Identifiant</param>
        /// <returns>L'établissement comptable parent au CI</returns>
        EtablissementComptableEnt GetEtablissementComptableByCIId(int ciId);

        /// <summary>
        /// Permet de récupérer la liste des types de CI.
        /// </summary>
        /// <returns>Liste des types de CI</returns>
        IEnumerable<CITypeEnt> GetCITypes();

        /// <summary>
        /// Retourne les cis appartenant à un établissement comptable pour picklist
        /// </summary>
        /// <param name="organisationId">identifiant unique de l'organisation de l'établissemet comptable</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">taille de la page</param>
        /// <returns>Liste de Ci appartenant à une société</returns>
        IEnumerable<OrganisationEnt> SearchLightOrganisationCiByOrganisationPereId(int organisationId, int page, int pageSize);

        /// <summary>
        /// Get Ci by organisation id
        /// </summary>
        /// <param name="organisationId">Organisation id</param>
        /// <returns>CiEnt avec l'identifiant organisation id</returns>
        CIEnt GetCiByOrganisationId(int organisationId);

        /// <summary>
        /// Get Ci by identifiant unique de société
        /// </summary>
        /// <param name="societeId">identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        List<CIEnt> GetCIListBySocieteId(int societeId);

        /// <summary>
        /// Get liste CiId by identifiant unique de société
        /// </summary>
        /// <param name="societeId">identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        List<int> GetCiIdListBySocieteId(int societeId);

        // <summary>
        /// Get liste ciid par identifiant unique de société pour traitement des exports reception intérimaire
        /// </summary>
        /// <param name="societeId">identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        List<int> GetCiIdListBySocieteIdForInterimaire(int societeId);

        /// <summary>
        /// Renvoie la liste des Ci par liste des codes
        /// </summary>
        /// <param name="codeList">Liste des codes des Cis à renvoyer</param>
        /// <returns>Liste des Cis qui corresponds aux codes demandés</returns>
        IEnumerable<CIEnt> GetCiByCodeList(IEnumerable<string> codeList);

        /// <summary>
        /// SearchLight CI pour Compte Interne SEP
        /// </summary>
        /// <param name="text">Texte à rechercher</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="currentCiId">CI actuel sur lequel on va associer un Compte Interne SEP</param>
        /// <returns>Liste de CI</returns>
        List<CIEnt> SearchLightCompteInterneSep(string text, int page, int pageSize, int currentCiId);

        /// <summary>
        /// Permet d'obtenir la liste des cis generique absence
        /// </summary>
        /// <returns>liste de CIEnt</returns>
        List<CIEnt> GetCisAbsenceGenerique();

        #endregion

        #region Gestion des Devises

        /// <summary>
        ///   Retourne la liste des relations CI/Devise
        /// </summary>
        /// <returns>Liste des CI/Devise</returns>
        /// <param name="ciId">identifiant de la relation CI/Devise</param>
        IEnumerable<CIDeviseEnt> GetCIDevise(int ciId);

        /// <summary>
        ///   Retourne la devise de référence d'un CI.
        /// </summary>
        /// <returns>Devise de référence d'un CI</returns>
        /// <param name="ciId">identifiant du ci à mettre a jour</param>
        DeviseEnt GetDeviseRef(int ciId);

        /// <summary>
        ///   Récupère la liste des devises secondaire à partir d'un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne une enumeration de devise</returns>
        IEnumerable<DeviseEnt> GetCIDeviseSecList(int ciId);

        /// <summary>
        ///   Evalue si le Ci possèdent plusieurs Devises
        /// </summary>
        /// <param name="idCI"> Identifiant du CI </param>
        /// <returns> Vrai si le Ci possède plusieurs Devises, faux sinon </returns>
        bool IsCiHaveManyDevises(int idCI);

        /// <summary>
        ///   Création / Mise à jour / Suppression des relations CIDevise
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ciDeviseList">liste des relation Ci Devise</param>    
        /// <returns>Liste des CIDevise mise à jour</returns>
        IEnumerable<CIDeviseEnt> ManageCIDevise(int ciId, IEnumerable<CIDeviseEnt> ciDeviseList);

        /// <summary>
        ///   Mise à jour d'un CIDevise 
        /// </summary>
        /// <param name="ciDeviseEnt">CIDEvise à mettre à jour</param>
        /// <returns>CIDevise mis à jour</returns>
        CIDeviseEnt UpdateCIDevise(CIDeviseEnt ciDeviseEnt);

        #endregion

        #region Gestion des Sociétés

        /// <summary>
        ///   Récupère la société d'un CI en fonction de son identifiant.
        /// </summary>
        /// <param name="ciId">Identifiant du ci</param>
        /// <param name="withIncludeTypeSociete">Indique si on veux charger le type de la société</param>
        /// <returns>La société</returns>
        SocieteEnt GetSocieteByCIId(int ciId, bool withIncludeTypeSociete = false);

        #endregion

        #region Gestion des Ressources 



        /// <summary>
        ///   Ajoute ou modifie une relation CI/Ressource
        /// </summary>
        /// <param name="ciRessourceList">Liste de CIRessource</param>
        /// <returns>Liste de CIRessource modifié et ajouté</returns>
        IEnumerable<CIRessourceEnt> ManageCIRessource(IEnumerable<CIRessourceEnt> ciRessourceList);
        #endregion


        #region Gestion des dates

        /// <summary>
        /// Retourne la date d'ouverture du CI donné. Si aucun CI n'existe pour l'id passé alors la fonction retourne null
        /// </summary>
        /// <param name="ciId">l'id du ci dont on veut la date</param>
        /// <returns>un DateTime nullable contenant la date d'ouverture ou null</returns>
        DateTime? GetDateOuvertureCi(int ciId);

        #endregion

        /// <summary>
        /// Retourne les ci dont le code est contenu dans la liste ciCodes.
        /// ATTENTION: cette methode est a utliser avec precausion.
        /// En effet, le Code ci n'est pas unique, il est unique pour une societe donnée !!!
        /// </summary>
        /// <param name="ciCodes">liste de code ci</param>
        /// <returns>Les ci qui ont le code contenu dans ciCodes </returns>
        List<CIEnt> GetCisByCodes(List<string> ciCodes);

        /// <summary>
        /// Retourne une liste de ci en fonction d'une liste ce ciid
        /// </summary>
        /// <param name="ciIds">Liste de ciid</param>
        /// <returns>Liste de ci</returns>
        List<CIEnt> GetCisByIds(List<int> ciIds);

        IReadOnlyList<int> GetCiIdsBySocieteIds(List<int> societeIds);

        /// <summary>
        /// Retourne une liste de ci en fonction d'une liste ce ciid
        /// </summary>
        /// <param name="ciIds">Liste de ciid</param>
        /// <returns>Liste de ci</returns>
        List<CIEnt> GetCisByIdsLight(List<int> ciIds);

        /// <summary>
        /// Retourne un ci qui n'est pas tracké
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <returns> un ci qui n'est pas tracké</returns>
        CIEnt GetCiForCompare(int ciId);

        /// <summary>
        ///   Ajoute ou met à jour des CI selon la liste en paramètre
        /// </summary>
        /// <param name="cis">Liste des CI</param>    
        /// <param name="updateOrganisation">Mets aussi a jour l'organisation quand on met a jour le ci</param>   
        /// <returns>Les CI ajoutés</returns>
        List<CIEnt> AddOrUpdateCIList(List<CIEnt> cis, bool updateOrganisation = false);


        /// <summary>
        ///   Insertion de masse des CIDevise
        /// </summary>
        /// <param name="ciDeviseList">Liste de CIDevise</param>
        void BulkAddCIDevise(IEnumerable<CIDeviseEnt> ciDeviseList);

        /// <summary>
        ///   Récupère le ci de passage d'un ci sep ainsi que sa société
        /// </summary>
        /// <param name="ciId">Identifiant unique du ci sep </param>
        /// <returns>Ci de passage avec sa societe inclus</returns>
        CIEnt GetCICompteInterneByCiid(int ciId);

        /// <summary>
        /// Récupére un ci pour l'alimentation Figgo
        /// </summary>
        /// <param name="societeId">societeID</param>
        /// <param name="etablissementComptableId">EtablissementComtableId</param>
        /// <returns>entite Ci</returns>
        CIEnt GetCIBySocieteIdAndEtablissementIdForFiggo(int societeId, int etablissementComptableId);

        /// <summary>
        /// Get ci for affectation by ci id
        /// </summary>
        /// <param name="ciId">Ci Identifier</param>
        /// <returns>Ci ent</returns>
        CIEnt GetCiForAffectationByCiId(int ciId);

        /// <summary>
        /// Récupère le ci par code et l'identifiant de la société 
        /// </summary>
        /// <param name="code">Code CI</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Ci de passage avec sa societe inclus</returns>
        CIEnt GetCIByCodeAndSocieteId(string code, int societeId);

        Task<IEnumerable<CIModel>> SearchLightModelAsync(string text, int page, int pageSize, int? personnelSocieteId = null);
    }
}
