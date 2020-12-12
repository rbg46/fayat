using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Rapport;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données des rapports.
    /// </summary>
    public interface IRapportRepository : IRepository<RapportEnt>
    {
        /// <summary>
        ///   Récupère un Rapport en fonction de son Identifiant
        /// </summary>
        /// <param name="rapportId">Identifiant du RapportId</param>
        /// <param name="includes">Includes lors du get</param>
        /// <returns>Un Rapport</returns>
        RapportEnt Get(int rapportId, List<Expression<Func<RapportEnt, object>>> includes);

        /// <summary>
        ///   liste rapport
        /// </summary>
        /// <returns>Liste complète des Rapports.</returns>
        IQueryable<RapportEnt> GetAll();

        /// <summary>
        ///   liste rapport pour la synchronisation mobile.
        /// </summary>
        /// <returns>Liste complète des Rapports.</returns>
        IQueryable<RapportEnt> GetAllSync();

        /// <summary>
        ///   Retourne une liste de rapports
        /// </summary>
        /// <returns>liste rapport</returns>
        IQueryable<RapportEnt> GetLightQuery();

        /// <summary>
        ///   Retourne la liste des rapports
        /// </summary>
        /// <returns>La liste des rapports.</returns>
        IEnumerable<RapportEnt> GetRapportList();

        /// <summary>
        ///   Recherche la liste des rapports correspondants aux prédicats
        /// </summary>
        /// <param name="predicateWhere">expression lambda contenant les critères de recherche</param>
        /// <param name="etablissementPaieIdList">list d'identifiant unique d'établissement de paie</param>
        /// <param name="sortFilter">Chaine de caractère conteneant l'expression de tri</param>
        /// <param name="totalCount">le total de rapports de la requête.</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>IEnumerable contenant les rapports correspondants aux critères de recherche</returns>
        IEnumerable<RapportEnt> SearchRapportWithFilter(Expression<Func<RapportEnt, bool>> predicateWhere, List<int?> etablissementPaieIdList, string sortFilter, out int totalCount, int? page = 1, int? pageSize = 20);

        /// <summary>
        ///   Retourne le rapport portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport à retrouver.</param>
        /// <returns>le rapport retrouvée, sinon nulle.</returns>
        RapportEnt GetRapportById(int rapportId);

        /// <summary>
        ///   Recupère le commentaire d'un rapport pour un tache particulière
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport</param>
        /// <param name="tacheId">Identifiant de la tache</param>
        /// <returns>Retourne le commentaire pour un rapport et une tache</returns>
        string GetCommentairesByRapportIdAndTacheId(int rapportId, int tacheId);

        /// <summary>
        ///   Supprime de ligne de Rapport en fonction de leur existence
        /// </summary>
        /// <param name="rl"> Lignes de Rapports</param>
        void DeleteRapportLigne(RapportLigneEnt rl);

        /// <summary>
        ///   Ajoute une ligne de Prime
        /// </summary>
        /// <param name="rlp"> Lignes de Rapports</param>
        void AddRapportLignePrime(RapportLignePrimeEnt rlp);

        /// <summary>
        ///   Ajoute une ligne de Rapport
        /// </summary>
        /// <param name="rlt"> Lignes de Rapports</param>
        void AddRapportLigneTache(RapportLigneTacheEnt rlt);

        /// <summary>
        ///   Update une ligne de Prime
        /// </summary>
        /// <param name="rlp"> Lignes de Rapports</param>
        void UpdateRapportLignePrime(RapportLignePrimeEnt rlp);

        /// <summary>
        ///   Update une ligne de Rapport
        /// </summary>
        /// <param name="rlt"> Lignes de Rapports</param>
        void UpdateRapportLigneTache(RapportLigneTacheEnt rlt);

        /// <summary>
        ///   Retourne le statut du rapport en fonction du code statut
        /// </summary>
        /// <param name="statutCode">Le code du statut</param>
        /// <returns>Un statut de rapport</returns>
        RapportStatutEnt GetRapportStatutByCode(string statutCode);

        /// <summary>
        ///   Retourne la liste des statuts d'un rapport.
        /// </summary>
        /// <param name="forMobile">Si la liste est destinée au mobile</param>
        /// <returns>Renvoie la liste des statuts d'un rapport.</returns>
        IQueryable<RapportStatutEnt> GetRapportStatutList(bool forMobile);

        /// <summary>
        /// Permet de récupérer une liste de rapport pour l'application mobile
        /// </summary>
        /// <param name="sinceDate">Date de la dernière synchronisation.</param>
        /// <returns>Une liste de rapport</returns>
        IEnumerable<RapportEnt> GetRapportsMobile(DateTime? sinceDate = null);

        /// <summary>
        /// Permet d'extraire une liste de rapports pour l'exportation
        /// </summary>
        /// <param name="filterRapport">filtres des rapports</param>
        /// <returns>liste de rapports</returns>
        IEnumerable<RapportEnt> GetRapportsExportApi(FilterRapportFesExport filterRapport);

        /// <summary>
        /// Permet de récupérer la liste des sorties astreintes associées à une ligne de rapport
        /// </summary>
        /// <param name="rapportLigneId">L'identifiant de la ligne du rapport</param>
        /// <returns>Liste des sorties astreintes</returns>
        IEnumerable<RapportLigneAstreinteEnt> GetRapportLigneAstreintes(int rapportLigneId);

        /// <summary>
        /// Permet de suprimer une liste des sorties astreintes
        /// </summary>
        /// <param name="rapportLigneAstreintes">La liste des sorties astreintes</param>
        void DeleteRapportLigneAstreintes(IEnumerable<RapportLigneAstreinteEnt> rapportLigneAstreintes);

        /// <summary>
        /// Permet de mettre à jour une sortie astreinte associée à une ligne de rapport
        /// </summary>
        /// <param name="rapportLigneAstreinte">La sortie astreinte</param>
        void UpdateRapportLigneAstreinte(RapportLigneAstreinteEnt rapportLigneAstreinte);

        /// <summary>
        /// Ajouter une sortie astreinte
        /// </summary>
        /// <param name="rapportLigneAstreinte">La sortie astreinte</param>
        void AddRapportLigneAstreinte(RapportLigneAstreinteEnt rapportLigneAstreinte);

        /// <summary>
        /// Supprimer la liste des sorties astreintes pour une ligne de rapport
        /// </summary>
        /// <param name="rapportLigneId">L'identifiant de la ligne de rapport</param>
        void DeleteRapportLigneAstreintes(int rapportLigneId);

        /// <summary>
        /// Get list rapport journalier pour le rapport hebdo
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="personnelsIds">List des personnels Id</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>List des rapports</returns>
        List<RapportEnt> GetCiRapportHebdomadaire(int ciId, IEnumerable<int> personnelsIds, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Get list rapport journalier pour le rapport hebdo
        /// </summary>
        /// <param name="ciIds">Ci identifiers</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <param name="dateFin">Date fin</param>
        /// <param name="statut">statut du personnel</param>
        /// <returns>List des rapports</returns>
        Dictionary<int, List<RapportEnt>> GetCiRapportHebdomadaire(IEnumerable<int> ciIds, DateTime dateDebut, DateTime dateFin, int statut);

        /// <summary>
        /// Get list rapport journalier pour le rapport hebdo
        /// </summary>
        /// <param name="ciIds">Ci identifiers</param>
        /// <param name="personnelId">Personnel Identifier</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <param name="dateFin">Date fin</param>
        /// <param name="statut">statut du personnel</param>
        /// <returns>List des rapports</returns>
        Dictionary<int, List<RapportEnt>> GetCiRapportHebdomadaireEmployee(IEnumerable<int> ciIds, int personnelId, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Récuperer la liste des sorties astreintes pour un rapport
        /// </summary>
        /// <param name="rapportId">L'identifiant du rapport</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="ciId">L'identifiant du CI</param>
        /// <param name="datePointage">Date de pointage</param>
        /// <returns>La liste des sorties des astreintes</returns>
        List<RapportLigneAstreinteEnt> GetRapportLigneAstreintes(int rapportId, int personnelId, int ciId, DateTime datePointage);

        /// <summary>
        /// Check rapport pour la societe FES
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="dateChantier">Date de chantier</param>
        /// <returns>List des rapports journaliers</returns>
        RapportEnt CheckRepportsForFES(int ciId, DateTime dateChantier);

        /// <summary>
        /// Permet de suprimer une sortie astreinte
        /// </summary>
        /// <param name="rapportLigneAstreinte">La sortie astreinte</param>
        void DeleteRapportLigneAstreinte(RapportLigneAstreinteEnt rapportLigneAstreinte);

        /// <summary>
        /// Check rapport existant
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="dateChantier">Date chantier</param>
        /// <returns>Rapport identifier</returns>
        int CheckRapportExistance(int ciId, DateTime dateChantier);

        /// <summary>
        /// Retourne les CI des rapports
        /// </summary>
        /// <returns>Requête des CI des rapports</returns>
        /// <remarks>GROSSE METHODE DE FIX POUR LA MEP. A VIRER !!!!!!!!!!!!! </remarks>
        IQueryable<RapportEnt> GetRapportsCis();

        /// <summary>
        /// Retourne liste de rapport ligne par Ci après une date de cloture étant vérouiller et contenant des intérimaires
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <param name="statut">Statut du ci</param>
        /// <returns>Liste de rapport ligne</returns>
        IEnumerable<RapportLigneEnt> GetRapportLigneVerrouillerByCiIdForReceptionInterimaire(int ciId, int statut);

        /// <summary>
        /// Filtre une liste des Identifiant Ci après une date de cloture étant vérouiller et contenant des intérimaires
        /// </summary>
        /// <param name="ciIds">Liste d'Identifiant unique d'un CI</param>
        /// <returns>Liste d'identifiant ci</returns>
        IEnumerable<int> GetCiIdsAvailablesForReceptionInterimaire(IEnumerable<int> ciIds);

        /// <summary>
        /// Retourne liste de rapport ligne par Ci après une date de cloture étant vérouiller et contenant des materiels externe
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <param name="statut">Statut du ci</param>
        /// <returns>Liste de rapport ligne</returns>
        IEnumerable<RapportLigneEnt> GetRapportLigneVerrouillerByCiIdForReceptionMaterielExterne(int ciId, int statut);

        /// <summary>
        /// Récupérer le total des heures normales pointées d'un personnel (travail sans majorations et absences) sur toutes les affaires
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <returns>Nombre des heures</returns>
        double GetTotalHoursWithoutMajorations(int personnelId, DateTime datePointage);

        /// <summary>
        /// Récupérer le total des heures pointées d'un personnel (travail et absences) sur toutes les affaires
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <returns>Nombre des heures</returns>
        double GetTotalHoursWorkAndAbsence(int personnelId, DateTime datePointage);

        /// <summary>
        /// Get total hours work and absence and majoration for validation
        /// </summary>
        /// <param name="personnelId">Personneel identifier</param>
        /// <param name="datePointage">Date du chantier</param>
        /// <returns>Total des heures</returns>
        double GetTotalHoursWorkAndAbsenceWithMajoration(int personnelId, DateTime datePointage);

        /// <summary>
        /// Ajout en masse de rapports
        /// </summary>
        /// <param name="rapportList">La liste des rappports à ajouter</param>
        void AddRangeRapportList(IEnumerable<RapportEnt> rapportList);

        /// <summary>
        /// Renvoie la liste des rapport entre 2 dates . Les rapports retournés conçernent les ci envoyés
        /// </summary>
        /// <param name="ciList">Liste des Cis</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>List de rapports en les 2 dates</returns>
        IEnumerable<RapportEnt> GetRapportListBetweenDatesByCiList(IEnumerable<int> ciList, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Récupération de liste des rapports en fonction d'une liste d'identifiants de rapport
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapport</param>
        /// <returns>Liste de rapport</returns>
        IEnumerable<RapportEnt> GetRapportListWithRapportLignesNoTracking(IEnumerable<int> rapportIds);

        /// <summary>
        /// Get list rapport journalier pour le rapport hebdo
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>List des rapports</returns>
        List<RapportEnt> GetCiRapportHebdomadaire(int ciId, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Retourne un rapport journalier en fonction d'un CiId et d'une date
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="date">Date du chantier</param>
        /// <returns>Un rapport journalier en fonction d'un CiId et d'une date</returns>
        RapportEnt GetRapportByCiIdAndDate(int ciId, DateTime date, int? statutRapport = null);

        /// <summary>
        /// Renvoie la liste des rapport entre 2 dates . Les rapports retournés conçernent les ci envoyés
        /// </summary>
        /// <param name="ciList">Liste des Cis</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>List de rapports en les 2 dates</returns>
        IEnumerable<RapportEnt> GetRapportListWithRapportLignesBetweenDatesByCiList(IEnumerable<int> ciList, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Retourne un rapportEnt en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">liste des ci CI</param>
        /// <param name="date">La date du pointage</param>
        /// <param name="personnelId">personnelId</param>
        /// <returns>rapportEnt </returns>
        RapportEnt GetRapportByPersonnelIdAndDatePointagesFiggo(int ciId, DateTime date, int personnelId, int typePersonnel);

        /// <summary>
        /// Récupere une liste de rapportLigne définit par sa date et un personnelId et un ciId
        /// </summary>
        /// <param name="date">date du pointage</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// /// <param name="ciId">identifiant du ci</param>
        /// <returns>List des rapportLigne</returns>
        List<RapportLigneEnt> GetRapportLigneByDateAndPersonnelAndCi(DateTime date, int personnelId, int ciId);

        /// <summary>
        /// Retourne  un pointage réels en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <returns>Une liste de pointages </returns>
        RapportEnt GetRapportsByCilIdAndDatePointagesFiggo(int ciId, DateTime datePointage, int TypePersonnel);

        /// <summary>
        /// Retourne un rapportEnt en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">liste des ci CI</param>
        /// <param name="date">La date du pointage</param>
        /// <param name="personnelId">personnelId</param>
        /// <returns>rapportEnt </returns>
        List<RapportEnt> GetAllRapportByPersonnelIdAndDatePointagesFiggo(List<int> ciId, DateTime date, int personnelId, int typePersonnel);

        /// <summary>
        /// Récupere une liste de rapportLigne définit par sa date et un personnelId
        /// </summary>
        /// <param name="ciId">identifiant du ci</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// /// <param name="date">date d'absence</param>
        /// <returns>List des rapportLigne</returns>
        List<RapportLigneEnt> GetRapportLigneByCiAndPersonnel(int ciId, int personnelId, DateTime date);

        /// <summary>
        /// Recupere liste des rapports by List des Cis
        /// </summary>
        /// <param name="ciList">List des Cis Ids</param>
        /// <returns></returns>
        List<RapportEnt> GetRapportsListbyCiList(List<int> ciList);

        /// <summary>
        /// Get Rapport ligne list for validation per affaire
        /// </summary>
        /// <param name="ciList">List Ci Ids</param>
        /// <param name="personnelIds">List personnel Ids</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="dateFin">Date Fin</param>
        /// <returns>List des rapports lignes</returns>
        Task<IEnumerable<RapportLigneEnt>> GetRapportsLignesValidationAffairesByResponsableAsync(IEnumerable<int> ciList, IEnumerable<int> personnelIds, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Get Rapport For Verrouillage
        /// </summary>
        /// <param name="rapportIds">list des Id de rapport</param>
        /// <returns>List de rapports</returns>
        List<RapportEnt> GetRapportToLock(IEnumerable<int> rapportIds);

        IReadOnlyList<RapportLigneEnt> GetAllRapportLigneBasedOnPersonnelAffectation(int personelId, DateTime datePointage);
    }
}
