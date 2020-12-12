namespace Fred.Web.Shared.Models
{
  public class PersonnelImageModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant
    /// </summary>    
    public int PersonnelImageId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du personnel
    /// </summary>    
    public int PersonnelId { get; set; }

    /// <summary>
    ///   Obtient ou définit la signature scannée du salarié
    /// </summary>    
    public byte[] Signature { get; set; }

    /// <summary>
    ///   Obtient ou définit la signature au format base 64
    /// </summary>
    public string SignatureBase64 { get; set; }

    /// <summary>
    ///   Obtient ou définit la photo de profil scannée du salarié
    /// </summary>    
    public byte[] PhotoProfil { get; set; }

    /// <summary>
    ///   Obtient ou définit la photo de profil au format base 64
    /// </summary>
    public string PhotoProfilBase64 { get; set; }
  }
}
