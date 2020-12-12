using System.ComponentModel.DataAnnotations;
using System;

namespace Fred.Web.Models.Referential.Light
{
  public class DeviseLightModel
  {
    /// <summary>
    /// Obtient ou définit Code Devise de Devise.
    /// </summary>

    public int DeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit Code ISO Nombre Devise de Devise.
    /// </summary>
    public string IsoNombre { get; set; }

    /// <summary>
    /// Obtient ou définit Symbole de la devise de Devise.
    /// </summary>
    public string Symbole { get; set; }
  }
}