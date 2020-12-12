using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
  /// <summary>
  /// Model de Rapport hebdomadaire pour enregistrer
  /// </summary>
  public class RapportHebdoSaveViewModel
  {
    /// <summary>
    /// La liste des noeuds d'un rapport hebdomadaire
    /// </summary>
    public List<RapportHebdoNode<AstreintePointageHebdoCell>> AstreintePanelViewModel { get; set; }

    /// <summary>
    /// Majoration view model
    /// </summary>
    public IList<MajorationPersonnelCiModel> MajorationPanelViewModel { get; set; }

    /// <summary>
    /// Pointage view model
    /// </summary>
    public IEnumerable<RapportHebdoNode<PointageCell>> PointagePanelViewModel { get; set; }

    /// <summary>
    /// Prime view model
    /// </summary>
    public IList<PrimeRapportHebdoModel> PrimePanelViewModel { get; set; }

    /// <summary>
    /// Date de lundi
    /// </summary>
    public DateTime MondayDate { get; set; }
  }
}
