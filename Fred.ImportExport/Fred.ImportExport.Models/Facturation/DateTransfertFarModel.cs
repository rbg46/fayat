using System.Collections.Generic;

namespace Fred.ImportExport.Models.Facturation
{
  /// <summary>
  /// Représente un model pour les dates de transfert de FAR.
  /// </summary>
  public class DateTransfertFarModel
  {
    /// <summary>
    /// Obtient ou définit l'année de la période de clôture.
    /// </summary>
    public int Annee { get; set; }

    /// <summary>
    /// Obtient ou définit le mois de la période de clôture.
    /// </summary>
    public int Mois { get; set; }

    /// <summary>
    /// Obtient ou définit l'auteur SAP du transfert de FAR.
    /// </summary>
    public string AuteurSap { get; set; }

    /// <summary>
    /// Obtient ou définit le code de la société.
    /// </summary>
    public string SocieteCode { get; set; }

    /// <summary>
    /// Obtient ou définit une liste de code CI.
    /// </summary>
    public List<string> CiCodes { get; set; }
  }
}
