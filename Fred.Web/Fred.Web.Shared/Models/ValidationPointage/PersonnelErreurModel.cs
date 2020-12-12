using Fred.Web.Models.Personnel;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models
{
  public class PersonnelErreurModel<T> where T : class
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant du personnel concerné par l'erreur
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    ///   Obtient ou définit le personnel concerné par l'erreur
    /// </summary>
    public PersonnelModel Personnel { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des erreurs (Soit ControlePointageErreurModel ou RemonteeVracErreurModel)
    /// </summary>
    public virtual ICollection<T> Erreurs { get; set; }
  }
}