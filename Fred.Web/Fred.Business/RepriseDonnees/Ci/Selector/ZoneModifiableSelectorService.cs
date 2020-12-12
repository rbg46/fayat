using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.Ci.Selector
{
    /// <summary>
    /// Recupere la zone Modifiable
    /// </summary>
    public class ZoneModifiableSelectorService : IZoneModifiableSelectorService
    {

        /// <summary>
        /// Retourne la zone modifiable
        /// </summary>
        /// <param name="repriseExcelCi">repriseExcelCi</param>
        /// <returns>la valeur chaine converti en boolean </returns>
        public bool GetZoneModifiable(RepriseExcelCi repriseExcelCi)
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
