using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Moyen;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Web.Models.PointagePersonnel;
using Fred.Web.Shared.Models.EtatPaie;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel;

namespace Fred.Business.Rapport.Pointage
{
    /// <summary>
    ///   Interface des gestionnaires des pointages
    /// </summary>
    public interface IPointageManager : IManager<RapportLigneEnt>
    {
        /// <summary>
        /// Récupère un RapportLigne en fonction de son Identifiant
        /// </summary>
        /// <param name="rapportLigneId">Identifiant du RapportLigneId</param>
        /// <param name="includes">Includes lors du get</param>
        /// <returns>Un pointage</returns>
        RapportLigneEnt Get(int rapportLigneId, List<Expression<Func<RapportLigneEnt, object>>> includes);

        /// <summary>
        /// Récupérer la liste des rapports ligne.
        /// </summary>
        /// <returns>La liste des pointages</returns>
        IEnumerable<RapportLigneEnt> GetRapportLigneAllSync();

        /// <summary>
        /// Récupérer la liste des rapport ligne prime.
        /// </summary>
        /// <returns>La liste des rapports ligne prime</returns>
        IEnumerable<RapportLignePrimeEnt> GetRapportLignePrimeAllSync();

        /// <summary>
        /// Récupérer la liste des rapport ligne tache.
        /// </summary>
        /// <returns>La liste des rapports ligne tache</returns>
        IEnumerable<RapportLigneTacheEnt> GetRapportLigneTacheAllSync();

        /// <summary>
        /// Récupérer la liste des rapport statut.
        /// </summary>
        /// <returns>La liste des rapports statut</returns>
        IEnumerable<RapportStatutEnt> GetRapportStatutAllSync();

        /// <summary>
        /// Récupérer la liste des rapport tache.
        /// </summary>
        /// <returns>La liste des rapports tache.</returns>
        IEnumerable<RapportTacheEnt> GetRapportTacheAllSync();

        /// <summary>
        ///   Récupère la liste des pointages correspondant aux critères de recherche
        /// </summary>
        /// <param name="searchRapportLigne">Critère de recherche</param>
        /// <param name="applyReadOnly">Variable permettant d'appliquer les règles de gestion au pointage</param>
        /// <param name="isHebdomadaire">si le cas est Hebdomadaire</param>
        /// <returns>IEnumerable contenant la liste des pointages correspondants aux critères</returns>
        IEnumerable<RapportLigneEnt> SearchPointageReelWithFilter(SearchRapportLigneEnt searchRapportLigne, bool applyReadOnly, bool isHebdomadaire = false);

        /// <summary>
        ///   Récupère la liste des pointages correspondant aux critères de recherche
        /// </summary>
        /// <param name="searchRapportLigne">Critère de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>IEnumerable contenant la liste des pointages correspondants aux critères</returns>
        IEnumerable<RapportLigneEnt> SearchPointageReelWithFilterByPage(SearchRapportLigneMoyenEnt searchRapportLigne, int page, int pageSize);

        /// <summary>
        ///   Récupère la liste des pointages correspondant aux critères de recherche
        /// </summary>
        /// <param name="searchRapportLigne">Critère de recherche</param>
        /// <returns>IEnumerable contenant la liste des pointages correspondants aux critères</returns>
        List<RapportLigneEnt> SearchPointageReelWithMoyenFilter(SearchRapportLigneMoyenEnt searchRapportLigne);
        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de pointage mensuel.
        /// </summary>
        /// <returns>Nouvelle instance de pointage mensuel intialisée</returns>
        PointageMensuelEnt GetNewPointageMensuel();

        /// <summary>
        ///   Récupère la liste des filtre pour la recherche des pointages
        /// </summary>
        /// <returns>Retourne un objet représentant les filtres</returns>
        SearchRapportLigneEnt GetFiltersList();

        /// <summary>
        ///   Crée une nouvelle ligne de rapport vide (version light)
        /// </summary>
        /// <returns>RapportLigneEnt vide</returns>
        RapportLigneEnt GetNewPointageReelLight();

