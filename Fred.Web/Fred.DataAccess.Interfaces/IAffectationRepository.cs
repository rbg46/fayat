using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Web.Models.Affectation;
using Fred.Web.Shared.Models.EtatPaie;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Affecation repository interface
    /// </summary>
    public interface IAffectationRepository : IRepository<AffectationEnt>
    {
        /// <summary>
        /// Add or update a personnel to equipe favorite
        /// </summary>
        /// <param name="personnelId">PersonnelId</param>
        /// <param name="currentUserId">Resposable administratif d'une equipe</param>
        /// <param name="isInFavoriteTeam">True si le personnel doit etre dans l'equipe favorite</param>
        void AddUpdateToFavoriteTeam(int personnelId, int currentUserId, bool isInFavoriteTeam);

        /// <summary>
        /// Get la liste des personnels affectés a un CI
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <returns>List des affectations</returns>
        IEnumerable<AffectationEnt> GetAffectationListByCi(int ciId);

        /// <summary>
        /// Get la liste des personnels affectés a des CIs
        /// </summary>
        /// <param name="ciIdList">list des "Ci identifier"</param>
        /// <param name="etablissementPaieIdList">List Etablissement Paie Id </param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>List des affectations</returns>
        IEnumerable<PersonnelEnt> GetPersonnelListAffectedCiList(IEnumerable<int> ciIdList, IEnumerable<int?> etablissementPaieIdList, EtatPaieExportModel etatPaieExportModel);

        /// <summary>
        /// Get la liste des personnels affectés a un etablissement de paie
        /// </summary>
        /// <param name="etablissementPaieIdList">List Etablissement Paie Id </param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>List des affectations</returns>
        IEnumerable<int> GetPersonnelIdListAffectedEtablissementList(IEnumerable<int> etablissementPaieIdList, EtatPaieExportModel etatPaieExportModel);

        /// <summary>
        /// Get affecation by id
        /// </summary>
        /// <param name="id">Affecation id</param>
        /// <returns>List des affectations</returns>
        AffectationEnt GetAffectationById(int id);

        /// <summary>
        /// Supprimer une liste des affectations 
        /// </summary>
        /// <param name="affectationLisIds">liste des affectations identifiers</param>
        void DeleteAffectations(List<int> affectationLisIds);

        /// <summary>
        /// Get list des astreintes d'une affectation
        /// </summary>
        /// <param name="affectationId">Affectation identifier</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>List des astreintes</returns>
        IEnumerable<AstreinteEnt> GetAstreintesByAffectationIdAndDate(int affectationId, DateTime dateDebut, DateTime dateFin);

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
        /// Supprimer une liste des astreintes 
        /// </summary>
        /// <param name="astreintesList">List des astreintes</param>
        void DeleteAstreintes(List<AstreinteEnt> astreintesList);
        /// <summary>
        /// Supprimer une liste des affectations 
        /// </summary>
        /// <param name="affectationList">liste des affectations identifiers</param>
        void DeleteAffectations(List<AffectationEnt> affectationList);

        /// <summary>
        /// Récupérer une astreinte d'un personnel dans un CI et une date précise
        /// </summary>
        /// <param name="ciId">L'idenitifiant du CI</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreinteDate">La date de l'astreinte</param>
        /// <returns>L'astreinte</returns>
        AstreinteEnt GetAstreinte(int ciId, int personnelId, DateTime astreinteDate);

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
        /// Récupérer la liste des CI dans le quel le personnel est affecté
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>La liste des CI</returns>
        List<CIEnt> GetPersonnelAffectationCiList(int personnelId);

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
        /// Récupération ou création d'une Affectation avec besoin de Delegation (exemple : pour FES)
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
        /// Update affectation list
        /// </summary>
        /// <param name="affectationModelList">Affectaion model list</param>
        void UpdateAffectationList(IEnumerable<AffectationModel> affectationModelList);

        /// <summary>
        /// Get list des astreintes d'une affectation
        /// </summary>
        /// <param name="affectationId">Affectation identifier</param>
        /// <returns>List des astreintes</returns>
        List<AstreinteEnt> GetAstreintesByAffectationId(int affectationId);

        /// <summary>
        /// Ajouter une astreinte
        /// </summary>
        /// <param name="affectationId">Affectation identifier</param>
        /// <param name="startDate">Start date</param>
        /// <param name="dayOfWeek">Jour de la semaine</param>
        void AddAstreinte(int affectationId, DateTime startDate, int dayOfWeek);

        /// <summary>
        /// Update Astreinte
        /// </summary>
        /// <param name="astreinteId">astreinte identifier</param>
        void UpdateAstreinte(int astreinteId);

        /// <summary>
        /// Update affectation
        /// </summary>
        /// <param name="affectation">Affectation</param>
        /// <param name="isDelegate">Is delegate</param>
        void UpdateAffectation(AffectationEnt affectation, bool isDelegate);

        /// <summary>
        /// Get la liste des personnels affectés a un CI
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>affectations</returns>
        AffectationEnt GetAffectationByCiAndPersonnel(int ciId, int personnelId);

        /// <summary>
        /// Get la liste des personnels affectés a un CI
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>affectations</returns>
        List<AffectationEnt> GetAffectationByListPersonnelId(List<int> personnelId);

        /// <summary>
        /// Get la liste des personnels affectés a un  etablissement de paie par l'organisation id
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>List des affectations</returns>
        IEnumerable<int> GetPersonnelIdAffectedEtablissementByOrganisationId(EtatPaieExportModel etatPaieExportModel);

        /// <summary>
        /// Récupérer la liste des CI dans le quel le personnel est affecté
        /// </summary>
        /// <typeparam name="TCI">Le type de CI désiré.</typeparam>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="selector">Selector permettant de constuire un TCI en fonction d'un CIEnt.</param>
        /// <returns>La liste des CI</returns>
        List<TCI> GetPersonnelAffectationCiList<TCI>(int personnelId, Expression<Func<CIEnt, TCI>> selector);

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
        /// <param name="ciList">List des ci</param>
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
