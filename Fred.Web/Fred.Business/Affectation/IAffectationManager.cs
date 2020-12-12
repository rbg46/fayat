using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Utilisateur;
using Fred.Web.Shared.Models.EtatPaie;

namespace Fred.Business.Affectation
{
    /// <summary>
    /// Gestionnaire des affectations
    /// </summary>
    public interface IAffectationManager : IManager<AffectationEnt>
    {
        /// <summary>
        /// Ajouter ou modifier une list des affectations
        /// </summary>
        /// <param name="calendarAffectationViewEnt">Calendar affectation view entity</param>
        void AddOrUpdateAffectationList(CalendarAffectationViewEnt calendarAffectationViewEnt);

        /// <summary>
        /// Get la liste des personnels affectés a un CI
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="dateDebut">Date debut de la semaine</param>
        /// <param name="dateFin">Date fin de la semaine</param>
        /// <returns>Calendar affectation view entity</returns>
        CalendarAffectationViewEnt GetAffectationListByCi(int ciId, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Récuperer la liste des affectation par list  CI
        /// </summary>
        /// <param name="ciIds">L'identifiant du CI</param>
        /// <param name="etablissementPaieIdList">List Etablissement Paie Id </param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>La liste des affectations</returns>
        IEnumerable<PersonnelEnt> GetPersonneListByCiIdList(IEnumerable<int> ciIds, IEnumerable<int?> etablissementPaieIdList, EtatPaieExportModel etatPaieExportModel);

        /// <summary>
        /// Récuperer la liste des affectation par etablissement paie
        /// </summary>
        /// <param name="etablissementPaieIdList">List Etablissement Paie Id </param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>La liste des affectations</returns>
        IEnumerable<int> GetPersonnelIdListAffectedEtablissementList(IEnumerable<int> etablissementPaieIdList, EtatPaieExportModel etatPaieExportModel);

        /// <summary>
        /// Supprimer une liste des affectations 
        /// </summary>
        /// <param name="ciId">Ci Id</param>
        /// <param name="affectationLisIds">liste des affectations identifiers</param>
        void DeleteAffectations(int ciId, List<int> affectationLisIds);

        /// <summary>
        /// Récupérer une astreinte d'un personnel dans un CI et une date précise
        /// </summary>
        /// <param name="ciId">L'idenitifiant du CI</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreinteDate">La date de l'astreinte</param>
        /// <returns>L'astreinte</returns>
        AstreinteEnt GetAstreinte(int ciId, int personnelId, DateTime astreinteDate);

        /// <summary>
        /// Get list des astreintes par personnel et ci
        /// </summary>
        /// <param name="personnelId">personnel identifier</param>
        /// <param name="ciId">ci identifier</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>List des astreintes</returns>
        IEnumerable<AstreinteEnt> GetAstreintesByPersonnelIdAndCiId(int personnelId, int ciId, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Retourne les astreintes pour un personnel donné et qui appartiennent a la fois a un ci de la liste ciIds et a une DateAstreinte de la liste astreinteDates
        /// </summary>
        /// <param name="ciIds">ciIds</param>
        /// <param name="personnelId">personnelId</param>
        /// <param name="astreinteDates">astreinteDates</param>
        /// <returns>Retourne les astreintes pour un personnel donné</returns>
        IEnumerable<AstreinteEnt> GetAstreintes(IEnumerable<int> ciIds, int personnelId, IEnumerable<DateTime> astreinteDates);

        /// <summary>
        ///  Récupérer une astreinte d'un personnel dans un CI et une date précise a partir d'une liste d'astreinte
        /// </summary>
        /// <param name="astreintes">astreintes</param>
        /// <param name="ciId">L'idenitifiant du CI</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreinteDate">La date de l'astreinte</param>
        /// <returns>L'astreinte</returns>
        AstreinteEnt GetAstreinte(IEnumerable<AstreinteEnt> astreintes, int ciId, int personnelId, DateTime astreinteDate);

        /// <summary>
        /// Récupérer l'identifiant d'une astreinte d'un personnel dans un CI et une date précise
        /// </summary>
        /// <param name="ciId">L'idenitifiant du CI</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreinteDate">La date de l'astreinte</param>
        /// <returns>L'astreinte</returns>
        int? GetAstreinteId(int ciId, int personnelId, DateTime astreinteDate);

        /// <summary>
        /// Récuperer la liste des affectation d'un CI
        /// </summary>
        /// <param name="ciId">L'identifiant du CI</param>
        /// <returns>La liste des affectations</returns>
        IEnumerable<AffectationEnt> GetAffectationsByCiId(int ciId);

        /// <summary>
        /// Récupérer la liste des CI dans le quel le personnel est affecté
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>La liste des CI</returns>
        List<CIEnt> GetPersonnelAffectationCiList(int personnelId);

        /// <summary>
        /// Récupérer la liste des CI dans le quel le personnel est affecté
        /// </summary>
        /// <typeparam name="TCI">Le type de CI désiré.</typeparam>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="selector">Selector permettant de constuire un TCI en fonction d'un CIEnt.</param>
        /// <returns>La liste des CI</returns>
        List<TCI> GetPersonnelAffectationCiList<TCI>(int personnelId, Expression<Func<CIEnt, TCI>> selector);

        /// <summary>
        /// Get identifiers of Etam and Iac affected to a CI
        /// </summary>
        /// <param name="ciIdList">List ci Identifier</param>
        /// <returns>List des personnels Ids</returns>
        IEnumerable<int> GetEtamAndIacAffectationListByCiList(IEnumerable<int> ciIdList);

        /// <summary>
        /// Get Personnel id list affected to a CI
        /// </summary>
        /// <param name="ciIdList">List ci Identifier</param>
        /// <returns>List des personnels Ids</returns>
        IEnumerable<int> GetPersonnelsAffectationListByCiList(IEnumerable<int> ciIdList);

        /// <summary>
        /// Obtient le ci par defaut d'un personnel
        /// </summary>
        /// <param name="presonnelId">personnelid</param>
        /// <returns>return Ci </returns>
        CIEnt GetDefaultCi(int presonnelId);

        /// <summary>
        /// Récupération ou création d'une Affectation
        /// </summary>
        /// <param name="personnelId">Id du personnel</param>
        /// <param name="ciId">Id du CI</param>
        /// <param name="isDelegate">Delegation</param>
        /// <returns>Affectation Entité</returns>
        AffectationEnt GetOrCreateAffectation(int personnelId, int ciId, bool isDelegate);

        /// <summary>
        /// Récupération ou New d'une Affectation sans besoin de Delegation (exemple : pour RazelBec)
        /// </summary>
        /// <param name="personnelId">Id du personnel</param>
        /// <param name="ciId">Id du CI</param>
        /// <returns>Affectation Entité</returns>
        Task<AffectationEnt> GetOrNewAffectationAsync(int personnelId, int ciId);

        /// <summary>
        /// Update delegue role affectation
        /// </summary>
        /// <param name="utilisateurId">Utilisateur Id</param>
        /// <param name="listAffectations">List affectations</param>
        void UpdateDelegueRoleAffectation(int utilisateurId, IEnumerable<AffectationSeuilUtilisateurEnt> listAffectations);

        /// <summary>
        /// Vérifier si le personnel a un pointage sur un ci avant de le supprimer
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="ciId">identifiant de l'affaire</param>
        /// <returns>true or false</returns>
        bool CheckPersonnelBeforeDelete(int personnelId, int ciId);

        /// <summary>
        ///  Récuperer l'affectation d'un CI et d'un personnel
        /// </summary>
        /// <param name="ciId">L'identifiant du CI</param>
        /// <param name="personnelId">L'identifiant du Personnel</param>
        /// <returns>affectations</returns>
        AffectationEnt GetAffectationByCiAndPersonnel(int ciId, int personnelId);

        /// <summary>
        /// Récuperer la liste des affectations des personnels passé en paramétres
        /// </summary>
        /// <param name="personneId">Identifiant des personnels</param>
        /// <returns>List d'affectation</returns>
        List<AffectationEnt> GetAffectationByListPersonnelId(List<int> personneId);

        /// <summary>
        /// Get la liste des personnels affectés a un  etablissement de paie par l'organisation id
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>List des affectations</returns>
        IEnumerable<int> GetPersonnelIdAffectedEtablissementByOrganisationId(EtatPaieExportModel etatPaieExportModel);

        /// <summary>
        /// Récupérer la liste des CI dans le quel le personnel est affecté
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>La liste des CI</returns>
        List<CIEnt> GetPersonnelAffectationCiListFiggo(int personnelId);

        /// <summary>
        /// verifier si le personnel a ci par defaut
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>true si le personnel a un ci par defaut</returns>
        bool CheckIfPersonnelHasDefaultCi(int personnelId);

        /// <summary>
        /// Recuperer list des ouvriers Ids par list des ci Ids
        /// </summary>
        /// <param name="ciList"></param>
        /// <returns>List des ouvriers ids</returns>
        Task<IEnumerable<AffectationEnt>> GetOuvriersListIdsByCiListAsync(List<int> ciList);

        /// <summary>
        /// Récupérer la liste des CI actifs dans le quel le personnel est affecté
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>La liste des CI</returns>
        IEnumerable<CIEnt> GetPersonnelActifAffectationCiList(int personnelId);
    }
}
