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
using Fred.Entities.Utilisateur;
using Fred.Web.Models.Personnel;

namespace Fred.Business.Personnel
{
    /// <summary>
    /// Gestionnaire du personnel.
    /// </summary>
    public interface IPersonnelManager : IManager<PersonnelEnt>
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
        /// Retourne une nouvelle instannce de personnelEnt
        /// </summary>
        /// <returns>Objet personnel.</returns>
        PersonnelEnt GetNewPersonnel();

        /// <summary>
        /// Retourne une nouvelle instance de UtilisateurEnt
        /// </summary>
        /// <returns>Objet personnel.</returns>
        UtilisateurEnt GetNewUtilisateur();

        /// <summary>
        /// Retourne la liste du personnel
        /// </summary>
        /// <returns>Liste du personnel.</returns>
        IEnumerable<PersonnelEnt> GetPersonnelList();

        /// <summary>
        /// Retourne la liste du personnel
        /// </summary>
        /// <returns>Liste du personnel.</returns>
        IEnumerable<PersonnelEnt> GetOutgoingPersonnelsList(string suffixDisableLogin = "-old");

        /// <summary>
        /// Retourne la liste du personnel pour la synchronisation mobile.
        /// </summary>
        /// <returns>Liste du personnel.</returns>
        IEnumerable<PersonnelEnt> GetPersonnelListSync();

        /// <summary>
        /// Retourne le personnel portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel à retrouver.</param>
        /// <returns>Le personnel retrouvé, sinon nulle.</returns>
        PersonnelEnt GetPersonnel(int personnelId);

        /// <summary>
        /// Retourne le personnel portant l'identifiant unique indiqué.
        /// </summary>
        /// <typeparam name="TPersonnel">Le type de personnel souhaité.</typeparam>
        /// <param name="personnelId">Identifiant du personnel à retrouver.</param>
        /// <param name="selector">Selector permettant de constuire un TPersonnel à partir d'un PersonnelEnt.</param>
        /// <returns>Le personnel retrouvé, sinon null.</returns>
        TPersonnel GetPersonnel<TPersonnel>(int personnelId, Expression<Func<PersonnelEnt, TPersonnel>> selector);

        /// <summary>
        /// Retourne le personnel portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel à retrouver.</param>
        /// <returns>Le personnel retrouvé, sinon nulle.</returns>
        PersonnelEnt GetSimplePersonnel(int personnelId);


        /// <summary>
        /// Retourne le personnel en fonction de son email
        /// </summary>
        /// <param name="email">Email du personnel</param>
        /// <returns>Personnel</returns>
        PersonnelEnt GetPersonnelByEmail(string email);

        /// <summary>
        /// Ajoute un personnel
        /// </summary>
        /// <param name="personnel">Le personnel à ajouter</param>
        /// <returns>L'identifiant unique du personnel ajouté.</returns>
        PersonnelEnt Add(PersonnelEnt personnel, int? userId = null);

        /// <summary>
        /// Ajoute un personnel
        /// </summary>
        /// <param name="personnel">Le personnel à ajouter</param>
        /// <returns>Personnel mis à jour</returns>
        PersonnelEnt Update(PersonnelEnt personnel, int? userId = null);

        void UpdateDateEntreePersonnelInterimaire(int interimaireId, DateTime? interimaireDateEntree, int? userId = null);

        /// <summary>
        /// Ajouter un personnel en tant qu'Utilisateur de FRED
        /// </summary>
        /// <param name="personnel">Personnel à ajouter en tant qu'Utilisateur</param>
        /// <returns>Personnel/Utilisateur ajouté</returns>
        PersonnelEnt AddPersonnelAsUtilisateur(PersonnelEnt personnel);

        /// <summary>
        /// Retourne un personnel ou null s'il n'existe pas.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="matricule">Matricule du personnel</param>
        /// <returns>Le personnel s'il existe, sinon null.</returns>
        PersonnelEnt GetPersonnel(int societeId, string matricule);

        /// <summary>
        /// Retourne le matériel du personnel
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>Le matériel par défaut du personnel.</returns>
        MaterielEnt GetMaterielDefault(int personnelId);

        /// <summary>
        /// Retourne la liste des personnels liés à une société spécifique
        /// </summary>
        /// <param name="codeSociete">Le code de la société</param>
        /// <returns>Liste du personnel pour la société spécifier.</returns>
        IEnumerable<PersonnelEnt> GetPersonnelListByCodeSocietePaye(string codeSociete);

        /// <summary>
        /// Récupère l'affectation intérimaire active pour une date donnée
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="date">date</param>
        /// <returns>l'affectation intérimaire active  pour une date donnée</returns>
        ContratInterimaireEnt GetAffectationInterimaireActive(int personnelId, DateTime date);

