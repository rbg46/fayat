using System.Collections.Generic;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les référentiels étendus.
    /// </summary>
    public interface IReferentielEtenduRepository : IRepository<ReferentielEtenduEnt>
    {
        /// <summary>
        ///   Retourne le referentielEtendu avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="referentielEtenduId">Identifiant du referentielEtendu à retrouver.</param>
        /// <param name="withRessourceInclude">Si vrai on charge les dépendances ressource et sous-chapitre</param>
        /// <returns>Le referentielEtendu retrouvé, sinon null.</returns>
        ReferentielEtenduEnt GetById(int referentielEtenduId, bool withRessourceInclude = false);

        /// <summary>
        ///   Retourne la liste des referentielEtendus en fonction d'une société.
        /// </summary>
        /// <param name="societeId">Identifiant de la societe.</param>
        /// <returns>Liste des referentielEtendus qui ont une nature.</returns>
        IEnumerable<ReferentielEtenduEnt> GetList(int societeId);

        /// <summary>
        ///   Retourne la liste des referentielEtendus pour une societe spécifique.
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Liste des referentielEtendus.</returns>
        IEnumerable<ReferentielEtenduEnt> GetListBySocieteId(int societeId);

        /// <summary>
        ///   Retourne la liste des Référentiels Etendus pour une société donnée.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="onlyActive">Indique si seules les ressources actives doivent être retrournées</param>
        /// <returns>Liste des referentielEtendus.</returns>
        IEnumerable<ChapitreEnt> GetReferentielEtenduAsChapitreList(int societeId, bool onlyActive = true);

        /// <summary>
        ///   Retourne la liste des Référentiels Etendus pour une société donnée.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Liste des referentielEtendus.</returns>
        List<ChapitreEnt> GetReferentielEtenduAsChapitreListLightBySocieteId(int societeId);

        IReadOnlyList<ReferentielEtenduEnt> Get(List<int> ressourceIds, int societeId);


        /// <summary>
        ///   Retourne la liste des Référentiels Etendus pour une liste de ressources.
        /// </summary>
        /// <param name="ressourceIdList">Liste de ressource id</param>
        /// <returns>Liste des referentielEtendus.</returns>
        List<ChapitreEnt> GetReferentielEtenduAsChapitreListLightByRessourceIdList(List<int> ressourceIdList);

        /// <summary>
        ///   Retourne la liste des Référentiels Etendus pour une société donnée sous la forme d'une liste de chapitres
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="codeTypeRessource">Code type de ressource</param>
        /// <returns>Liste des referentielEtendus.</returns>
        IEnumerable<ChapitreEnt> GetReferentielEtenduAsChapitreList(int societeId, string codeTypeRessource);

        /// <summary>
        /// Retourne la liste des ressources ainsi que le référentiel étendu et le param associé
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="filter">filtre sur le libellé de la ressource</param>
        /// <returns>Liste de ressources</returns>
        IEnumerable<ChapitreEnt> GetChapitresFromReferentielEtendus(int societeId, string filter = "");

        /// <summary>
        /// Recupere La liste des natures pour une societe et un code chapitre
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="chapitreCode">chapitreCode</param>
        /// <returns>Liste de nature</returns>
        IEnumerable<NatureEnt> GetNaturesByChapitreCodeAndSocieteId(int societeId, string chapitreCode);


        /// <summary>
        /// Recupere La liste des natures pour une societe 
        /// </summary>
        /// <param name="societeId">societeId</param>  
        /// <returns>Liste de nature</returns>
        IEnumerable<ChapitreEnt> GetAllChapitresWithNatures(int societeId);

        /// <summary>
        ///   Retourne une ressource ainsi que le référentiel étendu et les params associés
        /// </summary>
        /// <param name="ressourceId">L'identifiant de la ressource</param>
        /// <param name="societeId">L'identifiant de la société</param>
        /// <returns>Une ressource</returns>
        RessourceEnt GetRessourceWithRefEtenduAndParams(int ressourceId, int societeId);

        /// <summary>
        ///   Calcul le différentiel entre le référentiel étendu et son référentiel fixe, et l'ajoute au référentiel étendu
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <param name="listReferentielEtendu">Liste des refertentiel étendu</param>
        /// <returns>retourne la liste des référentiels étendus</returns>
        IEnumerable<ReferentielEtenduEnt> DifferentialReferentielFixeReferentielEtendu(int societeId, List<ReferentielEtenduEnt> listReferentielEtendu);

        /// <summary>
        ///   Ajoute un nouveau referentielEtendu
        /// </summary>
        /// <param name="referentielEtendu">Rôle à ajouter</param>
        /// <returns>Referentiel Etendu crée</returns>
        ReferentielEtenduEnt AddReferentielEtendu(ReferentielEtenduEnt referentielEtendu);

        /// <summary>
        ///   Met à jour un referentielEtendu
        /// </summary>
        /// <param name="referentielEtendu">ReferentielEtendu à mettre à jour</param>
        /// <returns>Référentiel Etendu mis à jour</returns>
        ReferentielEtenduEnt UpdateReferentielEtendu(ReferentielEtenduEnt referentielEtendu);

        /// <summary>
        ///   Supprime un Module
        /// </summary>
        /// <param name="referentielEtenduId">ID du referentielEtendu à supprimé</param>
        void DeleteReferentielEtendu(int referentielEtenduId);

        /// <summary>
        ///   Retourne le paramétrage referentielEtendu avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="paramReferentielEtenduId">Identifiant du parametrage referentielEtendu à retrouver.</param>
        /// <returns>Le referentielEtendu retrouvé, sinon null.</returns>
        ParametrageReferentielEtenduEnt GetParametrageById(int paramReferentielEtenduId);

        /// <summary>
        ///   Retourne un paramétrage en fonction de son organisation, sa devise et son refrentielEtendu
        /// </summary>
        /// <param name="organisationId">ID du de l'organisation</param>
        /// <param name="deviseId">ID du de la devise</param>
        /// <param name="referentielId">ID du référentiel étendu</param>
        /// <returns>Un paramétrage de référentiel etendu</returns>
        ParametrageReferentielEtenduEnt GetParametrageReferentielEtendu(int organisationId, int deviseId, int referentielId);

        /// <summary>
        ///   Renvoi l'affectation passée en paramètre, après suppression de ces dépendances
        /// </summary>
        /// <param name="parametrage">affectation dont on veut détacher les dépendances</param>
        /// <returns>List des paramétrage chargé avec les données des paramétrages des organisations parents</returns>
        ParametrageReferentielEtenduEnt InitParametrageParentList(ParametrageReferentielEtenduEnt parametrage);

        /// <summary>
        /// Retourne le référentiel étendu correspondant aux paramètres
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>le référentiel étendu correspondant aux paramètres</returns>
        ReferentielEtenduEnt GetByRessourceIdAndSocieteId(int ressourceId, int societeId);

        /// <summary>
        ///   Retourne la liste des paramétrage des référentiels etendus pour un eorganisation et une devise
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="deviseId">ID du de la devise</param>
        /// <param name="filter">filtrage des ressources</param>
        /// <returns>liste des paramétrage des référentiels etendus de la société </returns>
        IEnumerable<ParametrageReferentielEtenduEnt> GetParametrageReferentielEtendu(int societeId, int deviseId, string filter = "");

        /// <summary>
        ///   Calcul le différentiel entre le référentiel étendu et son référentiel fixe, et l'ajoute au référentiel étendu
        ///   Le paramètre <paramref name="filter"/> permet de filtrer les ressources
        /// </summary>
        /// <param name="orgaList">Liste des organisations</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <param name="listParam">liste de paramétrage référentiel étendu</param>
        /// <param name="filter">Filtrage des ressources</param>
        /// <returns>retourne la liste des référentiels étendus</returns>
        IEnumerable<ParametrageReferentielEtenduEnt> DifferentialParametrageReferentielFixeReferentielEtendu(List<OrganisationEnt> orgaList, int deviseId, List<ParametrageReferentielEtenduEnt> listParam, string filter);

        /// <summary>
        ///   Calcul le différentiel entre le référentiel étendu et son référentiel fixe, et l'ajoute au référentiel étendu
        ///   Pour filtre les ressources, utilisez la version qui prend un filtre en paramètre
        /// </summary>
        /// <param name="orgaList">Liste des organisations</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <param name="listParam">liste de paramétrage référentiel étendu</param>
        /// <returns>retourne la liste des référentiels étendus</returns>
        IEnumerable<ParametrageReferentielEtenduEnt> DifferentialParametrageReferentielFixeReferentielEtendu(List<OrganisationEnt> orgaList, int deviseId, List<ParametrageReferentielEtenduEnt> listParam);

        /// <summary>
        ///   Transforme une liste de Paramétrage Référentiel Etendu en une liste de Chapitres
        /// </summary>
        /// <param name="paramRefEtenduList">Liste de paramétrage référentiel étendu</param>
        /// <returns>Liste de Chapitre</returns>
        IEnumerable<ChapitreEnt> GetParametrageReferentielEtenduAsChapitreList(IEnumerable<ParametrageReferentielEtenduEnt> paramRefEtenduList);

        /// <summary>
        ///   Récupération du ReferentielEtendu à partir de la ressource et de la societe
        /// </summary>
        /// <param name="idRessource">Identifiant unique de la ressource</param>
        /// <param name="idSociete">Identifiant unique de la societe</param>
        /// <param name="withInclude">Si vrai on charge la ressource et le sous-chapitre dont elle dépend</param>
        /// <returns>Le referentiel etendu</returns>
        ReferentielEtenduEnt GetReferentielEtenduByRessourceAndSociete(int idRessource, int idSociete, bool withInclude = false);

        /// <summary>
        /// Permet de récuperer les réferentiels etendus a partir d'un ci
        /// </summary>
        /// <param name="ciId">Identifiant de la ci</param>
        /// <returns>liste des réferentiels etendus</returns>
        IEnumerable<ReferentielEtenduEnt> GetReferentielsEtendusByCi(int ciId);

        IEnumerable<ReferentielEtenduEnt> GetReferentielsEtendusBySocieteId(int societeId);

        /// <summary>
        /// Permet d'ajouter les ressources spécifiques CI à la liste des réferenitiel etendus
        /// </summary>
        /// <param name="ciId">Identifiant du ci</param>
        /// <param name="listReferentielEtendu">liste des réferenitiel etendus</param>
        void AddRessourcesSpecifiqueInReferentielEtendu(int ciId, ref List<ReferentielEtenduEnt> listReferentielEtendu);

        /// <summary>
        /// Gets the list recommande by societe identifier.
        /// </summary>
        /// <param name="societeId">The societe identifier.</param>
        /// <returns>IEnumerable of ReferentielEtenduEnt</returns>
        IEnumerable<ReferentielEtenduEnt> GetListRecommandeBySocieteId(int societeId);

        /// <summary>
        /// Récupére une ressource par code et code du groupe.
        /// </summary>
        /// <param name="code">Code de la ressource.</param>
        /// <param name="groupeCode">Le code du groupe.</param>
        /// <returns>La ressource créée.</returns>
        List<RessourceEnt> GetRessourceByCodeAndGroupeCode(string code, string groupeCode);
    }
}
