﻿using System.Collections.Generic;

namespace Fred.Business.ExplorateurDepense
{
  /// <summary>
  /// Représente l'object résultat de la récupération de la liste des dépenses pour l'explorateur de dépenses
  /// </summary>
  public class ExplorateurDepenseResult
  {
    /// <summary>
    ///   Obtient ou définit la liste des dépenses
    /// </summary>
    public List<ExplorateurDepenseGeneriqueModel> Depenses { get; set; }

    /// <summary>
    ///   Obtient ou définit la somme des quantités
    /// </summary>
    public decimal QuantiteTotal { get; set; }

    /// <summary>
    ///   Obtient ou définit la somme des PUHT
    /// </summary>
    public decimal PUHTTotal { get; set; }

    /// <summary>
    ///   Obtient ou définit la somme des montants HT
    /// </summary>
    public decimal MontantHTTotal { get; set; }

    /// <summary>
    ///   Obtient ou définit le code de l'unité
    /// </summary>
    public string CodeUnite { get; set; }
  }
}
