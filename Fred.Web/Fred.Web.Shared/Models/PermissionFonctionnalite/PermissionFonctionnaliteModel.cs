namespace Fred.Web.Shared.Models
{
  public class PermissionFonctionnaliteModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une permission data.
    /// </summary>   
    public int PermissionFonctionnaliteId { get; set; }
    
    /// <summary>
    ///   Obtient ou définit la clé de la permission
    /// </summary>
    
    public int PermissionId { get; set; }

    /// <summary>
    /// Permission cette classe est non mappé car est n'est pas stocké dans la base 
    /// </summary>
   
    public PermissionModel Permission { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique de la fonctionnalité.
    /// </summary>    
   
    public int FonctionnaliteId { get; set; }
   
  }
}
