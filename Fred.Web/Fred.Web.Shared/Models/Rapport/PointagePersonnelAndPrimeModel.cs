using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;

namespace Fred.Web.Models.Rapport
{
    /// <summary>
    /// Classe nécessaire au transit vers l'API via HTTP pour passer une donnée rapport et prime
    /// </summary>
    public class PointagePersonnelAndPrimeModel
  {
    /// <summary>
    /// Obtient ou définit une ligne de rapport
    /// </summary>
    public PointagePersonnelModel<CIModel> RapportLigne { get; set; }

    /// <summary>
    /// Obtient ou définit une prime
    /// </summary>
    public PrimeModel Prime { get; set; }
  }
}
