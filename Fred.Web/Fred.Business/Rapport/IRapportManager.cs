using System;
using System.Collections.Generic;
using Fred.Business.Personnel;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport
{
    /// <summary>
    ///   Interface des gestionnaires des rapports
    /// </summary>
    public interface IRapportManager : IManager<RapportEnt>,
                                       ISearchFeature,
                                       ICrudFeature
    {
        /// <summary>
        ///   Teste un pointage quel que soit son type (personnel et/ou materiel)
        /// </summary>
        /// <param name="rapport">Le pointage que l'on vient de vérifier</param>
        /// <returns>Retourne le rapport </returns>
        RapportEnt CheckRapport(RapportEnt rapport);

        /// <summary>
        ///   Liste les identifiants de rapport en erreur
        /// </summary>
        /// <param name="rapportIds">Liste des identifiants des rapports à vérifier</param>
        /// <returns>Retourne la liste des identifiants de rapport en erreur</returns>
        List<int> GetListRapportIdWithError(List<int> rapportIds);

        /// <summary>
        /// Retourne Vrai si la date du chantier est dans une période clôturée
        /// </summary>
        /// <param name="rapport">Rapoort de chantier</param>
        /// <returns>Vrai si la date du chantier est dans une période clôturée</returns>
        bool IsDateChantierInPeriodeCloture(RapportEnt rapport);

        /// <summary>
        /// Retourne la liste des rapports
        /// </summary>
        /// <returns>La liste des rapports</returns>
        IEnumerable<object> GetRapportListAllSync();

        /// <summary>
        /// Permet d'extraire une liste de rapports pour l'exportation
        /// </summary>
        /// <param name="filterRapport">filtres des rapports</param>
        /// <returns>La liste des rapports</returns>
        IEnumerable<RapportEnt> GetRapportsExportApi(FilterRapportFesExport filterRapport);

        /// <summary>
        /// Méthode d'initisalition des informations des sorties astreintes pour un rapport
        /// </summary>
        /// <param name="rapport">Le rapport à modifier</param>
        void InitializeAstreintesInformations(RapportEnt rapport);

        /// <summary>
        /// Check rapport for societe FES
        /// </summary>
        /// <param name="ciId">Ci Identifier</param>
        /// <param name="dateChantier">Date du chantier</param>
        /// <returns>True if Rapport exist </returns>
        int CheckRepportsForFES(int ciId, DateTime dateChantier);

        /// <summary>
        /// Retourne liste de rapport ligne par Ci après une date de cloture étant vérouiller et contenant des intérimaires
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <param name="statut">Statut du ci</param>
        /// <returns>Liste de rapport ligne</returns>
        IEnumerable<RapportLigneEnt> GetRapportLigneVerrouillerByCiIdForReceptionInterimaire(int ciId, int statut);

        /// <summary>
        /// Retourne liste de rapport ligne par Ci après une date de cloture étant vérouiller et contenant des Materiels externe
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <param name="statut">Statut du ci</param>
        /// <returns>Liste de rapport ligne</returns>
        IEnumerable<RapportLigneEnt> GetRapportLigneVerrouillerByCiIdForReceptionMaterielExterne(int ciId, int statut);

        /// <summary>
        /// Retourne un rapport avec son CIid
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport</param>
        /// <returns>Rapport</returns>
        /// <remarks>GROS FIX EN URGENCE !</remarks>
        RapportEnt GetRapportCi(int rapportId);

        /// <summary>
        /// Retourne la liste des personnels auteur de rapport
        /// </summary>
        /// <param name="search">Object de recherche</param>
        /// <param name="groupeId">Groupe de l'utilisateur courant</param>
        /// <param name="listOrga">Liste des organisations disponible pour l'utilisateur connecté</param>
        /// <returns>La liste des personnels auteur de rapport</returns>
        IEnumerable<PersonnelEnt> SearchRapportAuthor(SearchLightPersonnelModel search, int? groupeId, IEnumerable<int> listOrga);

        /// <summary>
        /// Renvoie la liste des rapport entre 2 dates . Les rapports retournés conçernent les ci envoyés
        /// </summary>
        /// <param name="ciList">Liste des Cis</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>List de rapports en les 2 dates</returns>
        IEnumerable<RapportEnt> GetRapportListWithRapportLignesBetweenDatesByCiList(IEnumerable<int> ciList, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Retourne une liste de pointages réels en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <param name="personnelId">personnelId</param>
        /// <returns>Une liste de pointages </returns>
        RapportEnt GetRapportsByPersonnelIdAndDatePointagesFiggo(int ciId, DateTime datePointage, int personnelId, int typePersonnel);


        /// <summary>
        /// Retourne  un pointage réels en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <returns>Une liste de pointages </returns>
        RapportEnt GetRapportsByCilIdAndDatePointagesFiggo(int ciId, DateTime datePointage, int typePersonnel);


        /// <summary>
        /// Récupere une liste de rapportLigne définit par sa date et un personnelId et un ciId
        /// </summary>
        /// <param name="date">date du pointage</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="ciId">identifiant du ci</param>
        /// <returns>List des rapportLigne</returns>
        List<RapportLigneEnt> GetRapportLigneByDateAndPersonnelAndCi(DateTime date, int personnelId, int ciId);

        /// <summary>
        /// Retourne une liste de pointages réels en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <param name="personnelId">identifiant du persoonel</param>
        /// <returns>Une liste de pointages </returns>
        List<RapportEnt> GetAllRapportsByPersonnelIdAndDatePointagesFiggo(List<int> ciId, DateTime datePointage, int personnelId, int typePersonnel);

        /// <summary>
        /// Récupere une liste de rapportLigne définit par sa date et un personnelId
        /// </summary>
        /// <param name="ciId">identifiant du ci</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="date">date d'absence</param>
        /// <returns>List des rapportLigne</returns>
        List<RapportLigneEnt> GetRapportLigneByCiAndPersonnels(int ciId, int personnelId, DateTime date);

    }
}