        /// <summary>
        /// Retourne le membre du personnel en fonction de son nom et prénom
        /// </summary>
        /// <param name="nom">Nom du personnel</param>
        /// <param name="prenom">Prénom du personnel</param>
        /// <returns>Le membre du personnel retrouvé, sinon null.</returns>
        PersonnelEnt GetPersonnelByNomPrenom(string nom, string prenom);

        /// <summary>
        /// Récupération du personnel en fonction de son nom, prénom et groupe
        /// </summary>
        /// <param name="nom">Nom du personnel</param>
        /// <param name="prenom">Prénom du personnel</param>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Personnel</returns>
        PersonnelEnt GetPersonnelByNomPrenom(string nom, string prenom, int groupeId);

        /// <summary>
        /// Initialise une nouvelle instance de la classe de recherche des commandes
        /// </summary>
        /// <returns>Objet de filtrage + tri des commandes initialisé</returns>
        SearchPersonnelEnt GetNewFilter();

        /// <summary>
        /// Retourne le nombre total de commandes filtrées
        /// </summary>
        /// <param name="filter">Objet de recherche et de tri des commandes</param>
        /// <returns>Retourne le nombre total de commandes filtrée, triée et paginée</returns>
        int GetCountPersonnel(SearchPersonnelEnt filter);

        /// <summary>
        /// Permet de récupérer le prédicat de recherche du personnel.
        /// </summary>
        /// <param name="filters">Filtre de recherche</param>
        /// <param name="page">Numero de page à récupéré</param>
        /// <param name="pageSize">Nombre d'éléments par page</param>
        /// <returns>Retourne la condition de recherche du personnel</returns>
        SearchPersonnelsWithFiltersResult SearchPersonnelsWithFilters(SearchPersonnelEnt filters, int page, int pageSize);

        /// <summary>
        /// Vérifie la validité et enregistre le personnel importé depuis ANAËL Paie
        /// </summary>
        /// <param name="personnels">Liste des entités dont il faut vérifier la validité</param>
        /// <param name="societeId">Société pour laquelle on vérifie le personnel</param>
        /// <returns>Liste des entités qui sont ajoutées en base</returns>
        List<PersonnelEnt> ManageImportedPersonnels(IEnumerable<PersonnelEnt> personnels, int? societeId);

        /// <summary>
        /// Creation ou mise a jour des affectation suite a un import du personnel
        /// </summary>
        /// <param name="newOrUpdatedAffectations">Liste d'affectation a mettre a jour ou a créer</param>
        void ImportPersonnelsAffectations(List<PersonnelAffectationResult> newOrUpdatedAffectations);

        /// <summary>
        /// Récupère la liste de toutes les affectations des intérimaires
        /// </summary>
        /// <returns>Liste des affectation des intérimaires</returns>
        IEnumerable<ContratInterimaireEnt> GetAffectationInterimaireList();

        /// <summary>
        /// Récupère la liste des affectations d'un intérimaire en fonction de l'identifiant Personnel intérimaire
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personne intérimaire</param>
        /// <returns>Liste des affectation du personnel intérimaire</returns>
        IEnumerable<ContratInterimaireEnt> GetAffectationInterimaireList(int personnelId);

        /// <summary>
        /// Récupère la liste de toutes les affectations des intérimaires avec pagination
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personne intérimaire</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des affectation des intérimaires</returns>
        IEnumerable<ContratInterimaireEnt> GetAffectationInterimaireList(int personnelId, int page, int pageSize);

        /// <summary>
        /// Retourne un model DateEntreeSortie
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>un model DateEntreeSortie</returns>
        DateEntreeSortieModel GetDateEntreeSortie(int personnelId);

        /// <summary>
        /// Mise à jour d'une affectation d'un intérimaire
        /// </summary>
        /// <param name="affectation">Affectation d'un intérimaire à mettre à jour</param>
        /// <returns>Affectation mise à jour</returns>
        ContratInterimaireEnt AddAffectationInterimaire(ContratInterimaireEnt affectation);

        /// <summary>
        /// Mise à jour d'une affectation d'un intérimaire
        /// </summary>
        /// <param name="affectation">Affectation d'un intérimaire à mettre à jour</param>
        /// <returns>Affectation mise à jour</returns>
        ContratInterimaireEnt UpdateAffectationInterimaire(ContratInterimaireEnt affectation);

        /// <summary>
        /// Génère le prochain matricule d'un nouvel intérimaire en fonction du dernier matricule intérimaire en base
        /// </summary>
        /// <returns>Nouveau matricule généré</returns>
        string GetNextMatriculeInterimaire();

        /// <summary>
        /// Suppression logique d'un personnel et/ou compte Utlisateur du personnel
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        void DeleteById(int personnelId);

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
        /// retourne le personnel a partir de son identifiant
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>personnel</returns>
        PersonnelEnt GetPersonnelById(int? personnelId);

        /// <summary>
        /// Get groupe du personnel by personnel identifier
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>Groupe entité</returns>
        GroupeEnt GetPersonnelGroupebyId(int personnelId);

