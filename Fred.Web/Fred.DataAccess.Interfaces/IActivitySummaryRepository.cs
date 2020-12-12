using Fred.Entities.ActivitySummary;
using System.Collections.Generic;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Permet de faire un rapport,etat des lieux les activités en cours des utilisateurs FRED
    /// </summary>
    public interface IActivitySummaryRepository : IMultipleRepository
    {
        /// <summary>
        /// Retourne les informations necessaires pour calculer les travaux en cours
        /// </summary>
        /// <returns>List de ActivitySummaryRequestResult</returns>
        List<ActivityRequestWithCountResult> GetDataForCalculateWorkInProgress();

        /// <summary>
        /// Retourne la liste des jalons
        /// </summary>
        /// <returns>List de ActivityRequestWithDateResult</returns>
        List<ActivityRequestWithDateResult> GetJalons();

        /// <summary>
        /// Retourne la liste des Emails pour ine liste de personnelIds
        /// </summary>
        /// <param name="personnelIds">personnelIds</param>
        /// <returns>Liste de EmailOfPersonnelResult</returns>
        List<PersonnelInfoForSendEmailResult> GetPersonnelsDataForSendEmail(List<int> personnelIds);

        /// <summary>
        /// Retourne les ci actifs
        /// </summary>
        /// <param name="onlyChantierFred">Indique si seuls les chantiers gérés par FRED sont retournés</param>
        /// <returns>Liste des ids des ci actifs</returns>
        List<int> GetCiActifs(bool onlyChantierFred);

        /// <summary>
        /// Retourne les ci actifs
        /// </summary>
        /// <returns>Liste des ids des ci actifs</returns>
        List<int> GetCiActifs();
    }
}
