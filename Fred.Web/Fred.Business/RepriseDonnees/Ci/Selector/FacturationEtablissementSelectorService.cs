using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.Ci.Selector
{
    /// <summary>
    /// Recupere la FacturationEtablissement
    /// </summary>
    public class FacturationEtablissementSelectorService : IFacturationEtablissementSelectorService
    {

        /// <summary>
        /// Retourne FacturationEtablissement
        /// </summary>
        /// <param name="repriseExcelCi">repriseExcelCi</param>
        /// <returns>la valeur chaine converti en boolean </returns>
        public bool GetFacturationEtablissement(RepriseExcelCi repriseExcelCi)
        {
            var zoneModifiable = repriseExcelCi.ZoneModifiable.Trim().ToUpper();

            if (zoneModifiable == "O")
            {
                return true;
            }
            if (zoneModifiable == "N")
            {
                return false;
            }
            if (zoneModifiable.IsNullOrEmpty())
            {
                return false;
            }

            return false;
        }
    }
}
