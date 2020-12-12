using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;

namespace Fred.Web.Models.Rapport
{
    /// <summary>
    /// Classe nécessaire au transit vers l'API via HTTP pour passer une donnée rapport et tache
    /// </summary>
    public class PointagePersonnelAndTacheModel
  {
    /// <summary>
    /// Obtient ou définit une ligne de rapport
    /// </summary>
    public PointagePersonnelModel<CIModel> RapportLigne { get; set; }

    /// <summary>
    /// Obtient ou définit une tache
    /// </summary>
    public TacheModel Tache { get; set; }
  }
}