        /// <summary>
        ///   Ajoute une tache au paramétrage du rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="pointageReel">Le rapport</param>
        /// <param name="tache">La tache</param>
        /// <returns>Un rapport</returns>
        RapportLigneEnt AddTacheToPointageReel(RapportLigneEnt pointageReel, TacheEnt tache);

        /// <summary>
        ///   Créer une ligne de tache vide
        /// </summary>
        /// <param name="pointageReel">La ligne du rapport</param>
        /// <param name="tache">La tache</param>
        /// <returns>Une tache correspondant à une ligne de rapport</returns>
        RapportLigneTacheEnt GetNewPointageReelTache(RapportLigneEnt pointageReel, TacheEnt tache);

        /// <summary>
        ///   Ajoute une prime au paramétrage du rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="pointage">Le pointage</param>
        /// <param name="prime">La prime</param>
        /// <returns>Un pointage</returns>
        RapportLigneEnt AddPrimeToPointage(RapportLigneEnt pointage, PrimeEnt prime);

        /// <summary>
        ///   Créer une ligne de rapport vide
        /// </summary>
        /// <param name="pointage">La ligne du rapport</param>
        /// <param name="prime">La prime</param>
        /// <returns>Une ligne de rapport</returns>
        PointagePrimeBase GetNewPointagePrime(RapportLigneEnt pointage, PrimeEnt prime);

        /// <summary>
        ///   Vérification du type de ligne de rapport saisi. false : personnel, null : personnel + matériel, true : matériel
        /// </summary>
        /// <param name="pointageReel">Pointage pour lequel effectuer le traitement</param>
        void SetPointageReelType(RapportLigneEnt pointageReel);

        /// <summary>
        ///   Teste un pointage quel que soit son type (personnel et/ou materiel)
        /// </summary>
        /// <param name="pointage">Le pointage que l'on vient de vérifier</param>
        /// <returns>Le pointage</returns>
        RapportLigneEnt CheckPointage(RapportLigneEnt pointage);

        /// <summary>
        ///   Teste une liste de pointage quel que soit son type (personnel et/ou materiel)
        /// </summary>
        /// <param name="lstPointages">La liste de pointage à tester</param>
        /// <returns>Une liste de pointages</returns>
        IEnumerable<RapportLigneEnt> CheckListPointages(IEnumerable<RapportLigneEnt> lstPointages);

        /// <summary>
        ///   Teste une liste de pointage quel que soit son type (uniquement materiel)
        /// </summary>
        /// <param name="lstPointages">La liste de pointage à tester</param>
        /// <returns>Une liste de pointages</returns>
        IEnumerable<RapportLigneEnt> CheckListPointagesMaterielOnly(IEnumerable<RapportLigneEnt> lstPointages);

        /// <summary>
        ///   Traite les données d'une ligne de rapport
        /// </summary>
        /// <param name="pointage">La ligne de raport</param>
        void TraiteDonneesPointage(RapportLigneEnt pointage);

        /// <summary>
        ///   Traite l'état d'un pointage dans le context Entity Framework
        /// </summary>
        /// <param name="pointage">La ligne de rapport</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="codeGroupe">Code du groupe</param>
        void TraiteEtatPointage(RapportLigneEnt pointage, int utilisateurId, string codeGroupe);

        /// <summary>
        /// Modifie l'identifiant du contrat interimaire d'une liste de rappoer ligne
        /// </summary>
        /// <param name="rapportLignesIds">Liste des identifiants rapport ligne</param>
        /// <param name="contratInterimaireId">Identifiant contrat interimaire</param>
        void UpdateContratId(List<int> rapportLignesIds, int contratInterimaireId);

        /// <summary>
        ///   Ajoute une ligne de rapport
        /// </summary>
        /// <param name="pointage">La base de pointage</param>
        void AddPointage(RapportLigneEnt pointage);

        /// <summary>
        ///   Ajoute une ligne de rapport avec enregistrement
        /// </summary>
        /// <param name="pointage">La base de pointage</param>
        void AddPointageWithSave(RapportLigneEnt pointage);

        /// <summary>
        ///   Met à jour une ligne de rapport
        /// </summary>
        /// <param name="pointage">pointage réel à mettre à jour</param>
        void UpdatePointage(RapportLigneEnt pointage);

