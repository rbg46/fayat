using Fred.Web.Models.Groupe;
using Fred.Web.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.ReferentielFixe
{
  public class TypeRessourceModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un type de ressource.
    /// </summary>
    public int TypeRessourceId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un type de ressource.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un type de ressource.
    /// </summary>
    public string Libelle { get; set; }

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