namespace Fred.ImportExport.Models.Commande
{
  /// <summary>
  /// Représente une ligne de commande à recevoir de / envoyer vers SAP.
  /// </summary>
  public class CommandeLigneSapModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une ligne de commande.
    /// </summary>
    public int CommandeLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une ligne de commande.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit le prix unitaire d'une ligne de commande.
    /// </summary>
    public decimal PUHT { get; set; }

    /// <summary>
    /// Obtient ou définit la quantité d'une dépense.
    /// </summary>
    public decimal Quantite { get; set; }

    /// <summary>
    /// L'identifiant de la ligne de commande SAP, pour l'import de commande.
    /// </summary>
    public string CommandeLigneSap { get; set; }

    /// <summary>
    /// Le code de l'article, pour l'import de commande.
    /// </summary>
    public string ArticleCode { get; set; }

    #region ForeignKey

    /// <summary>
    /// Obtient ou définit l'unité d'une ligne de commande.
    /// En attente de dev
    /// CommandeLigneEnt.UniteId => Unite.Code 
    /// </summary>
    public string UniteCode { get; set; } = "M3";

    /// <summary>
    /// Obtient ou définit le libellé d'une ressource.
    /// CommandeLigneEnt.RessourceId => RessourceEnt.Libelle
    /// </summary>
    public string RessourceLibelle { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une ressource.
    /// CommandeLigneEnt.RessourceId => RessourceEnt.Code
    /// </summary>
    public string RessourceCode { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une tâche.
    /// CommandeLigneEnt.TacheId =>TacheEnt.Libelle
    /// </summary>
    public string TacheLibelle { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une tâche.
    /// CommandeLigneEnt.TacheId => TacheEnt.Code
    /// </summary>
    public string TacheCode { get; set; }

    /// <summary>
    /// Obtient ou définit le code de la nature analytique ANAEL.
    /// CommandeLigneEnt.RessourceId => RessourceEnt.ReferentielEtendus => ReferentielEtenduEnt.NatureId => NatureEnt.Code
    /// </summary>
    public string NatureCode { get; set; }

    #endregion ForeignKey
  }
}