        /// <summary>
        ///   Met à jour une ligne de rapport pour la réception interimaire
        /// </summary>
        /// <param name="pointage">pointage réel à mettre à jour</param>
        /// <param name="utilisateurIdFredIE">identifiant unique utilisateur fred IE</param>
        void UpdatePointageForReceptionInterimaire(RapportLigneEnt pointage, int utilisateurIdFredIE);

        /// <summary>
        ///   Met à jour une ligne de rapport pour la réception materiel externe
        /// </summary>
        /// <param name="pointage">pointage réel à mettre à jour</param>
        void UpdatePointageForReceptionMaterielExterne(RapportLigneEnt pointage);

        /// <summary>
        ///   Supprime une ligne de rapport
        /// </summary>
        /// <param name="pointage">La ligne de rapport à supprimer</param>
        /// <param name="saveUow">Enrgistrer l'unit of work ou pas</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        void DeletePointage(RapportLigneEnt pointage, bool saveUow = true, int? utilisateurId = null);

        /// <summary>
        ///   Supprime une ligne de rapport
        /// </summary>
        /// <param name="pointageList">Les lignes de rapport à supprimer</param>
        /// <param name="saveUow">Enrgistrer l'unit of work ou pas</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        void DeletePointageList(IEnumerable<RapportLigneEnt> pointageList, bool saveUow = true, int? utilisateurId = null);

        /// <summary>
        ///   Applique les règles de gestion au pointage REEL
        /// </summary>
        /// <param name="pointage">Un pointage</param>
        /// <param name="domaine">Le domaine qui est impacté</param>
        /// <returns>     Un pointage reel    </returns>
        RapportLigneEnt ApplyValuesRGPointageReel(RapportLigneEnt pointage, string domaine);

        /// <summary>
        ///   Applique les règles de gestion au pointage
        /// </summary>
        /// <param name="pointage">Un pointage à traiter</param>
        /// <returns>Un pointage</returns>
        RapportLigneEnt ApplyReadOnlyRGPointageReel(RapportLigneEnt pointage);

        /// <summary>
        ///   Retourne la liste ligne de rapport par user/mois/annee/organisation d'un rapport.
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="organisationPere">organisation Pere</param>
        /// <returns>Renvoie la liste des lignes de rapport.</returns>
        IEnumerable<PointageBase> GetListePointageMensuel(EtatPaieExportModel etatPaieExportModel, int? organisationPere = null);

        /// <summary>
        ///   Retourne la liste ligne de rapport par user/mois/annee/organisation d'un rapport(fes).
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="organisationPere">Identifiant de l'organisation pére</param>
        /// <returns>Renvoie la liste des lignes de rapport.</returns>
        IEnumerable<RapportLigneEnt> GetListePointageMensuelfes(EtatPaieExportModel etatPaieExportModel, int? organisationPere = null);

        /// <summary>
        ///   Retourne la liste ligne de rapport par user/jour/mois/annee/organisation d'un rapport.
        /// </summary>
        /// <param name="year">annee du rapport </param>
        /// <param name="month">mois du rapport </param>
        /// <param name="day">jour du rapport</param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Renvoie la liste des lignes de rapport.</returns>
        IEnumerable<RapportLigneEnt> GetListePointageHebdomadaire(int year, int month, int day, EtatPaieExportModel etatPaieExportModel);

        /// <summary>
        ///   Applique les règles de gestion au pointage
        /// </summary>
        /// <param name="pointage">Un pointage</param>
        /// <returns>   Un pointage de base  </returns>
        PointageBase ApplyReadOnlyRGPointageBase(PointageBase pointage);


        /// <summary>
        ///   Retourne les informations de pointage d'un personnel en fonction de ce personnel et d'une période
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="periode">La période du pointage</param>
        /// <returns>Les informations de pointage du personnel</returns>
        Task<PointagePersonnelInfo> GetListPointagesByPersonnelIdAndPeriodeAsync(int personnelId, DateTime periode);

        /// <summary>
        ///   Génération des samedi en CP
        /// </summary>
        /// <param name="pointage">pointage</param>
        void GenerationPointageSamediCP(RapportLigneEnt pointage);

