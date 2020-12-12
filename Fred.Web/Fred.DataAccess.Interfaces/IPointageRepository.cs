using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Web.Models.PointagePersonnel;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les pointages.
    /// </summary>
    public interface IPointageRepository : IPointageBaseRepository<RapportLigneEnt>
    {
        /// <summary>
        ///   Récupère un Lot de Pointage en fonction de son Identifiant
        /// </summary>
        /// <param name="rapportLigneId">Identifiant du lot de pointage</param>
        /// <param name="includes">Includes lors du get</param>
        /// <returns>Lot de pointage</returns>
        RapportLigneEnt Get(int rapportLigneId, List<Expression<Func<RapportLigneEnt, object>>> includes);

        /// <summary>
        /// Récupère les rapports lignes pour la synchronisation mobile.
        /// </summary>
        /// <returns>Les rapports lignes.</returns>
        IEnumerable<RapportLigneEnt> GetRapportLigneAllSync();

        /// <summary>
        /// Récupère les rapports lignes primes pour la synchronisation mobile.
        /// </summary>
        /// <returns>Les rapports lignes primes.</returns>
        IEnumerable<RapportLignePrimeEnt> GetRapportLignePrimeAllSync();

        /// <summary>
        /// Récupère les rapports lignes taches pour la synchronisation mobile.
        /// </summary>
        /// <returns>Les rapports lignes taches.</returns>
        IEnumerable<RapportLigneTacheEnt> GetRapportLigneTacheAllSync();

        /// <summary>
        ///   Récupère l'entité RapportLigneTache correspondant au identifiants passés en paramètre
        /// </summary>
        /// <param name="rapportLigneTacheId">Identifiant de la liaison pointage/tâche</param>
        /// <returns>Retourne l'entité RapportLigneTache correspondant au identifiants passés en paramètre</returns>
        RapportLigneTacheEnt GetRapportLigneTacheById(int rapportLigneTacheId);

        /// <summary>
        /// Récupère les rapports statuts pour la synchronisation mobile.
        /// </summary>
        /// <returns>Les rapports statuts.</returns>
        IEnumerable<RapportStatutEnt> GetRapportStatutAllSync();



        /// <summary>
        /// Récupère les rapports taches pour la synchronisation mobile.
        /// </summary>
        /// <returns>Les rapports taches.</returns>
        IEnumerable<RapportTacheEnt> GetRapportTacheAllSync();

        /// <summary>
        ///   Récupère toutes les lignes de pointages (Light) d'un rapport
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport</param>
        /// <returns>Liste des lignes de pointages</returns>
        IEnumerable<RapportLigneEnt> GetAllLight(int rapportId);

        /// <summary>
        ///   Recherche la liste des pointages réels correspondants aux prédicats
        /// </summary>
        /// <param name="predicateWhere">Prédicat de filtrage des résultats</param>
        /// <param name="orderBy">Clause de tri "champ + ordre, champ + ordre"</param>
        /// <returns>IEnumerable contenant les rapports correspondants aux critères de recherche</returns>
        IEnumerable<RapportLigneEnt> SearchPointageWithFilter(Expression<Func<RapportLigneEnt, bool>> predicateWhere, string orderBy);

        /// <summary>
        /// Indique si des pointages existent.
        /// </summary>
        /// <param name="predicates">Prédicats de filtrage des résultats.</param>
        /// <returns>True si des pointages existent, sinon false.</returns>
        bool IsPointagesExists(params Expression<Func<RapportLigneEnt, bool>>[] predicates);

        /// <summary>
        ///   Recherche la liste des pointages réels correspondant aux prédicats par page
        /// </summary>
        /// <param name="predicateWhere">Prédicat de filtrage des résultats</param>
        /// <param name="orderBy">Clause de tri "champ + ordre, champ + ordre"</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>IEnumerable contenant les lignes de rapport correspondant aux critères de recherche et à la page concernée</returns>
        IEnumerable<RapportLigneEnt> SearchPointageWithFilterByPage(Expression<Func<RapportLigneEnt, bool>> predicateWhere, string orderBy, int page, int pageSize);

        /// <summary>
        ///   Recherche la liste des pointages réels correspondant aux prédicats par page
        /// </summary>
        /// <param name="predicateWhere">Prédicat de filtrage des résultats</param>
        /// <param name="orderBy">Clause de tri "champ + ordre, champ + ordre"</param>
        /// <returns>IEnumerable contenant les lignes de rapport correspondant aux critères de recherche et à la page concernée</returns>
        List<RapportLigneEnt> SearchPointageReelWithMoyenFilter(Expression<Func<RapportLigneEnt, bool>> predicateWhere, string orderBy);

        /// <summary>
        ///   Retourne une liste de pointages réels en fonction du personnel et d'une date
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="date">La date du pointage</param>
        /// <returns>Une liste de pointages réels</returns>
        IEnumerable<RapportLigneEnt> GetListPointagesReelsByPersonnelIdAndDatePointage(int personnelId, int ciId, DateTime date);

        /// <summary>
        ///   Retourne une liste de pointages réels en fonction du ci et d'une date
        /// </summary>
        /// <param name="ciId">L'identifiant du CI</param>
        /// <param name="periode">La periode</param>
        /// <returns>Une liste de pointages réels</returns>
        IEnumerable<RapportLigneEnt> GetListPointagesReelsByCiIdFromPeriode(int ciId, DateTime periode);

        /// <summary>
        ///   Retourne une liste de pointages réels en fonction du personnel et d'un mois
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="month">La période du pointage</param>
        /// <returns>Une liste de pointages réels</returns>
        IEnumerable<RapportLigneEnt> GetListPointagesOnMonthForPersonnelId(int personnelId, DateTime month);

        /// <summary>
        ///   Récupère la liste des pointage réels d'un rapport
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport</param>
        /// <returns>Une liste de lignes de rapport</returns>
        IEnumerable<RapportLigneEnt> GetPointageReelByRapportId(int rapportId);

        /// <summary>
        /// Retourne un pointage sans Attachement au contexte
        /// </summary>
        /// <param name="rapportLigneId">Identifiant du pointage</param>
        /// <returns>Pointage</returns>
        RapportLigneEnt GetById(int rapportLigneId);

        /// <summary>
        ///   Retourne une liste de pointages réels pour un personnel dans un exercice paie
        /// </summary>
        /// <param name="personnel">Le personnel</param>
        /// <returns>Le une liste de pointages</returns>
        IEnumerable<RapportLigneEnt> GetListPointagesReelsInExerciceByPersonnel(PersonnelEnt personnel);

        /// <summary>
        ///   Récupère la liste du personnel supervisé parr l'utilisateur
        /// </summary>
        /// <param name="referentId">identifiant de l'utilisateur référent (correspondant paie)</param>
        /// <returns>List des entité Personnel représentant les personnes supervisé</returns>
        IEnumerable<PersonnelEnt> GetPerimetrePointageByCreatorValidator(int referentId);

        /// <summary>
        ///   Récupère la liste des Rapport Ligne Reel par User
        /// </summary>
        /// <param name="userid">identifiant de l'utilisateur (GSP)</param>
        /// <param name="annee">Année de la période</param>
        /// <param name="mois">Mois de la période</param>
        /// <returns>Liste des RapportLigne reelles</returns>
        IEnumerable<int> GetAllPersonnelReelbyUser(int userid, int annee, int mois);

        /// <summary>
        ///   Récupère la liste des Rapport Ligne Reel par User
        /// </summary>
        /// <param name="userid">identifiant de l'utilisateur (GSP)</param>
        /// <param name="annee">Année de filtrage</param>
        /// <param name="mois">Mois de filtrage</param>
        /// <returns>Liste des RapportLigne reelles</returns>
        IEnumerable<PointageBase> GetPointageVerrouillesByUserId(int userid, int annee, int mois);

        /// <summary>
        /// Indique si des pointages verrouillés par un utilisateur existent.
        /// </summary>
        /// <param name="userid">identifiant de l'utilisateur (GSP)</param>
        /// <param name="annee">Année de filtrage</param>
        /// <param name="mois">Mois de filtrage</param>
        /// <returns>True si des pointages existent, sinon false.</returns>
        bool IsPointagesVerrouillesByUserExists(int userid, int annee, int mois);

        /// <summary>
        /// Récupère la liste des Rapport Ligne Reel par User
        /// </summary>
        /// <param name="userid">identifiant de l'utilisateur (GSP)</param>
        /// <param name="datemin">Date de Début</param>
        /// <param name="datemax">Date de fin</param>
        /// <returns>Liste des RapportLignes</returns>
        IEnumerable<PointageBase> GetPointageVerrouillesByUserIdByPeriode(int userid, DateTime datemin, DateTime datemax);

        /// <summary>
        /// Get personnel summary 
        /// </summary>
        /// <param name="personnelIdList">Personnel id list</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>IEnumerable of Personnel rapport summary</returns>
        IEnumerable<PersonnelRapportSummaryEnt> GetPersonnelPointageSummary(List<int> personnelIdList, DateTime mondayDate);

        /// <summary>
        /// Get ci pointage summary
        /// </summary>
        /// <param name="ciIdList">Ci id list</param>
        /// <param name="personnelStatut">Personnel statut</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>IEnumerable de CiPointageSummary</returns>
        IEnumerable<RapportLigneEnt> GetCiPointageSummary(List<int> ciIdList, string personnelStatut, DateTime mondayDate);

        /// <summary>
        /// Supprimer la liste des lignes de rapport sauf celle spécifié
        /// </summary>
        /// <param name="rapportId">L'identifiant du rapport </param>
        /// <param name="rapportLigneIdList">Les identifiants des rapports ligne à garder</param>
        /// <returns>Liste des RapportLigne qui n'existe pas dans la liste rapportLigneIdList</returns>
        IEnumerable<RapportLigneEnt> GetOtherRapportLignes(int rapportId, List<int> rapportLigneIdList);

        /// <summary>
        /// Retourne vrai si la prime journalière elle ddéjà dans un autre pointage
        /// </summary>
        /// <param name="primeId">identifiant de la prime</param>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="datePointage">Date du pointage</param>
        /// <param name="rapportligneId">Identifiant de la ligne de rapport</param>
        /// <returns>La prime journalière si elle existe</returns>
        bool IsPrimeJournaliereAlreadyExists(int primeId, int personnelId, DateTime datePointage, int rapportligneId);

        /// <summary>
        /// Recupere les rapports lignes pour determiner si la prime journalière elle déjà dans un autre pointage
        /// </summary>
        /// <param name="personnelIds">Liste de personnelIds</param>
        /// <param name="datesPointagesOfPointages">datesPointagesOfPointages</param>
        /// <returns>des rapports lignes</returns>
        IEnumerable<RapportLigneEnt> GetRapportLignesWithPrimesForCalculatePrimeJournaliereAlreadyExists(List<int> personnelIds, List<DateTime> datesPointagesOfPointages);

        /// <summary>
        /// Retourne vrai si la prime journalière elle déjà dans un autre pointage a partir d'une liste de rapports lignes
        /// </summary>
        /// <param name="rapportLignes">Liste de rapport lignes</param>
        /// <param name="primeId">identifiant de la prime</param>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="datePointage">Date du pointage</param>
        /// <param name="rapportligneId">Identifiant de la ligne de rapport</param>
        /// <returns>La prime journalière si elle existe</returns>
        bool IsPrimeJournaliereAlreadyExists(IEnumerable<RapportLigneEnt> rapportLignes, int primeId, int personnelId, DateTime datePointage, int rapportligneId);

        /// <summary>
        /// Recupere les rapports lignes pour determiner le total des heures normales
        /// </summary>
        /// <param name="personnelIds">personnelIds</param>
        /// <param name="ciIds">ciIds</param>
        /// <param name="pointageDates">pointageDates</param>
        /// <returns>des rapports lignes</returns>
        IEnumerable<RapportLigneEnt> GetRapportLignesForCalculateTotalHeuresNormalesAndMajorations(List<int> personnelIds, IEnumerable<int> ciIds, IEnumerable<DateTime> pointageDates);

        /// <summary>
        /// Permet de récupérer le total des heures pointées( HeureNormale + HeureMajoration) a partir d'une liste de pointages
        /// </summary>
        /// <param name="rapportLignes">pointages</param>
        /// <param name="personnelId">personnelId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="pointageDate">pointageDate</param>
        /// <returns>le total des heures normales</returns>
        double CalculateTotalHeuresNormalesAndMajorations(IEnumerable<RapportLigneEnt> rapportLignes, int personnelId, int ciId, DateTime pointageDate);

        /// <summary>
        /// Check rapport ligne existance
        /// </summary>
        /// <param name="rapportId">Rapport identifier</param>
        /// <param name="personnelId">Personnel Identifier</param>
        /// <returns>RapportLigne Identifier if exist</returns>
        int CheckRapportLigneExistance(int rapportId, int personnelId);

        /// <summary>
        ///   Retourne une liste de pointage dans une période donné et pour un périmètre précisé pour l'export excel des pointages intérimaire
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet Pointage Personnel Export Model</param>
        /// <param name="ciid">Liste d'identifiant unique de ci</param>
        /// <returns>Liste de pointages</returns>
        IEnumerable<RapportLigneEnt> GetListPointagesInterimaireVerrouillesByPeriode(PointagePersonnelExportModel pointagePersonnelExportModel, IEnumerable<int> ciid);

        /// <summary>
        /// Get ETAM/IAC summary 
        /// </summary>
        /// <param name="personnelIdList">Personnel id list</param>
        /// <param name="mondayDate">La date du lundi</param>
        /// <returns>IEnumerable of Personnel rapport summary</returns>
        IEnumerable<PersonnelRapportSummaryEnt> GetEtamIacPointageSummary(List<int> personnelIdList, DateTime mondayDate);

        /// <summary>
        /// Get list des rapports ligne for synthese mensuelle
        /// </summary>
        /// <param name="personnelId">Identifiants du personnel</param>
        /// <param name="firstDayInMonth">le premier jour du mmois</param>
        /// <param name="lastDayInMonth">le dernier jour du mois</param>
        /// <returns>List des rapports ligne</returns>
        IEnumerable<RapportLigneEnt> GetRapportLigneForSynthesMensuelle(int personnelId, DateTime firstDayInMonth, DateTime lastDayInMonth);

        /// <summary>
        /// Récuperer la list des personnels Ids affecte au Ci
        /// </summary>
        /// <param name="ciList">List des Ci</param>
        /// <param name="firstDayInMonth">Date du premier jour</param>
        /// <param name="lastDayInMonth">Date du dernier jour du mois</param>
        /// <returns>List des peronnel identifiant </returns>
        IEnumerable<RapportLigneEnt> GetEtamIacAffectationByCiList(IEnumerable<int> ciList, DateTime firstDayInMonth, DateTime lastDayInMonth);

        /// <summary>
        /// Get Etam Iac rapports for validation
        /// </summary>
        /// <param name="personnelId">Etam/Iac identifier</param>
        /// <param name="firstDayInMonth">Date du premier jour du mois</param>
        /// <param name="lastDayInMonth">Dernier jour du mois</param>
        /// <returns>List des rapports Lignes</returns>
        IEnumerable<RapportLigneEnt> GetEtamIacRapportsForValidation(int personnelId, DateTime firstDayInMonth, DateTime lastDayInMonth);

        /// <summary>
        ///   Retourne le nombre de pointage sur un contrat interimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Interimaire</param>
        /// <returns>Un nombre de pointage</returns>
        int GetPointageForContratInterimaire(ContratInterimaireEnt contratInterimaireEnt);

        /// <summary>
        ///   Retourne une liste de pointage dans une période donné et pour un périmètre précisé pour l'export excel des pointages personnel hebdomadaire
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet Pointage Personnel Export Model</param>
        /// <param name="ciid">Liste d'identifiant unique de ci</param>
        /// <returns>Liste de pointage</returns>
        IEnumerable<RapportLigneEnt> GetListPointagePersonnelHebdomadaire(PointagePersonnelExportModel pointagePersonnelExportModel, IEnumerable<int> ciid);

        /// <summary>
        /// Retourne l'identifiant du matériel
        /// </summary>
        /// <param name="rapportLigneId">L'ientifiant ddu pointage</param>
        /// <returns>L'identifiant du matériel</returns>
        int? GetMaterielId(int rapportLigneId);

        /// <summary>
        /// Retourne l'identifiant du personnel
        /// </summary>
        /// <param name="rapportLigneId">L'ientifiant ddu pointage</param>
        /// <returns>L'identifiant du personnel</returns>
        int? GetPersonnelId(int rapportLigneId);

        /// <summary>
        /// Retouner la list des rapports lignes qui contient des primes TR ou IR
        /// </summary>
        /// <param name="personnelId">Personnel identifer</param>
        /// <param name="mondayDate">Date début de semaine</param>
        /// <param name="sundayDate">Date fin du semaine</param>
        /// <returns>List des rapports lignes</returns>
        List<RapportLigneEnt> PrimePersonnelAffected(int personnelId, DateTime mondayDate, DateTime sundayDate);

        /// <summary>
        ///   Retourne les pointages vérouiller par rapport à personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique d'un personnel</param>
        /// <returns>Liste de pointage vérouiller</returns>
        IEnumerable<RapportLigneEnt> GetPointageVerrouillerByPersonnelId(int personnelId);

        /// <summary>
        /// Ajout ou mise à jour de masse des lignes de rapports
        /// </summary>
        /// <param name="rapportLignes">Liste de lignes de rapports</param>
        void AddOrUpdateRapportLigneList(IEnumerable<RapportLigneEnt> rapportLignes);

        /// <summary>
        /// Récuperer les pointages selon le personnel et son Affaire
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="ciId">identifiant de l'affaire</param>
        /// <returns>liste des Rapport Ligne</returns>
        IEnumerable<RapportLigneEnt> GetpointageByPersonnelAndCi(int personnelId, int ciId);

        /// <summary>
        /// Retourne la liste des pointage pour les personnels et les Cis envoyés en paramétres dans la plage des dates définies
        /// </summary>
        /// <param name="ciIdList">La liste des id des Cis</param>
        /// <param name="personnelIdList">La liste des id des personnels</param>
        /// <param name="startDate">Date de début limite</param>
        /// <param name="endDate">Date de fin limite</param>
        /// <returns>Retourne la liste des pointage</returns>
        IEnumerable<RapportLigneEnt> GetPointageByCisPersonnelsAndDates(IEnumerable<int> ciIdList, IEnumerable<int> personnelIdList, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Récuperer les pointages selon le personnel et son Affaire et pour une semaine
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="ciId">Identifiant de l'affaire</param>
        /// <param name="mondayDate">premier jour de la semaine </param>
        /// <returns>liste des Rapport ligne</returns>
        IEnumerable<RapportLigneEnt> GetPointageByPersonnelAndCiByDate(int personnelId, int ciId, DateTime mondayDate);

        /// <summary>
        /// Get Pointage pour Challenge Securite
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet Pointage Personnel Export Model</param>
        /// <param name="ciid">Liste d'identifiant unique de ci</param>
        /// <returns>IEnumerable of Personnel rapport summary</returns>
        IEnumerable<PersonnelRapportSummaryEnt> GetPointageChallengeSecurite(PointagePersonnelExportModel pointagePersonnelExportModel, IEnumerable<int> ciid);

        /// <summary>
        /// Récupére le pointage des moyens entre 2 dates . Le pointage des moyen est reconnu  par la colonne AffectationMoyenId non nulle.
        /// </summary>
        /// <param name="startDate">Date de début de pointage limite</param>
        /// <param name="endDate">Date de fin de pointage limite</param>
        /// <returns>Liste des lignes des rapports</returns>
        ExportTibcoRapportLigneModel[] GetPointageMoyenBetweenDates(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Retouner la list des rapports lignes des Majorations
        /// </summary>
        /// <param name="personnelId">Personnel identifer</param>
        /// <param name="mondayDate">Date début de semaine</param>
        /// <param name="sundayDate">Date fin du semaine</param>
        /// <returns>List des rapports lignes</returns>
        List<RapportLigneEnt> MajorationPersonnelAffected(int personnelId, DateTime mondayDate, DateTime sundayDate);

        /// <summary>
        ///   Récupère la liste des pointages, dans le périmètre de l'utilisateur connecté, des rapports verrouillés d'une période donnée
        /// </summary>
        /// <param name="periode">Période choisie</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="etablissementPaieIdList">Liste des indentifiants d'établissements de paie</param>
        /// <returns>Liste de pointages</returns>
        IEnumerable<RapportLigneEnt> GetAllLockedPointagesForSocieteOrEtablissementForVerificationCiSep(DateTime periode, int? societeId, IEnumerable<int> etablissementPaieIdList);

        /// <summary>
        /// Get Rapport ligne by rapport and personnel identifiers
        /// </summary>
        /// <param name="rapportId">Rapport identifier</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>Rapport ligne</returns>
        RapportLigneEnt GetRapportLigneByRapportIdAndPersonnelId(int rapportId, int personnelId);

        /// <summary>
        /// Get rapport ligne statut
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="datePointage">Date pointage</param>
        /// <returns>Rapport ligne statut</returns>
        string GetRapportLigneStatutCode(int personnelId, int ciId, DateTime datePointage);

        /// <summary>
        /// Recuperer les rapports lignes par l'affectation moyen identifiant
        /// </summary>
        /// <param name="affectationMoyenIdList">Affectation moyen liste des identifiants</param>
        /// <returns>List des rapports lignes</returns>
        List<RapportLigneEnt> GetPointageByAffectaionMoyenIds(IEnumerable<int> affectationMoyenIdList);

        /// <summary>
        /// Retouner la liste des lignes de rapport
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="mondayDate">Date début de semaine</param>
        /// <param name="sundayDate">Date fin du semaine</param>
        /// <returns>Liste des lignes de rapport</returns>
        IEnumerable<RapportLigneEnt> GetRapportLigneByPersonnelIdAndWeek(int personnelId, DateTime mondayDate, DateTime sundayDate);

        /// <summary>
        /// Get Rapport Lignes des absence FIGGO pour une période
        /// </summary>
        /// <param name="dateDebut">Date de debut</param>
        /// <param name="dateFin">Date de fin</param>
        /// <returns>listes des absences FIGGO</returns>
        IEnumerable<RapportLigneEnt> GetListPointagesFiggoByPeriode(DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Retourne la liste des pointage pour le personnels et le Ci envoyés en paramétres dans la plage des dates définies
        /// </summary>
        /// <param name="ciId">id du Ci</param>
        /// <param name="personnelId">id du personnel</param>
        /// <param name="startDate">Date de début limite</param>
        /// <param name="endDate">Date de fin limite</param>
        /// <returns>Retourne la liste des pointage</returns>
        IEnumerable<RapportLigneEnt> GetPointageByCiPersonnelAndDates(int ciId, int personnelId, DateTime startDate, DateTime endDate);

        /// <summary>
        ///  Retourne les pointages interimaires qui n'ont pas encore été réceptionnés
        /// </summary>      
        /// <param name="interimaireId">interimaireId</param>
        /// <param name="dateDebut">dateDebut</param>
        /// <param name="dateFin">dateFin</param>
        /// <returns>Liste d'ids des reception non receptionnees</returns>
        List<RapportLigneEnt> GetPointagesInterimaireNonReceptionnees(int interimaireId, DateTime dateDebut, DateTime dateFin);

        Task<IEnumerable<RapportLigneEnt>> GetPointagesAsync(DateTime period);
        /// <summary>
        /// Récupére le pointage des personnels
        /// </summary>
        /// <param name="filter">option de sélection</param>
        /// <returns>Liste des lignes des rapports</returns>
        List<RapportLigneSelectModel> GetPointagePersonnelByFilter(ExportPointagePersonnelFilterModel filter);

        /// <summary>
        /// Sauvegarde les modifications apportée à un rapport ligne
        /// </summary>
        /// <param name="rapportLigne">Rapport ligne à modifier</param>
        Task UpdateRangeRapportLigneAsync(IEnumerable<RapportLigneEnt> rapportLigne);

        /// <summary>
        /// Modifie l'identifiant du contrat interimaire d'une liste de rappoer ligne
        /// </summary>
        /// <param name="rapportLignesIds">Liste des identifiants rapport ligne</param>
        /// <param name="contratInterimaireId">Identifiant contrat interimaire</param>
        void UpdateContratId(List<int> rapportLignesIds, int contratInterimaireId);


        /// <summary>
        /// Récupére une liste de rapport ligne parune liste d'ID
        /// </summary>
        /// <param name="idsList">La liste des ids</param>
        /// <returns></returns>
        IEnumerable<RapportLigneEnt> GetRapportLignesByIds(List<int> idsList);

        /// <summary>
        /// Retourne une liste de pointages réels en fonction du List du ci et d'une date
        /// </summary>
        /// <param name="ciIds">List Ci ID</param>
        /// <param name="periode">La periode</param>
        /// <returns>Une liste de pointages réels</returns>
        IEnumerable<RapportLigneEnt> GetListPointagesReelsByCiIdListFromPeriode(List<int> ciIds, DateTime periode);

        /// <summary>
        /// Get pointage By Personnel And Ci And Date Astreinte
        /// </summary>
        /// <param name="personnelId"></param>
        /// <param name="ciId"></param>
        /// <param name="dateAstreinte"></param>
        /// <returns></returns>
        RapportLigneEnt GetpointageByPersonnelAndCiAndDateAstreinte(int personnelId, int ciId, DateTime dateAstreinte);

        List<RapportLigneEnt> GetRapportLignesToUpdateValorisationFromListBaremeStorm(List<int> ressourcesIds, List<int> ciIds, List<CiDernierePeriodeComptableNonCloturee> ciDernierePeriodeComptableCloturees);
        RapportLigneEnt GetPointageByPersonnelIdAndDatePointage(int personnelId, int ciId, DateTime date);
    }
}
