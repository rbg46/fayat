using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Ci.Selector
{
    /// <summary>
    /// Recupere la FacturationEtablissement
    /// </summary>
    public interface IFacturationEtablissementSelectorService : IService
    {
        /// <summary>
        /// Retourne FacturationEtablissement
        /// </summary>
        /// <param name="repriseExcelCi">repriseExcelCi</param>
        /// <returns>la valeur chaine converti en boolean </returns>
        bool GetFacturationEtablissement(RepriseExcelCi repriseExcelCi);
    }
}
