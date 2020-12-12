namespace Fred.Web.Shared.Models
{
  public class PermissionModel
  {

    public int PermissionId { get; set; }
    
    /// <summary>
    /// Code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Decrit la permission
    /// </summary>
    public string Libelle { get; set; }


    /// <summary>
    /// Obtient ou définit le libelle du référentiel prime
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient ou définit le code du référentiel prime
    /// </summary>
    public string CodeRef => this.Code;

  }
}
