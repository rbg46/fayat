using System;

namespace Fred.Entities.Rapport
{
  /// <summary>
  /// Représente le modèle des filtres pour l'extraction des rapports FES
  /// </summary>
  public class FilterRapportFesExport
  {
    /// <summary>
    /// Obtient ou définit la date de début
    /// </summary>
    public DateTime DateDebut { get; set; }

    /// <summary>
    /// Obtient ou définit la date de fin
    /// </summary>
    public DateTime DateFin { get; set; }

    /// <summary>
    /// Obtient ou définit la societe
    /// </summary>
    public string CodeSociete { get; set; }
  }
}
