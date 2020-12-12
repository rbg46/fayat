using Fred.Entities.Personnel;
using System.Collections.Generic;

namespace Fred.Entities.ValidationPointage
{
  /// <summary>
  ///   Classe PersonnelErreur
  /// </summary>
  /// <typeparam name="T">Type d'erreur : ControlePointageErreurEnt ou RemonteeVracErreurEnt</typeparam>
  public class PersonnelErreur<T> where T : class
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant du personnel concerné par l'erreur
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    ///   Obtient ou définit le personnel concerné par l'erreur
    /// </summary>
    public PersonnelEnt Personnel { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des erreurs
    /// </summary>
    public virtual ICollection<T> Erreurs { get; set; }
  }
}