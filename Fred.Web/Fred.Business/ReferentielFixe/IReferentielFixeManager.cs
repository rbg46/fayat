using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.Commande.Models;
using Fred.Entities.Carburant;
using Fred.Entities.CI;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Web.Shared.Models.RessourceRecommandee;

namespace Fred.Business.ReferentielFixe
{
    /// <summary>
    /// Interface du gestionnaire du référentiel fixe
    /// </summary>
    public interface IReferentielFixeManager : IManager
    {
        #region Chapitre

        /// <summary>
        /// Retourne le chapitre avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="chapitreId">Identifiant du chapitre à retrouver.</param>
        /// <returns>Le chapitre retrouvé, sinon null.</returns>
        ChapitreEnt GetChapitreById(int chapitreId);

        /// <summary>
        /// Retourne la liste des chapitres.
        /// </summary>
        /// <returns>Liste des chapitres.</returns>
        IEnumerable<ChapitreEnt> GetChapitreList();

        /// <summary>
        /// Obtient la liste des chapitres en fonction du groupeId de l'utilisateur connecté
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur connecté</param>
        /// <returns>Retourne la liste des chapitres</returns>
        IEnumerable<ChapitreEnt> GetChapitreListByUtilisateurId(int userId);

        /// <summary>
        /// Obtient la collection des chapitres (suppprimés inclus)
        /// </summary>
        /// <returns>La collection des chapitres</returns>
        IEnumerable<ChapitreEnt> GetAllChapitreList();

        /// <summary>
        /// Obtient la collection des chapitres appartenant à un groupe spécifié
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>La collection des chapitres</returns>
        IEnumerable<ChapitreEnt> GetChapitreListByGroupeId(int groupeId);

        /// <summary>
        /// Retourne le type de ressource en fonction de son code
        /// </summary>
        /// <param name="code">Code de la ressource</param>
        /// <returns>Le type de ressource</returns>
        TypeRessourceEnt GetTypeRessourceByCode(string code);

        /// <summary>
        /// Ajoute un nouveau chapitre
        /// </summary>
        /// <param name="chapitreEnt">Rôle à ajouter</param>
        /// <returns> L'identifiant du chapitre ajouté</returns>
        ChapitreEnt AddChapitre(ChapitreEnt chapitreEnt);

        /// <summary>
        /// Met à rout un chapitre
        /// </summary>
        /// <param name="chapitreEnt">Rôle à mettre à jour</param>
        /// <returns>Chapitre mis à jour</returns>
        ChapitreEnt UpdateChapitre(ChapitreEnt chapitreEnt);

        /// <summary>
        /// Supprime un ChapitreModule
        /// </summary>
        /// <param name="chapitreId">ID du chapitre à dissocier du module</param>
        void DeleteChapitreById(int chapitreId);

        /// <summary>
        /// Indique si le code existe déjà pour les chapitres d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        bool IsCodeChapitreExist(string code, int groupeId);

        /// <summary>
        /// Cherche une liste de Chapitre.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des Chapitres.</param>
        /// <returns>Une liste de Chapitre.</returns>
        IEnumerable<ChapitreEnt> SearchChapitres(string text);

        /// <summary>
        /// Méthode de recherche de chapitre(s)
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des Chapitres.</param>
        /// <param name="groupeId">L'identifiant du groupe</param>
        /// <returns>Une liste d' items de référentiel</returns>
        IEnumerable<ChapitreEnt> SearchChapitres(string text, int groupeId);

        /// <summary>
        /// Get Fes Chapitre List Moyen
        /// </summary>
        /// <returns>Une liste d' items de référentiel</returns>
        IEnumerable<int> GetFesChapitreListMoyen();

        #endregion

        #region Sous-Chapitre

        /// <summary>
        /// Retourne le sousChapitre avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="sousChapitreID">Identifiant du sousChapitre à retrouver.</param>
        /// <returns>Le sousChapitre retrouvé, sinon null.</returns>
        SousChapitreEnt GetSousChapitreById(int sousChapitreID);

        /// <summary>
        /// Retourne la liste des sousChapitres.
        /// </summary>
        /// <returns>Liste des sousChapitres.</returns>
        IEnumerable<SousChapitreEnt> GetSousChapitreList();

        /// <summary>
        /// Obtient la collection des sous-chapitres (suppprimés inclus)
        /// </summary>
        /// <returns>La collection des sous-chapitres</returns>
        IEnumerable<SousChapitreEnt> GetAllSousChapitreList();

        /// <summary>
        /// Obtient la collection des sous-chapitres appartenant à un chapitre spécifié
        /// </summary>
        /// <param name="chapitreId">Identifiant du chapitre.</param>
        /// <returns>La collection des sous-chapitres</returns>
        IEnumerable<SousChapitreEnt> GetSousChapitreListByChapitreId(int chapitreId);

        /// <summary>
        /// Ajoute un nouveau sousChapitre
        /// </summary>
        /// <param name="sousChapitreEnt">Rôle à ajouter</param>
        /// <returns> L'identifiant du sousChapitre ajouté</returns>
        SousChapitreEnt AddSousChapitre(SousChapitreEnt sousChapitreEnt);

