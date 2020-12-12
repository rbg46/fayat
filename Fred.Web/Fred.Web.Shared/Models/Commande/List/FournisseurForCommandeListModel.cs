namespace Fred.Web.Shared.Models.Commande.List
{
  public class FournisseurForCommandeListModel
  {
    /// <summary>
    /// Obtient ou définit le code d'un fournisseur.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un fournisseur.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit l'Adresse du fournisseur cad
    ///  c'est à dire l'Adresse de facturation du compte tiers importé d'Anaël (TADR1, TADR2 et TADR3).
    /// </summary>
    public string Adresse { get; set; }

    /// <summary>
    /// Obtient ou définit le code postal du fournisseur
    /// c'est à dire le code postal du compte tiers importé d'Anaël (TPOST).
    /// </summary>
    public string CodePostal { get; set; }

    /// <summary>
    /// Obtient ou définit la ville du fournisseur
    /// c'est à dire le nom de la ville du compte tiers importé d'Anaël (TVILLE).
    /// </summary>
    public string Ville { get; set; }

    /// <summary>
    /// Obtient ou définit le téléphone du fournisseur
    ///  c'est à dire le numéro de téléphone du compte tiers importé d'Anaël (TTEL).
    /// </summary>
    public string Telephone { get; set; }

    /// <summary>
    /// Obtient ou définit l'email du fournisseur
    /// c'est à dire l'Adresse mail du compte tiers importé d'Anaël (TEMAIL).
    /// </summary>
    public string Email { get; set; }
  }
}
