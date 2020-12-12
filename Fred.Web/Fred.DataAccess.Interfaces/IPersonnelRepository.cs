using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Budget.Dao.Budget.ExcelExport;
using Fred.Entities.Groupe;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Import;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données pour les membres du personnel.
    /// </summary>
    public interface IPersonnelRepository : IFredRepository<PersonnelEnt>
    {
        /// <summary>
        /// Retourne la requête de récupération du personnel
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        List<PersonnelEnt> Get(List<Expression<Func<PersonnelEnt, bool>>> filters,
                                       Func<IQueryable<PersonnelEnt>, IOrderedQueryable<PersonnelEnt>> orderBy = null,
                                       List<Expression<Func<PersonnelEnt, object>>> includeProperties = null,
                                       int? page = null,
                                       int? pageSize = null,
                                       bool asNoTracking = true);
        /// <summary>
        ///   Retourne la liste du personnel.
        /// </summary>
        /// <returns>La liste du personnel.</returns>
        IEnumerable<PersonnelEnt> GetPersonnelList();

        /// <summary>
        ///   Retourne la liste du personnel sortis.
        /// </summary>
        /// <returns>La liste du personnel.</returns>
        IEnumerable<PersonnelEnt> GetOutgoingPersonnelsList(string suffixDisableLogin = "-old");

        /// <summary>
        /// Retourne la liste des personnels adaptée aux besoins de l'export excel
        /// </summary>
        /// <param name="filter">Le filtre utilisé pour les personnels</param>
        /// <param name="isConnectedUserSuperAdmin">True si l'utilisateur actuellement connecté est super Admin, false sinon</param>
        /// <param name="connectedUserGroupId">id du groupe contenant auquel l'utilisateur connecté appartient</param>
        /// <returns>Une liste du personnel, jamais null</returns>
        IEnumerable<PersonnelEnt> GetPersonnellListForExportExcel(SearchPersonnelEnt filter, bool isConnectedUserSuperAdmin, int? connectedUserGroupId);

        /// <summary>
        ///   Retourne la liste du personnel pour la synchronisation mobile.
        /// </summary>
        /// <returns>La liste du personnel.</returns>
        IEnumerable<PersonnelEnt> GetPersonnelListSync();

        /// <summary>
        /// Permet de récupérer une liste de délégués potentiels
        /// </summary>
        /// <param name="delegantId">Affectation d'un intérimaire à mettre à jour</param>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>LA liste de délégués potentiels</returns>
        IEnumerable<PersonnelEnt> GetDelegue(int delegantId, int societeId, string recherche, int page, int pageSize);

        /// <summary>
        /// Permet de récupérer une liste de délégués potentiels
        /// </summary>
        /// <param name="delegantId">Identifiant du délégant</param>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche2">Prénom recherché</param>
        /// <param name="recherche3">Autres infos recherchées</param>
        /// <returns>LA liste de délégués potentiels</returns>
        IEnumerable<PersonnelEnt> GetDelegue(int delegantId, int societeId, string recherche, int page, int pageSize, string recherche2, string recherche3);

        /// <summary>
        ///   Retourne le personnel portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel à retrouver.</param>
        /// <param name="withDependencies">Si vrai charge les dépendence</param>
        /// <returns>Le personnel retrouvé, sinon nulle.</returns>
        PersonnelEnt GetPersonnel(int personnelId, bool withDependencies = false);

        /// <summary>
        ///   Retourne le personnel portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel à retrouver.</param>
        /// <returns>Le personnel retrouvé, sinon nulle.</returns>
        PersonnelEnt GetSimplePersonnel(int personnelId);

        /// <summary>
        /// Retourne un personnel ou null s'il n'existe pas.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="matricule">Matricule du personnel</param>
        /// <returns>Le personnel s'il existe, sinon null.</returns>
        PersonnelEnt GetPersonnel(int societeId, string matricule);

        /// <summary>
        ///   Retourne le membre du personnel en fonction de son nom et prénom
        /// </summary>
        /// <param name="nom">Nom du personnel</param>
        /// <param name="prenom">Prénom du personnel</param>
        /// <param name="groupeId">Identifiant du groupe du Personnel</param>
        /// <returns>Le membre du personnel retrouvé, sinon null.</returns>
        PersonnelEnt GetPersonnelByNomPrenom(string nom, string prenom, int? groupeId);

        /// <summary>
        /// Retourne un personnel par son Identifiant
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>personnel</returns>
        PersonnelEnt GetPersonnelById(int? personnelId);


        /// <summary>
        ///   Retourne le personnel en fonction de son email
        /// </summary>
        /// <param name="email">Email du personnel</param>
        /// <returns>Personnel</returns>
        PersonnelEnt GetPersonnelByEmail(string email);

        /// <summary>
        ///   Retourne le matériel du personnel
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>Le matériel par défaut du personnel.</returns>
        MaterielEnt GetMaterielDefault(int personnelId);

        /// <summary>
        ///   Retourne la liste des personnels interne liés à une société spécifique
        /// </summary>
        /// <param name="codeSociete">Le code de la société</param>
        /// <returns>Liste des personnels internes appartenant une société spécifique</returns>
        IEnumerable<PersonnelEnt> GetPersonnelListByCodeSocietePaye(string codeSociete);

        /// <summary>
        ///   Ajoute un personnel
        /// </summary>
        /// <param name="personnel">Le personnel interne à ajouter</param>
        /// <returns>Retourne l'identifiant unique du personnel ajouté</returns>
        PersonnelEnt AddPersonnel(PersonnelEnt personnel);

        /// <summary>
        ///   Mise à jour d'un personnel
        /// </summary>
        /// <param name="personnel">Le personnel à ajouter</param>
        /// <returns>Personnel mis à jour</returns>
        PersonnelEnt UpdatePersonnel(PersonnelEnt personnel);

        /// <summary>
        ///   Récupère l'affectation intérimaire active pour une date donnée
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="date">Date</param>
        /// <returns>l'affectation intérimaire active pour une date donnée</returns>
        ContratInterimaireEnt GetContratInterimaireActive(int personnelId, DateTime date);

        /// <summary>
        ///   Récupère la liste de toutes les affectations des intérimaires
        /// </summary>
        /// <returns>Liste des affectation des intérimaires</returns>
        IEnumerable<ContratInterimaireEnt> GetContratInterimaireList();

        /// <summary>
        ///   Récupère la liste des affectations d'un intérimaire en fonction de l'identifiant Personnel intérimaire
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personne intérimaire</param>
        /// <returns>Liste des affectation du personnel intérimaire</returns>
        IEnumerable<ContratInterimaireEnt> GetContratInterimaireList(int personnelId);

        /// <summary>
        ///   Récupère la liste de toutes les affectations des intérimaires avec pagination
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personne intérimaire</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des affectation des intérimaires</returns>
        IEnumerable<ContratInterimaireEnt> GetContratInterimaireList(int personnelId, int page, int pageSize);

        /// <summary>
        /// Mise à jour d'une affectation d'un intérimaire
        /// </summary>
        /// <param name="affectation">Affectation d'un intérimaire à mettre à jour</param>
        /// <returns>Affectation mise à jour</returns>
        ContratInterimaireEnt AddContratInterimaire(ContratInterimaireEnt affectation);

        /// <summary>
        /// Mise à jour d'une affectation d'un intérimaire
        /// </summary>
        /// <param name="affectation">Affectation d'un intérimaire à mettre à jour</param>
        /// <returns>Affectation mise à jour</returns>
        ContratInterimaireEnt UpdateContratInterimaire(ContratInterimaireEnt affectation);

        /// <summary>
        ///   Ajoute ou met à jour du Personnels selon la liste en paramètre
        /// </summary>
        /// <param name="personnels">Liste des Personnels</param>    
        void AddOrUpdatePersonnelList(IEnumerable<PersonnelEnt> personnels);

        /// <summary>
        /// Creation ou mise a jour des affectation suite a un import du personnel
        /// </summary>
        /// <param name="personnelAffectationResults">Liste d'affectation a mettre a jour ou a créer</param>
        void ImportPersonnelsAffectations(List<PersonnelAffectationResult> personnelAffectationResults);

        /// <summary>
        /// Retourne la liste des intérimaires actifs pour un Ci donné
        /// </summary>
        /// <param name="ciId">Identifiant</param>
        /// <returns>La liste des intérimaires actifs pour un Ci donné</returns>
        IRepositoryQuery<PersonnelEnt> GetInterimaireActifList(int ciId);

        /// <summary>
        /// Récupérer les identifiants des personnels qui sont sous la responsabilité d'un manager
        /// </summary>
        /// <param name="managerId">L'identifiant du manager</param>
        /// <returns>La liste des identifiants</returns>
        List<int> GetManagedPersonnelIds(int managerId);

        /// <summary>
        /// Récupérer les identifiants des personnels qui sont sous la responsabilité d'un manager
        /// </summary>
        /// <param name="managerId">L'identifiant du manager</param>
        /// <returns>La liste des identifiants</returns>
        List<int> GetManagedEmployeeIdList(int managerId);

        /// <summary>
        /// Vérifier si un personnel est un ETAM ou IAC
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>Booléan indiquant si le personnel est un ETAM \ IAC</returns>
        bool IsEtamOrIac(int personnelId);

        /// <summary>
        /// Get groupe du personnel by personnel identifier
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>Groupe entité</returns>
        GroupeEnt GetPersonnelGroupebyId(int personnelId);

        /// <summary>
        /// Récupérer les identifiants des ETAM \ IAC qui sont sous la responsabilité d'un manager
        /// </summary>
        /// <param name="managerId">L'identifiant du manager</param>
        /// <returns>La liste des identifiants</returns>
        List<int> GetManagedEtamsAndIacs(int managerId);

        /// <summary>
        /// Retourne une liste de personnel Fes dernièrement créé ou mise à jour pour l'export vers tibco
        /// </summary>
        /// <param name="byPassDate">booléan qui indique si l'on se base sur la dernière date d'execution ou non</param>
        /// <param name="lastExecutionDate">DateTime dernière date d'execution du flux</param>
        /// <returns>Une liste de personnel fes</returns>
        IEnumerable<PersonnelEnt> GetPersonnelFesForExportToTibco(bool byPassDate, DateTime? lastExecutionDate);

        /// <summary>
        /// Retourne si true si le personnel est manager d'un autre personnel
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>true ou false</returns>
        bool PersonnelIsManager(int personnelId);

        /// <summary>
        ///   Permet de détacher les entités dépendantes pour éviter de les prendre en compte dans la sauvegarde du contexte.
        /// </summary>
        /// <param name="persoInterne">objet dont les dépendances sont à détachées</param>
        void DetachDependancies(PersonnelEnt persoInterne);

        /// <summary>
        /// Get personnels list by etablissement paie identifiant
        /// </summary>
        /// <param name="etablissementPaieId">Etablissement paie Id</param>
        /// <returns>List des personnels</returns>
        IEnumerable<PersonnelEnt> GetPersonnelsListByEtabPaieId(int etablissementPaieId);

        /// <summary>
        /// Retourne un personnel en fonction de son identifiant pour l'export Excel de la comparaison de budget.
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel.</param>
        /// <returns>Le personnel.</returns>
        PersonnelDao GetPersonnelPourExportExcelHeader(int personnelId);

        /// <summary>
        ///   Retourne la liste du personnel + Personnels qui ont une habilitation
        /// </summary>
        /// <returns>La liste du personnel.</returns>
        Task<List<PersonnelEnt>> GetPersonnelEnteteCommandeAsync(Expression<Func<PersonnelEnt, bool>> predicateForPersonelWithAffectation, Expression<Func<PersonnelEnt, bool>> predicatePersonnelWithHabilitation, int? page = 1, int? pageSize = 20);

        /// <summary>
        /// Retourne une liste de personnel 
        /// </summary>
        /// <param name="personnelIds">Id des personnels</param>
        /// <returns>List de personnel</returns>
        List<PersonnelEnt> GetPersonnelByListPersonnelId(List<int?> personnelIds);

        /// <summary>
        /// Cherche un interimaire par matricule externe et groupe code
        /// </summary>
        /// <param name="matriculeExterne">Matricule externe</param>
        /// <param name="groupeCode">Groupe code</param>
        /// <param name="systemeInterimaire">System interimaire (Pixid par exemple)</param>
        /// <returns>Personnel interimaire si existe</returns>
        PersonnelEnt GetPersonnelInterimaireByExternalMatriculeAndGroupeId(string matriculeExterne, string groupeCode, string systemeInterimaire);

        /// <summary>
        /// Recupére une liste de personnel appartenant a une société
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Liste de Personnels</returns>
        List<PersonnelEnt> GetPersonnelBySociete(int societeId, DateTime datedebut);

        /// <summary>
        /// Recupére une liste de personnels appartenant a des établissement comptables
        /// </summary>
        /// <param name="etablissementComptablesId">Liste des ids des etablissement Comptables </param>
        /// <returns>Liste de Personnels</returns>
        List<PersonnelEnt> GetPersonnelByEtablissementComptable(List<int> etablissementComptablesId, DateTime datedebut);

        /// <summary>
        /// Recupére une liste de personnel appartenant a une société
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <param name="statutPersonnelList">List des personnels statut</param>
        /// <param name="dateDebut">Date debut</param>
        /// <returns>Liste de Personnels</returns>
        Task<List<PersonnelEnt>> GetPersonnelBySocieteId(int societeId, IEnumerable<string> statutPersonnelList, DateTime dateDebut);

        /// <summary>
        /// Recupére une liste de personnel appartenant a une List des sociétés
        /// </summary>
        /// <param name="societeIdsList">identifiant de la société</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="statutPersonnelList">List des personnels statut</param>
        /// <returns>Liste de Personnels</returns>
        Task<List<PersonnelEnt>> GetPersonnelsBySocieteIdsListAsync(List<int> societeIdsList, DateTime dateDebut, IEnumerable<string> statutPersonnelList);

        /// <summary>
        /// Search Personnels With Filters Optimized
        /// </summary>
        /// <param name="filters">Filtre de recherche</param>
        /// <param name="page">Numero de page à récupéré</param>
        /// <param name="pageSize">Nombre d'éléments par page</param>
        /// <param name="splitIsActived">split is activated</param>
        /// <param name="isSuperAdmin">is super admin</param>
        /// <param name="currentUserGroupeId">idntifient du groupe de l'utilisateur courant</param>
        /// <param name="totalCount">total count</param>
        /// <returns>Retourne la condition de recherche du personnel</returns>
        IEnumerable<PersonnelListResultViewModel> GetPersonnelsByFilterOptimzed(SearchPersonnelEnt filters, int page, int pageSize, bool splitIsActived, bool isSuperAdmin, int? currentUserGroupeId, out int totalCount);
    }
}
