namespace Fred.Web.Models.Favori
{
  public class FavoriModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un favori
    /// </summary>
    public int FavoriId { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'utilisateur de rattachement d'un favori
    /// </summary>
    public int UtilisateurId { get; set; }

    /// <summary>
    /// Obtient ou définit le libelle à afficher sur le favori
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit la couleur du favori
    /// </summary>
    public string Couleur { get; set; }

    /// <summary>
    /// Obtient ou définit le l'objet de recherche serialisé
    /// </summary>
    public byte[] Search { get; set; }

    /// <summary>
    ///   Obtient ou définit le Search*Ent
    /// </summary>
    public object Filtre { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre d'élément du resultat de recherche
    /// </summary>
    public int nombreElement
    {
      get; set;
    }

    /// <summary>
    /// Obtient ou définit la description du favoris
    /// </summary>
    public string description
    {
      get; set;
    }

    /// <summary>
    /// Obtient ou définit le type du favori
    /// </summary>
    public string TypeFavori
    {
      get; set;
    }

    /// <summary>
    /// Obtient ou définit l'url du favori
    /// </summary>
    public string UrlFavori
    {
      get; set;
    }
  }
}