        /// <summary>
        /// Indique si le pointage est un samedi en congés payé.
        /// </summary>
        /// <param name="pointage">Le pointage concerné.</param>
        /// <returns>True si le pointage est un samedi en congés payé, sinon false.</returns>
        bool IsSamediCP(RapportLigneEnt pointage);

        /// <summary>
        /// Duplique un pointage
        /// </summary>
        /// <param name="rapportLigneId">ID du rapportLigne</param>
        /// <param name="ciId">ciId</param>
        /// <param name="startDate">startDate</param>
        /// <param name="endDate">endDate</param>
        /// <returns>DuplicatePointageResult</returns>
        DuplicatePointageResult DuplicatePointage(int rapportLigneId, int ciId, DateTime startDate, DateTime endDate);

        /// <summary>
        ///   Retourne un pointage avec indemnité de déplacement selon le CI et le personnel associé
        /// </summary>
        /// <param name="pointage">Ligne d'un rapport</param>
        /// <returns>
        ///   Une Ligne d'un rapport
        /// </returns>
        RapportLigneEnt GetOrCreateIndemniteDeplacementForRapportLigne(RapportLigneEnt pointage);

        /// <summary>
        ///   Retourne un pointage avec indemnité de déplacement selon le CI et le personnel associé
        /// </summary>
        /// <param name="pointage">Ligne d'un rapport</param>
        /// <param name="warnings">Les avertissements. Peut-être null.</param>
        /// <param name="refresh">Indique s'il s'agit d'un rafraichissement.</param>
        /// <returns>Ligne d'un rapport avec indemnité déplacement</returns>
        RapportLigneEnt GetOrCreateIndemniteDeplacementForRapportLigne(RapportLigneEnt pointage, out List<string> warnings, bool refresh);

        /// <summary>
        /// Indique s'il est possible de calculer les indemnités de déplacement.
        /// </summary>
        /// <param name="pointage">Le pointage concerné.</param>
        /// <param name="errors">Les erreurs qui font que les indemnités de déplacement ne peuvent pas être calculées.</param>
        /// <returns>True s'il est possible de faire le calcul, sinon false.</returns>
        bool CanCalculateIndemniteDeplacement(RapportLigneEnt pointage, out List<string> errors);

        /// <summary>
        ///   Compte le nombre de lignes de pointages d'un rapport
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport</param>
        /// <returns>Nombre de pointages du rapport</returns>
        int CountPointage(int rapportId);

        /// <summary>
        ///   Récupère la liste des pointages, dans le périmètre de l'utilisateur connecté, des rapports verrouillés d'une période donnée
        /// </summary>
        /// <param name="periode">Période choisie</param>
        /// <returns>Liste de pointages</returns>
        IEnumerable<RapportLigneEnt> GetAllLockedPointages(DateTime periode);

        /// <summary>
        ///   Récupère la liste des pointages, dans le périmètre de l'utilisateur connecté, des rapports verrouillés d'une période donnée
        /// </summary>
        /// <param name="periode">Période choisie</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="etablissementPaieIdList">Liste des indentifiants d'établissements de paie</param>
        /// <returns>Liste de pointages</returns>
        IEnumerable<RapportLigneEnt> GetAllLockedPointagesForSocieteOrEtablissement(DateTime periode, int? societeId, IEnumerable<int> etablissementPaieIdList);

        /// <summary>
        ///   Récupère la liste des pointages, dans le périmètre de l'utilisateur connecté, des rapports verrouillés d'une période donnée
        /// </summary>
        /// <param name="periode">Période choisie</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="etablissementPaieIdList">Liste des indentifiants d'établissements de paie</param>
        /// <returns>Liste de pointages</returns>
        IEnumerable<RapportLigneEnt> GetAllLockedPointagesForSocieteOrEtablissementForVerificationCiSep(DateTime periode, int? societeId, IEnumerable<int> etablissementPaieIdList);

        /// <summary>
        ///   Récupération des pointages dans le périmètre de l'utilisateur connecté (Minimum habilité Gestionnaire)
        /// </summary>
        /// <param name="userId">Identifiant de l'Utilisateur</param>
        /// <param name="period">Période choisie</param>
        /// <returns>Liste de pointages</returns>
        Task<IEnumerable<RapportLigneEnt>> GetPointagesAsync(DateTime period);

