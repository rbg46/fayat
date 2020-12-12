using Fred.Web.Models.Groupe;
using Fred.Web.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Organisation
{
  public class TypeOrganisationModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un Groupe.
    /// </summary>
    public int TypeOrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une OrgaGroupe.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un Groupe.
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