        /// <summary>
        /// Met à rout un sousChapitre
        /// </summary>
        /// <param name="sousChapitreEnt">Rôle à mettre à jour</param>
        /// <returns>Sous chapitre mis à jour</returns>
        SousChapitreEnt UpdateSousChapitre(SousChapitreEnt sousChapitreEnt);

        /// <summary>
        /// Supprime un SousChapitreModule
        /// </summary>
        /// <param name="sousChapitreId">ID du sousChapitre à dissocier du module</param>
        void DeleteSousChapitreById(int sousChapitreId);

        /// <summary>
        /// Indique si le code existe déjà pour les sous-chapitres d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        bool IsCodeSousChapitreExist(string code, int groupeId);

        /// <summary>
        /// Cherche une liste de SousChapitre.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des SousChapitres.</param>
        /// <returns>Une liste de SousChapitre.</returns>
        IEnumerable<SousChapitreEnt> SearchSousChapitres(string text);

        /// <summary>
        /// Cherche une liste de SousChapitre.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des SousChapitres.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Une liste de SousChapitre.</returns>
        IEnumerable<SousChapitreEnt> SearchSousChapitres(string text, int groupeId);

        #endregion

        #region Ressource

        /// <summary>
        /// Retourne le ressource avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="ressourceID">Identifiant du ressource à retrouver.</param>
        /// <returns>Le ressource retrouvé, sinon null.</returns>
        RessourceEnt GetRessourceById(int ressourceID);

        /// <summary>
        /// Retourne le ressource avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="ressourceID">Identifiant du ressource à retrouver.</param>
        /// <returns>Le ressource retrouvé, sinon null.</returns>
        RessourceEnt FindById(int ressourceID);

        /// <summary>
        /// Retourne le ressource en fonction de son code
        /// </summary>
        /// <param name="code">Code de la ressource à retrouver.</param>
        /// <returns>Le ressource retrouvé, sinon null.</returns>
        RessourceEnt GetRessource(string code);

        /// <summary>
        /// Retourne les ressources en fonction d'un code et d'un groupe.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="groupId">Identifiant du groupe.</param>
        /// <returns>Les ressources concernées.</returns>
        IEnumerable<RessourceEnt> GetRessources(string code, int groupId);

        /// <summary>
        /// Retourne la liste des ressources.
        /// </summary>
        /// <returns>Liste des ressources.</returns>
        IEnumerable<RessourceEnt> GetRessourceList();

        /// <summary>
        /// Recupére une liste de RessourceEnt pour une liste de code ressource
        /// </summary>
        /// <param name="ressourceCodes">Liste de code ressource</param>
        /// <returns>Une liste en lecture seul de <see cref="RessourceEnt"/></returns>
        IReadOnlyList<RessourceEnt> GetRessourceList(List<string> ressourceCodes);

        /// <summary>
        /// Recupére une liste de RessourceEnt pour une liste de code ressource et un code de société
        /// </summary>
        /// <param name="ressourceCodes">Liste de code ressource</param>
        /// <param name="societeIds">Liste d'indentifiant de société</param>
        /// <returns>Une liste de <see cref="RessourceEnt"/></returns>
        IEnumerable<RessourceEnt> GetRessourceList(List<string> ressourceCodes, List<int> societeIds);

        /// <summary>
        /// Obtient la collection des ressources (suppprimées inclus)
        /// </summary>
        /// <returns>La collection des ressources</returns>
        IEnumerable<RessourceEnt> GetAllRessourceList();

        /// <summary>
        /// Obtient la collection des ressources appartenant à un sous-chapitre spécifié
        /// </summary>
        /// <param name="sousChapitreId">Identifiant du sous-chapitre.</param>
        /// <returns>La collection des ressources</returns>
        IEnumerable<RessourceEnt> GetRessourceListBySousChapitreId(int sousChapitreId);

        /// <summary>
        /// Obtient la collection des types de ressources
        /// </summary>
        /// <returns>La collection des types de ressource</returns>
        IEnumerable<CarburantEnt> GetCarburantList();

        /// <summary>
        /// Renvoi la liste des ressources
        /// </summary>
        /// <param name="groupId">Identifiant du groupe</param>
        /// <returns>retourne la liste des ressources appartenant à un groupe</returns>
        IEnumerable<RessourceEnt> GetRessourceListByGroupeId(int groupId);

        /// <summary>
        /// Ajoute une nouvelle ressource
        /// </summary>
        /// <param name="ressourceEnt">Rôle à ajouter</param>
        /// <returns> L'identifiant du ressource ajouté</returns>
        RessourceEnt AddRessource(RessourceEnt ressourceEnt);

        /// <summary>
        /// Met à rout un ressource
        /// </summary>
        /// <param name="ressourceEnt">Rôle à mettre à jour</param>
        /// <returns>La ressource mise à jour</returns>
        RessourceEnt UpdateRessource(RessourceEnt ressourceEnt);

        /// <summary>
        /// Supprime un RessourceModule
        /// </summary>
        /// <param name="ressourceId">ID du ressource à dissocier du module</param>
        void DeleteRessourceById(int ressourceId);