        /// <summary>
        /// Récupère la tache par défaut d'un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne la tache par défaut pour un CI donné</returns>
        TacheEnt GetTacheParDefaut(int ciId);

        /// <summary>
        /// Intialise un nouveau pointage avec la tache par défaut du Ci
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>Retourne le pointage initialisé</returns>
        RapportLigneEnt InitTacheParDefautPointagePersonnel(int ciId, int personnelId);

        /// <summary>
        ///   Création le fichier Excel ou Pdf contenant les pointages d'un personnel pour une période donnée (au format byte[])
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="typeExport">Type d'export (Excel ou Pdf)</param>
        /// <param name="isFes">Indique si l'édition est à destination de FES</param>
        /// <returns>Fichier d'export au format byte[]</returns>
        Task<byte[]> GetPointagePersonnelExportAsync(int personnelId, DateTime periode, int typeExport, bool isFes, string templateFolderPath);


        /// <summary>
        ///   Création le fichier Excel ou Pdf contenant les pointages d'un personnel pour une période donnée (au format byte[])
        /// </summary>
        /// <param name="pointagePersonnelExportModel">pointage export</param>
        /// <returns>Fichier d'export au format byte[]</returns>
        byte[] GetPointageInterimaireExport(PointagePersonnelExportModel pointagePersonnelExportModel);

        /// <summary>
        ///   Création du nom de fichier d'export des pointages
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="periode">Période choisie</param>
        /// <returns>Nom du fichier</returns>
        string GetPointagePersonnelExportFilename(int personnelId, DateTime periode);

        /// <summary>
        ///   Création du nom de fichier d'export des pointages
        /// </summary>
        /// <param name="dateDebut">Date début</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>Nom du fichier</returns>
        string GetPointageInterimaireExportFilename(DateTime dateDebut, DateTime dateFin);

        /// <summary>
        ///   Insère une entité de liaison RapportLignePrimeEnt dans le contexte
        /// </summary>
        /// <param name="rapportLignePrime">entité à insére</param>
        void InsertRapportLignePrime(RapportLignePrimeEnt rapportLignePrime);

        /// <summary>
        ///   Insère une entité de liaison RapportLigneAstreinteEnt dans le contexte
        /// </summary>
        /// <param name="rapportLigneAstreinte">entité à insére</param>
        void InsertRapportLigneAstreinte(RapportLigneAstreinteEnt rapportLigneAstreinte);

        /// <summary>
        ///   Update une entité de liaison RapportLigneAstreinteEnt dans le contexte
        /// </summary>
        /// <param name="rapportLigneAstreinte">entité à insére</param>
        void UpdateRapportLigneAstreinte(RapportLigneAstreinteEnt rapportLigneAstreinte);

        /// <summary>
        ///   Insère une entité de liaison RapportLigneMajorationEnt dans le contexte
        /// </summary>
        /// <param name="rapportLigneMajoration">entité à insére</param>
        void InsertRapportLigneMajoration(RapportLigneMajorationEnt rapportLigneMajoration);

        /// <summary>
        ///   Insère une entité de liaison RapportLignePrimeEnt dans le contexte
        /// </summary>
        /// <param name="rapportLigneTache">entité à insére</param>
        void InsertRapportLigneTache(RapportLigneTacheEnt rapportLigneTache);

        /// <summary>
        ///   Supprime une entité de liaison RapportLignePrimeEnt dans le contexte
        /// </summary>
        /// <param name="rapportLignePrime">entité à insére</param>
        void DeleteRapportLignePrime(RapportLignePrimeEnt rapportLignePrime);

        /// <summary>
        ///   Supprime une entité de liaison RapportLignePrimeEnt dans le contexte
        /// </summary>
        /// <param name="rapportLigneTache">entité à insére</param>
        void DeleteRapportLigneTache(RapportLigneTacheEnt rapportLigneTache);

        /// <summary>
        /// Permet de récupérer toutes les lignes de pointage d'un rapport
        /// associétés à un materiel STORM.
        /// </summary>
        /// <param name="rapportId">L'identifiant du rapport</param>
        /// <returns>Une liste de pointages.</returns>
        List<RapportLigneEnt> GetAllPointagesForMaterielStorm(int rapportId);

