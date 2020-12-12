using Fred.Web.Models.Holding;
using Fred.Web.Models.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Pole
{
  public class PoleModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un pole.
    /// </summary>
    public int PoleId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'organisation du pole
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'organisation du pole
    /// </summary>
    public OrganisationModel Organisation { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un pole.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un pole.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une holding.
    /// </summary>
    public int HoldingId { get; set; }

    /// <summary>
    /// Obtient ou définit l'objet groupe attaché à une holding
    /// </summary>
    public HoldingModel Holding { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle
    {
      get
      {
        return this.Code + " - " + this.Libelle;
      }
    }
  }
}