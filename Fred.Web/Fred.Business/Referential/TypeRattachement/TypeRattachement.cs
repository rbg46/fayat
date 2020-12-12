namespace Fred.Business.Referential.TypeRattachement
{
  /// <summary>
  ///   Classe utlisé dans le contexte de calcul des indemnités de déplacement
  /// </summary>
  public class TypeRattachement
  {
    /// <summary>
    ///   Type de rattachement Secteur - correspond à l'agence de rattachement de l'établissement de rattachement du personnel
    /// </summary>
    public const string Secteur = "S";

    /// <summary>
    ///   Type de rattachement Domicile - correspond au domicile du personnel
    /// </summary>
    public const string Domicile = "D";

    /// <summary>
    ///   Type de rattachement Secteur - correspond à l'établissement de rattachement du personnel
    /// </summary>
    public const string Agence = "A";

    /// <summary>
    ///   Libelle Type de rattachement Secteur - correspond à l'agence de rattachement de l'établissement de rattachement du
    ///   personnel
    /// </summary>
    public const string SecteurLibelle = "Secteur";

    /// <summary>
    ///   Libelle Type de rattachement Domicile - correspond au domicile du personnel
    /// </summary>
    public const string DomicileLibelle = "Domicile";

    /// <summary>
    ///   Libelle Type de rattachement Secteur - correspond à l'établissement de rattachement du personnel
    /// </summary>
    public const string AgenceLibelle = "Agence";

    /// <summary>
    ///   Initialise une nouvelle instance de la classe <see cref="TypeRattachement" />
    /// </summary>
    /// <param name="id">Identifiant du type de rattachement</param>
    /// <param name="code">Code type rattachement</param>
    /// <param name="libelle">Libelle type rattachement</param>
    public TypeRattachement(int id, string code, string libelle)
    {
      TypeRattachementId = id;
      Code = code;
      Libelle = libelle;
    }

    /// <summary>
    ///   Obtient ou définit l'identifiant du Type de Rattachement
    /// </summary>
    public int TypeRattachementId { get; set; }

    /// <summary>
    ///   Obtient ou définit le code du TypeRattachement
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///   Obtient ou définit le libelle du TypeRattachement
    /// </summary>
    public string Libelle { get; set; }
  }
}