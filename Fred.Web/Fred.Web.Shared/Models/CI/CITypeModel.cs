namespace Fred.Web.Models.CI
{
  public class CITypeModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un type de <see cref="CIEnt"/>.
    /// </summary>
    public int CITypeId { get; set; }

    /// <summary>
    /// Obtient ou définit la designation du type.
    /// </summary>
    public string Designation { get; set; }

    /// <summary>
    /// Obtient ou définit la clé ressource pour récupérer le nom du type.
    /// </summary>
    public string RessourceKey { get; set; }

    /// <summary>
    /// Définir si le type est selectionné pour le filtre.
    /// </summary>
    public bool Selected { get; set; }
  }
}