        /// <summary>
        /// Permet de récupérer toutes les lignes de pointage d'un rapport
        /// associétés à un personnel .
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>Une liste de rapport lignes</returns>
        List<RapportLigneEnt> GetPointagesPersonnelsForSap(int rapportId);

        /// <summary>
        /// Supprimer la liste des lignes de rapport sauf celle spécifié
        /// </summary>
        /// <param name="rapportId">L'identifiant du rapport </param>
        /// <param name="rapportLigneIdList">Les identifiants des rapports ligne à garder</param>
        /// <returns>Liste des RapportLigne qui n'existe pas dans la liste rapportLigneIdList</returns>
        IEnumerable<RapportLigneEnt> GetOtherRapportLignes(int rapportId, List<int> rapportLigneIdList);

        /// <summary>
        /// Get personnel summary 
        /// </summary>
        /// <param name="personnelIdList">Personnel id list</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>IEnumerable of Personnel rapport summary</returns>
        IEnumerable<PersonnelRapportSummaryEnt> GetPersonnelPointageSummary(List<int> personnelIdList, DateTime mondayDate);

        /// <summary>
        /// Get Ci pointage summary
        /// </summary>
        /// <param name="ciIdList">Ci id list</param>
        /// <param name="personnelStatut">Personnel statut</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>Enumerable de CiPointage Summary</returns>
        IEnumerable<CiPointageSummaryEnt> GetCiPointageSummary(List<int> ciIdList, string personnelStatut, DateTime mondayDate);

        /// <summary>
        ///   Créer une ligne de majoration vide
        /// </summary>
        /// <param name="pointageReel">La ligne du rapport</param>
        /// <param name="majoration">La majoration</param>
        /// <returns>Une majoration correspondant à une ligne de rapport</returns>
        RapportLigneMajorationEnt GetNewPointageReelMajoration(RapportLigneEnt pointageReel, CodeMajorationEnt majoration);

        /// <summary>
        /// Get total hours work and absence and majoration for validation
        /// </summary>
        /// <param name="personnelId">Personneel identifier</param>
        /// <param name="datePointage">Date du chantier</param>
        /// <returns>Total des heures</returns>
        double GetTotalHoursWorkAndAbsenceWithMajoration(int personnelId, DateTime datePointage);

        /// <summary>
        ///   Retourne le nombre de pointage sur un contrat interimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Interimaire</param>
        /// <returns>Un nombre de pointage</returns>
        int GetPointageForContratInterimaire(ContratInterimaireEnt contratInterimaireEnt);

        /// <summary>
        ///   Création le fichier Excel ou Pdf contenant les pointages d'un personnel pour une période donnée (au format byte[])
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <returns>Fichier d'export au format byte[]</returns>
        byte[] GetPointagePersonnelHebdomadaireExport(PointagePersonnelExportModel pointagePersonnelExportModel);

