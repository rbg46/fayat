using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using System;
using System.Collections.Generic;

namespace Fred.Business.ReferentielEtendu
{
    /// <summary>
    /// Interface des gestionnaires des Référentiels Etendus
    /// </summary>
    public interface IReferentielEtenduManager : IManager<ReferentielEtenduEnt>
    {
        /// <summary>
        /// Retourne le referentielEtendu avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="referentielEtenduId">Identifiant du referentielEtendu à retrouver.</param>
        /// <returns>Le referentielEtendu retrouvé, sinon null.</returns>
        ReferentielEtenduEnt GetById(int referentielEtenduId);

        /// <summary>
        /// Retourne la liste des referentielEtendus en fonction d'une société.
        /// </summary>
        /// <param name="societeId">Identifiant de la societe.</param>
        /// <returns>Liste des referentielEtendus qui ont une nature.</returns>
        IEnumerable<ReferentielEtenduEnt> GetList(int societeId);

        /// <summary>
        /// Retourne la liste des referentielEtendus pour une societe spécifique.
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Liste des referentielEtendus.</returns>
        IEnumerable<ReferentielEtenduEnt> GetListBySocieteId(int societeId);

        /// <summary>
        /// Retourne la liste des Référentiels Etendus pour une société donnée.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="onlyActive">Indique si seules les ressources actives doivent être retrournées</param>
        /// <returns>Liste des referentielEtendus.</returns>
        IEnumerable<ChapitreEnt> GetReferentielEtenduAsChapitreList(int societeId, bool onlyActive = true);

        /// <summary>
        /// Retourne la liste des Référentiels Etendus pour une société donnée.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Liste des referentielEtendus.</returns>
        List<ChapitreEnt> GetReferentielEtenduAsChapitreListLightBySocieteId(int societeId);

        /// <summary>
        /// Retourne la liste des Référentiels Etendus pour une société donnée.
        /// </summary>
        /// <param name="ressourceIdList">Liste de ressource ids</param>
        /// <returns>Liste des referentielEtendus.</returns>
        List<ChapitreEnt> GetReferentielEtenduAsChapitreListLightByRessourceIdList(List<int> ressourceIdList);

        /// <summary>
        /// Retourne la liste des Référentiels Etendus pour une société donnée sous la forme d'une liste de chapitres
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="codeTypeRessource">Code type de ressource</param>
        /// <returns>Liste des referentielEtendus.</returns>
        IEnumerable<ChapitreEnt> GetReferentielEtenduAsChapitreList(int societeId, string codeTypeRessource);

        /// <summary>
        /// Retourne la liste des chapitres/sous-chapîtres listant les ressources ainsi que le référentiel étendu et le param associé pour une société
        /// </summary>   
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="filter">filtre sur le libellé de la ressource</param>
        /// <returns>Liste des chapitres</returns>
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
        IEnumerable<ChapitreEnt> GetAllNatures(int societeId);

        /// <summary>
        /// Retourne une ressource ainsi que le référentiel étendu et les params associés
        /// </summary>
        /// <param name="ressourceId">L'identifiant de la ressource</param>
        /// <param name="societeId">L'identifiant de la société</param>
        /// <returns>Une ressource</returns>
        RessourceEnt GetRessourceWithRefEtenduAndParams(int ressourceId, int societeId);

        /// <summary>
        /// Permet de créer un détail de ressource.
        /// </summary>
        /// <param name="code">Code de la ressource.</param>
        /// <param name="groupeCode">Le code du groupe.</param>
        /// <returns>La ressource créée.</returns>
        List<RessourceEnt> GetRessourceByCodeAndGroupeCode(string code, string groupeCode);

        /// <summary>
        /// Ajoute ou met à jour un nouveau referentielEtendu
        /// </summary>
        /// <param name="referentielEtendu">Rôle à ajouter</param>
        /// <returns>Référentiel Etendu mis à jour</returns>
        ReferentielEtenduEnt Update(ReferentielEtenduEnt referentielEtendu);

        /// <summary>
        /// Ajoute ou Met à jour une liste de référentiel étendu
        /// </summary>
        /// <param name="refEtenduList">Référentiel Etendu</param>
        /// <returns>Liste des référentiels étendu</returns>
        IEnumerable<ReferentielEtenduEnt> ManageReferentielEtenduList(IEnumerable<ReferentielEtenduEnt> refEtenduList);

        /// <summary>
        /// Supprime un Module
        /// </summary>
        /// <param name="referentielEtenduId">ID du referentielEtendu à supprimé</param>
        void DeleteById(int referentielEtenduId);