        /// <summary>
        /// Indique si le code existe déjà pour les ressources d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        bool IsCodeRessourceExist(string code, int groupeId);

        /// <summary>
        /// Renvoi le dernier code incrémenté de un pour un sous-chapitre spécifique
        /// </summary>
        /// <param name="sousChapitre">Sous-chapitre dont on recherche le prochain code.</param>
        /// <returns>Le prochain code disponible</returns>
        string GetNextRessourceCode(SousChapitreEnt sousChapitre);

        /// <summary>
        /// Cherche une liste de Ressource.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des Ressources.</param>
        /// <returns>Une liste de Ressource.</returns>
        IEnumerable<RessourceEnt> SearchRessources(string text);

        /// <summary>
        /// Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <param name="societeId">L'identifiant de la société</param>
        /// <param name="page">La page courante</param>
        /// <param name="pageSize">La taille de la page</param>
        /// <param name="resourceTypeId">Identifiant du type de ressource</param>
        /// <param name="ressourceId">L'identifiant de la ressource. Si ce paramètre est renseigné, cela veut dire qu'on veut récupérer les ressources qui ont la même nature</param>
        /// <param name="achats">Indique si on veut uniquement les ressources disponible dans le module achat</param>
        /// <returns>Une liste d' items de référentiel</returns>
        IEnumerable<RessourceEnt> SearchLight(string text, int societeId, int page, int pageSize, int? resourceTypeId, int? ressourceId, bool? achats = false);

        /// <summary>
        /// Rechercher dans le référentiel selon les filtres passés en paramètres
        /// </summary>
        /// <param name="filter">Le filtre</param>
        /// <returns>Une liste d'items de référentiel</returns>
        IEnumerable<RessourceEnt> SearchLight(SearchRessourcesAchatModel filter);

        /// <summary>
        /// SearchLight des ressources natures en fonction du référentiel étendu (FRED_SOCIETE_RESSOURCE_NATURE)
        /// </summary>
        /// <param name="text">Texte à rechercher</param>
        /// <param name="ci">CI</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="ressourceRecommandeesOnly">Identifiant Type de ressource</param>
        /// <param name="ressourceIdNatureFilter">identifiant de la nature</param>
        /// <returns>Liste de ressources avec le flag de recommandation</returns>
        List<RessourceEnt> SearchRessourcesRecommandees(string text, CIEnt ci, int page, int pageSize, bool ressourceRecommandeesOnly, int? ressourceIdNatureFilter);

        /// <summary>
        /// Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <param name="societeId">L'identifiant de la société</param>
        /// <param name="page">La page courante</param>
        /// <param name="pageSize">La taille de la page</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <returns>Une liste d' items de référentiel</returns>
        IEnumerable<RessourceEnt> SearchLightByNature(string text, int societeId, int page, int pageSize, int? ressourceId);

        /// <summary>
        /// Retourne la liste de toutes les ressources d'un CI et d'une société,
        /// avec leur sous-chapitre et chapitre respectif
        /// </summary>
        /// <param name="societeId">L'identifiant de la société concernée</param>
        /// <param name="ciId">L'identifiant du CI concerné</param>
        /// <returns>Liste de Ressource</returns>
        List<RessourceEnt> GetListRessourceBySocieteIdWithSousChapitreEtChapitre(int societeId, int ciId);

        /// <summary>
        /// Récupère la liste des ressources recommandées correspondant aux référentiels étendus
        /// </summary>
        /// <param name="etablissementCIOrganisationId">Identifiant de l'organisation à laquelle l'établissement comptable du CI courant appartient</param>
        /// <returns>Une liste de ressources recommandées</returns>
        List<RessourceRecommandeeFromEtablissementCIOrganisationModel> GetRessourceRecommandeeList(int etablissementCIOrganisationId);

        /// <summary>
        /// Vérifie que le code de la ressource n'est pas déjà utilisé
        /// </summary>
        /// <param name="code">Code de la ressource à comparer</param>
        /// <returns>Vrai si le code de la ressource existe</returns>
        bool IsRessourceExistByCode(string code);

        /// <summary>
        /// GetRessourcesByChapitres
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="societeId">societeId</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="chapitreCodes">chapitreCodes</param>
        /// <returns> Liste de ReferentielEtenduEnt</returns>
        IEnumerable<ReferentielEtenduEnt> GetRessourcesByChapitres(string text, int societeId, int page, int pageSize, List<string> chapitreCodes);

        /// <summary>
        /// Retourne la liste des identifiants des ressources possédant la même nature que la ressource en entrée
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des identifiants des ressources possédant la même nature que la ressource source</returns>
        List<int> GetListRessourceIdByRessourceWithSameNatureInRefEtendu(int ressourceId, int societeId);

        Task<List<RessourceEnt>> SearchRessourcesForAchatAsync(SearchRessourcesAchatModel searchFilters);

        #endregion

        /// <summary>
        /// Méthode de recherche d'un type de ressource
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <returns>Une liste d' items de référentiel</returns> 
        Task<IEnumerable<TypeRessourceEnt>> SearchRessourceTypesByCodeOrLabelAsync(string text);
    }
}