        /// <summary>
        ///   Création du nom de fichier d'export des pointages
        /// </summary>
        /// <param name="dateComptable">Date comptable</param>
        /// <param name="dateDebut">Date début</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>Nom du fichier</returns>
        string GetPointagePersonnelHebdomadaireExportFilename(DateTime dateComptable, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Get list des primes affected to list of personnel
        /// </summary>
        /// <param name="primePersonnelModel">prime Personnel get model</param>
        /// <returns>List des primes affected</returns>
        List<PrimePersonnelAffectationEnt> PrimePersonnelAffected(PrimesPersonnelsGetEnt primePersonnelModel);

        /// <summary>
        ///   Retourne les pointages vérouiller par rapport à personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique d'un personnel</param>
        /// <returns>Liste de pointage vérouiller</returns>
        IEnumerable<RapportLigneEnt> GetPointageVerrouillerByPersonnelId(int personnelId);

        /// <summary>
        /// Check if rapport ligne existance
        /// </summary>
        /// <param name="rapportId">Rapport identifier</param>
        /// <param name="personnelId">Personnel Identifier</param>
        /// <returns>RapportLigne Identifier if exist</returns>
        int CheckRapportLigneExistance(int rapportId, int personnelId);

        /// <summary>
        /// Get prime
        /// </summary>
        /// <param name="rapportLigneId">Rapport ligne identifier</param>
        /// <param name="primeId">Prime identifier</param>
        /// <returns>Rapport Ligne prime</returns>
        RapportLignePrimeEnt FindPrime(int rapportLigneId, int primeId);

        /// <summary>
        /// Verifier si le rapport comporte des pointages
        /// </summary>
        /// <param name="listRapportLigne">listes des rapports ligne</param>
        /// <returns>le rapport a enregistré</returns>
        List<RapportLigneEnt> CheckPointageForSave(IEnumerable<RapportLigneEnt> listRapportLigne);

        /// <summary>
        /// Ajout ou mise à jour en masse
        /// </summary>
        /// <param name="rapportLignes">Liste de lignes de rapport</param>
        void AddOrUpdateRapportLigneList(IEnumerable<RapportLigneEnt> rapportLignes);

        /// <summary>
        /// Récupére les rapport lignes et determine si le personnel a des pointages
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="ciId">identifiant de l'affaire</param>
        /// <returns>true or false</returns>
        bool GetpointageByPersonnelAndCi(int personnelId, int ciId);

        /// <summary>
        /// Vérifie si le personnel a des pointages dans une semaines
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="ciId">identifiant de l'affaire</param>
        /// <param name="mondayDate">premier jour de la semaine</param>
        /// <returns>true or false</returns>
        bool CheckPointageByPersonnelAndCi(int personnelId, int ciId, DateTime mondayDate);

        /// <summary>
        /// Checks the rapport ligne before save.
        /// </summary>
        /// <param name="rapportLigne">The rapport ligne.</param>
        /// <returns>Boolean</returns>
        bool CheckRapportLigneBeforeSave(RapportLigneEnt rapportLigne);

        /// <summary>
        /// récupére les pointages des personnels passé en paramétre
        /// </summary>
        /// <param name="personnelModel">model contenant la liste des personnels et la date </param>
        /// <returns>liste des personnels avec leur pointages de la semaine</returns>
        List<RapportHebdoPersonnelWithTotalHourEnt> GetPointageByPersonnelIDAndInterval(RapportHebdoPersonnelWithAllCiEnt personnelModel);

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
        /// Récupére les rapport lignes et determine si le personnel a des pointages pour le rendre readonly
        /// </summary>
        /// <param name="ciId">identifiant de l'affaire</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>true or false</returns>
        bool GetpointageByPersonnelAndCiForReadOnly(int ciId, int personnelId);


        /// <summary>
        /// Récupére les pointages de personnel pour challenge sécurité pour une période donnée (au format byte[])
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <returns>Fichier d'export au format byte[]</returns>
        byte[] GetPointageChallengeSecuriteExport(PointagePersonnelExportModel pointagePersonnelExportModel, string templateFolderPath);

        /// <summary>
        ///   Création du nom de fichier d'export des pointages challenge securite
        /// </summary>
        /// <param name="dateComptable">Date comptable</param>
        /// <param name="dateDebut">Date début</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>Nom du fichier</returns>
        string GetPointageChallengeSecuriteExportFilename(DateTime dateComptable, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Récupére le pointage des moyens entre 2 dates . Le pointage des moyen est reconnu  par la colonne AffectationMoyenId non nulle.
        /// </summary>
        /// <param name="startDate">Date de début de pointage limite</param>
        /// <param name="endDate">Date de fin de pointage limite</param>
        /// <returns>Liste des lignes des rapports</returns>
        ExportTibcoRapportLigneModel[] GetPointageMoyenBetweenDates(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get list des Majorations affected to list of personnel
        /// </summary>
        /// <param name="majorationPersonnelModel">Majoration Personnel get model</param>
        /// <returns>List des primes affected</returns>
        List<MajorationPersonnelAffectationEnt> MajorationPersonnelAffected(MajorationPersonnelsGetEnt majorationPersonnelModel);

        /// <summary>
        /// Retourne Vrai si au moins un pointage est présent pour l'édition
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Vrai si au moins un pointage est présent pour l'édition</returns>
        bool IsExcelControlePointagesNotEmpty(EtatPaieExportModel etatPaieExportModel);

        /// <summary>
        /// Get Rapport ligne by rapport and personnel identifiers
        /// </summary>
        /// <param name="rapportId">Rapport identifier</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>Rapport ligne</returns>
        RapportLigneEnt GetRapportLigneByRapportIdAndPersonnelId(int rapportId, int personnelId);

        /// <summary>
        /// Compareer le statut du rapport ligne a statut donné
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="datePointage">Date Pointage</param>
        /// <param name="statutCode">Statut code a comparer</param>
        /// <returns>True si le rapport statut egal au statut passer en param</returns>
        bool CheckRapportLigneStatut(int ciId, int personnelId, DateTime datePointage, string statutCode);

        /// <summary>
        /// Récuperer les rapports lignes par l'affectation moyen identifier
        /// </summary>
        /// <param name="affectationMoyenIdList">Affectation moyen list des identifiers</param>
        /// <returns>List des rapports lignes</returns>
        IEnumerable<RapportLigneEnt> GetPointageByAffectaionMoyenIds(IEnumerable<int> affectationMoyenIdList);

        /// <summary>
        /// Permet d'inserer un nouveau rapport Ligne
        /// </summary>
        /// <param name="rapportLigne">rapport ligne</param>
        void InsertRapportLigneFiggo(RapportLigneEnt rapportLigne);

        /// <summary>
        /// Get Logs des absence FIGGO importées
        /// </summary>
        /// <param name="dateDebut">Date de debut</param>
        /// <param name="dateFin">Date de fin</param>
        /// <returns>Logs des absences FIGGO importé</returns>
        FiggoLogModel GetLogFiggo(DateTime dateDebut, DateTime dateFin);

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
        /// Met a jour un rapport Ligne
        /// </summary>
        /// <param name="pointage">rapport ligne</param>
        void UpdatePointageForFiggo(RapportLigneEnt pointage);

        /// <summary>
        ///   Créer une ligne de tache vide
        /// </summary>
        /// <param name="pointageReel">La ligne du rapport</param>
        /// <param name="tache">La tache</param>
        /// <param name="heure">heure tache</param>
        /// <returns>Une tache correspondant à une ligne de rapport</returns>
        RapportLigneTacheEnt GetNewPointageTacheFiggo(RapportLigneEnt pointageReel, TacheEnt tache, double heure);

        /// <summary>
        ///  Retourne les pointages interimaires qui n'ont pas encore été réceptionnés
        /// </summary>      
        /// <param name="interimaireId">interimaireId</param>
        /// <param name="dateDebut">dateDebut</param>
        /// <param name="dateFin">dateFin</param>
        /// <returns>Liste d'ids des reception non receptionnees</returns>
        List<RapportLigneEnt> GetPointagesInterimaireNonReceptionnees(int interimaireId, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Récupére le pointage des personnels pour export vers tibco
        /// </summary>
        /// <param name="filter">options</param>
        /// <returns>Model des lignes des rapports au format tibco</returns>
        ExportPointagePersonnelTibcoModel GetPointagePersonnelForTibco(ExportPointagePersonnelFilterModel filter);

        /// <summary>
        /// Permet de controler les saisies pour Tibco
        /// </summary>
        /// <param name="filter">Object de recherche</param>
        /// <returns>liste de model des erreurs</returns>
        IEnumerable<ControleSaisiesTibcoModel> ControleSaisiesForTibco(ExportPointagePersonnelFilterModel filter);

        /// <summary>
        ///  check si l'export analytique a des erreurs
        /// </summary>
        /// <param name="filter">Object de recherche</param>
        /// <returns></returns>
        string CheckExportAnalytiqueErrors(ExportPointagePersonnelFilterModel filter);

        /// <summary>
        /// Récupére une liste de rapport ligne parune liste d'ID
        /// </summary>
        /// <param name="idsList">La liste des ids</param>
        /// <returns></returns>
        IEnumerable<RapportLigneEnt> GetRapportLignesByIds(List<int> idsList);

        void UpdateRapportLigneAndRapportLigneTache(IEnumerable<RapportLigneEnt> rapportsLignes, IEnumerable<RapportLigneTacheEnt> rapportLignesTaches);
    }
}
