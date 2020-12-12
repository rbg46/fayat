using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Ci.Selector
{
    /// <summary>
    /// Recupere la zone Modifiable
    /// </summary>
    public interface IZoneModifiableSelectorService : IService
    {
        /// <summary>
        /// Retourne la zone modifiable
        /// </summary>
        /// <param name="repriseExcelCi">repriseExcelCi</param>
        /// <returns>la valeur chaine converti en boolean </returns>
        bool GetZoneModifiable(RepriseExcelCi repriseExcelCi);
    }
}
