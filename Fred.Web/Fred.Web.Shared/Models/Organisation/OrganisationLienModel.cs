using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Organisation
{
  public class OrganisationLienModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une commande.
    /// </summary>
    public int OrganisationLiensId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un utilisateur.
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit le groupe associé
    /// </summary>
    public virtual OrganisationModel Organisation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un Role.
    /// </summary>
    public int? SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit le groupe associé
    /// </summary>
    public SocieteModel Societe { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un Role.
    /// </summary>
    public int? EtablissementComptableId { get; set; }

    /// <summary>
    /// Obtient ou définit le groupe associé
    /// </summary>
    public EtablissementComptableModel EtablissementComptable { get; set; }
  }
}