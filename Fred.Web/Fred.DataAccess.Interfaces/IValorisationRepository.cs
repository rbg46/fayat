using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Rapport;
using Fred.Entities.Valorisation;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Interface de valorisation
    /// </summary>
    public interface IValorisationRepository : IRepository<ValorisationEnt>
    {
        /// <summary>
        /// Supprime les valorisations en paramètres
        /// </summary>
        /// <param name="listValo">Liste de valorisation</param>
        void DeleteValorisations(List<ValorisationEnt> listValo);

        /// <summary>
        /// Ajoute une valorisation
        /// </summary>
        /// <param name="valorisationEnt">Objet valorisation</param>
        /// <returns>La valorisation</returns>
        ValorisationEnt AddValorisation(ValorisationEnt valorisationEnt);

        /// <summary>
        /// Renvoi le calcul d'un total de valorisation pour un CI et une période
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        decimal Total(int ciId, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Renvoi le calcul d'un total de valorisation pour un CI et une période
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <param name="ressourceId">Identifiant d'une ressource de sélection</param>
        /// <param name="tacheId">Identifiant d'une tache de sélection</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        decimal Total(int ciId, DateTime dateDebut, DateTime dateFin, int? ressourceId, int? tacheId);

        /// <summary>
        /// Renvoi le total de valorisation pour un chapitre
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <param name="chapitreId">Identifiant d'un chapitre de sélection</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        decimal TotalByChapitreId(int ciId, DateTime dateDebut, DateTime dateFin, int chapitreId);

        /// <summary>
        /// Retourne des lignes de valorisations
        /// </summary>
        /// <param name="ciId">Identifiant de ci</param>
        /// <param name="periode">Période</param>
        /// <returns>Liste de valorisations</returns>
        IEnumerable<ValorisationEnt> GetByCiAndPeriod(int ciId, DateTime periode);

        /// <summary>
        /// Retourne des lignes de valorisations
        /// </summary>
        /// <param name="ciId">Identifiant de ci</param>
        /// <param name="annee">Année</param>
        /// <param name="mois">Mois</param>
        /// <returns>Liste de valorisations</returns>
        IEnumerable<ValorisationEnt> GetByCiAndYearAndMonth(int ciId, int annee, int mois);

        IReadOnlyList<ValorisationEnt> GetByCiAndYearAndMonth(List<int> ciIds, int annee, int mois);

        /// <summary>
        /// Retourne une valorisation
        /// </summary>
        /// <param name="groupRemplacementId">L'id du groupe de remplacement</param>
        /// <returns>Une valo</returns>
        IEnumerable<ValorisationEnt> GetByGroupRemplacementId(int groupRemplacementId);

        /// <summary>
        /// Renvoi le total de valorisation pour une liste de chapitre codes
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <param name="chapitreCodes">liste de chapitre codes</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        decimal TotalByChapitreCodes(int ciId, DateTime dateDebut, DateTime dateFin, List<string> chapitreCodes);

        /// <summary>
        /// Retourne la liste des valorisations associées à un rapport
        /// </summary>
        /// <param name="rapportId">identifiant du rapport</param>
        /// <returns>valorisations</returns>
        IEnumerable<ValorisationEnt> GetByRapportId(int rapportId);

        /// <summary>
        /// Retourne des lignes de valorisations
        /// </summary>
        /// <param name="listRapportLigneId">Liste d'identifiant unique de rapport ligne</param>
        /// <returns>Liste de valorisations</returns>
        IEnumerable<ValorisationEnt> GetByListRapportLigneId(List<int> listRapportLigneId);

        /// <summary>
        /// Retourne la liste des valorisations sans reception intérimaire selon les paramètres
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant du CI</param>
        /// <param name="periodeDebut">Date comptable periode début</param>
        /// <param name="periodeFin">Date comptable periode fin</param>
        /// <returns>Liste de dépense achat</returns>
        Task<IEnumerable<ValorisationEnt>> GetValorisationsWithoutReceptionInterimaireAsync(List<int> ciIds, DateTime periodeDebut, DateTime periodeFin);

        /// <summary>
        /// Récupération des valorisations (explorateur des dépenses)
        /// </summary>
        /// <param name="ciId">Identifiant de ci</param>
        /// <returns>Liste de valorisations</returns>
        Task<IEnumerable<ValorisationEnt>> GetValorisationsListAsync(int ciId);

        /// <summary>
        /// Récupération des valorisations
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="ressourceId"> Identifiant de la Ressource </param>
        /// <param name="periodeDebut">Date de début</param>
        /// <param name="periodeFin">Date de fin</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <returns>Enumarable de <see cref="ValorisationEnt"/></returns>
        Task<IEnumerable<ValorisationEnt>> GetValorisationsListAsync(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId);

        IReadOnlyList<ValorisationEnt> GetValorisationWithoutInterimaireOrMaterielExterneFromCiIdAfterPeriodeWithQuantityGreaterThanZeroAndWithoutLockPeriod(int ciId, DateTime periode);

        Task<IReadOnlyList<ValorisationEnt>> GetValorisationWithoutInterimaireOrMaterielExterneFromCiIdsAfterPeriodeWithQuantityGreaterThanZeroAndWithoutLockPeriod(List<int> ciIds, DateTime periode);

        IEnumerable<ValorisationEnt> GetByRapportIdAndRapportLigneId(int rapportId, int rapportLigneId);
        List<RapportRapportLigneVerrouPeriode> GetVerrouPeriodesList(List<RapportLigneEnt> rapportLignes);
        List<ValorisationEnt> GetValorisationsByRapporLignesIds(List<int> rapportLignesIds);

        /// <summary>
        /// Retourne la liste des valorisations selon les paramètres
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant du CI</param>
        /// <param name="periodeDebut">Date comptable periode début</param>
        /// <param name="periodeFin">Date comptable periode fin</param>
        /// <returns>Liste de dépense achat</returns>
        Task<IEnumerable<ValorisationEnt>> GetValorisationsAsync(IEnumerable<int> ciIds, DateTime periodeDebut, DateTime periodeFin);
    }
}
