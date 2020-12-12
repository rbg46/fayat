using System;
using Fred.Entities.Personnel;

namespace Fred.Business.Personnel
{
    /// <summary>
    /// Utilitaire consernant le personnel.
    /// </summary>
    public static class PersonnelHelper
  {

    /// <summary>
    /// Permet de savoir si un personnel est actif.
    /// </summary>
    /// <param name="personnel">personnel</param>
    /// <param name="now">Aujourd hui</param>
    /// <returns>True si le personnel est actif</returns>
    public static bool GetPersonnelIsActive(this PersonnelEnt personnel, DateTime? now = null)
    {
      DateTime dateTimeNow = DateTime.UtcNow;
      if (now.HasValue)
      {
        dateTimeNow = now.Value;
      }
      return PersonnelHelper.GetPersonnelIsActive(personnel.DateSuppression, personnel.DateEntree, personnel.DateSortie, dateTimeNow);
    }

    /// <summary>
    /// Permet de savoir si un personnel est actif.   
    /// </summary>
    /// <param name="dateSupression">dateSupression du personnel</param>
    /// <param name="dateEntree">dateEntree du personnel</param>
    /// <param name="dateSortie">dateSortie du personnel</param>
    /// <param name="now">Aujourd hui</param>
    /// <returns>true si le personnel est actif</returns>
    private static bool GetPersonnelIsActive(DateTime? dateSupression, DateTime? dateEntree, DateTime? dateSortie, DateTime now)
    {
      if (!dateEntree.HasValue)
      {
        return false;
      }
      if (dateSupression.HasValue && dateSupression.Value.Date <= now.Date)
      {
        return false;
      }
      if (dateSortie.HasValue && dateSortie.Value.Date < now.Date)
      {
        return false;
      }
      if (dateEntree.Value.Date > now.Date)
      {
        return false;
      }
      return true;
    }
  }
}
