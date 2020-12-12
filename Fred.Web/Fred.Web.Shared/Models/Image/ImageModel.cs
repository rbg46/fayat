namespace Fred.Web.Shared.Models.Image
{
  /// <summary>
  /// Model pour la liste des Images
  /// </summary>
  public class ImageModel
  {

    /// <summary>
    /// Id
    /// </summary>
   
    public int ImageId { get; set; }
    
    /// <summary>
    /// Le chemin 
    /// </summary>
   
    public string Path { get; set; }
    
    /// <summary>
    /// Type d'image voir Enumeration TypeImage
    /// </summary>   
    public int Type { get; set; }


  }
}
