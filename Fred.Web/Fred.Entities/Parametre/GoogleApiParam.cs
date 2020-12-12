using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities
{
  /// <summary>
  ///   Représente un ensemble de paramètre pour la surveillance du nombre d'appels à la lib Google API.
  /// </summary>
  public class GoogleApiParam
  {
    private DateTime dateCourante;

    /// <summary>
    ///   Définit le nombre max d'appel à Google API par jour 
    /// </summary>
    public int Quota { get; set; }

    /// <summary>
    ///   Définit le nombre d'appel à Google API pour la journée courante  
    /// </summary>
    public int IndexCourant { get; set; }

    /// <summary>
    ///   Définit la date de mise à jour de l'index courant
    /// </summary>
    public DateTime DateCourante
    {
      get
      {
        return DateTime.SpecifyKind(dateCourante, DateTimeKind.Utc);
      }
      set
      {
        dateCourante = DateTime.SpecifyKind(value, DateTimeKind.Utc);
      }
    }
  }
}