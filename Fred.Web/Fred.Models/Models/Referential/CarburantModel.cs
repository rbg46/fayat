using Fred.Web.Models.Carburant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Referential
{
  public class CarburantModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un Groupe.
    /// </summary>
    public int CarburantId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une Unité
    /// </summary>
    public int UniteId { get; set; }

    /// <summary>
    /// Obtient ou définit une Unité
    /// </summary>
    public UniteModel Unite { get; set; }

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
    public string CodeLibelle { get; set; }

    /// <summary>
    ///   Obtient ou définir la liste des paramétrage carburant
    /// </summary>
    public ICollection<CarburantOrganisationDeviseModel> ParametrageCarburants { get; set; }
  }
}