        /// <summary>
        /// Recherche de personnels dans le référentiel
        /// </summary>
        /// <param name="search">Model SearchLightPersonnel</param>          
        /// <returns>Une liste de personnel</returns>
        Task<IEnumerable<PersonnelEnt>> SearchLightAsync(SearchLightPersonnelModel search);
        /// <summary>
        /// Recherche de personnels dans le référentiel
        /// </summary>
        /// <param name="search">Model SearchLightPersonnel</param>        
        /// <returns>Une liste de personnel</returns>
        IEnumerable<PersonnelEnt> SearchLightForTeam(SearchLightPersonnelModel search);

        /// <summary>
        /// Retourne une liste du personnel dont l'id est contenu dans la liste passé ene parametre
        /// </summary>
        /// <param name="personnelIds">Liste des personnels a selectionner</param>
        /// <returns>Liste du personnel.</returns>
        List<PersonnelEnt> GetPersonnelsByIds(List<int> personnelIds);

        /// <summary>
        /// Retourne un personnel sans le mettre dans le context(AsNoTracking)
        /// Utile pour faire une comparaison des valeurs de champs.
        /// </summary>
        /// <param name="personnelId">ciId</param>
        /// <returns>Un ci détaché du contexte</returns>
        PersonnelEnt GetPersonnelForCompare(int personnelId);

        /// <summary>
        /// Retourne la liste des personnels du niveau hierarchique inférieur
        /// </summary>
        /// <param name="search">Objet de recherche SearchLightPersonnel</param>
        /// <returns>La liste des personnels du niveau hierarchique inférieur</returns>
        IEnumerable<PersonnelEnt> SearchNmoins1(SearchLightPersonnelModel search);

        /// <summary>
        /// Retourne la liste des auteurs de rapport
        /// </summary>
        /// <param name="search">Modèle de recherche personnel</param>
        /// <param name="groupeId">Groupe de l'utilisateur courant</param>
        /// <param name="listUserId">Liste des users, auteur de rapport</param>
        /// <returns>La liste des auteurs de rapport</returns>
        IEnumerable<PersonnelEnt> SearchRapportAuthor(SearchLightPersonnelModel search, int? groupeId, IEnumerable<int> listUserId);

        /// <summary>
        /// Recherche de personnels dans le référentiel+ les habilitations
        /// </summary>
        /// <param name="search">Model SearchLightPersonnel</param>        
        /// <returns>Une liste de personnel</returns>
        Task<List<PersonnelEnt>> SearchPersonnelEnteteCommandeAsync(SearchLightPersonnelModel search);

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
        /// Update personnels pointable by etablisssemnt paie idenetifiant
        /// </summary>
        /// <param name="etablissementPaieId">Etablissement paie id</param>
        /// <param name="isNonPointable">is pointable</param>
        void UpdatePersonnelsPointableByEtatPaieId(int etablissementPaieId, bool isNonPointable);

        /// <summary>
        /// Retourne les infos de l'utilisateur
        /// </summary>
        /// <param name="personnel">personnel</param>
        /// <returns>infos de l'utilisateur</returns>
        string GetPersonnelMatriculeNomPrenom(PersonnelDao personnel);

        /// <summary>
        /// Retourne une liste de personnel 
        /// </summary>
        /// <param name="ListPersonnelId">Id des personnels</param>
        /// <returns>List de personnel</returns>
        List<PersonnelEnt> GetPersonnelByListPersonnelId(List<int?> ListPersonnelId);

        /// <summary>
        /// Cherche un interimaire par matricule externe et groupe code
        /// </summary>
        /// <param name="matriculeExterne">Matricule externe</param>
        /// <param name="groupeCode">Groupe code</param>
        /// <param name="systemeInterimaire">System interimaire (Pixid par exemple)</param>
        /// <returns>Personnel interimaire si existe</returns>
        PersonnelEnt GetPersonnelInterimaireByExternalMatriculeAndGroupeId(string matriculeExterne, string groupeCode, string systemeInterimaire);

        /// <summary>
        /// Get etab paie list for validation pointage vrac Fes Async
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="societeId">Societe Id</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="StatutPersonnelList">List des personnels statut</param>
        /// <returns>Etab Paie List</returns>
        Task<IEnumerable<PersonnelEnt>> GetPersonnelListForValidationPointageVracFesAsync(int page, int pageSize, string recherche, int? societeId, DateTime dateDebut, IEnumerable<string> StatutPersonnelList);

        /// <summary>
        /// Search Personnels With Filters Optimized
        /// </summary>
        /// <param name="filters">Filtre de recherche</param>
        /// <param name="page">Numero de page à récupéré</param>
        /// <param name="pageSize">Nombre d'éléments par page</param>
        /// <returns>Retourne la condition de recherche du personnel</returns>
        SearchPersonnelsWithFiltersResult SearchPersonnelsWithFiltersOptimized(SearchPersonnelEnt filters, int page, int pageSize);
    }
}