        /// <summary>
        /// Retourne la liste des paramétrage des référentiels etendus pour un eorganisation et une devise classé en chapitre
        /// </summary>
        /// <param name="organisationId">ID du de l'organisation</param>
        /// <param name="deviseId">ID du de la devise</param>
        /// <param name="filter">filtre des ressources</param>
        /// <returns>liste des paramétrage des référentiels etendus</returns>
        Tuple<IEnumerable<OrganisationEnt>, IEnumerable<ChapitreEnt>> GetParametrageReferentielEtendu(int organisationId, int deviseId, string filter = "");

        /// <summary>
        /// Retourne la liste des paramétrage des référentiels etendus pour un eorganisation et une devise
        /// </summary>
        /// <param name="organisationId">organisationId</param>
        /// <param name="deviseId">ID du de la devise</param>
        /// <param name="filter">filtre des ressources</param>
        /// <returns>liste des paramétrage des référentiels etendus</returns>
        IEnumerable<ParametrageReferentielEtenduEnt> GetParametrageReferentielEtenduMergedFlatList(int organisationId, int deviseId, string filter = "");

        /// <summary>
        /// Permet de récupérer la liste des organisations parents
        /// </summary>
        /// <param name="organisationId">Id de l'organisation courante</param>
        /// <returns>Liste d'organisations</returns>
        List<OrganisationEnt> GetOrganisationParentList(int organisationId);

        /// <summary>
        /// Retourne la liste des paramétrage des référentiels etendus pour un organisation et une devise et un référentiel
        /// étendu
        /// </summary>
        /// <param name="organisationId">ID du de l'organisation</param>
        /// <param name="deviseId">ID du de la devise</param>
        /// <param name="referentielId">ID du de la référentiel étendu</param>
        /// <returns>Un paramétrage de référentiel etendu de la société </returns>
        ParametrageReferentielEtenduEnt GetParametrageReferentielEtendu(int organisationId, int deviseId, int referentielId);

        /// <summary>
        /// Ajoute ou met à jour un nouveau referentiel Etendu
        /// </summary>
        /// <param name="parametrageReferentielEtendu"> Paramétrage à ajouter ou mettre à jour </param>
        /// <returns> Identifiant du referentielEtendu ajouté ou mis à jour </returns>
        ParametrageReferentielEtenduEnt AddOrUpdateParametrageReferentielEtendu(ParametrageReferentielEtenduEnt parametrageReferentielEtendu);

        /// <summary>
        /// Récupération du ReferentielEtendu à partir de la ressource et de la societe
        /// </summary>
        /// <param name="idRessource">Identifiant unique de la ressource</param>
        /// <param name="idSociete">Identifiant unique de la societe</param>
        /// <param name="withInclude">Si vrai on charge la ressource et le sous-chapitre dont elle dépend</param>
        /// <returns>Le referentiel etendu</returns>
        ReferentielEtenduEnt GetReferentielEtenduByRessourceAndSociete(int idRessource, int idSociete, bool withInclude = false);

        /// <summary>
        /// Retourne la liste des referentielEtendus pour une societe spécifique.
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Liste des referentielEtendus.</returns>
        IEnumerable<ChapitreEnt> GetAllReferentielEtenduAsChapitreList(int societeId);

        /// <summary>
        /// Création d'une liste d'entité pour l'export excel. 
        /// </summary>
        /// <param name="orgaId">Identifiant unique de l'organisation</param>
        /// <param name="deviseId">Identifiant unique de la devise</param>
        /// <param name="filter">filtre</param>
        /// <returns>une liste de  ParametrageReferentielEtenduExportEnt</returns>
        List<ParametrageReferentielEtenduExportEnt> GenerateListForExportExcel(int orgaId, int deviseId, string filter);

        /// <summary>
        /// Retourne le référentiel étendu correspondant aux paramètres
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>le référentiel étendu correspondant aux paramètres</returns>
        ReferentielEtenduEnt GetByRessourceIdAndSocieteId(int ressourceId, int societeId);

        /// <summary>
        /// Gets all referentiel etendu recommande as chapitre list.
        /// </summary>
        /// <param name="societeId">The societe identifier.</param>
        /// <returns>IEnumerable of ChapitreEnt</returns>
        IEnumerable<ChapitreEnt> GetAllReferentielEtenduRecommandeAsChapitreList(int societeId);

        /// <summary>
        /// Récupère la liste des ressources sous forme de liste de chapitres (Chapitre -> Sous Chapitre -> Ressource)
        /// 
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="societeId">Identifiant de la société</param>    
        /// <returns>Liste de chapitres</returns>
        IEnumerable<ChapitreEnt> GetAllCIRessourceListAsChapitreList(int ciId, int societeId);

        IReadOnlyList<ReferentielEtenduEnt> Get(List<int> ressourceIds, int societeId);
    }
}
